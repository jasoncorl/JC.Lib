using System;
using System.Security.Cryptography;


namespace JC.Lib.IO.Text
{
  /// <summary>
  /// 生成随机字符串，选择自 ArLi2003 的 Blog 
  /// </summary>
  public class RandomStr
  {

    public const string myVersion = "1.2";

    /********
    *  Const and Function
    *  ********/

    private static readonly int defaultLength = 8;

    private static int GetNewSeed()
    {
      byte[] rndBytes = new byte[4];
      RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
      rng.GetBytes(rndBytes);
      return BitConverter.ToInt32(rndBytes, 0);
    }

    /********
     *  getRndCode of all char .
     *  ********/

    private static string BuildRndCodeAll(int strLen)
    {
      System.Random RandomObj = new System.Random(GetNewSeed());
      string buildRndCodeReturn = null;
      for (int i = 0; i < strLen; i++)
      {
        buildRndCodeReturn += (char)RandomObj.Next(33, 125);
      }
      return buildRndCodeReturn;
    }

    /// <summary>
    /// 生成所有字符的随机串，长度默认8
    /// </summary>
    /// <returns></returns>
    public static string GetRndStrOfAll()
    {
      return BuildRndCodeAll(defaultLength);
    }
    
    /// <summary>
    /// 生成所有字符的随机串，长度指定
    /// </summary>
    /// <param name="LenOf">指定长度</param>
    /// <returns></returns>
    public static string GetRndStrOfAll(int LenOf)
    {
      return BuildRndCodeAll(LenOf);
    }

    /********
     *  getRndCode of only .
     *  ********/

    private static string sCharLow = "abcdefghijklmnopqrstuvwxyz";
    private static string sCharUpp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string sNumber = "0123456789";

    public static string BuildRndCodeOnly(string StrOf, int strLen)
    {
      System.Random RandomObj = new System.Random(GetNewSeed());
      string buildRndCodeReturn = null;
      for (int i = 0; i < strLen; i++)
      {
        buildRndCodeReturn += StrOf.Substring(RandomObj.Next(0, StrOf.Length - 1), 1);
      }
      return buildRndCodeReturn;
    }

    /// <summary>
    /// 生成大小写字母的随机串
    /// </summary>
    /// <returns></returns>
    public static string GetRndStrOnlyFor()
    {
      return BuildRndCodeOnly(sCharLow + sNumber, defaultLength);
    }

    /// <summary>
    /// 生成指定长度大小写字符串的随机码
    /// </summary>
    /// <param name="LenOf"></param>
    /// <returns></returns>
    public static string GetRndStrOnlyFor(int LenOf)
    {
      return BuildRndCodeOnly(sCharLow + sNumber, LenOf);
    }

    /// <summary>
    /// 生成可选择大写与数字的字符串随机码
    /// </summary>
    /// <param name="bUseUpper">大写是否可用</param>
    /// <param name="bUseNumber">数字是否可用</param>
    /// <returns></returns>
    public static string GetRndStrOnlyFor(bool bUseUpper, bool bUseNumber)
    {
      string strTmp = sCharLow;
      if (bUseUpper) strTmp += sCharUpp;
      if (bUseNumber) strTmp += sNumber;

      return BuildRndCodeOnly(strTmp, defaultLength);
    }

    /// <summary>
    /// 生成指定长度可选大写与数字的字符串随机码
    /// </summary>
    /// <param name="LenOf">指定长度</param>
    /// <param name="bUseUpper">大写是否可用</param>
    /// <param name="bUseNumber">数字是否可用</param>
    /// <returns></returns>
    public static string GetRndStrOnlyFor(int LenOf, bool bUseUpper, bool bUseNumber)
    {
      string strTmp = sCharLow;
      if (bUseUpper) strTmp += sCharUpp;
      if (bUseNumber) strTmp += sNumber;

      return BuildRndCodeOnly(strTmp, LenOf);
    }
  }
}
