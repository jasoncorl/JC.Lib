using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.Lib.Demo
{
  public partial class frmJieba : Form
  {
    public frmJieba()
    {
      InitializeComponent();
    }

    private delegate void SetTextCallback(string strMessage);


    private void button3_Click(object sender, EventArgs e)
    {
      JiebaSegmenter jb = new JiebaSegmenter();
      var words = jb.Cut(this.richTextBox1.Text);
      MsgDtlHandle(string.Format("【默认精确】：{0}", string.Join("/ ", words)));
    }

    //显示消息
    private void MsgDtlHandle(string strMessage)
    {
      try
      {
        if (this.richTextBox2.InvokeRequired)
        {
          SetTextCallback d = new SetTextCallback(MsgDtlHandle);
          this.Invoke(d, new object[] { strMessage });
        }
        else
        {
          this.richTextBox2.AppendText(strMessage + "\n");
        }
        this.richTextBox2.ScrollToCaret();
      }
      catch
      {
        ;
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      JiebaSegmenter jb = new JiebaSegmenter();
      var words = jb.CutForSearch(this.richTextBox1.Text);
      MsgDtlHandle(string.Format("【搜索引擎模式】：{0}", string.Join("/ ", words)));
    }

    private void button2_Click(object sender, EventArgs e)
    {
      JiebaSegmenter jb = new JiebaSegmenter();
      var words = jb.Cut(this.richTextBox1.Text, cutAll: true);
      MsgDtlHandle(string.Format("【全模式】：{0}", string.Join("/ ", words)));
    }
  }
}
