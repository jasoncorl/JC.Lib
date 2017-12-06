using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Cryptography;
using System.IO;

namespace JC.Lib
{
  /// <summary>
  /// 字符串操作，By J.C.
  /// </summary>
  public class StringHelper
  {
    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="p"></param>
    /// <returns>返回加密后的字符串</returns>
    public static string Encryption(string p)
    {
      ////当总byte数n>1 并且 n* 8/6有余数时，解密会有问题，所以在解密时必须冗余处理最后一位
      int a = 0, s = 0;
      int d = p.Length;
      int g = -1;
      string f = "";
      int h = 0;
      byte[] b = Encoding.UTF8.GetBytes(p);//数字字母1byte，中文3byte
      for (int i = 0; i < b.Length; i++)
      {
        int l = (int)b[i];
        s = (s << 8) + l;
        a += 8;

        //每6位处理一次
        while (a >= 6)
        {
          int k = s >> (a - 6);
          //k附加处理，以下过程反向
          //k = (k == 95) ? 63 : ((k == 44) ? 62 : ((k >= 97) ? (k - 61) : ((k >= 65) ? (k - 55) : (k - 48))));
          s = s - (k << (a - 6));
          a -= 6;
          k = (k == 63) ? 95 : ((k == 62) ? 44 : ((k >= 36) ? (k + 61) : ((k >= 10) ? (k + 55) : (k + 48))));
          f += (char)k;
        }
      }
      if (a != 0) f += (char)(s >> (a - 6));
      Console.Write("en:" + a);

      return f;
    }

    /// <summary>
    /// 解密经过加密的字符串，By Jason
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static string Decryption(string p)
    {
      int a = 0, s = 0;
      int d = p.Length;
      int g = -1;
      string f = "";
      int h = 0;
      for (int j = 0; j < d; j++)
      {
        int k = (int)p[j];
        k = (k == 95) ? 63 : ((k == 44) ? 62 : ((k >= 97) ? (k - 61) : ((k >= 65) ? (k - 55) : (k - 48))));
        s = (s << 6) + k;

        a += 6; 
        while (a >= 8)
        {
          int l = s >> (a - 8);
          if (h > 0)
          {
            g = (g << 6) + (l & (0x3f));
            h--;
            if (h == 0) { f += (char)g; };
          }
          else
          {
            if (l >= 224)
            {
              g = l & (0xf); h = 2;
            }
            else if (l >= 128)
            {
              g = l & (0x1f);
              h = 1;
            }
            else
            {
              f += (char)l;
            };
          };
          s = s - (l << (a - 8));
          a -= 8;
        };
      };
      //if (a != 0) f += (char)(s >> (a - 8));
      //Console.Write("de:" + a);
      return f;
    }

    #region DES加密方式,分块加密解密会有问题

    //默认密钥向量
    //private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    private static byte[] Keys = { 12, 24, 48, 36, 48, 60, 72, 84 }; 
    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="encryptKey">加密密钥,要求为8位</param>
    /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
    public static string EncryptDES(string encryptString, string encryptKey)
    {
      try
      {
        byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
        byte[] rgbIV = Keys;
        byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return Convert.ToBase64String(mStream.ToArray());
        //FileStream fs = new FileStream("c:\\t.bin",FileMode.CreateNew, FileAccess.ReadWrite);
        //fs.Write(mStream.ToArray(), 0, mStream.ToArray().Length);
        //fs.Close();
        //return "OK";
      }
      catch
      {
        return encryptString;
      }
    }

    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的字符串</param>
    /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
    public static string DecryptDES(string decryptString, string decryptKey)
    {
      try
      {
        byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
        byte[] rgbIV = Keys;
        byte[] inputByteArray = Convert.FromBase64String(decryptString);
        //FileStream fs = new FileStream("c:\\t.bin", FileMode.Open, FileAccess.Read);
        //byte[] inputByteArray = new byte[fs.Length];
        //fs.Read(inputByteArray, 0, (int)fs.Length);
        //fs.Close();

        DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return Encoding.UTF8.GetString(mStream.ToArray());
      }
      catch
      {
        return decryptString;
      }
    }
    #endregion

    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="codeName">加密采用的编码方式</param>
    /// <param name="source">待加密的明文</param>
    /// <returns></returns>
    public static string EncodeBase64(Encoding encode, string source)
    {

      byte[] bytes = encode.GetBytes(source);
      try
      {
        return Convert.ToBase64String(bytes);
      }
      catch
      {
        return source;
      }
    }

    /// <summary>
    /// Base64加密，采用utf8编码方式加密
    /// </summary>
    /// <param name="source">待加密的明文</param>
    /// <returns>加密后的字符串</returns>
    public static string EncodeBase64(string source)
    {
      return EncodeBase64(Encoding.UTF8, source);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string DecodeBase64(Encoding encode, string result)
    {
      string decode = "";
      byte[] bytes = Convert.FromBase64String(result);
      try
      {
        decode = encode.GetString(bytes);
      }
      catch
      {
        decode = result;
      }
      return decode;
    }

    /// <summary>
    /// Base64解密，采用utf8编码方式解密
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string DecodeBase64(string result)
    {
      return DecodeBase64(Encoding.UTF8, result);
    }
  }
}
