using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using webCommon;
using System.Net;

namespace testDemo
{
    public partial class frmUploadDemo : Form
    {
        public frmUploadDemo()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPost _hp = new HttpPost();
            try
            {
                _hp.FormFieldsAndValue = "UploadType=ADTemplete&SubFolder=&PrefixFileName=&UploadRowIndex=";

                _hp.RequestUrl = "http://192.168.1.30:88/CommonFiles/FileUp/upload.asp";
                _hp.FormEnctype = 2;
                //_hp.UploadFile(@"F:\My Documents\VS6.0 Pro\fileUPInte\testupload\index_files\navbits_start.gif");
                this.textBox1.Text = _hp.UploadFolder(@"D:\FullChannel21").ToString();

                //_hp.RequestUrl = "http://192.168.1.30:88/CommonFiles/FileUp/test.asp";
                //_hp.FormEnctype = 1;
                //_hp.UploadFieldData();
                this.textBox1.Text += _hp.ResponseInfo;
            }
            catch (Exception eeee)
            {
                this.textBox1.Text = eeee.ToString();
            }

        }

      private void frmUploadDemo_Load(object sender, EventArgs e)
      {

      }
    }
}