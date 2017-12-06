using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.IO;

namespace JC.Lib.IO
{
  /// <summary>
  /// 自定义数据文件格式：文件头4位 + 版本4位 + 数据名称长度4位 + 不定长数据名称。
  /// </summary>
  public class JdfFile : FileStream
  {
    private const string ENCRYPTDES_KEY = "df!^&%$#";
    private Encoding encoding = Encoding.UTF8;
    private const int BLOCK_LENGTH = 1024 * 000; //分块大小，由于是字典压缩算法，不固定分块大小，可能会出错
    /// <summary>
    /// 文件头
    /// </summary>
    private string fileHeader = "jdf";
    private int fileHeaderLength = 0;
    /// <summary>
    /// 文件版本
    /// </summary>
    private string fileVersion = "";
    private int fileVersionLength = 0;

    private int dataNameLength = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="mode"></param>
    /// <param name="dataname">存储的数据名称</param>
    public JdfFile(string path, FileMode mode, string dataname)
      : this(path, mode, FileAccess.ReadWrite, "1.0", dataname)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="mode"></param>
    /// <param name="version"></param>
    /// <param name="dataname"></param>
    public JdfFile(string path, FileMode mode, string version, string dataname)
      : this(path, mode, FileAccess.ReadWrite, version, dataname)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="mode"></param>
    /// <param name="access"></param>
    /// <param name="dataname">存储的数据名称</param>
    public JdfFile(string path, FileMode mode, FileAccess access, string version, string dataname)
      : base(path, mode, access)
    {
      byte[] buffer = null;
      if (this.Length == 0)
      {
        #region 文件头
        //this.Write("jdf");
        buffer = encoding.GetBytes(StringHelper.EncryptDES(this.fileHeader, ENCRYPTDES_KEY));
        //Lz77Compression lz = new Lz77Compression();
        byte[] bl = System.BitConverter.GetBytes(buffer.Length);
        base.Write(bl, 0, bl.Length);
        base.Write(buffer, 0, buffer.Length);
        this.fileHeaderLength = buffer.Length;
        #endregion

        #region 文件版本
        //this.Write(version); 
        buffer = null;
        buffer = encoding.GetBytes(StringHelper.EncryptDES(version, ENCRYPTDES_KEY));
        //Lz77Compression lz = new Lz77Compression();
        bl = System.BitConverter.GetBytes(buffer.Length);
        base.Write(bl, 0, bl.Length);
        base.Write(buffer, 0, buffer.Length);
        this.fileVersionLength = buffer.Length;
        #endregion
        //this.Write(dataname);
        this.WriteDataName(dataname);
      }
      else
      {
        byte[] buf = new byte[4];

        this.Position = 0;
        this.Read(buf, 0, 4);
        this.fileHeaderLength =  System.BitConverter.ToInt32(buf, 0);
        buffer = new byte[this.fileHeaderLength];
        this.Read(buffer, 0, this.fileHeaderLength);
        if (StringHelper.DecryptDES(this.encoding.GetString(buffer, 0, this.fileHeaderLength), ENCRYPTDES_KEY) != this.fileHeader)
        {
          this.Close();
          throw new Exception("非法的文件格式");
        }

        this.Read(buf, 0, 4);
        this.fileVersionLength = System.BitConverter.ToInt32(buf, 0);
        buffer = new byte[this.fileHeaderLength];
        this.Read(buffer, 0, this.fileHeaderLength);
        this.fileVersion = StringHelper.DecryptDES(this.encoding.GetString(buffer, 0, this.fileHeaderLength), ENCRYPTDES_KEY);

        this.DataName = this.GetDataName() ;
        if (this.dataName != dataname)
        {
          this.Close();
          throw new Exception("不匹配的数据文件");
        }
      }
    }

    ~JdfFile()
    {
      this.Flush();
      this.Close();
      this.Dispose();
    }

    private string dataName = "";
    /// <summary>
    /// 存储的数据名
    /// </summary>
    public string DataName
    {
      set { this.dataName = value; }
      get { return this.dataName; }
    }

    /// <summary>
    /// 文件版本
    /// </summary>
    public string FileVersion
    {
      set { this.fileVersion = value; }
      get { return this.fileVersion; }
    }

    /// <summary>
    /// 写入数据名称
    /// </summary>
    /// <param name="name"></param>
    private void WriteDataName(string name)
    {
      this.dataName = name;
      byte[] buffer = encoding.GetBytes(StringHelper.EncryptDES(name, ENCRYPTDES_KEY));
      //byte[] buffer = encoding.GetBytes(StringHelper.Encryption(name));
      //Lz77Compression lz = new Lz77Compression();
      byte[] bl = System.BitConverter.GetBytes(buffer.Length);
      base.Write(bl, 0, bl.Length);
      base.Write(buffer, 0, buffer.Length);
      this.dataNameLength = bl.Length +  buffer.Length;
    }

    private string GetDataName()
    {
      string ret = "";
      byte[] bl = new byte[4];
      this.Read(bl, 0, bl.Length);
      int dhl = System.BitConverter.ToInt32(bl, 0);
      byte[] buffer = new byte[dhl];
      this.Read(buffer, 0, buffer.Length);
      ret = StringHelper.DecryptDES(encoding.GetString(buffer), ENCRYPTDES_KEY);
      this.dataNameLength = bl.Length + buffer.Length;
      return ret;
    }

    ///// <summary>
    ///// 写入二进制流
    ///// </summary>
    ///// <param name="buffer"></param>
    //private void Write(byte[] buffer)
    //{
    //  this.Write(buffer, 0, buffer.Length);
    //}

    /// <summary>
    /// 写入字符串
    /// </summary>
    /// <param name="value"></param>
    public void Write(string value)
    {
      //byte[] buffer = encoding.GetBytes(StringHelper.Encryption(value));
      byte[] buffer = encoding.GetBytes(StringHelper.EncryptDES(value, ENCRYPTDES_KEY));
      //Lz77Compression lz = new Lz77Compression();
      base.Write(buffer, 0, buffer.Length);
      this.Flush();
    }

    public override void Write(byte[] array, int offset, int count)
    {
      string s = encoding.GetString(array, offset, count);
      this.Write(s);
      //base.Write(array, offset, count);
    }

    public override void Close()
    {
      base.Close();
    }

    /// <summary>
    /// 转化为数据集
    /// </summary>
    /// <returns></returns>
    public DataSet ToDataSet()
    {
      DataSet ret = new DataSet();
      string fileTemp = base.Name + ".temp";
      FileStream fs = new FileStream(fileTemp, FileMode.Create);
      byte[] buf = new byte[4*1024];
      byte[] bufWrite;
      int read = 0;
      string s = "";
      this.Position = 4 + this.fileHeaderLength + 4 + this.fileVersionLength + this.dataNameLength;
      while (true)
      {
        buf = new byte[this.Length];
        read = this.Read(buf, 0, buf.Length);
        if (read == 0) break;
        s = StringHelper.DecryptDES(encoding.GetString(buf, 0, read), ENCRYPTDES_KEY);
        //s = StringHelper.Decryption(encoding.GetString(buf, 0, read));
        bufWrite = null; ;
        bufWrite = encoding.GetBytes(s);
        fs.Write(bufWrite, 0, bufWrite.Length);
      }
      fs.Flush();
      fs.Close();
      fs.Dispose();
      ret.ReadXml(fileTemp,XmlReadMode.Auto);
      System.IO.File.Delete(fileTemp);
      return ret;
    }
  }
}
