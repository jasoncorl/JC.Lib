using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JC.Web.UI.UserControl.Demo
{
  public partial class UploadFrm : System.Web.UI.Page
  {
    private static int readed = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
      this.Timer1.Enabled = false;
    }

    private void Upload()
    {
      readed = 0;
      byte[] fileBytes = this.FileUpload1.FileBytes;
      Stream readStream = (Stream)this.FileUpload1.FileContent;
      byte[] buffer = new byte[256];
      int count = 0;
      while ((count = readStream.Read(buffer, 0, 256)) > 0)
      {
        readed += count;
      }

      readStream.Close();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
      this.Timer1.Enabled = true;
      Thread obj = new Thread(new ThreadStart(Upload));
      obj.IsBackground = true;
      obj.Start();
      obj.Join();
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
      Thread obj = new Thread(new ThreadStart(delegate()
      {
        txtLastUpdate.Text = "readed:" + readed.ToString();
      }));
      obj.Start();
      obj.Join();
    }
  }
}