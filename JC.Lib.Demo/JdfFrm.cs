using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using JC.Lib;
using JC.Lib.IO;
using JC.Lib.Data;

namespace JC.Lib.Demo
{
  public partial class JdfFrm : Form
  {
    private SqlData sqlData = null;
    private JdfFile jdf = null;
    public JdfFrm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      sqlData = new SqlData("server=192.168.2.168;database=jaoa;uid=sa;pwd=ke781228();");
      DataSet ds = sqlData.GetDs("select top 1 * from t_cms", null);
      sqlData.CloseConn();
      jdf = new JdfFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + DateTime.Now.ToString("yyyy-MM-dd") + ".jdf", FileMode.Create, "测试数据");

      jdf.Write(ds.GetXml());

      //System.IO.TextReader tr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "2011-08-04.jdf.xml");
      //string s = tr.ReadToEnd();
      //jdf.Write(s);
      //tr.Close();

      //jdf.Close();

      //jdf = new JdfFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + DateTime.Now.ToString("yyyy-MM-dd") + ".jdf", FileMode.Open, "测试数据");
      jdf.ToDataSet();
      jdf.Close();

      //string s = "";
      //string ss = "";
      //s = StringHelper.Encryption("L");
      //ss = StringHelper.Decryption(s);
      //Console.Write("");
    }
  }
}
