using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.EnterpriseServices;

using webCommon.IO;

namespace webCommon
{
  /// <summary>
  /// COM+组件封装
  /// </summary>
  [ComVisible(true)]
  [MustRunInClientContext]
  public class ZipEntityComPlus : ServicedComponent
  {
    public ZipEntityComPlus() { }
    private ZipEntity zip;
    private string filename = "";
    public string FileName
    {
      get { return filename; }
      set { filename = value; }
    }

    [ComVisible(true)]
    public void LoadFile()
    {
      this.zip = new ZipEntity(filename);
    }

    public void LoadFile(string ZipFile)
    {
      FileName = ZipFile;
      LoadFile();
    }

    public void AddFile(string name)
    {
      zip.Add(new Entry(name, ""));
    }

    public void Save()
    {
      zip.Save();
    }

    public void Save(string file)
    {
      zip.Save(file);
    }

    public void Close()
    {
      zip.Close();
    }

    public void UnZipAll(string SaveDir)
    {
      zip.UnZipAll(SaveDir);
    }
  }
}
