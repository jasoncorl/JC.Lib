using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JC.Lib;
using System.Net;

using System.Web;
using System.Threading;

namespace JC.Lib.Demo
{
  public partial class frmUpload : Form
  {
    private HttpPost _hp = null;

    private delegate void SetTextCallback(string strMessage);
    private string sUploadType = "";
    private string sLocalFolder = "";

    public frmUpload()
    {
      InitializeComponent();
      this.UploadType.Items.AddRange(new object[] { "", "Ebook", "Musics" });
      _hp = new HttpPost();
      _hp.receiveMessage += new HttpPost.onMessageHandle(MsgDtlHandle);
    }

    private void btnUpload_Click(object sender, EventArgs e)
    {
      sUploadType = (string)this.UploadType.SelectedItem;
      sLocalFolder = this.ClientFolder.Text.Trim();
      if (sUploadType == "")
      {
        MessageBox.Show("��ѡ���ϴ����ͣ�");
        return;
      }
      if (sLocalFolder == "")
      {
        MessageBox.Show("��ָ���ϴ��ļ��У�");
        return;
      }
      else
      {
        if (!System.IO.Directory.Exists(sLocalFolder))
        {
          MessageBox.Show("������ϴ��ļ��У�");
          return;
        }
      }

      ThreadPool.QueueUserWorkItem(new WaitCallback(MyUpload));
    }
    
    private void MyUpload(object ThreadNum)
    {
      try
      {
        _hp.MyEncoding = Encoding.UTF8;
        _hp.FormFieldsAndValue = "UploadType=" + sUploadType + "&SubFolder=&PrefixFileName=&UploadRowIndex=&Batch=True&ClientFolder=" + HttpUtility.UrlEncode(sLocalFolder);

        //_hp.RequestUrl = "http://ysoa.szyscf.com/lib/Upload.aspx";
        _hp.RequestUrl = "http://localhost/ysoa/lib/Upload.aspx";
      
        _hp.FormEnctype = 2;
        _hp.UploadFolderOneByOne(sLocalFolder);
        MsgDtlHandle("ȫ���ļ��ϴ���ϣ�");
      }
      catch (Exception eeee)
      {
        MsgDtlHandle(eeee.ToString());
      }
    }

    private void frmUploadDemo_Load(object sender, EventArgs e)
    {

    }



    /// <summary>
    /// ��ʾ��Ϣ
    /// </summary>
    /// <param name="strMessage"></param>
    /// <param name="strMsgType"></param>
    private void MsgDtlHandle(string strMessage)
    {
      try
      {
        if (this.rtbMsg.InvokeRequired)
        {
          SetTextCallback d = new SetTextCallback(MsgDtlHandle);
          this.Invoke(d, new object[] { strMessage });
        }
        else
        {
          //��ʾ�ı�����5000��
          if (rtbMsg.Lines.Length > 5000) { rtbMsg.Clear(); }
          this.rtbMsg.AppendText("\n" + strMessage);
          this.rtbMsg.ScrollToCaret();
        }
      }
      catch
      {
        ;
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      folderBrowserDialog1.ShowDialog();
      this.ClientFolder.Text = folderBrowserDialog1.SelectedPath;
    }

  }
}