using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JC.Lib.IO
{
  /// <summary>
  /// Log 的摘要说明
  /// </summary>
  public static class Log
  {
    public static StreamWriter OpenFile(string strFileFullPath)
    {
      FileInfo f = new FileInfo(strFileFullPath);

      if (!Directory.Exists(f.Directory.ToString()))
      {
        Directory.CreateDirectory(f.Directory.ToString());
      }

      if (!File.Exists(strFileFullPath))
      {
        return File.CreateText(strFileFullPath);
      }
      else
      {
        return new StreamWriter(strFileFullPath, true);
      }
    }

    public static void WriteLog(string logText)
    {
      try
      {
        string sLogFileFullPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"AppLog\";
        if (!Directory.Exists(sLogFileFullPath))
        {
          Directory.CreateDirectory(sLogFileFullPath);
        }
        sLogFileFullPath += DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        StreamWriter _sw = OpenFile(sLogFileFullPath);
        _sw.WriteLine(logText);
        _sw.Close();
      }
      catch
      {
        ;
      }
    }

    /// <summary>
    /// 写文本日志
    /// </summary>
    /// <param name="TxtFile">指定日志文本文件</param>
    /// <param name="logText">内容</param>
    public static void WriteLog(string TxtFile, string logText)
    {
      try
      {
        StreamWriter _sw = OpenFile(TxtFile);
        _sw.WriteLine(logText);
        _sw.Close();
      }
      catch
      {
        ;
      }
    }

    public static void AlertMsg2Moblie(string strMsg)
    {

    }
  }
}
