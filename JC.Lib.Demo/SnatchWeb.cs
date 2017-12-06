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
  public partial class SnatchWeb : Form
  {
    public SnatchWeb()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Text = JC.Lib.Web.HttpReqHelper.Get(this.textBox1.Text);
    }
  }
}
