using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Lib
{
  public static class IPReader
  {
    private static object lockReader = new object();
    private static string ipfilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"QQWry.dat";
    private static QQwryIPReader reader = new QQwryIPReader(ipfilePath);
    public static IPLocation GetIPLocation(string ip)
    {
      lock (lockReader)
      {
        return reader.GetIPLocation(ip);
      }
    }
  }

  ///<summary>
  /// 地理位置,包括国家和地区、其他备注信息
  ///</summary>
  public struct IPLocation
  {
    /// <summary>
    /// 地区
    /// </summary>
    public string AreaRegion { get; set; }
    /// <summary>
    /// 其他备注信息，如电信运营商
    /// </summary>
    public string Remark { get; set; }
  }

  /// <summary>
  /// 来自网络的IP库阅读器
  /// </summary>
  public class QQwryIPReader
  {
    private byte[] fileBuffer;
    private long offsetRead = 0;  //读取偏移，相当于FileStream.Position
    //FileStream ipFile;
    long ip;
    //string ipfilePath;

    ///<summary>
    /// 构造函数
    ///</summary>
    ///<param name="ipfilePath">纯真IP数据库路径</param>
    public QQwryIPReader(string ipfilePath)
    {
      //this.ipfilePath = ipfilePath;
      FileStream fs = new FileStream(ipfilePath, FileMode.Open, FileAccess.Read);
      this.fileBuffer = new byte[fs.Length];
      fs.Read(fileBuffer, 0, (int)fs.Length);
      fs.Close();
      fs.Dispose();
    }

    ///<summary>
    /// 获取指定IP所在地理位置
    ///</summary>
    ///<param name="strIP">要查询的IP地址</param>
    ///<returns></returns>
    public IPLocation GetIPLocation(string strIP)
    {
      offsetRead = 0;
      ip = IPToLong(strIP);
      long[] ipArray = BlockToArray(ReadIPBlock());
      long offset = SearchIP(ipArray, 0, ipArray.Length - 1) * 7 + 4;
      offsetRead += offset;//跳过起始IP
      offsetRead = ReadLongX(3) + 4;//跳过结束IP

      IPLocation loc = new IPLocation();

      int flag = fileBuffer[offsetRead];//读取标志
      offsetRead++;

      if (flag == 1)//表示国家和地区被转向
      {
        offsetRead = ReadLongX(3);
        flag = fileBuffer[offsetRead];//读取标志
        offsetRead++;
      }
      long countryOffset = offsetRead;
      loc.AreaRegion = ReadString(flag);

      if (flag == 2)
      {
        offsetRead = countryOffset + 3;
      }
      flag = fileBuffer[offsetRead];//读取标志
      offsetRead++;

      loc.Remark = ReadString(flag);
      return loc;
    }

    ///<summary>
    /// 将字符串形式的IP转换位long
    ///</summary>
    ///<param name="strIP"></param>
    ///<returns></returns>
    public long IPToLong(string strIP)
    {
      byte[] ip_bytes = new byte[8];
      string[] strArr = strIP.Split(new char[] { '.' });
      for (int i = 0; i < 4; i++)
      {
        ip_bytes[i] = byte.Parse(strArr[3 - i]);
      }
      return BitConverter.ToInt64(ip_bytes, 0);
    }
    ///<summary>
    /// 将索引区字节块中的起始IP转换成Long数组
    ///</summary>
    ///<param name="ipBlock"></param>
    long[] BlockToArray(byte[] ipBlock)
    {
      long[] ipArray = new long[ipBlock.Length / 7];
      int ipIndex = 0;
      byte[] temp = new byte[8];
      for (int i = 0; i < ipBlock.Length; i += 7)
      {
        Array.Copy(ipBlock, i, temp, 0, 4);
        ipArray[ipIndex] = BitConverter.ToInt64(temp, 0);
        ipIndex++;
      }
      return ipArray;
    }

    ///<summary>
    /// 从IP数组中搜索指定IP并返回其索引
    ///</summary>
    ///<param name="ipArray">IP数组</param>
    ///<param name="start">指定搜索的起始位置</param>
    ///<param name="end">指定搜索的结束位置</param>
    ///<returns></returns>
    int SearchIP(long[] ipArray, int start, int end)
    {
      int middle = (start + end) / 2;
      if (middle == start)
        return middle;
      else if (ip < ipArray[middle])
        return SearchIP(ipArray, start, middle);
      else
        return SearchIP(ipArray, middle, end);
    }

    ///<summary>
    /// 读取IP文件中索引区块
    ///</summary>
    ///<returns></returns>
    byte[] ReadIPBlock()
    {
      long startPosition = ReadLongX(4);
      long endPosition = ReadLongX(4);
      long count = (endPosition - startPosition) / 7 + 1;//总记录数
      offsetRead = startPosition;
      byte[] ipBlock = new byte[count * 7];
      Array.Copy(fileBuffer, offsetRead, ipBlock, 0, ipBlock.Length);
      offsetRead = startPosition;

      return ipBlock;
    }

    ///<summary>
    /// 从IP文件中读取指定字节并转换位long
    ///</summary>
    ///<param name="bytesCount">需要转换的字节数，主意不要超过8字节</param>
    ///<returns></returns>
    long ReadLongX(int bytesCount)
    {
      byte[] _bytes = new byte[8];
      Array.Copy(fileBuffer, offsetRead, _bytes, 0, bytesCount);
      offsetRead += bytesCount;

      return BitConverter.ToInt64(_bytes, 0);
    }

    ///<summary>
    /// 从IP文件中读取字符串
    ///</summary>
    ///<param name="flag">转向标志</param>
    ///<returns></returns>
    string ReadString(int flag)
    {
      if (flag == 1 || flag == 2)//转向标志
        offsetRead = ReadLongX(3);
      else
        offsetRead -= 1;

      List<byte> list = new List<byte>();
      byte b = fileBuffer[offsetRead];
      offsetRead++;

      while (b > 0)
      {
        list.Add(b);
        if (offsetRead >= fileBuffer.Length)
        {
          break;
        }
        b = fileBuffer[offsetRead];
        offsetRead++;
      }
      return Encoding.Default.GetString(list.ToArray());
    }
  }
}
