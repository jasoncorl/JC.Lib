using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using JC.Lib;

namespace JC.Lib.Demo
{
  public partial class EncryptionFrm : Form
  {
    public EncryptionFrm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.richTextBox2.Text = StringHelper.EncryptDES(this.richTextBox1.Text,"ldhd.com");
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Text = StringHelper.DecryptDES(this.richTextBox2.Text, "ldhd.com");
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.richTextBox2.Text = StringHelper.Encryption(this.richTextBox1.Text);
    }

    private void button4_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Text = StringHelper.Decryption(this.richTextBox2.Text);
    }

    private void button5_Click(object sender, EventArgs e)
    {
      this.richTextBox2.Text = StringHelper.EncodeBase64(Encoding.ASCII, this.richTextBox1.Text);
    }

    private void button6_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Text = StringHelper.DecodeBase64(Encoding.ASCII, this.richTextBox2.Text);
    }

    private void button7_Click(object sender, EventArgs e)
    {
      this.richTextBox2.Text = DecryptDES(EncryptDES(this.richTextBox1.Text, "ldhd.com"), "ldhd.com");
    }


    #region DES加密方式,分块加密解密会有问题

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
    #endregion
  }
}
