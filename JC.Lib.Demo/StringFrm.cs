using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JC.Lib.Demo
{
  public partial class StringFrm : Form
  {
    public StringFrm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int hash = JC.Lib.MathHelper.JavaHashCode(this.textBox1.Text);
      int hash1 = this.textBox1.Text.GetHashCode();
      //this.textBox3.Text = (hash > 0 ? "1" : "0") + JC.Lib.MathHelper.Get10to36((Math.Abs(hash)));
      this.textBox3.Text = (hash > 0 ? "1" : "0") + Math.Abs(hash).ToString();
      this.textBox4.Text = (hash1 > 0 ? "1" : "0") + Math.Abs(hash1).ToString();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.textBox3.Text = JC.Lib.MathHelper.Get10to36(Convert.ToInt32(this.textBox4.Text));
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.textBox3.Text = JC.Lib.MathHelper.Get36to10(this.textBox4.Text).ToString();
    }
  }
}
