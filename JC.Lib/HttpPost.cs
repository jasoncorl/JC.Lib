using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.IO;


namespace JC.Lib
{
  /// <summary>
  /// ���������ύ��ʽ��FormEnctype=2ʱ���Խ��������ļ����У��ϴ�������һ��������
  /// </summary>
  public class HttpPost
  {
    //ȫ�ֱ����ʽ
    //Encoding _MyEncoding = Encoding.UTF8;
    Encoding _MyEncoding = Encoding.Default;
    //Encoding _MyEncoding = Encoding.ASCII;
    //Encoding _MyEncoding = Encoding.GetEncoding("gb2312");

    HttpWebRequest _MyWebHttpRequest;

    private string myRequestUrl = "";

    private string myMethod = "POST";

    private int myFormEnctype = 1;

    //������Ҫ�û�����������е�¼��Ŀǰ������
    private string myUser = "";
    private string myPass = "";

    private string myFormFieldsAndValue = "";
    private string myUploadFile = "";
    private string myUploadFolder = "";

    private byte[] myUploadData;
    private byte[] myResponseInfo = null;

    //private string myBoundary = "-----------------------------7d7148306a0770";    //����д���У�Ϊʲô�أ�
    private string myBoundary = "--xyz";

    //�¼�ί��==================================================��ʼ
    public delegate void onMessageHandle(string strMessage);

    public event onMessageHandle receiveMessage;
    //�¼�ί��==================================================����

    #region ����Ƭ��
    public Encoding MyEncoding
    {
      set { _MyEncoding = value; }
      get { return _MyEncoding; }
    }
    /// <summary>
    /// ���ݵݽ���ʽ
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
    /// ����������,1:"application/x-www-form-urlencoded" 2:"multipart/form-data" 3:"text/plain",Ĭ��Ϊ1
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
    /// �û���
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
    /// ����
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
    /// ����ҳ��Uri
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
    /// ���б���ֵ���ַ����������ʽҪ��f1=v1&f2=v2
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
    /// ��Ҫ�ϴ����ļ�·��+�ļ���
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
    /// ��Ҫ�ϴ����ļ���Ŀ¼
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
    /// Ҫ�ϴ������ݶ�������
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
    /// �õ���Ӧ��Ϣ
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
    /// ������Ӧ��Ϣ
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

    #endregion ����

    /// <summary>
    /// Post��ͨ�����ݣ����ñ�����֮ǰ��������RequestUrl
    /// </summary>
    /// <returns>true�ɹ���falseʧ��</returns>
    public Boolean UploadFieldData()
    {
      try
      {
        UploadData = _MyEncoding.GetBytes(this.FormFieldsAndValue);

        bool bRet = this.PostData();
        this.receiveMessage(string.Format("�������ύ�ɹ���{0}����", this.FormFieldsAndValue));
        return bRet;
      }
      catch (WebException we)
      {
        this.receiveMessage(string.Format("��������{0}����", we.ToString()));
        this.SetResponseInfo = _MyEncoding.GetBytes(we.ToString());
        return false;
      }
    }


    /// <summary>
    /// Post�����ļ������ñ�����֮ǰ��������RequestUrl
    /// </summary>
    /// <returns>true�ɹ���falseʧ��</returns>
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
        this.receiveMessage(string.Format("�ļ���{0}���ϴ��У�", strFileFullPath));
        bool bRet = this.PostData();
        this.receiveMessage(string.Format("�ļ���{0}���ϴ���ϣ�", strFileFullPath));
        return bRet;
      }
      catch (WebException we)
      {
        this.receiveMessage(string.Format("��������{0}����", we.ToString()));
        this.SetResponseInfo = _MyEncoding.GetBytes(we.ToString());
        return false;
      }
    }

    /// <summary>
    /// Post�ļ��У�ͬʱ�ϴ������ñ�����֮ǰ��������RequestUrl
    /// </summary>
    /// <returns>true�ɹ���falseʧ��</returns>
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
        this.receiveMessage(string.Format("�ļ��С�{0}���ϴ��У�", strDir));
        bool bRet = this.PostData();
        this.receiveMessage(string.Format("�ļ��С�{0}���ϴ���ϣ�", strDir));
        return bRet;
      }
      catch (WebException we)
      {
        this.receiveMessage(string.Format("��������{0}����", we.ToString()));
        this.SetResponseInfo = _MyEncoding.GetBytes(we.ToString());
        return false;
      }
    }

    /// <summary>
    /// Post�ļ��У�����ļ��ϴ��������ļ��У����ñ�����֮ǰ��������RequestUrl
    /// </summary>
    /// <param name="strDir"></param>
    public void UploadFolderOneByOne(string strDir)
    {
      //��ָ��Ŀ¼����Ŀ¼�²����ļ�,�г���Ŀ¼���ļ�
      DirectoryInfo Dir = new DirectoryInfo(strDir);
      try
      {
        if (Dir.Exists)
        {
          foreach (FileInfo f in Dir.GetFiles("*.*"))     //�����ļ�
          {
            UploadFile(Dir + "\\" + f.ToString());
          }

          foreach (DirectoryInfo d in Dir.GetDirectories())   //������Ŀ¼   
          {
            UploadFolderOneByOne(Dir + "\\" + d.ToString());
          }
        }
        else
        {
          throw new Exception("ָ��Ŀ¼����" + strDir + "�������ڣ�");
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
        //��������
        _MyWebHttpRequest = (HttpWebRequest)WebRequest.Create(new Uri(RequestUrl));
        _MyWebHttpRequest.Timeout = 300000;
        _MyWebHttpRequest.Method = myMethod;
        _MyWebHttpRequest.ContentType = Enctype + "; boundary=" + myBoundary.Replace("-", "");
        _MyWebHttpRequest.ContentLength = UploadData.Length;

        Stream _MyReqStream = _MyWebHttpRequest.GetRequestStream();
        _MyReqStream.Write(UploadData, 0, UploadData.Length);
        _MyReqStream.Flush();
        _MyReqStream.Close();

        //������Ӧ��Ϣ
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
      //������Ӧ��Ϣ
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
    /// ��ȡ������
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
    /// ��ȡ�����ļ�����������
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

          //�õ��ϴ��ļ�����   
          FileStream _fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
          BinaryReader _br = new BinaryReader(_fs);
          byte[] bytFile = _br.ReadBytes((int)_fs.Length);
          _br.Close();
          _fs.Close();

          //���鷵������
          //byte[] btyRet = new byte[bytFileHead.Length + bytFile.Length];
          //bytFileHead.CopyTo(btyRet, 0);
          //bytFile.CopyTo(btyRet, bytFileHead.Length);

          byte[] bytSplit = _MyEncoding.GetBytes("\r\n"); //������
          //Console.WriteLine("bytSplit.Length" + bytSplit.Length);
          byte[] btyRet = new byte[bytFileHead.Length + bytFile.Length + bytSplit.Length];
          bytFileHead.CopyTo(btyRet, 0);
          bytFile.CopyTo(btyRet, bytFileHead.Length);
          bytSplit.CopyTo(btyRet, bytFileHead.Length + bytFile.Length);
          return btyRet;
        }
        else
        {
          throw new Exception("ָ���ļ�����" + strFileName + "�������ڣ�");
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
    /// ��ȡ�ļ����������ļ��Ķ���������
    /// </summary>
    /// <param name="strDir"></param>
    /// <returns></returns>
    private byte[] GetFolderData(string strDir)
    {
      byte[] bytRet = null;
      //��ָ��Ŀ¼����Ŀ¼�²����ļ�,�г���Ŀ¼���ļ�
      DirectoryInfo Dir = new DirectoryInfo(strDir);
      try
      {
        if (Dir.Exists)
        {
          foreach (FileInfo f in Dir.GetFiles("*.*"))     //�����ļ�
          {
            //Console.WriteLine("�ļ���" + Dir + "\\" + f.ToString());
            //�õ��ļ�����
            byte[] bytFile = GetFileData(Dir + "\\" + f.ToString());
            int iBytRetLen = bytRet == null ? 0 : bytRet.Length;
            Array.Resize(ref bytRet, iBytRetLen + bytFile.Length);
            bytFile.CopyTo(bytRet, iBytRetLen);
          }

          foreach (DirectoryInfo d in Dir.GetDirectories())   //������Ŀ¼   
          {
            byte[] bytFolder = GetFolderData(Dir + "\\" + d.ToString());

            if (bytFolder != null)
            {
              int iBytRetLen = bytRet == null ? 0 : bytRet.Length;
              Array.Resize(ref bytRet, iBytRetLen + bytFolder.Length);
              bytFolder.CopyTo(bytRet, iBytRetLen);
            }
            //Console.WriteLine("Ŀ¼��" + Dir + "\\" + d.ToString() + "\\"); 
          }
          return bytRet;
        }
        else
        {
          throw new Exception("ָ��Ŀ¼����" + strDir + "�������ڣ�");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
