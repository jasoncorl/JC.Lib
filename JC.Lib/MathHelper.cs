using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Lib
{
  /// <summary>
  /// 特殊运算处理 Write by J.C. on 2008-08-16
  /// </summary>
  public static class MathHelper
  {
    /// <summary>
    /// 10进制到36进制(0-9,A-Z)的转换
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string Get10to36(int input)
    {
      string sRet = "";
      if (input >= 0)
      {
        while (input > 35)
        {
          int remainder = input % 36;
          input = input / 36;
          if (remainder < 10)
          {
            sRet = (char)(remainder + (int)'0') + sRet;
          }
          else
          {
            sRet = (char)(remainder - 10 + (int)'A') + sRet;
          }
        }
        if (input < 10)
        {
          sRet = (char)(input + (int)'0') + sRet;
        }
        else
        {
          sRet = (char)(input - 10 + (int)'A') + sRet;
        }
      }
      else
      {
        sRet = "-" + Get10to36(Math.Abs(input));
      }
      return sRet;
    }

    /// <summary>
    /// 36进制(0-9,A-Z)到10进制的转换
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int Get36to10(string input)
    {
      input = input.ToUpper();
      int iRet = 0;
      for (int i = input.Length - 1; i >= 0; i--)
      {
        int iTemp = (int)input[i];
        if (iTemp <= 57)
        {
          iRet += (iTemp - (int)'0') * (int)System.Math.Pow(36, input.Length - 1 - i);
        }
        else
        {
          iRet += (iTemp + 10 - (int)'A') * (int)System.Math.Pow(36, input.Length - 1 - i);
        }
      }
      return iRet;
    }

    /// <summary>
    /// JAVA的hashcode
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int JavaHashCode(String str)
    {
      if (str == null)
      {
        return 0;
      }
      int h = 0;
      char[] val = str.ToCharArray();
      for (int i = 0; i < str.Length; i++)
      {
        h = 31 * h + val[i];
      }
      return h;
    }

    /// <summary>
    /// 对整形数组冒泡顺序排序
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static int[] PopAsc(int[] a)
    {
      int j = a.Length - 1;
      while (j >= 0)
      {
        for (int i = a.Length - 1; i > 0; i--)
        {
          if (a[i] < a[i - 1])
          {
            int temp = a[i - 1];
            a[i - 1] = a[i];
            a[i] = temp;
          }
        }
        j--;
      }
      return a;
    }
  }
}
