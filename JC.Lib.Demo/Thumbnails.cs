using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.IO;
using JC.Lib.Drawing;

namespace JC.Lib.Demo
{
  /// <summary>
  /// 批量生成图片缩略图
  /// </summary>
  public partial class Thumbnails : Form
  {
    private GraphicsHelper _gh = null;
    private GraphicsHelper _gh500 = new GraphicsHelper();

    private delegate void SetTextCallback(string strMessage);
    //private string ResourceDir = @"D:\es_MediaResource\Images";
    private string ResourceDir = @"";
    private string DescDir = @"";
    private float MaxSize = (float)0.0;

    public Thumbnails()
    {
      InitializeComponent();
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      this.BackColor = Color.Transparent;
      _gh = new GraphicsHelper();
      _gh.receiveMessage += new GraphicsHelper.onMessageHandle(MsgDtlHandle);
      _gh500.receiveMessage += new GraphicsHelper.onMessageHandle(MsgDtlHandle);

      //Console.WriteLine(Math.Sin(60 * Math.PI / 180));
      //Console.WriteLine(2 / Math.Sqrt(5));
      //Console.WriteLine(Math.Sin(30 * Math.PI / 180));
      //Console.WriteLine(Math.Sin(45 * Math.PI / 180));
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.txtResDir.Text == "")
      {
        MessageBox.Show("请选择源文件夹！");
        return;
      }
      if (this.txtDescDir.Text == "")
      {
        MessageBox.Show("请选择目标文件夹！");
        return;
      }
      if (this.txtMaxSize.Text == "")
      {
        MessageBox.Show("请输入最大尺寸！");
        return;
      }
      try
      {
        MaxSize = Convert.ToSingle(this.txtMaxSize.Text);
      }
      catch
      {
        MessageBox.Show("请输入整数或者小数！");
        return;
      }

      ResourceDir = this.txtResDir.Text;
      DescDir = this.txtDescDir.Text;

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
      string sDesc = "";
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
            sDesc = DescDir + Dir.ToString().Substring(ResourceDir.Length);
            DirectoryInfo TempDescDir = new DirectoryInfo(sDesc);
            if (!TempDescDir.Exists)
            {
              TempDescDir.Create();
            }
            //大小更改
            _gh.GenThumbnails(Dir + "\\" + f.ToString(), TempDescDir + @"\" + f.Name, MaxSize);
            /*
            //剪切
            //_gh.CutImage(Dir + "\\" + f.ToString(), TempDescDir + @"\" + f.Name, 18, 26, 0, 0);


            #region 先生成一套500px的图片
            /*
            if (!Directory.Exists(TempDescDir + @"-500\"))
            {
              Directory.CreateDirectory(TempDescDir + @"-500\");
            }
            _gh500.GenThumbnails(TempDescDir + @"\" + f.Name, TempDescDir + @"-500\" + f.Name, 480);
            //文字水印
            //_gh2.AddWaterMarkText(TempDescDir + @"-500\" + f.Name, "http://shop34979805.taobao.com", WaterMarkPositions.CENTER, 18, "arial", -45);
            //_gh2.AddWaterMarkText(TempDescDir + @"-500\" + f.Name, "蓉儿时尚", WaterMarkPositions.BOTTOM_RIGHT, 0, "隶书", 0);
            ////_gh2.AddWaterMarkText(TempDescDir + @"-500\" + f.Name, "秀派衣橱", WaterMarkPositions.BOTTOM_RIGHT, 0, "隶书", 0);
            //图片水印
            _gh500.AddWaterMarkImg(TempDescDir + @"-500\" + f.Name, @"F:\My Documents\My Ps\淘宝\图片水印副本.png", WaterMarkPositions.BOTTOM_RIGHT, 10, 0);
            //边框
            //_gh500.AddBorder(TempDescDir + @"-500\" + f.Name, @"F:\My Documents\My Ps\照片相框边框素材\35个木质相框照片边框素材\21.gif", 10, 10, 10, 10);
            //纯色边框
            _gh.AddBorder(TempDescDir + @"-500\" + f.Name, Color.Thistle, 5, 5, 5, 5);
            #endregion
            
            //文字水印
            //_gh.AddWaterMarkText(TempDescDir + @"\" + f.Name, "http://shop34979805.taobao.com", WaterMarkPositions.CENTER, 18, "arial", -45);
            //_gh.AddWaterMarkText(TempDescDir + @"\" + f.Name, "蓉儿时尚", WaterMarkPositions.BOTTOM_RIGHT, 0, "隶书", 0);
            ////_gh.AddWaterMarkText(TempDescDir + @"\" + f.Name, "秀派衣橱", WaterMarkPositions.BOTTOM_RIGHT, 0, "隶书", 0);
             */
            
            //图片水印
            //_gh.AddWaterMarkImg(TempDescDir + @"\" + f.Name, @"F:\My Documents\淘宝店\店铺\图片水印副本.png", WaterMarkPositions.BOTTOM_RIGHT, 10, 0);
            //_gh.AddWaterMarkImg(TempDescDir + @"\" + f.Name, @"F:\My Documents\淘宝店\店铺图片\图片水印副本.png", WaterMarkPositions.TOP_LEFT, 10, 30);
            //边框
            //_gh.AddBorder(TempDescDir + @"\" + f.Name, @"F:\My Documents\My Ps\照片相框边框素材\35个木质相框照片边框素材\21.gif", 20, 20, 20, 20);
            ////纯色边框
            //_gh.AddBorder(TempDescDir + @"\" + f.Name, Color.Thistle, 10, 10, 10, 10);
          }

          foreach (DirectoryInfo d in Dir.GetDirectories())   //查找子目录   
          {
            //如果目标目录在源目录下，则略过
            if (d.FullName == sDesc) continue;
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

    private void btnDescFolderBrower_Click(object sender, EventArgs e)
    {
      folderBrowserDialog1.SelectedPath = this.txtDescDir.Text;
      folderBrowserDialog1.ShowDialog();
      this.txtDescDir.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {

    }
  }
}
