using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.IO;


namespace JC.Lib
{
  /// <summary>
  /// 服务器表单提交方式，FormEnctype=2时可以将服务器文件（夹）上传到另外一个服务器
  /// </summary>
  public class HttpPost
  {
    //全局编码格式
    //Encoding _MyEncoding = Encoding.UTF8;
    Encoding _MyEncoding = Encoding.Default;
    //Encoding _MyEncoding = Encoding.ASCII;
    //Encoding _MyEncoding = Encoding.GetEncoding("gb2312");

    HttpWebRequest _MyWebHttpRequest;

    private string myRequestUrl = "";

    private string myMethod = "POST";

    private int myFormEnctype = 1;

    //可能需要用户名、密码进行登录（目前保留）
    private string myUser = "";
    private string myPass = "";

    private string myFormFieldsAndValue = "";
    private string myUploadFile = "";
    private string myUploadFolder = "";

    private byte[] myUploadData;
    private byte[] myResponseInfo = null;

    //private string myBoundary = "-----------------------------7d7148306a0770";    //这样写不行，为什么呢？
    private string myBoundary = "--xyz";

    //事件委托==================================================开始
    public delegate void onMessageHandle(string strMessage);

    public event onMessageHandle receiveMessage;
    //事件委托==================================================结束

    #region 属性片段
    public Encoding MyEncoding
    {
      set { _MyEncoding = value; }
      get { return _MyEncoding; }
    }
    /// <summary>
    /// 数据递交方式
    /// </summary>
    public string Method
    {
      set
      {
        myMethod = value;
      }
      get
      {
        return myMethod;
      }
    }
    /// <summary>
    /// 表单窗体类型,1:"application/x-www-form-urlencoded" 2:"multipart/form-data" 3:"text/plain",默认为1
    /// </summary>
    public int FormEnctype
    {
      set
      {
        myFormEnctype = value;
      }
      get
      {
        return myFormEnctype;
      }
    }

    private string Enctype
    {
      get
      {
        switch (FormEnctype)
        {
          case 1:
            return "application/x-www-form-urlencoded";
          case 2:
            return "multipart/form-data";
          case 3:
            return "text/plain";
          default:
            return "application/x-www-form-urlencoded";
        }
      }
    }

    /// <summary>
    /// 用户名
    /// </summary>
    public string User
    {
      set
      {
        myUser = value;
      }
      get
      {
        return myUser;
      }
    }
    /// <summary>
    /// 密码
    /// </summary>
    public string Pass
    {
      set
      {
        myPass = value;
      }
      get
      {
        return myPass;
      }
    }

    /// <summary>
    /// 请求页面Uri
    /// </summary>
    public string RequestUrl
    {
      set
      {
        myRequestUrl = value;
      }
      get
      {
        return myRequestUrl;
      }
    }

    /// <summary>
    /// 表单列表及其值对字符串，具体格式要求：f1=v1&f2=v2
    /// </summary>
    public string FormFieldsAndValue
    {
      set
      {
        myFormFieldsAndValue = value;
      }
      get
      {
        return myFormFieldsAndValue;
      }
    }

    /// <summary>
    /// 需要上传的文件路径+文件名
    /// </summary>
    public string UploadFileName
    {
      set
      {
        myUploadFile = value;
      }
      get
      {
        return myUploadFile;
      }
    }

    /// <summary>
    /// 需要上传的文件夹目录
    /// </summary>
    public string UploadFolderDir
    {
      set
      {
        myUploadFolder = value;
      }
      get
      {
        return myUploadFolder;
      }
    }

    /// <summary>
    /// 要上传的数据二进制流
    /// </summary>
    public byte[] UploadData
    {
      set
      {
        myUploadData = value;
      }
      get
      {
        return myUploadData;
      }
    }

    /// <summary>
    /// 得到响应信息
    /// </summary>
    public string ResponseInfo
    {
      get
      {
        if (myResponseInfo != null)
        {
          return _MyEncoding.GetString(myResponseInfo);
        }
        else
        {
          return "";
        }
      }
    }

    /// <summary>
    /// 设置相应信息
    /// </summary>
    private byte[] SetResponseInfo
    {
      set
      {
        int iBytRetLen = myResponseInfo == null ? 0 : myResponseInfo.Length;
        Array.Resize(ref myResponseInfo, iBytRetLen + value.Length);
        value.CopyTo(myResponseInfo, iBytRetLen);
      }
    }

    #endregion 属性

    /// <summary>
    /// Post普通表单数据，调用本方法之前必须设置RequestUrl
    /// </summary>
    /// <returns>true成功，false失败</returns>
    public Boolean UploadFieldData()
    {
      try
      {
        UploadData = _MyEncoding.GetBytes(this.FormFieldsAndValue);

        bool bRet = this.PostData();
        this.receiveMessage(string.Format("表单数据提交成功【{0}】！", this.FormFieldsAndValue));
        return bRet;
      }
      catch (WebException we)
      {
        this.receiveMessage(string.Format("发生错误【{0}】！", we.ToString()));
        this.SetResponseInfo = _MyEncoding.GetBytes(we.ToString());
        return false;
      }
    }


    /// <summary>
    /// Post单个文件，调用本方法之前必须设置RequestUrl
    /// </summary>
    /// <returns>true成功，false失败</returns>
    public Boolean UploadFile(string strFileFullPath)
    {
      try
      {
        byte[] bytFileds = _MyEncoding.GetBytes(GetFiledData());
        byte[] bytFileData = GetFileData(strFileFullPath);
        byte[] bytEnd = _MyEncoding.GetBytes("\r\n" + myBoundary + "--\r\n");
        UploadData = new byte[bytFileds.Length + bytFileData.Length + bytEnd.Length];
        bytFileds.CopyTo(this.UploadData, 0);
        bytFileData.CopyTo(this.UploadData, bytFileds.Length);
        bytEnd.CopyTo(this.UploadData, bytFileds.Length + bytFileData.Length);
        this.receiveMessage(string.Format("文件【{0}】上传中！", strFileFullPath));
        bool bRet = this.PostData();
        this.receiveMessage(string.Format("文件【{0}】上传完毕！", strFileFullPath));
        return bRet;
      }
      catch (WebException we)
      {
        this.receiveMessage(string.Format("发生错误【{0}】！", we.ToString()));
        this.SetResponseInfo = _MyEncoding.GetBytes(we.ToString());
        return false;
      }
    }

    /// <summary>
    /// Post文件夹，同时上传，调用本方法之前必须设置RequestUrl
    /// </summary>
    /// <returns>true成功，false失败</returns>
    public Boolean UploadFolder(string strDir)
    {
      try
      {
        byte[] bytFileds = _MyEncoding.GetBytes(GetFiledData());
        byte[] bytFileData = this.GetFolderData(strDir);

        byte[] bytEnd = _MyEncoding.GetBytes("\r\n" + myBoundary + "--\r\n");
        UploadData = new byte[bytFileds.Length + bytFileData.Length + bytEnd.Length];
        bytFileds.CopyTo(this.UploadData, 0);
        bytFileData.CopyTo(this.UploadData, bytFileds.Length);
        bytEnd.CopyTo(this.UploadData, bytFileds.Length + bytFileData.Length);
        this.receiveMessage(string.Format("文件夹【{0}】上传中！", strDir));
        bool bRet = this.PostData();
        this.receiveMessage(string.Format("文件夹【{0}】上传完毕！", strDir));
        return bRet;
      }
      catch (WebException we)
      {
        this.receiveMessage(string.Format("发生错误【{0}】！", we.ToString()));
        this.SetResponseInfo = _MyEncoding.GetBytes(we.ToString());
        return false;
      }
    }

    /// <summary>
    /// Post文件夹，逐个文件上传包括子文件夹，调用本方法之前必须设置RequestUrl
    /// </summary>
    /// <param name="strDir"></param>
    public void UploadFolderOneByOne(string strDir)
    {
      //在指定目录及子目录下查找文件,列出子目录及文件
      DirectoryInfo Dir = new DirectoryInfo(strDir);
      try
      {
        if (Dir.Exists)
        {
          foreach (FileInfo f in Dir.GetFiles("*.*"))     //查找文件
          {
            UploadFile(Dir + "\\" + f.ToString());
          }

          foreach (DirectoryInfo d in Dir.GetDirectories())   //查找子目录   
          {
            UploadFolderOneByOne(Dir + "\\" + d.ToString());
          }
        }
        else
        {
          throw new Exception("指定目录：“" + strDir + "”不存在！");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private Boolean PostData()
    {
      try
      {
        //发送请求
        _MyWebHttpRequest = (HttpWebRequest)WebRequest.Create(new Uri(RequestUrl));
        _MyWebHttpRequest.Timeout = 300000;
        _MyWebHttpRequest.Method = myMethod;
        _MyWebHttpRequest.ContentType = Enctype + "; boundary=" + myBoundary.Replace("-", "");
        _MyWebHttpRequest.ContentLength = UploadData.Length;

        Stream _MyReqStream = _MyWebHttpRequest.GetRequestStream();
        _MyReqStream.Write(UploadData, 0, UploadData.Length);
        _MyReqStream.Flush();
        _MyReqStream.Close();

        //返回响应信息
        this.SetResponseInfo = GetResponseInfo();
        _MyWebHttpRequest.Abort();
        return true;
      }
      catch (Exception PostEx)
      {
        throw PostEx;
      }
    }

    private byte[] GetResponseInfo()
    {
      //返回响应信息
      try
      {
        HttpWebResponse _MyHttpResponse = (HttpWebResponse)_MyWebHttpRequest.GetResponse();
        Stream _MyRespStream = _MyHttpResponse.GetResponseStream();

        StreamReader readStream = new StreamReader(_MyRespStream, _MyEncoding);
        Char[] read = new Char[256];
        // Reads 256 characters at a time.
        int count = readStream.Read(read, 0, 256);
        String sResponse = "";
        while (count > 0)
        {
          sResponse = sResponse + new String(read, 0, count);
          count = readStream.Read(read, 0, 256);
        }

        readStream.Close();
        _MyHttpResponse.Close();
        this.receiveMessage(sResponse);
        return _MyEncoding.GetBytes(sResponse);
      }
      catch (Exception ResExp)
      {
        throw ResExp;
      }
    }

    /// <summary>
    /// 获取表单数据
    /// </summary>
    /// <returns></returns>
    private string GetFiledData()
    {
      StringBuilder _sb = new StringBuilder("");
      try
      {
        string[] arrFields = myFormFieldsAndValue.Split(new string[] { "&" }, StringSplitOptions.None);
        for (int i = 0; i < arrFields.Length; i++)
        {
          string[] arrField = arrFields[i].Split(new string[] { "=" }, StringSplitOptions.None);
          string textTemplate = myBoundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
          _sb.Append(String.Format(textTemplate, arrField[0], arrField[1]));
        }
      }
      catch (Exception SbExp)
      {
        throw SbExp;
      }
      return _sb.ToString();
    }

    /// <summary>
    /// 获取单个文件二进制数据
    /// </summary>
    /// <param name="strFileName"></param>
    /// <returns></returns>
    private byte[] GetFileData(string strFileName)
    {
      try
      {
        if (File.Exists(strFileName))
        {
          string sFileField = myBoundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
          string data = String.Format(sFileField, "FileURL", strFileName, "image/gif");
          byte[] bytFileHead = _MyEncoding.GetBytes(data);

          //得到上传文件数据   
          FileStream _fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
          BinaryReader _br = new BinaryReader(_fs);
          byte[] bytFile = _br.ReadBytes((int)_fs.Length);
          _br.Close();
          _fs.Close();

          //重组返回数组
          //byte[] btyRet = new byte[bytFileHead.Length + bytFile.Length];
          //bytFileHead.CopyTo(btyRet, 0);
          //bytFile.CopyTo(btyRet, bytFileHead.Length);

          byte[] bytSplit = _MyEncoding.GetBytes("\r\n"); //不能少
          //Console.WriteLine("bytSplit.Length" + bytSplit.Length);
          byte[] btyRet = new byte[bytFileHead.Length + bytFile.Length + bytSplit.Length];
          bytFileHead.CopyTo(btyRet, 0);
          bytFile.CopyTo(btyRet, bytFileHead.Length);
          bytSplit.CopyTo(btyRet, bytFileHead.Length + bytFile.Length);
          return btyRet;
        }
        else
        {
          throw new Exception("指定文件：“" + strFileName + "”不存在！");
        }
      }
      catch (Exception fse)
      {
        //this.SetResponseInfo = _MyEncoding.GetBytes(fse.ToString());
        //return null;
        throw fse;
      }

    }

    /// <summary>
    /// 获取文件夹下所有文件的二进制数据
    /// </summary>
    /// <param name="strDir"></param>
    /// <returns></returns>
    private byte[] GetFolderData(string strDir)
    {
      byte[] bytRet = null;
      //在指定目录及子目录下查找文件,列出子目录及文件
      DirectoryInfo Dir = new DirectoryInfo(strDir);
      try
      {
        if (Dir.Exists)
        {
          foreach (FileInfo f in Dir.GetFiles("*.*"))     //查找文件
          {
            //Console.WriteLine("文件：" + Dir + "\\" + f.ToString());
            //得到文件数据
            byte[] bytFile = GetFileData(Dir + "\\" + f.ToString());
            int iBytRetLen = bytRet == null ? 0 : bytRet.Length;
            Array.Resize(ref bytRet, iBytRetLen + bytFile.Length);
            bytFile.CopyTo(bytRet, iBytRetLen);
          }

          foreach (DirectoryInfo d in Dir.GetDirectories())   //查找子目录   
          {
            byte[] bytFolder = GetFolderData(Dir + "\\" + d.ToString());

            if (bytFolder != null)
            {
              int iBytRetLen = bytRet == null ? 0 : bytRet.Length;
              Array.Resize(ref bytRet, iBytRetLen + bytFolder.Length);
              bytFolder.CopyTo(bytRet, iBytRetLen);
            }
            //Console.WriteLine("目录：" + Dir + "\\" + d.ToString() + "\\"); 
          }
          return bytRet;
        }
        else
        {
          throw new Exception("指定目录：“" + strDir + "”不存在！");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
