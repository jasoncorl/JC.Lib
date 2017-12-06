using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using System.EnterpriseServices;


namespace JC.Lib.IO
{
  using WORD = System.UInt16;
  using DWORD = System.UInt32;
  /// <summary>
  /// Lz77压缩算法
  /// </summary>
  public class Lz77Compression
  {
    const int MAX_WINDOW_SIZE = 65536;
    /// <summary>
    /// 滑动窗口
    /// </summary>
    private unsafe byte* pWnd;
    private int nWndSize = 0;
    /// <summary>
    /// 当前分配位置
    /// </summary>
    private int HeapPos = 0;
    /// <summary>
    /// 当前输出位置(字节偏移)
    /// </summary>
    private int CurByte = 0 ;
    /// <summary>
    /// 当前输出位置(位偏移)
    /// </summary>
    private int CurBit = 0 ;
    /// <summary>
    /// 256 * 256 指向SortHeap中下标的指针
    /// </summary>
    private WORD[] SortTable = new WORD[MAX_WINDOW_SIZE];

    /// <summary>
    /// 因为窗口不滑动，没有删除节点的操作，所以节点可以在SortHeap 中连续分配 
    /// </summary>
    private STIDXNODE[] SortHeap;

    public Lz77Compression()
    {
      SortHeap = new STIDXNODE[MAX_WINDOW_SIZE];   
    }
    
    private void InitSortTable()
    {
      SortTable = new WORD[sizeof(WORD) * MAX_WINDOW_SIZE];
      //memset(SortTable, 0, sizeof(WORD) * 65536);   
      nWndSize = 0;
      HeapPos = 1;   
    }

    private struct STIDXNODE
    {
      public WORD off;		// 在src中的偏移 
      public WORD off2;		// 用于对应的2字节串为重复字节的节点 
      // 指从 off 到 off2 都对应了该2字节串 
      public WORD next;		// 在SortHeap中的指针 
    }

    private unsafe int GetSameLen(byte* src, int srclen, int nSeekStart, int offset)
    {
      int i = 2; // 已经匹配了2个字节   
      int maxsame = Math.Min(srclen - nSeekStart, nWndSize - offset);
      while (i < maxsame
              && src[nSeekStart + i] == pWnd[offset + i])
        i++;
      //_ASSERT(nSeekStart + i <= srclen && offset + i <= nWndSize);
      return i;
    }   

    /// <summary>
    /// 试图在滑动窗口中找出最长的匹配字符串
    /// </summary>
    /// <param name="src"></param>
    /// <param name="srclen"></param>
    /// <param name="nSeekStart"></param>
    /// <param name="offset">输出：窗口中匹配字符串相对窗口边界的偏移</param>
    /// <param name="len">输出：可匹配的长度</param>
    /// <returns>是否匹配</returns>
    private unsafe bool SeekPhase(byte* src, int srclen, int nSeekStart, int* offset, int* len)
    {
      int j, m, n;

      if (nSeekStart < srclen - 1)
      {
        byte ch1, ch2;
        ch1 = src[nSeekStart]; ch2 = src[nSeekStart + 1];
        WORD p;
        p = SortTable[ch1 * 256 + ch2];
        if (p != 0)
        {
          m = 2; n = SortHeap[p].off;
          while (p != 0)
          {
            j = GetSameLen(src, srclen,
                nSeekStart, SortHeap[p].off);
            if (j > m)
            {
              m = j;
              n = SortHeap[p].off;
            }
            p = SortHeap[p].next;
          }
          (*offset) = n;
          (*len) = m;
          return true;
        }
      }
      return false;
    }   

    /// <summary>
    /// 取log2(n)的lower_bound
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private int LowerLog2(int n)
    {
      int i = 0;
      if (n > 0)
      {
        int m = 1;
        while (true)
        {
          if (m == n)
            return i;
          if (m > n)
            return i - 1;
          m <<= 1;
          i++;
        }
      }
      else
        return -1;
    }
     
    /// <summary>
    /// 复制内存中的位流，起始位的表示约定为从字节的高位至低位（由左至右）依次为 0，1，... , 7，要复制的两块数据区不能有重合
    /// </summary>
    /// <param name="memDest">目标数据区</param>
    /// <param name="nDestPos">目标数据区第一个字节中的起始位</param>
    /// <param name="memSrc">源数据区</param>
    /// <param name="nSrcPos">源数据区第一个字节的中起始位</param>
    /// <param name="nBits">要复制的位数</param>
    private unsafe void CopyBits(byte* memDest, int nDestPos,
              byte* memSrc, int nSrcPos, int nBits)
    {
      int iByteDest = 0, iBitDest;
      int iByteSrc = 0, iBitSrc = nSrcPos;

      int nBitsToFill, nBitsCanFill;

      while (nBits > 0)
      {
        // 计算要在目标区当前字节填充的位数   
        nBitsToFill = Math.Min(nBits, iByteDest>0 ? 8 : 8 - nDestPos);
        // 目标区当前字节要填充的起始位   
        iBitDest = iByteDest>0 ? 0 : nDestPos;
        // 计算可以一次从源数据区中复制的位数   
        nBitsCanFill = Math.Min(nBitsToFill, 8 - iBitSrc);
        // 字节内复制   
        CopyBitsInAByte(memDest + iByteDest, iBitDest,
            memSrc + iByteSrc, iBitSrc, nBitsCanFill);
        // 如果还没有复制完 nBitsToFill 个   
        if (nBitsToFill > nBitsCanFill)
        {
          iByteSrc++; iBitSrc = 0; iBitDest += nBitsCanFill;
          CopyBitsInAByte(memDest + iByteDest, iBitDest,
                  memSrc + iByteSrc, iBitSrc,
                  nBitsToFill - nBitsCanFill);
          iBitSrc += nBitsToFill - nBitsCanFill;
        }
        else
        {
          iBitSrc += nBitsCanFill;
          if (iBitSrc >= 8)
          {
            iByteSrc++; iBitSrc = 0;
          }
        }

        nBits -= nBitsToFill;   // 已经填充了nBitsToFill位   
        iByteDest++;
      }           
    }
    /// <summary>
    /// 在一个字节范围内复制位流，参数含义同 CopyBits 的参数。说明：此函数由 CopyBits 调用，不做错误检查，即：假定要复制的位都在一个字节范围内
    /// </summary>
    /// <param name="memDest">目标数据区</param>
    /// <param name="nDestPos">目标数据区第一个字节中的起始位</param>
    /// <param name="memSrc">源数据区</param>
    /// <param name="nSrcPos">源数据区第一个字节的中起始位</param>
    /// <param name="nBits">要复制的位数</param>
    private unsafe void CopyBitsInAByte(byte* memDest, int nDestPos,
              byte* memSrc, int nSrcPos, int nBits)
    {
      byte b1, b2;
      b1 = *memSrc;
      b1 <<= nSrcPos; b1 >>= 8 - nBits;   // 将不用复制的位清0   
      b1 <<= 8 - nBits - nDestPos;      // 将源和目的字节对齐   
      *memDest |= b1;     // 复制值为1的位   
      b2 = 0xff; b2 <<= 8 - nDestPos;       // 将不用复制的位置1   
      b1 |= b2;
      b2 = 0xff; b2 >>= nDestPos + nBits;
      b1 |= b2;
      *memDest &= b1;     // 复制值为0的位   
    }   

    /// <summary>
    /// 将位指针*piByte(字节偏移), *piBit(字节内位偏移)后移num位
    /// </summary>
    /// <param name="piByte"></param>
    /// <param name="piBit"></param>
    /// <param name="num"></param>
    private void MovePos(ref int piByte, ref int piBit, int num)
    {
      num += piBit;
      piByte += num / 8;
      piBit = num % 8;
    }

    /// <summary>
    /// 将DWORD值从高位字节到低位字节排列
    /// </summary>
    /// <param name="pDW"></param>
    private void InvertDWord(ref DWORD pDW)
    {
      byte[] Temp= BitConverter.GetBytes( pDW );
      byte[] Temp1 = new byte[4] { Temp[3], Temp[2], Temp[1], Temp[0]};
      pDW = BitConverter.ToUInt32(Temp1, 0);
      //return;
      //union UDWORD{ DWORD dw; BYTE b[4]; };
      //UDWORD* pUDW = (UDWORD*)pDW;   
      //BYTE b;   
      //b = pUDW->b[0];  pUDW->b[0] = pUDW->b[3]; pUDW->b[3] = b;   
      //b = pUDW->b[1];  pUDW->b[1] = pUDW->b[2]; pUDW->b[2] = b;   
    }   

    /// <summary>
    /// 输出压缩码
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="code">要输出的数</param>
    /// <param name="bits">要输出的位数(对isGamma=TRUE时无效) </param>
    /// <param name="isGamma">是否输出为γ编码</param>
    private unsafe void OutCode(byte* dest, DWORD code, int bits, bool isGamma)
    {
      if (isGamma)
      {
        byte* pb;
        DWORD outVal;
        // 计算输出位数   
        int GammaCode = (int)code - 1;
        int q = LowerLog2(GammaCode);
        if (q > 0)
        {
          outVal = 0xffff;
          pb = (byte*)&outVal;
          // 输出q个1   
          CopyBits(dest + CurByte, CurBit, pb, 0, q);
          MovePos(ref CurByte,ref CurBit, q);
        }
        // 输出一个0   
        outVal = 0;
        pb = (byte*)&outVal;
        CopyBits(dest + CurByte, CurBit, pb + 3, 7, 1);
        MovePos(ref CurByte, ref CurBit, 1);
        if (q > 0)
        {
          // 输出余数, q位   
          int sh = 1;
          sh <<= q;
          outVal = (uint)GammaCode - (uint)sh;
          pb = (byte*)&outVal;
          InvertDWord(ref outVal);
          CopyBits(dest + CurByte, CurBit, pb + (32 - q) / 8, (32 - q) % 8, q);
          MovePos(ref CurByte, ref CurBit, q);
        }
      }
      else
      {
        DWORD dw = (DWORD)code;
        byte* pb = (byte*)&dw;
        InvertDWord(ref dw);
        CopyBits(dest + CurByte, CurBit, pb + (32 - bits) / 8, (32 - bits) % 8, bits);
        MovePos(ref CurByte, ref CurBit, bits);
      } 
    }

    /// <summary>
    /// 取log2(n)的upper_bound
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private int UpperLog2(int n)
    {
      int i = 0;
      if (n > 0)
      {
        int m = 1;
        while (true)
        {
          if (m >= n)
            return i;
          m <<= 1;
          i++;
        }
      }
      else
        return -1;
    }

    /// <summary>
    /// 向索引中添加一个2字节串
    /// </summary>
    /// <param name="off"></param>
    private unsafe void InsertIndexItem(int off)
    {
      WORD q;
      byte ch1, ch2;
      ch1 = pWnd[off]; ch2 = pWnd[off + 1];

      if (ch1 != ch2)
      {
        // 新建节点   
        q = (WORD)HeapPos;
        HeapPos++;
        SortHeap[q].off = (ushort)off;
        SortHeap[q].next = SortTable[ch1 * 256 + ch2];
        SortTable[ch1 * 256 + ch2] = q;
      }
      else
      {
        // 对重复2字节串   
        // 因为没有虚拟偏移也没有删除操作，只要比较第一个节点   
        // 是否和 off 相连接即可   
        q = SortTable[ch1 * 256 + ch2];
        if (q != 0 && off == SortHeap[q].off2 + 1)
        {
          // 节点合并   
          SortHeap[q].off2 = (ushort)off;
        }
        else
        {
          // 新建节点   
          q = (WORD)HeapPos;
          HeapPos++;
          SortHeap[q].off = (ushort)off;
          SortHeap[q].off2 = (ushort)off;
          SortHeap[q].next = SortTable[ch1 * 256 + ch2];
          SortTable[ch1 * 256 + ch2] = q;
        }
      }
    }

    /// <summary>
    /// 将窗口向右滑动n个字节
    /// </summary>
    /// <param name="n"></param>
    private void ScrollWindow(int n)
    {
      for (int i = 0; i < n; i++)
      {
        nWndSize++;
        if (nWndSize > 1)
          InsertIndexItem(nWndSize - 2);
      }
    }

    /// <summary>
    /// 得到字节byte第pos位的值,pos顺序为高位起从0记数（左起） 
    /// </summary>
    /// <param name="BYTE"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private byte GetBit(byte BYTE, int pos)
    {
      int j = 1;
      j <<= 7 - pos;
      if ((BYTE & j) > 0)
        return 1;
      else
        return 0;
    }

    /// <summary>
    /// 压缩源文件到目标文件
    /// </summary>
    /// <param name="FileSrc"></param>
    /// <param name="FileDesc"></param>
    /// <returns></returns>
    public unsafe bool Compress(string FileSrc, string FileDest)
    {
      FileStream fsSrc = new FileStream(FileSrc,FileMode.Open);
      if (File.Exists(FileDest)) File.Delete(FileDest);
      FileStream fsDest = new FileStream(FileDest,FileMode.OpenOrCreate);
      byte[] bytSrc = new byte[MAX_WINDOW_SIZE], bytDest = new byte[MAX_WINDOW_SIZE + 16];

		  WORD flag1, flag2;
      long soulen = fsSrc.Length;
      int last = (int)soulen, act;   
      while ( last > 0 )   
      {   
        act = Math.Min(MAX_WINDOW_SIZE, last);
        //bytSrc = new byte[act];
        //bytDest = new byte[act + 16];

        fsSrc.Read(bytSrc,0,act);

        last -= act;   
        if (act == MAX_WINDOW_SIZE)  // out 65536 bytes
            flag1 = 0;
        else       // out last blocks   
        {
          flag1 = (WORD)act;
        }
        fsDest.Write(BitConverter.GetBytes(flag1), 0, sizeof(WORD));
        int destlen;
        fixed (byte* pSrc = bytSrc, pDst = bytDest)
        {
          destlen = Compress(pSrc, act, pDst);
        }

        if (destlen == 0)// can't compress the block   
        {
          flag2 = flag1;
          fsDest.Write(BitConverter.GetBytes(flag2), 0, sizeof(WORD));
          fsDest.Write(bytSrc, 0, act);
        }   
        else   
        {
          flag2 = (WORD)destlen;
          fsDest.Write(BitConverter.GetBytes(flag2), 0, sizeof(WORD));
          fsDest.Write(bytDest, 0, destlen);
        }
      }
      fsSrc.Close();
      fsDest.Close();
      return true;
    }

    /// <summary>
    /// 一次性压缩并写入文件，大文件处理时不建议使用
    /// </summary>
    /// <param name="FileSrc"></param>
    /// <param name="FileDest"></param>
    public void CompressOnePush(string FileSrc, string FileDest)
    {
      FileStream fsSrc = new FileStream(FileSrc, FileMode.Open);
      if (File.Exists(FileDest)) File.Delete(FileDest);
      FileStream fsDest = new FileStream(FileDest, FileMode.OpenOrCreate);
      byte[] temp = new byte[fsSrc.Length];
      fsSrc.Read(temp, 0, (int)fsSrc.Length);
      byte[] temp1 = Compress(temp);
      fsDest.Write(temp1, 0, (int)temp1.Length);

      fsSrc.Close();
      fsDest.Close();
    }

    /// <summary>
    /// 压缩指定字节流
    /// </summary>
    /// <param name="Src"></param>
    /// <returns>返回已压缩的字节流</returns>
    public unsafe byte[] Compress(byte[] Src)
    {
      byte[] Ret = null, Temp;
      byte[] bytSrc = new byte[MAX_WINDOW_SIZE], bytDest = new byte[MAX_WINDOW_SIZE + 16];

      WORD flag1, flag2;
      long soulen = Src.Length;
      int last = (int)soulen, act;
      while (last > 0)
      {
        act = Math.Min(MAX_WINDOW_SIZE, last);

        Array.Copy(Src, soulen - last, bytSrc, 0, act);

        last -= act;
        if (act == MAX_WINDOW_SIZE)  // out 65536 bytes
          flag1 = 0;
        else       // out last blocks   
        {
          flag1 = (WORD)act;
        }

        Temp = new byte[(Ret == null ? 0 : Ret.Length) + sizeof(WORD)];
        if (Ret != null) Ret.CopyTo(Temp, 0);
        BitConverter.GetBytes(flag1).CopyTo(Temp, (Ret == null ? 0 : Ret.Length));
        Ret = Temp;

        int destlen;
        fixed (byte* pSrc = bytSrc, pDst = bytDest)
        {
          destlen = Compress(pSrc, act, pDst);
        }

        if (destlen == 0)// can't compress the block   
        {
          flag2 = flag1;

          Temp = new byte[(Ret == null ? 0 : Ret.Length) + sizeof(WORD) + act];
          Ret.CopyTo(Temp, 0);
          BitConverter.GetBytes(flag2).CopyTo(Temp, Ret == null ? 0 : Ret.Length);
          Array.Copy(bytSrc, 0, Temp, (Ret == null ? 0 : Ret.Length) + sizeof(WORD), act);
          Ret = Temp;
        }
        else
        {
          flag2 = (WORD)destlen;

          Temp = new byte[(Ret == null ? 0 : Ret.Length) + sizeof(WORD) + destlen];
          Ret.CopyTo(Temp, 0);
          BitConverter.GetBytes(flag2).CopyTo(Temp, (Ret == null ? 0 : Ret.Length));
          Array.Copy(bytDest, 0, Temp, (Ret == null ? 0 : Ret.Length) + sizeof(WORD), destlen);
          Ret = Temp;
        }
      }

      return Ret;
    }
    /// <summary>
    /// 压缩一段字节流
    /// </summary>
    /// <param name="src">源数据区</param>
    /// <param name="srclen">源数据区字节长度</param>
    /// <param name="dest">压缩数据区，调用前分配srclen+5字节内存</param>
    /// <returns>返回值 > 0 压缩数据长度;= 0 数据无法压缩;< 0 压缩中异常错误</returns>
    public unsafe int Compress(byte* src, int srclen, byte* dest)
    {
      int i;
      CurByte = 0; CurBit = 0;
      int off, len;

      if (srclen > MAX_WINDOW_SIZE)
        return -1;

      pWnd = src;
      InitSortTable();
      for (i = 0; i < srclen; i++)
      {
        if (CurByte >= srclen)
          return 0;
        if (SeekPhase(src, srclen, i, &off, &len))
        {
          // 输出匹配术语 flag(1bit) + len(γ编码) + offset(最大16bit)   
          OutCode(dest, 1, 1, false);
          OutCode(dest, (uint)len, 0, true);

          // 在窗口不满64k大小时，不需要16位存储偏移   
          OutCode(dest, (uint)off, UpperLog2(nWndSize), false);

          ScrollWindow(len);
          i += len - 1;
        }
        else
        {
          // 输出单个非匹配字符 0(1bit) + char(8bit)   
          OutCode(dest, 0, 1, false);
          OutCode(dest, (DWORD)(src[i]), 8, false);
          ScrollWindow(1);
        }
      }
      int destlen = CurByte + ((CurBit>0) ? 1 : 0);
      if (destlen >= srclen)
        return 0;
      return destlen;   
    }
    
    /// <summary>
    /// 解压缩指定长度的byte流
    /// </summary>
    /// <param name="src"></param>
    /// <param name="srclen"></param>
    /// <param name="dest"></param>
    /// <returns>成功返回true，否则为false</returns>
    public unsafe bool DeCompress(byte* src, int srclen, byte* dest)
    {
      int i;
      CurByte = 0; CurBit = 0;
      pWnd = dest;     // 初始化窗口   
      nWndSize = 0;

      if (srclen > MAX_WINDOW_SIZE)
        return false;

      for (i = 0; i < srclen; i++)
      {
        byte b = GetBit(src[CurByte], CurBit);
        MovePos(ref CurByte, ref CurBit, 1);
        if (b == 0) // 单个字符   
        {
          CopyBits(dest + i, 0, src + CurByte, CurBit, 8);
          MovePos(ref CurByte, ref CurBit, 8);
          nWndSize++;
        }
        else        // 窗口内的术语   
        {
          int q = -1;
          while (b != 0)
          {
            q++;
            b = GetBit(src[CurByte], CurBit);
            MovePos(ref CurByte, ref CurBit, 1);
          }
          int len, off;
          DWORD dw = 0;
          byte* pb;
          if (q > 0)
          {
            pb = (byte*)&dw;
            CopyBits(pb + (32 - q) / 8, (32 - q) % 8, src + CurByte, CurBit, q);
            MovePos(ref CurByte, ref CurBit, q);
            InvertDWord(ref dw);
            len = 1;
            len <<= q;
            len += (int)dw;
            len += 1;
          }
          else
            len = 2;

          // 在窗口不满64k大小时，不需要16位存储偏移   
          dw = 0;
          pb = (byte*)&dw;
          int bits = UpperLog2(nWndSize);
          CopyBits(pb + (32 - bits) / 8, (32 - bits) % 8, src + CurByte, CurBit, bits);
          MovePos(ref CurByte, ref CurBit, bits);
          InvertDWord(ref dw);
          off = (int)dw;
          // 输出术语   
          for (int j = 0; j < len; j++)
          {
            dest[i + j] = pWnd[off + j];
          }
          nWndSize += len;
          i += len - 1;
        }
        // 滑动窗口   
        if (nWndSize > MAX_WINDOW_SIZE)
        {
          pWnd += nWndSize - MAX_WINDOW_SIZE;
          nWndSize = MAX_WINDOW_SIZE;
        }
      }

      return true;
    }
    
    /// <summary>
    /// 解压缩源文件到指定文件
    /// </summary>
    /// <param name="FileSrc"></param>
    /// <param name="FileDest"></param>
    /// <returns></returns>
    public unsafe bool DeCompress(string FileSrc, string FileDest)
    {
      FileStream fsSrc = new FileStream(FileSrc,FileMode.Open);
      if (File.Exists(FileDest)) File.Delete(FileDest);
      FileStream fsDest = new FileStream(FileDest,FileMode.OpenOrCreate);
      byte[] bytSrc = new byte[MAX_WINDOW_SIZE], bytDest = new byte[MAX_WINDOW_SIZE + 16];

      byte[] bytflag1 = new byte[sizeof(WORD)], bytflag2 = new byte[sizeof(WORD)];
		  WORD flag1, flag2;
      long soulen = fsSrc.Length;
      int last = (int)soulen, act;
      while (last > 0)
      {
        fsSrc.Read(bytflag1,0,sizeof(WORD));
        fsSrc.Read(bytflag2,0,sizeof(WORD));
        flag1 = BitConverter.ToUInt16(bytflag1,0);
        flag2 = BitConverter.ToUInt16(bytflag2,0);

        last -= 2 * sizeof(WORD);
        if (flag1 == 0)
          act = MAX_WINDOW_SIZE;
        else
          act = flag1;
        last -= flag2 > 0 ? (flag2) : act;

        if (flag2 == flag1)
        {
          fsSrc.Read(bytSrc, 0, act);
          //fread(soubuf, act, 1, in); 
          fsDest.Write(bytSrc, 0, act);                
        }
        else
        {
          fsSrc.Read(bytSrc, 0, flag2);
          //fread(destbuf, flag2, 1, in);
          fixed (byte* destbuf = bytDest, srcbuf = bytSrc)
          {
            if (!DeCompress(srcbuf, act, destbuf))
            {
              fsSrc.Close();
              fsDest.Close();
              return false;
            }
          }
          fsDest.Write(bytDest, 0, act);
        }
          //fwrite((BYTE*)soubuf, act, 1, out);     
      }
      fsSrc.Close();
      fsDest.Close();
      return true;
    }

    /// <summary>
    /// 解压一段字节流
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>
    public unsafe byte[] DeCompress(byte[] Src)
    {
      byte[] Ret=null, Temp;
      int offset = 0;
      byte[] bytSrc = new byte[MAX_WINDOW_SIZE], bytDest = new byte[MAX_WINDOW_SIZE + 16];

      byte[] bytflag1 = new byte[sizeof(WORD)], bytflag2 = new byte[sizeof(WORD)];
      WORD flag1, flag2;
      long soulen = Src.Length;
      int last = (int)soulen, act;
      while (last > 0)
      {
        //fsSrc.Read(bytflag1, 0, sizeof(WORD));
        Array.Copy(Src, offset, bytflag1, 0, sizeof(WORD));
        offset += sizeof(WORD);
        //fsSrc.Read(bytflag2, 0, sizeof(WORD));
        Array.Copy(Src, offset, bytflag2, 0, sizeof(WORD));
        offset += sizeof(WORD);

        flag1 = BitConverter.ToUInt16(bytflag1, 0);
        flag2 = BitConverter.ToUInt16(bytflag2, 0);

        last -= 2 * sizeof(WORD);
        if (flag1 == 0)
          act = MAX_WINDOW_SIZE;
        else
          act = flag1;
        last -= flag2 > 0 ? (flag2) : act;

        if (flag2 == flag1)
        {
          //fsSrc.Read(bytSrc, 0, act);
          bytSrc = new byte[act];
          Array.Copy(Src, offset, bytSrc, 0, act);
          offset += act;
          //fsDest.Write(bytSrc, 0, act);
          Temp = new byte[(Ret == null ? 0 : Ret.Length) + act];
          if (Ret != null) Ret.CopyTo(Temp, 0);
          bytSrc.CopyTo(Temp, (Ret == null ? 0 : Ret.Length));
          Ret = Temp;
        }
        else
        {
          //fsSrc.Read(bytSrc, 0, flag2);
          bytSrc = new byte[flag2];
          Array.Copy(Src, offset, bytSrc, 0, flag2);
          offset += flag2;

          fixed (byte* destbuf = bytDest, srcbuf = bytSrc)
          {
            if (!DeCompress(srcbuf, act, destbuf))
            {
              //fsSrc.Close();
              //fsDest.Close();
              //return false;
            }
          }
          //fsDest.Write(bytDest, 0, act);
          Temp = new byte[(Ret == null ? 0 : Ret.Length) + act];
          if (Ret != null) Ret.CopyTo(Temp, 0);
          Array.Copy(bytDest, 0, Temp, (Ret == null ? 0 : Ret.Length), act);
          Ret = Temp;
        }     
      }
      //fsSrc.Close();
      //fsDest.Close();
      return Ret;
      
      #region 文件中间人方式
      /*//本方式频繁操作文件，不适合线程批量处理
      string fileTemp1 = AppDomain.CurrentDomain.BaseDirectory + "temp1.t";
      string fileTemp2 = AppDomain.CurrentDomain.BaseDirectory + "temp2.t";
      FileStream fs = new FileStream(fileTemp1,FileMode.OpenOrCreate);
      fs.Write(Src,0,Src.Length);
      fs.Flush();
      fs.Close();
      DeCompress(fileTemp1,fileTemp2);
      fs = new FileStream(fileTemp2,FileMode.Open, FileAccess.Read);
      byte[] Temp = new byte[fs.Length];
      fs.Read(Temp, 0, (int)fs.Length);
      fs.Close();
      System.IO.File.Delete(fileTemp1);
      System.IO.File.Delete(fileTemp2);
      return Temp;
       */
      #endregion
    }
  }

  /// <summary>
  /// 压缩体结构，就是压缩文件结构
  /// </summary>
  public class ZipEntity
  {
    #region 文件头
    /// <summary>
    /// 头长度
    /// </summary>
    public uint HeadLen = 4 * 5;
    /// <summary>
    /// 总长度，其实就是Buffer长度
    /// </summary>
    public uint Length = 4 * 5;
    /// <summary>
    /// 滑动窗口大小
    /// </summary>
    public uint WinSize = 65536;
    /// <summary>
    /// 条目个数
    /// </summary>
    public uint EntryCount = 0;
    /// <summary>
    /// 条目表总长度
    /// </summary>
    public uint EntrysTableLen = 0;
    #endregion

    private byte[] buff= null;
    /// <summary>
    /// 整个对象的流。依次为：文件头 + 条目表 + 条目数据表
    /// </summary>
    public byte[] Buffer
    {
      get
      {
        return buff;
      }
    }

    private FileStream fs=null;

    /// <summary>
    /// 通过文件名，构造一个压缩结构，不存在则创建文件
    /// </summary>
    /// <param name="FileName"></param>
    public ZipEntity(string FileName)
    {
      if (FileName == "") return;
      bool isNews = true;
      if (File.Exists(FileName))
      {
        isNews = false;
      }
      fs = new FileStream(FileName, FileMode.OpenOrCreate);
      #region 初始化文件结构      
      if (!isNews)
      {
        //解析文件结构
        try
        {
          byte[] Temp = new byte[fs.Length];
          fs.Read(Temp,0,(int)fs.Length);
          HeadLen = BitConverter.ToUInt32(Temp, 0);
          Length = BitConverter.ToUInt32(Temp, sizeof(uint));
          WinSize = BitConverter.ToUInt32(Temp, sizeof(uint) * 2);
          EntryCount = BitConverter.ToUInt32(Temp, sizeof(uint) * 3);
          EntrysTableLen = BitConverter.ToUInt32(Temp, sizeof(uint) * 4);
          //buff = new byte[Temp.Length - sizeof(uint) * 5];
          //Array.Copy(Temp, sizeof(uint) * 5, buff, 0, Temp.Length - sizeof(uint) * 5);
          buff = Temp;
        }
        catch (Exception ex)
        {
          throw new Exception("文件解析错误，可能是非法压缩文件！",ex);
        }
      }
      else
      {
        byte[] Temp = new byte[HeadLen];
        BitConverter.GetBytes(HeadLen).CopyTo(Temp, 0);
        BitConverter.GetBytes(Length).CopyTo(Temp, sizeof(uint));
        BitConverter.GetBytes(WinSize).CopyTo(Temp, sizeof(uint) * 2);
        BitConverter.GetBytes(EntryCount).CopyTo(Temp, sizeof(uint) * 3);
        BitConverter.GetBytes(EntrysTableLen).CopyTo(Temp, sizeof(uint) * 4);
        buff = Temp;
      }
      #endregion
    }

    /// <summary>
    /// 保存，除非调用Close()，否则文件仍然被占用
    /// </summary>
    public void Save()
    {
      string FileName = fs.Name;
      fs.Position = 0;
      fs.Write(buff, 0, buff.Length);
      fs.Flush();
      fs.Close();
      //保持占用，重新打开，除非调用Close();
      fs = new FileStream(FileName, FileMode.Open);
    }

    /// <summary>
    /// 保存，除非调用Close()，否则文件仍然被占用
    /// </summary>
    /// <param name="FileName"></param>
    public void Save(string FileName)
    {
      fs.Close();
      fs = new FileStream(FileName,FileMode.Create);
      fs.Write(buff, 0, buff.Length);
      fs.Flush();
      fs.Close();
      //保持占用，重新打开，除非调用Close();
      fs = new FileStream(FileName, FileMode.Open);
    }

    public void Close()
    {
      fs.Close();
      fs.Dispose();
    }

    /// <summary>
    /// 解压所有条目，顺序解压
    /// </summary>
    /// <param name="SaveDir">存储目录</param>
    public void UnZipAll(string SaveDir)
    {
      try
      {
        //条目表
        byte[] EntryTablesBuffer = new byte[EntrysTableLen];
        Array.Copy(this.buff, HeadLen, EntryTablesBuffer, 0, EntrysTableLen);
        //文件数据
        byte[] FilesBuffer = new byte[Length - EntrysTableLen - HeadLen];
        Array.Copy(this.buff, HeadLen + EntrysTableLen, FilesBuffer, 0, Length - EntrysTableLen - HeadLen);
        //开始遍历Entry
        int iTablesOffBits = 0;
        int iFilesOffBits = 0;
        uint iCount = 1;
        while (iCount <= EntryCount)
        {
          uint EntryLen = BitConverter.ToUInt32(EntryTablesBuffer, iTablesOffBits);
          uint FileLen = BitConverter.ToUInt32(EntryTablesBuffer, iTablesOffBits + sizeof(uint) * 2 + sizeof(UInt16) * 2);
          byte[] temp = new byte[EntryLen - FileLen];
          Array.Copy(EntryTablesBuffer, iTablesOffBits, temp, 0, EntryLen - FileLen);
          Entry _entry = new Entry(temp);
          byte[] EntryFileBuffer = new byte[_entry.FileLen];
          Array.Copy(FilesBuffer, iFilesOffBits, EntryFileBuffer, 0, _entry.FileLen);

          string FileName = Path.Combine(SaveDir , BitConvertHelper.Byte2String(_entry.EntryDir, 0, _entry.EntryDirLen));
          if (!Directory.Exists(FileName)) Directory.CreateDirectory(FileName);
          FileName = Path.Combine(FileName , BitConvertHelper.Byte2String(_entry.EntryName, 0, _entry.EntryNameLen)); 
          FileStream fsSave = new FileStream(FileName,FileMode.Create);

          Lz77Compression lz77 = new Lz77Compression();
          byte[] btyFile = lz77.DeCompress(EntryFileBuffer);
          fsSave.Write(btyFile, 0, btyFile.Length);
          //fsSave.Write(EntryFileBuffer, 0, EntryFileBuffer.Length);

          fsSave.Close();

          iTablesOffBits += (int)(EntryLen - FileLen);
          iFilesOffBits += (int)_entry.FileLen;
          iCount++;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("解压缩文件错误！", ex);
      }
    }

    /// <summary>
    /// 解压指定目录的指定文件
    /// </summary>
    /// <param name="Dir">Zip目录</param>
    /// <param name="FileName">Zip条目</param>
    /// <param name="SaveDir">存储目录</param>
    public void UnZip(string Dir, string FileName, string SaveDir)
    {
    }

    /// <summary>
    /// 添加一个条目
    /// </summary>
    /// <param name="entry"></param>
    public void Add(Entry entry)
    {
        //检查重复条目

        //重新组织buff
        byte[] tempBuf = new byte[Length + entry.Length];
        //原文件头和条目表
        Array.Copy(buff, 0, tempBuf, 0, this.HeadLen + this.EntrysTableLen);
        //原文件流
        Array.Copy(buff, this.HeadLen + this.EntrysTableLen, tempBuf, this.HeadLen + this.EntrysTableLen + entry.Length - entry.FileLen, Length - this.HeadLen - this.EntrysTableLen);
        //copy 条目文件流到最后
        entry.FileBuffer.CopyTo(tempBuf, Length + entry.Length - entry.FileLen);
        //copy 条目表最后
        entry.Offset = (uint)(Length - HeadLen - EntrysTableLen);
        entry.Buffer.CopyTo(tempBuf, HeadLen + EntrysTableLen);

        Length += entry.Length;
        EntryCount++;
        EntrysTableLen += entry.Length - entry.FileLen;

        BitConverter.GetBytes(Length).CopyTo(tempBuf, sizeof(uint));
        BitConverter.GetBytes(EntryCount).CopyTo(tempBuf, sizeof(uint) * 3);
        BitConverter.GetBytes(EntrysTableLen).CopyTo(tempBuf, sizeof(uint) * 4);
        buff = tempBuf;
    }

    /// <summary>
    /// 删除一个条目
    /// </summary>
    /// <param name="entry"></param>
    public void Delete(Entry entry)
    {
    }
  }

  /// <summary>
  /// 压缩条目结构
  /// </summary>
  public class Entry
  {
    #region 结构顺序
    /// <summary>
    /// Entry实例的总长度
    /// </summary>
    public uint Length = 4 * 3 + 2 * 2;
    /// <summary>
    /// 条目名称长度
    /// </summary>
    public UInt16 EntryNameLen;
    /// <summary>
    /// 相对文件夹长度
    /// </summary>
    public UInt16 EntryDirLen;

    private uint offset=0;
    /// <summary>
    /// 起始字节位置。压缩体除掉文件头长度、条目表长度之后的字节，从0开始
    /// </summary>
    public uint Offset
    {
      get
      {
        return offset;
      }
      set
      {
        offset = value;
        //更改对象Buffer的对应字节
        BitConverter.GetBytes(offset).CopyTo(buff, 8);
      }
    }

    /// <summary>
    /// 文件长度
    /// </summary>
    public uint FileLen;
    /// <summary>
    /// 条目名称
    /// </summary>
    public byte[] EntryName;
    /// <summary>
    /// 条目相对文件夹位置
    /// </summary>
    public byte[] EntryDir;

    private byte[] buff;
    /// <summary>
    /// 本条目的字节流，不包括文件字节流
    /// </summary>
    public byte[] Buffer
    {
      get
      {
        return buff;
      }
    }

    private byte[] filebuff;
    /// <summary>
    /// 文件字节流
    /// </summary>
    public byte[] FileBuffer
    {
      get
      {
        return filebuff;
      }
    }
    #endregion
    
    /// <summary>
    /// 使用指定的字节流构造一个条目，不包含文件流
    /// </summary>
    /// <param name="Buf"></param>
    public Entry(byte[] Buf)
    {
      try
      {
        buff = Buf;
        Length = BitConverter.ToUInt32(buff, 0);
        EntryNameLen = BitConverter.ToUInt16(buff, sizeof(uint));
        EntryDirLen = BitConverter.ToUInt16(buff, sizeof(uint) + sizeof(UInt16));
        offset = BitConverter.ToUInt32(buff, sizeof(uint) + sizeof(UInt16) * 2);
        FileLen = BitConverter.ToUInt32(buff, sizeof(uint) * 2 + sizeof(UInt16) * 2);
        EntryName = new byte[EntryNameLen];
        Array.Copy(buff, sizeof(uint) * 3 + sizeof(UInt16) * 2, EntryName, 0, EntryNameLen);
        EntryDir = new byte[EntryDirLen];
        Array.Copy(buff, sizeof(uint) * 3 + sizeof(UInt16) * 2 + EntryNameLen, EntryDir, 0, EntryDirLen);
      }
      catch (Exception ex)
      {
        throw new Exception("构造条目错误！", ex);
      }
    }

    /// <summary>
    /// 通过指定文件构造一个条目
    /// </summary>
    /// <param name="FileName">需要压缩的文件</param>
    /// <param name="ZipInnerDir">压缩包里的路径</param>
    public Entry(string FileName, string ZipInnerDir)
    {
      System.IO.FileStream fs = null;
      try
      {
        fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
      }
      catch (FileNotFoundException exFnf)
      {
        if(fs!=null)fs.Close();
        throw exFnf;
      }
      byte[] file = new byte[(int)fs.Length];
      fs.Read(file, 0, (int)fs.Length);
      fs.Close();
      this.filebuff = new Lz77Compression().Compress(file);
      FileLen = (uint)filebuff.Length;
      Length += FileLen;

      string sFileName = Path.GetFileName(FileName);
      EntryName = BitConvertHelper.String2Byte(sFileName);
      EntryNameLen = (ushort)EntryName.Length;
      Length += EntryNameLen;
      EntryDir = BitConvertHelper.String2Byte(ZipInnerDir);
      EntryDirLen = (ushort)EntryDir.Length;
      Length += EntryDirLen;

      //Buff处理，不包括文件
      byte[] temp = new byte[Length - FileLen];
      BitConverter.GetBytes(Length).CopyTo(temp, 0);
      BitConverter.GetBytes(EntryNameLen).CopyTo(temp, 4);
      BitConverter.GetBytes(EntryDirLen).CopyTo(temp, 6);
      BitConverter.GetBytes(offset).CopyTo(temp, 8);
      BitConverter.GetBytes(FileLen).CopyTo(temp, 12);
      EntryName.CopyTo(temp, 16);
      EntryDir.CopyTo(temp, 16 + EntryName.Length);
      buff = temp;
    }
  }
  
  public static class BitConvertHelper
  {
    private static Encoding ec = Encoding.GetEncoding("gb2312");
    /// <summary>
    /// 将字符串转换成二进制数组
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] String2Byte(string Str)
    {
      byte[] bytRet = ec.GetBytes(Str);
      return bytRet;
    }

    /// <summary>
    /// 将二进制数组转换成字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Byte2String(byte[] b, int index, int count)
    {
      string sRet = ec.GetString(b, index, count).Trim("\0".ToCharArray());
      return sRet;
    }

    /// <summary>
    /// 将二进制数组转换成Uint数字
    /// </summary>
    /// <param name="b"></param>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static uint Byte2Uint(byte[] b, int index, int count)
    {
      byte[] Temp = new byte[count];
      for (int i = 0; i < count; i++)
      {
        Temp[i] = b[i + index];
      }
      return BitConverter.ToUInt32(Temp, 0);
    }

    /// <summary>
    /// 将二进制数组转换成Int数字
    /// </summary>
    /// <param name="b"></param>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static int Byte2Int(byte[] b, int index, int count)
    {
      byte[] Temp = new byte[count];
      for (int i = 0; i < count; i++)
      {
        Temp[i] = b[i + index];
      }
      return BitConverter.ToInt32(Temp, 0);
    }

    /// <summary>
    /// 将图片转换成二进制数组
    /// </summary>
    /// <param name="ImgFile"></param>
    /// <returns></returns>
    public static byte[] Img2Byte(string ImgFile)
    {
      FileStream _fs = new FileStream(ImgFile, FileMode.Open, FileAccess.Read);
      byte[] b = new byte[100];
      int i = 0;
      System.Collections.ArrayList buffer = new System.Collections.ArrayList();
      while (true)
      {
        int Readed = 0;
        if ((Readed = _fs.Read(b, 0, 100)) == 0) break;
        i++;
        for (int j = 0; j < Readed; j++)
        {
          buffer.Add(b[j]);
        }
      }
      b = new byte[buffer.Count];
      for (int j = 0; j < buffer.Count; j++)
      {
        b[j] = (byte)buffer[j];
      }
      _fs.Close();
      _fs.Dispose();
      return b;
    }
  }

  /// <summary>
  /// COM+组件封装
  /// </summary>
  [Transaction(TransactionOption.Disabled)]
  public class ZipEntityComPlus:ServicedComponent
  {
    public ZipEntityComPlus()
    {
      try { }
      catch (Exception e) { throw e; }
    }
    private ZipEntity zip;

    private string filename = "";
    public string FileName
    {
      get { return filename; }
      set { filename = value; }
    }

    public string Test()
    {
      return "Com Test!!";
    }


    public void LoadFile()
    {
      this.zip = new ZipEntity(filename);
    }

    public void LoadFile(string ZipFile)
    {
      FileName = ZipFile;
      LoadFile();
    }

    public void AddFile(string name)
    {
      zip.Add(new Entry(name,""));
    }

    public void AddFile(string name, string dir)
    {
      zip.Add(new Entry(name, dir));
    }

    public void Save()
    {
      zip.Save();
    }

    public void Save(string file)
    {
      zip.Save(file);
    }

    public void Close()
    {
      zip.Close();
    }

    public void UnZipAll(string SaveDir)
    {
      zip.UnZipAll(SaveDir);
    }
  }
}