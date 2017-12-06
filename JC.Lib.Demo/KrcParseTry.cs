using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace testDemo
{
  public partial class KrcParseTry : Form
  {
    private delegate void SetTextCallback(string strMessage);

    public KrcParseTry()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int c;
      StreamReader sr = new StreamReader(@"E:\music\kugoo\Lyrics\陈瑞 - 白狐.krc");
      while (true)
      {
        c = sr.Read();
        if (c == -1) break;
        MsgDtlHandle(c.ToString() + "\n");
        //MsgDtlHandle(((char)c).ToString());
      }
      sr.Close();
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
          //this.rtbMsg.AppendText("\n" + strMessage);
          this.rtbMsg.AppendText(strMessage);
          this.rtbMsg.ScrollToCaret();
        }
      }
      catch
      {
        ;
      }
    }
  }
}
