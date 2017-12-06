using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JC.Lib.Web;

namespace JC.Lib.Demo
{
  public partial class WebPostWithEncryptFrm : Form
  {
    public WebPostWithEncryptFrm()
    {
      InitializeComponent();
      //this.richTextBox1.Text = "<?xml version=\"1.0\" encoding=\"utf-8\"?><static_req><apps><app pkg=\"com.qihoo360.mobilesafe\" name=\"360卫士\" vcode=\"3.7.0\" vname=\"3.7.0\" md5=\"\" appid=\"\" isour=\"1\"  action=\"Scan\" runtime=\"\" actionfrom=\"\" evtsource=\"\" evtparas=\"\"/><app pkg=\"com.baoruan.navigate\" name=\"3G导航\" vcode=\"1.3.0\" vname=\"1.3.0\" md5=\"\" appid=\"\" isour=\"1\"  action=\"OpenAPP\" runtime=\"\" actionfrom=\"\" evtsource=\"\" evtparas=\"\"/><app pkg=\"cn.goapk.market\" name=\"AnZhi\" vcode=\"V4.2\" vname=\"V4.2\" md5=\"\" appid=\"\" isour=\"1\"  action=\"UnInstall\" runtime=\"\" actionfrom=\"\" evtsource=\"\" evtparas=\"\"/><app pkg=\"com.game4u.moshi\" name=\"AV色影\" vcode=\"9.1.5\" vname=\"9.1.5\" md5=\"\" appid=\"\" isour=\"1\"  action=\"Install\" runtime=\"\" actionfrom=\"\" evtsource=\"\" evtparas=\"\"/></apps></static_req>";
      this.richTextBox1.Text = "{\"cellphones\"=>[\"13012345610\", \"13012345611\", \"13510126864\"]}";
      //this.textBox1.Text = "http://localhost:8007/TaskManager/Alive?mid=817bab95-2cd6-4d4a-a608-38ecf847bdb2&appsid=699cd239-54b5-4bc1-bdfa-a9ce8d58fc00&imsi=460020896359598&imei=862115010620477&wifi=002715658131&contype=gprs&tfcard=1&mobile=13000000000&mobiletype=F307&tfleft=411952&memleft=174272";
      this.textBox1.Text = "https://f.toyou8.cn/api/v1/users/query.json";
    }

    private void button1_Click(object sender, EventArgs e)
    {
      string data = this.richTextBox1.Text;
      string url = this.textBox1.Text;
      WebHeaderCollection header = new WebHeaderCollection();
      if (this.txtDataHeader.Text.Trim() != "") header.Add(this.txtDataHeader.Text, this.txtDataHeaderValue.Text);
      if (this.txtDataHeader1.Text.Trim() != "") header.Add(this.txtDataHeader1.Text, this.txtDataHeaderValue1.Text);

      string text = "";
      try
      {
        HttpReqHelper.Timeout = 60000 * 10;
        if (chkEncrypt.Checked)
        {
          text = DecryptDES(HttpReqHelper.Post(url, EncryptDES(data, "ldhd.com"), "utf-8", header), "ldhd.com");
        }
        else
        {
          text = Encoding.UTF8.GetString(HttpReqHelper.Post(url, Encoding.UTF8.GetBytes(data), "utf-8", header));
        }
      }
      catch (Exception ex)
      {
        text = ex.ToString();
      }

      this.richTextBox2.Text = text;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      string url = this.textBox1.Text;
      WebHeaderCollection header = new WebHeaderCollection();
      if (this.txtDataHeader.Text.Trim() != "") header.Add(this.txtDataHeader.Text, this.txtDataHeaderValue.Text);
      if (this.txtDataHeader1.Text.Trim() != "") header.Add(this.txtDataHeader1.Text, this.txtDataHeaderValue1.Text);
      string text = "";
      try
      {
        HttpReqHelper.Timeout = 60000 * 10;
        byte[] byts = HttpReqHelper.GetResponseBytes(url, "utf-8", header);
        if (chkEncrypt.Checked)
        {
          text = DecryptDES(byts, "ldhd.com");
        }
        else
        {
          text = Encoding.UTF8.GetString(byts);
        }
      }
      catch (Exception ex)
      {
        text = ex.ToString();
      }
      this.richTextBox2.Text = text;
    }

    //默认密钥向量
    //private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    private static byte[] Keys = { 12, 24, 48, 36, 48, 60, 72, 84 };
    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="encryptKey">加密密钥,要求为8位</param>
    /// <returns>加密成功返回加密后的二进制数组，失败返回源串二进制数组</returns>
    public static byte[] EncryptDES(string encryptString, string encryptKey)
    {
      try
      {
        byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
        byte[] rgbIV = Keys;
        byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return mStream.ToArray();
      }
      catch
      {
        return Encoding.UTF8.GetBytes(encryptString);
      }
    }

    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="bytes">待解密的二进制数组</param>
    /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串，失败返源二进制数组的字符串</returns>
    public static string DecryptDES(byte[] bytes, string decryptKey)
    {
      try
      {
        byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
        byte[] rgbIV = Keys;
        //byte[] inputByteArray = Convert.FromBase64String(decryptString);
        DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        cStream.Write(bytes, 0, bytes.Length);
        cStream.FlushFinalBlock();
        return Encoding.UTF8.GetString(mStream.ToArray());
      }
      catch
      {
        return Encoding.UTF8.GetString(bytes);
      }
    }
  }
}
