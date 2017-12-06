using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JC.Web.UI.UserControl.Demo
{
  public partial class TelerikRadUploadFrm : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
      Telerik.Web.UI.UploadedFile file = this.RadUpload1.UploadedFiles[0];
      file.SaveAs(Server.MapPath("/") + file.FileName);
    }
  }
}