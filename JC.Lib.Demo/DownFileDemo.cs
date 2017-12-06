using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using JC.Lib.Data;
using JC.Lib.Web;
namespace JC.Lib.Demo
{
  public partial class DownFileDemo : Form
  {
    public DownFileDemo()
    {
      InitializeComponent();
    }
    private delegate void SetTextCallback(string strMessage);

    private void button1_Click(object sender, EventArgs e)
    {
      try
      {
        string sCon = "server=192.168.1.157;database=es_mediaresource;User ID=root;Password=root;CharSet=utf8;";
        MySqlData _MySqlData = new MySqlData(sCon);
        string[] arrType = new string[2] { "mp3", "photo" };
        string[] arrTypeHttpFolder = new string[2] { "http://r01.mmstoon.com/mp3/", "http://p01.mmstoon.com/mmsimg/comm/" };
        string[] arrSql = new string[2] { "select RingCode, ForSend from union_resource.t_mp3list", "select PhotoCode, ComImg from union_resource.t_photolist" };
        string[] arrSqlDealName = new string[2] { "Update union_resource.t_mp3list Set ForSend = @P2, Forlisten = @P2 Where RingCode = @P1", "Update union_resource.t_photolist Set Style128_128 = @P2,ComImg = @P2 Where PhotoCode = @P1" };
        string[] arrSqlDealFlag = new string[2] { "Update union_resource.t_mp3list Set Checked = 3 Where RingCode = @P1", "Update union_resource.t_photolist Set Checked = 3 Where PhotoCode = @P1" };
        string sLocalFolder = textBox1.Text.Trim();
        if (sLocalFolder != "")
        {
          //下载处理
          for (int i = 0; i < arrType.Length; i++)
          {
            MsgDtlHandle("正在下载" + arrType[i] + "...");
            DataSet _ds = _MySqlData.getDs(arrSql[i], null);
            for (int j = 0; j < _ds.Tables[0].Rows.Count; j++)
            {
              DataRow _curRow = _ds.Tables[0].Rows[j];
              MsgDtlHandle("正在下载" + arrTypeHttpFolder[i] + _curRow[1].ToString() + "...");
              //类型目录
              string sLocalPath = "";
              sLocalPath = sLocalFolder + "\\" + arrType[i];

              string sRetFileName = "";

              Boolean blnDownFlag = false;
              blnDownFlag = FileDown.DownFile(arrTypeHttpFolder[i], _curRow[1].ToString(), sLocalPath, 3, "", true, out sRetFileName);

              //文件数据处理
              //成功
              if (blnDownFlag) {
                //更新数据库文件名
                string sFileName = "";
                if (_curRow[1].ToString().IndexOf("/") > 0)
                {
                  string[] arrTemp = _curRow[1].ToString().Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                  sFileName = _curRow[1].ToString().Replace(arrTemp[arrTemp.Length - 1], sRetFileName);
                  //Console.WriteLine(_curRow[1].ToString() + "--->" + sFileName);
                }
                else {
                  sFileName = sRetFileName;
                }
                _MySqlData.ExecuteNonQuery(arrSqlDealName[i], new string[2] { _curRow[0].ToString(), sFileName });
                MsgDtlHandle("完成下载：" + arrTypeHttpFolder[i] + _curRow[1].ToString());
              } 
              //失败
              else {
                //更新数据状态为文件缺失
                _MySqlData.ExecuteNonQuery(arrSqlDealFlag[i], new string[1] { _curRow[0].ToString() });
                MsgDtlHandle(FileDown.ErrMsg + "|--|" + _curRow[0].ToString() + "|--|" + _curRow[0].ToString() + "\n");
                //Console.WriteLine("没有找到文件");
              }
              //Console.WriteLine(sRetFileName);
            }
          }

          _MySqlData.closeConn();
          MsgDtlHandle("全部下载任务完成！");
        }
      }
      catch (Exception ex)
      {
        MsgDtlHandle(ex.Message);
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
      {
        textBox1.Text = folderBrowserDialog1.SelectedPath;
      }
    }

    //显示消息
    private void MsgDtlHandle(string strMessage)
    {
      try
      {
        if (this.richTextBox1.InvokeRequired)
        {
          SetTextCallback d = new SetTextCallback(MsgDtlHandle);
          this.Invoke(d, new object[] { strMessage });
        }
        else
        {
          this.richTextBox1.AppendText( strMessage + "\n" );
        }
        this.richTextBox1.ScrollToCaret();
      }
      catch
      {
        ;
      }
    }
  }
}