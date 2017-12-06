using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using JC.Lib;
using JC.Lib.IO;

namespace JC.Lib.Demo
{
  public partial class ZipExpress : Form
  {
    public ZipExpress()
    {
      InitializeComponent();

      if (!Directory.Exists(InitDir))
      {
        Directory.CreateDirectory(InitDir);
      }
      File.Delete(FileName);
      LoadZipFile();
      FileBrower();

      //byte[] temp = Encoding.Default.GetBytes("Lzx!");
      //Int32 i = BitConverter.ToInt32(temp, 0);
      //Console.WriteLine(Convert.ToString(i, 16));
       //Console.WriteLine(Convert.ToString("0x02014b50", 10));
    }

    private string InitDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ZipFiles\");
    private string FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ZipFiles\") + "Temp_0.Temp";
    private ZipEntity _ZipEntity = null;
    private bool IsSave = true;
    System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();

    /// <summary>
    /// 新建压缩包
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button1_Click(object sender, EventArgs e)
    {
      this.CheckFile();
      if (messageBoxCS.Length != 0)
      {
        messageBoxCS.AppendFormat("确定要放弃操作重新建立文件吗？");
        messageBoxCS.AppendLine();
        if (MessageBox.Show(messageBoxCS.ToString(), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
        {
          _ZipEntity.Close();
          if (Path.GetExtension(FileName).ToLower() == ".temp")
          {
            File.Delete(FileName);
          }
        }
      }

      this.FileName = InitDir + "Temp_0.Temp";
      int i = 1;
      while (File.Exists(this.FileName))
      {
        this.FileName = InitDir + "Temp_" + i.ToString() + ".Temp";
        i++;
      }
      IsSave = false;
      this.LoadZipFile();
      FileBrower();
    }

    /// <summary>
    /// 打开并加载文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button2_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = InitDir;
      ofd.Multiselect = false;
      ofd.ShowDialog();
      if(this.FileName == ofd.FileName)
      {
        MessageBox.Show("相同文件已经打开了！");
        return;
      }
      if (ofd.FileName != "")
      {
        this.FileName = ofd.FileName;
        this.LoadZipFile();
      }
      ofd.Dispose();
      FileBrower();
    }

    /// <summary>
    /// 加载压缩文件
    /// </summary>
    private void LoadZipFile()
    {
      _ZipEntity = new ZipEntity(this.FileName);
    }

    /// <summary>
    /// 显示文件浏览
    /// </summary>
    private void FileBrower()
    {
      listView1.View = View.Details;
      listView1.LabelEdit = true;
      listView1.AllowColumnReorder = true;
      //listView1.CheckBoxes = true;
      listView1.FullRowSelect = true;
      listView1.GridLines = true;
      listView1.Sorting = SortOrder.Ascending;
      listView1.Clear();
      listView1.Columns.Add("文件名",listView1.Width/3, HorizontalAlignment.Left);
      listView1.Columns.Add("大小", listView1.Width / 3, HorizontalAlignment.Left);
      listView1.Columns.Add("文件夹", listView1.Width / 3, HorizontalAlignment.Left);

      //条目表
      byte[] EntryTablesBuffer = new byte[_ZipEntity.EntrysTableLen];
      Array.Copy(_ZipEntity.Buffer, _ZipEntity.HeadLen, EntryTablesBuffer, 0, _ZipEntity.EntrysTableLen);
      //开始遍历Entry
      int iTablesOffBits = 0;
      int iFilesOffBits = 0;
      uint iCount = 1;
      while (iCount <= _ZipEntity.EntryCount)
      {
        uint EntryLen = BitConverter.ToUInt32(EntryTablesBuffer, iTablesOffBits);
        uint FileLen = BitConverter.ToUInt32(EntryTablesBuffer, iTablesOffBits + sizeof(uint) * 2 + sizeof(UInt16) * 2);
        byte[] temp = new byte[EntryLen - FileLen];
        Array.Copy(EntryTablesBuffer, iTablesOffBits, temp, 0, EntryLen - FileLen);
        Entry _entry = new Entry(temp);

        string FileDir = BitConvertHelper.Byte2String(_entry.EntryDir, 0, _entry.EntryDirLen);
        string FileName = BitConvertHelper.Byte2String(_entry.EntryName, 0, _entry.EntryNameLen);

        iTablesOffBits += (int)(EntryLen - FileLen);
        iFilesOffBits += (int)_entry.FileLen;
        iCount++;

        ListViewItem item = listView1.Items.Add(FileName);
        item.SubItems.Add(FileLen.ToString());
        item.SubItems.Add(FileDir);
      }
    }

    /// <summary>
    /// 添加文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button3_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Multiselect = true;
      ofd.InitialDirectory = InitDir;
      ofd.ShowDialog();
      string[] FileNames = ofd.FileNames;
      ofd.Dispose();
      //添加文件列表到压缩包，并显示在列表
      for (int i = 0; i < FileNames.Length; i++)
      {
        //Console.WriteLine(FileNames[i]);
        Entry entry = new Entry(FileNames[i], @"");
        _ZipEntity.Add(entry);
      }
      IsSave = false;
      this.FileBrower();
    }

    private void ZipExpress_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!CancleApp())
      {
        e.Cancel = true;
      }
      else
      {
        e.Cancel = false;
      }
    }

    private void CheckFile()
    {
      messageBoxCS = new StringBuilder();
      if (!IsSave)
      {
        messageBoxCS.AppendFormat("未保存文件: {0}", Path.GetFileName(FileName));
        messageBoxCS.AppendLine();
      }
    }

    private bool CancleApp()
    {
      this.CheckFile();
      messageBoxCS.AppendFormat("确定要退出应用程序吗？");
      messageBoxCS.AppendLine();
      if (MessageBox.Show(messageBoxCS.ToString(), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
      {
        _ZipEntity.Close();
        if (Path.GetExtension(FileName).ToLower() == ".temp")
        {
          File.Delete(FileName);
        }
        return true;
      }
      else
      {
        return false;
      }
    }

    private void button4_Click(object sender, EventArgs e)
    {
      if (Path.GetExtension(FileName).ToLower() == ".temp")
      {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.InitialDirectory = InitDir;
        sfd.DefaultExt = "z";
        sfd.ShowDialog();
        if (sfd.FileName != "")
        {
          _ZipEntity.Save(sfd.FileName);
          this.FileName = sfd.FileName;
          IsSave = true;
        }
      }
      else
      {
        _ZipEntity.Save();
        IsSave = true;
      }
    }

    private void button5_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.ShowDialog();
      if (fbd.SelectedPath != "")
      {
        _ZipEntity.UnZipAll(fbd.SelectedPath);
      }
      //_ZipEntity.UnZipAll(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\Test");
    }

    private void button6_Click(object sender, EventArgs e)
    {
      this.Close();
    }    
  }
}
