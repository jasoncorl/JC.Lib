using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JC.Lib.Demo
{
  static class Program
  {
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      //Application.Run(new ImgBatch());
      //Application.Run(new Thumbnails());
      //Application.Run(new SnatchWeb());
      //Application.Run(new GPSCorrection());
      //Application.Run(new EncryptionFrm());
      //Application.Run(new ZipExpress());
      //Application.Run(new Temp());
      //Application.Run(new KrcParseTry());
      //Application.Run(new JdfFrm());
      //Application.Run(new WebPostWithEncryptFrm());
      //Application.Run(new IPAddressReaderFrm());
      Application.Run(new frmJieba());
    }
  }
}