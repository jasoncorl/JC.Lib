using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net;
using System.Text;
using System.IO;


namespace Jasoncorl.Web
{
  /// <summary>
  /// HttpReqHelper 的摘要说明:HTTP请求类，包括POST、Get方式
  /// </summary>
  public class HttpReqHelper
  {
    public static string MyReferer;
    public static int MyTimeout = 60000;
    public static string Referer
    {
      set
      {
        MyReferer = value;
      }
      get
      {
        return MyReferer;
      }
    }

    public static int Timeout
    {
      set
      {
        MyTimeout = value;
      }
      get
      {
        return MyTimeout;
      }
    }

    /// <summary>
    /// Post mode,默认UTF8编码
    /// </summary>
    /// <param name="strUrl">请求URL</param>
    /// <param name="strParam">请求参数字符串</param>
    /// <returns>返回请求状态描述</returns>
    public static string PostModel(string strUrl, string strParam)
    {
      return PostModel(strUrl, strParam, "UTF-8");
    }

    /// <summary>
    /// Post mode,指定编码方式
    /// </summary>
    /// <param name="strUrl">请求URL</param>
    /// <param name="strParam">请求参数字符串</param>
    /// <param name="strEncoding">编码方式，UTF8、GB2312....</param>
    /// <returns>返回请求状态描述</returns>
    public static string PostModel(string strUrl, string strParam, string strEncoding)
    {
      string sResp = "";
      return PostModel(strUrl, strParam, strEncoding, out sResp);
    }

    /// <summary>
    /// Post mode,指定编码方式
    /// </summary>
    /// <param name="strUrl">请求URL</param>
    /// <param name="strParam">请求参数字符串</param>
    /// <param name="strEncoding">编码方式，UTF-8、GB2312....</param>
    /// <param name="strResponse">响应体</param>
    /// <returns>返回请求状态描述</returns>
    public static string PostModel(string strUrl, string strParam, string strEncoding, out string strResponse)
    {
      HttpWebResponse wresp = null;
      strResponse = "";
      try
      {
        Encoding _MyEncoding = Encoding.GetEncoding(strEncoding);
        HttpWebRequest _WebReq = (HttpWebRequest)WebRequest.Create(strUrl);
        _WebReq.Method = "POST";
        _WebReq.Timeout = Timeout;
        _WebReq.Referer = Referer; //"http://www.travel-sky.cn/flytest/servers.asp";
        _WebReq.ContentType = "application/x-www-form-urlencoded";
        StringBuilder postData = new StringBuilder(1000);
        postData.Append(strParam);
        byte[] bytPost = _MyEncoding.GetBytes(postData.ToString());
        _WebReq.ContentLength = bytPost.Length;
        Stream _RequestStream = _WebReq.GetRequestStream();
        _RequestStream.Write(bytPost, 0, bytPost.Length);
        _RequestStream.Close();
        _RequestStream.Dispose();

        wresp = (HttpWebResponse)_WebReq.GetResponse();
        string sStatusDescription = wresp.StatusDescription;

        Stream _ResponseStream = wresp.GetResponseStream();
        Encoding encode = System.Text.Encoding.GetEncoding(strEncoding);
        StreamReader readStream = new StreamReader(_ResponseStream, encode);
        Char[] read = new Char[256];
        int count = readStream.Read(read, 0, 256);
        while (count > 0)
        {
          String str = new String(read, 0, count);
          strResponse += str;
          count = readStream.Read(read, 0, 256);
        }
        wresp.Close();
        readStream.Close();

        return sStatusDescription;
      }
      catch (Exception eee)
      {
        strResponse = "期望：" + eee.Message;
        return "期望：" + eee.Message;
      }
      finally
      {
        if (wresp != null)
        {
          wresp.GetResponseStream().Close();
          wresp.Close();
        }
      }
    }
    /// <summary>
    /// Get mode
    /// </summary>
    /// <param name="strUrl">完整的请求URL，包括参数串</param>
    /// <returns>返回请求状态描述</returns>
    public static string GetModel(string strUrl)
    {
      return GetModel(strUrl, "utf-8");
    }

    /// <summary>
    /// Get mode
    /// </summary>
    /// <param name="strUrl">完整的请求URL，包括参数串</param>
    /// <param name="Encoding">编码名称</param>
    /// <returns>返回请求状态描述</returns>
    public static string GetModel(string strUrl, string Encoding)
    {
      string strRet = null;
      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
        request.Timeout = 120000;
        request.ContentType = "application/x-www-form-urlencoded";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        System.IO.Stream resStream = response.GetResponseStream();
        Encoding encode = System.Text.Encoding.GetEncoding(Encoding);
        StreamReader readStream = new StreamReader(resStream, encode);

        Char[] read = new Char[256];
        int count = readStream.Read(read, 0, 256);
        while (count > 0)
        {
          String str = new String(read, 0, count);
          strRet = strRet + str;
          count = readStream.Read(read, 0, 256);
        }

        resStream.Close();
      }
      catch (Exception eee)
      {
        strRet = "期望：" + eee.Message;
      }
      return strRet;
    }


    public delegate void ProgressHandle(int progress);
    /// <summary>
    /// 进度事件
    /// </summary>
    public event ProgressHandle OnProgressHandle;

    public delegate void MessageHandle(string msg);
    /// <summary>
    /// 消息事件
    /// </summary>
    public event MessageHandle OnMessageHandle;

    private long totalSize = 0;
    private long downloaded = 0;

    private void OutMessage(string msg)
    {
      if (this.OnMessageHandle != null) this.OnMessageHandle(msg);
    }
    private void OutProgress(int progress)
    {
      if (this.OnProgressHandle != null) this.OnProgressHandle(progress);
    }

    public long TotalSize
    {
      get { return totalSize; }
    }

    public long DownloadedSize
    {
      get { return downloaded; }
    }

    /// <summary>
    /// 上传文件，影响进度事件
    /// </summary>
    /// <param name="url">接收地址</param>
    /// <param name="fileName">本地文件</param>
    /// <returns></returns>
    public void UploadFile(string url, string fileName)
    {
    }

    /// <summary>
    /// 下载文件专用，影响进度事件
    /// </summary>
    /// <param name="url">下载地址</param>
    /// <param name="fileName">本地保存文件名</param>
    /// <returns></returns>
    public void DownloadFile(string url, string fileName)
    {
      string strRet = null;
      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Timeout = 120000;
        request.ContentType = "application/x-www-form-urlencoded";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        totalSize = response.ContentLength;
        //response.ContentType

        System.IO.Stream resStream = response.GetResponseStream();
        StreamReader readStream = new StreamReader(resStream);

        FileStream fs = new FileStream(fileName, FileMode.Create);

        Char[] read = new Char[256];
        int count;
        while ((count = readStream.Read(read, 0, 256)) > 0)
        {
          String str = new String(read, 0, count);
          strRet = strRet + str;
          count = readStream.Read(read, 0, 256);
        }

        resStream.Close();
      }
      catch (Exception eee)
      {
        strRet = "期望：" + eee.Message;
      }
    }
  }
}