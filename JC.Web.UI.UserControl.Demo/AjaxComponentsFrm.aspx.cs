using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JC.Web.UI.UserControl.Demo
{
  public partial class AjaxComponentsFrm : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpdate1_Click(object sender, EventArgs e)
    {
      int i = 0;
      while (i < 5)
      {
        System.Threading.Thread.Sleep(1000);
        txtLastUpdate.Text = DateTime.Now.ToString();
        i++;
      }
      this.Timer1.Enabled = false;

    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
      txtLastUpdate.Text = DateTime.Now.ToString();
      //((Label)UpdateProgress1.FindControl("lblMsg")).Text = DateTime.Now.ToString();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
      ((Label)UpdateProgress2.FindControl("lblMsg2")).Text = DateTime.Now.ToString();
      int i = 0;
      while (i < 5)
      {
        System.Threading.Thread.Sleep(1000);
        Literal1.Text = DateTime.Now.ToString();
        i++;
      }
    }
  }
}