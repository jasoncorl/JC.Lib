using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.IO;

using JC.Lib.IO.Text;

namespace JC.Lib.Web
{
  /// <summary>
  /// 从指定URL下载文件
  /// </summary>
  public class FileDown
  {
    private static string sErrMsg = "";

    /// <summary>
    /// 最近一条的错误信息
    /// </summary>
    public static string ErrMsg { get { return sErrMsg; } }
    private static string MyErrMsg { set { sErrMsg = value; } }
    /// <summary>
    /// 指定本地全路径下载
    /// </summary>
    /// <param name="strSource"></param>
    /// <param name="strLocalPath"></param>
    /// <returns>下载成功返回true，否则返回false</returns>
    public static Boolean DownFile(string strSource, string strLocalPath)
    {
      try
      {
        Uri u = new Uri(strSource);
        HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
        mRequest.Timeout = 300000;
        mRequest.Method = "GET";
        mRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();
        Stream sIn = wr.GetResponseStream();
        FileStream fs = new FileStream(strLocalPath, FileMode.Create, FileAccess.Write);
        long length = wr.ContentLength;
        int i = 0;
        long j = 0;
        byte[] buffer = new byte[1024];
        while ((i = sIn.Read(buffer, 0, buffer.Length)) > 0)
        {
          j += i;
          fs.Write(buffer, 0, i);
        }

        sIn.Close();
        wr.Close();
        fs.Close();
        return true;
      }
      catch(Exception dfEx){
        MyErrMsg = dfEx.Message;
        return false;
      }
    }

    /// <summary>
    /// 下载文件到指定文件夹
    /// </summary>
    /// <param name="strSource">文件URL</param>
    /// <param name="strLocalFolder">本地文件夹</param>
    /// <param name="blnAutoName">是否自动命名</param>
    /// <param name="strPlusName">如果不是自动命名，本参数附在源文件名之后</param>
    /// <returns>下载成功返回true，否则返回false</returns>
    public static Boolean DownFile(string strSource, string strLocalFolder, Boolean blnAutoName, string strPlusName)
    {
      try
      {
        Uri u = new Uri(strSource);
        HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
        mRequest.Timeout = 300000;
        mRequest.Method = "GET";
        mRequest.ContentType = "application/x-www-form-urlencoded";
        mRequest.KeepAlive = false;
        HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();

        Stream sIn = wr.GetResponseStream();
        //得到源文件名称
        string sFileName = "";
        string[] arrTemp = strSource.Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        sFileName = arrTemp[arrTemp.Length - 1];
        if (!blnAutoName)
        {
          arrTemp = sFileName.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries);
          sFileName = string.Join(strPlusName + ".", arrTemp);
        }

        FileStream fs = new FileStream(strLocalFolder + sFileName, FileMode.Create, FileAccess.Write);
        long length = wr.ContentLength;
        int i = 0;
        long j = 0;
        byte[] buffer = new byte[1024];
        while ((i = sIn.Read(buffer, 0, buffer.Length)) > 0)
        {
          j += i;
          fs.Write(buffer, 0, i);
        }

        sIn.Close();
        wr.Close();
        fs.Close();
        return true;
      }
      catch (Exception exDF)
      {
        MyErrMsg = exDF.Message;
        return false;
        //throw exDF;
      }
    }

    /// <summary>
    /// 下载文件到指定文件夹
    /// </summary>
    /// <param name="strSourceMainUrl">源资源的主地址</param>
    /// <param name="strSourceSiteUrl">相对于主地址的站点目录地址，如果主地址已经表达完整则为空</param>
    /// <param name="strLocalFolder">本地下载主目录</param>
    /// <param name="intFileNameType">文件命名方式,1保持原文件名，2原文件名之后附加文件名，3随机重命名</param>
    /// <param name="strPlusName">文件命名方式为2时，本参数附在源文件名之后</param>
    /// <param name="blnKeepDirTree">是否保持同源资源目录结构</param>
    /// <param name="strReturnFileName">返回文件命名</param>
    /// <returns>下载成功返回true，否则返回false</returns>
    public static Boolean DownFile(string strSourceMainUrl, string strSourceSiteUrl, string strLocalFolder, int intFileNameType, string strPlusName, Boolean blnKeepDirTree, out string strReturnFileName)
    {
      try
      {
        string strSource = strSourceMainUrl + strSourceSiteUrl;
        Uri u = new Uri(strSource);
        HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
        mRequest.Timeout = 300000;
        mRequest.Method = "GET";
        mRequest.ContentType = "application/x-www-form-urlencoded";
        mRequest.KeepAlive = false;
        HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();

        Stream sIn = wr.GetResponseStream();

        //得到源文件名称
        string sFileName = "";
        string[] arrTemp = strSourceSiteUrl.Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        sFileName = arrTemp[arrTemp.Length - 1];

        if (blnKeepDirTree)
        {
          //得到源文件相对主目录的文件夹
          string sSubDir = "";
          if (sFileName != strSourceSiteUrl)
          {
            sSubDir = strSourceSiteUrl.Substring(0, strSourceSiteUrl.Length - sFileName.Length - 1);
          }
          strLocalFolder = strLocalFolder + "\\" + sSubDir;
        }

        if (!Directory.Exists(strLocalFolder))
        {
          Directory.CreateDirectory(strLocalFolder);
        }

        //附加重命名
        if (intFileNameType == 2)
        {
          arrTemp = sFileName.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries);
          sFileName = string.Join(strPlusName + ".", arrTemp);
        }

        //随机重命名
        if (intFileNameType == 3)
        {
          //文件后缀
          arrTemp = sFileName.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries);
          string sFileType = "." + arrTemp[arrTemp.Length - 1];
          //生成文件名
          sFileName = RandomStr.GetRndStrOnlyFor(10, true, true) + sFileType;
          //Console.WriteLine(strLocalFolder + "\\" + sFileName);
          while (File.Exists(strLocalFolder + "\\" + sFileName))
          {
            sFileName = RandomStr.GetRndStrOnlyFor(10, true, true) + sFileType;
          }
        }

        strLocalFolder = strLocalFolder + "\\" + sFileName;

        FileStream fs = new FileStream(strLocalFolder, FileMode.Create, FileAccess.Write);
        long length = wr.ContentLength;
        int i = 0;
        long j = 0;
        byte[] buffer = new byte[1024];
        while ((i = sIn.Read(buffer, 0, buffer.Length)) > 0)
        {
          j += i;
          fs.Write(buffer, 0, i);
        }

        sIn.Close();
        wr.Close();
        fs.Close();
        strReturnFileName = sFileName;

        return true;
      }
      catch (Exception exDF)
      {
        //Console.WriteLine(exDF.Message);
        MyErrMsg = exDF.Message;
        strReturnFileName = "";
        return false;
        //throw exDF;
      }
    }  
  }
}
