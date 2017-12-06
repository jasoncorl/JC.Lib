using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.IO;

using JC.Lib.IO.Text;

namespace JC.Lib.Web
{
  /// <summary>
  /// ��ָ��URL�����ļ�
  /// </summary>
  public class FileDown
  {
    private static string sErrMsg = "";

    /// <summary>
    /// ���һ���Ĵ�����Ϣ
    /// </summary>
    public static string ErrMsg { get { return sErrMsg; } }
    private static string MyErrMsg { set { sErrMsg = value; } }
    /// <summary>
    /// ָ������ȫ·������
    /// </summary>
    /// <param name="strSource"></param>
    /// <param name="strLocalPath"></param>
    /// <returns>���سɹ�����true�����򷵻�false</returns>
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
    /// �����ļ���ָ���ļ���
    /// </summary>
    /// <param name="strSource">�ļ�URL</param>
    /// <param name="strLocalFolder">�����ļ���</param>
    /// <param name="blnAutoName">�Ƿ��Զ�����</param>
    /// <param name="strPlusName">��������Զ�����������������Դ�ļ���֮��</param>
    /// <returns>���سɹ�����true�����򷵻�false</returns>
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
        //�õ�Դ�ļ�����
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
    /// �����ļ���ָ���ļ���
    /// </summary>
    /// <param name="strSourceMainUrl">Դ��Դ������ַ</param>
    /// <param name="strSourceSiteUrl">���������ַ��վ��Ŀ¼��ַ���������ַ�Ѿ����������Ϊ��</param>
    /// <param name="strLocalFolder">����������Ŀ¼</param>
    /// <param name="intFileNameType">�ļ�������ʽ,1����ԭ�ļ�����2ԭ�ļ���֮�󸽼��ļ�����3���������</param>
    /// <param name="strPlusName">�ļ�������ʽΪ2ʱ������������Դ�ļ���֮��</param>
    /// <param name="blnKeepDirTree">�Ƿ񱣳�ͬԴ��ԴĿ¼�ṹ</param>
    /// <param name="strReturnFileName">�����ļ�����</param>
    /// <returns>���سɹ�����true�����򷵻�false</returns>
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

        //�õ�Դ�ļ�����
        string sFileName = "";
        string[] arrTemp = strSourceSiteUrl.Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        sFileName = arrTemp[arrTemp.Length - 1];

        if (blnKeepDirTree)
        {
          //�õ�Դ�ļ������Ŀ¼���ļ���
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

        //����������
        if (intFileNameType == 2)
        {
          arrTemp = sFileName.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries);
          sFileName = string.Join(strPlusName + ".", arrTemp);
        }

        //���������
        if (intFileNameType == 3)
        {
          //�ļ���׺
          arrTemp = sFileName.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries);
          string sFileType = "." + arrTemp[arrTemp.Length - 1];
          //�����ļ���
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
