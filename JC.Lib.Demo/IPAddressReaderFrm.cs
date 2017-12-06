using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JC.Lib;

namespace JC.Lib.Demo
{
  public partial class IPAddressReaderFrm : Form
  {
    public IPAddressReaderFrm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      string ipfilePath =  AppDomain.CurrentDomain.BaseDirectory.ToString() + @"QQWry.dat";
      IPLocation loc = IPReader.GetIPLocation(textBox1.Text.Trim());
      this.textBox2.Text = loc.AreaRegion;
    }
  }
}
