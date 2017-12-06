using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;

namespace JC.Lib.IO.Text
{
  public class Text
  {
    public enum GetWordFilter
    {
      OnlyCh,
      OnlyEn,
      All,
    }

    /// <summary>
    /// 返回文本文件的字数，行数
    /// </summary>
    /// <param name="TxtFile"></param>
    /// <param name="Filter">字数计算方式</param>
    /// <param name="Wc">输出字数</param>
    /// <param name="Rc">输出行数</param>
    public static void GetTxtWordCount(string TxtFile, GetWordFilter Filter, out int Wc, out int Rc )
    {
      Int32 iRowCount = 0;
      Int32 iWC = 0;
      TextReader _tr = new StreamReader(TxtFile, JC.Lib.IO.Text.Text.GetEncoding(TxtFile));
      string sFileContent = "";// _tr.ReadToEnd();

      bool bReadEnd = false;
      while (!bReadEnd)
      {
        string sTemp = _tr.ReadLine();
        if (sTemp == null)
        {
          bReadEnd = true;
          break;
        }
        sFileContent += sTemp;
        iRowCount += 1;
      }
      _tr.Close();
      //某些特殊字符处理
      Regex _rg;
      MatchCollection _mathccoll;
      //汉字字数
      if (Filter == GetWordFilter.All || Filter == GetWordFilter.OnlyCh)
      {
        _rg = new Regex(@"([\u4e00-\u9fa5])");
        _mathccoll = _rg.Matches(sFileContent);
        iWC += _mathccoll.Count;
      }
      //英文单词，数字串
      if (Filter == GetWordFilter.All || Filter == GetWordFilter.OnlyEn)
      {
        _rg = new Regex("([a-zA-Z0-9]+)");
        _mathccoll = _rg.Matches(sFileContent);
        iWC += _mathccoll.Count;
      }
      Wc = iWC;
      Rc = iRowCount;
    }

    /// <summary>
    /// 取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// 
    public static Encoding GetEncoding(string fileName)
    {
      FileStream fs = new FileStream(fileName, FileMode.Open);
      Encoding targetEncoding = GetEncoding(fs);
      fs.Close();
      return targetEncoding;
    }

    /// <summary>
    /// 取得一个文本文件流的编码方式。
    /// </summary>
    /// <param name="stream">文本文件流。</param>
    /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
    /// <returns></returns>
    public static Encoding GetEncoding(FileStream stream)
    {
      Encoding targetEncoding = Encoding.Default;
      if (stream != null && stream.Length >= 2)
      {
        //保存文件流的前4个字节
        byte byte1 = 0;
        byte byte2 = 0;
        byte byte3 = 0;
        byte byte4 = 0;

        //保存当前Seek位置
        long origPos = stream.Seek(0, SeekOrigin.Begin);
        stream.Seek(0, SeekOrigin.Begin);
        int nByte = stream.ReadByte();
        byte1 = Convert.ToByte(nByte);
        byte2 = Convert.ToByte(stream.ReadByte());

        if (stream.Length >= 3)
        {
          byte3 = Convert.ToByte(stream.ReadByte());
        }

        if (stream.Length >= 4)
        {
          byte4 = Convert.ToByte(stream.ReadByte());
        }
        //根据文件流的前4个字节判断Encoding
        //Unicode {0xFF, 0xFE};
        //BE-Unicode {0xFE, 0xFF};
        //UTF8 = {0xEF, 0xBB, 0xBF};
        if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
        {
          targetEncoding = Encoding.BigEndianUnicode;
        }

        if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
        {
          targetEncoding = Encoding.Unicode;
        }

        if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
        {
          targetEncoding = Encoding.UTF8;
        }

        //恢复Seek位置
        stream.Seek(origPos, SeekOrigin.Begin);
      }
      return targetEncoding;
    }
  }
}
