using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JC.Lib.Drawing;

namespace JC.Lib.Demo
{
  public partial class ImgBatch : Form
  {
    private GraphicsHelper _gh = null;
    private GraphicsHelper _gh500 = new GraphicsHelper();

    private delegate void SetTextCallback(string strMessage);
    //private string ResourceDir = @"D:\es_MediaResource\Images";
    private string ResourceDir = @"";
    private float MaxSize = (float)0.0;

    public ImgBatch()
    {
      InitializeComponent();
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      this.BackColor = Color.Transparent;
      _gh = new GraphicsHelper();
      _gh.receiveMessage += new GraphicsHelper.onMessageHandle(MsgDtlHandle);
      _gh500.receiveMessage += new GraphicsHelper.onMessageHandle(MsgDtlHandle);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.txtResDir.Text == "")
      {
        MessageBox.Show("请选择源文件夹！");
        return;
      }

      ResourceDir = this.txtResDir.Text;

      ThreadPool.QueueUserWorkItem(new WaitCallback(GenThumbnails));
    }

    private void GenThumbnails(object ThreadNum)
    {
      DealDir(ResourceDir);
      MsgDtlHandle("全部图片处理完成！");
    }

    /// <summary>
    /// 处理指定
    /// </summary>
    /// <param name="strDir"></param>
    private void DealDir(string strDir)
    {
      //在指定目录及子目录下查找文件,列出子目录及文件
      DirectoryInfo Dir = new DirectoryInfo(strDir);
      try
      {
        if (Dir.Exists)
        {
          foreach (FileInfo f in Dir.GetFiles("*.*"))
          {
            if (f.Extension.ToLower() != ".jpg" && f.Extension.ToLower() != ".bmp"
              && f.Extension.ToLower() != ".gif" && f.Extension.ToLower() != ".png")
              continue;
            if (f.FullName.IndexOf("80x80") >= 0 || f.FullName.IndexOf("160x160") >= 0 || f.FullName.IndexOf("320x320") >= 0 || f.FullName.IndexOf("480x480") >= 0)
            {
              continue;
            }
            //大小更改
            string toFile = f.FullName + "_80x80.jpg";
            if (!File.Exists(toFile))
            {
              _gh.GenThumbnails(f.FullName, f.FullName + "_80x80.jpg", 80);
            }

            toFile = f.FullName + "_160x160.jpg";
            if (!File.Exists(toFile))
            {
              _gh.GenThumbnails(f.FullName, f.FullName + "_160x160.jpg", 160);
            }

            toFile = f.FullName + "_320x320.jpg";
            if (!File.Exists(toFile))
            {
              _gh.GenThumbnails(f.FullName, f.FullName + "_320x320.jpg", 320);
            }

            toFile = f.FullName + "_480x480.jpg";
            if (!File.Exists(toFile))
            {
              _gh.GenThumbnails(f.FullName, f.FullName + "_480x480.jpg", 480);
            }
          }

          foreach (DirectoryInfo d in Dir.GetDirectories())   //查找子目录   
          {
            //如果目标目录在源目录下，则略过
            //if (d.FullName == sDesc) continue;
            DealDir(Dir + "\\" + d.ToString());
          }
        }
        else
        {
          throw new Exception("指定目录：“" + strDir + "”不存在！");
        }
      }
      catch (Exception e)
      {
        MsgDtlHandle(e.Message);
        //throw e;
      }
    }

    /// <summary>
    /// 显示消息
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
          //显示文本保留5000行
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

    private void btnResFolderBrower_Click(object sender, EventArgs e)
    {
      folderBrowserDialog1.SelectedPath = this.txtResDir.Text;
      folderBrowserDialog1.ShowDialog();
      this.txtResDir.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void txtDescDir_TextChanged(object sender, EventArgs e)
    {

    }

    private void txtResDir_TextChanged(object sender, EventArgs e)
    {

    }

    private void label2_Click(object sender, EventArgs e)
    {

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void rtbMsg_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
