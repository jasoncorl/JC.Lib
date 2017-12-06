using System;
using System.Security.Cryptography;


namespace JC.Lib.IO.Text
{
  /// <summary>
  /// ��������ַ�����ѡ���� ArLi2003 �� Blog 
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
    /// ���������ַ��������������Ĭ��8
    /// </summary>
    /// <returns></returns>
    public static string GetRndStrOfAll()
    {
      return BuildRndCodeAll(defaultLength);
    }
    
    /// <summary>
    /// ���������ַ��������������ָ��
    /// </summary>
    /// <param name="LenOf">ָ������</param>
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
    /// ���ɴ�Сд��ĸ�������
    /// </summary>
    /// <returns></returns>
    public static string GetRndStrOnlyFor()
    {
      return BuildRndCodeOnly(sCharLow + sNumber, defaultLength);
    }

    /// <summary>
    /// ����ָ�����ȴ�Сд�ַ����������
    /// </summary>
    /// <param name="LenOf"></param>
    /// <returns></returns>
    public static string GetRndStrOnlyFor(int LenOf)
    {
      return BuildRndCodeOnly(sCharLow + sNumber, LenOf);
    }

    /// <summary>
    /// ���ɿ�ѡ���д�����ֵ��ַ��������
    /// </summary>
    /// <param name="bUseUpper">��д�Ƿ����</param>
    /// <param name="bUseNumber">�����Ƿ����</param>
    /// <returns></returns>
    public static string GetRndStrOnlyFor(bool bUseUpper, bool bUseNumber)
    {
      string strTmp = sCharLow;
      if (bUseUpper) strTmp += sCharUpp;
      if (bUseNumber) strTmp += sNumber;

      return BuildRndCodeOnly(strTmp, defaultLength);
    }

    /// <summary>
    /// ����ָ�����ȿ�ѡ��д�����ֵ��ַ��������
    /// </summary>
    /// <param name="LenOf">ָ������</param>
    /// <param name="bUseUpper">��д�Ƿ����</param>
    /// <param name="bUseNumber">�����Ƿ����</param>
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
