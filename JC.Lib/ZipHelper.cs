using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace JC.Lib
{
  public class ZipHelper
  {
    /// <summary>
    /// �ݹ�ѹ���ļ��з���
    /// </summary>
    /// <param name="FolderToZip"></param>
    /// <param name="s"></param>
    /// <param name="ParentFolderName"></param>
    private static bool ZipFileDictory(string FolderToZip, ZipOutputStream s, string ParentFolderName)
    {
      bool res = true;
      string[] folders, filenames;
      ZipEntry entry = null;
      FileStream fs = null;
      Crc32 crc = new Crc32();

      try
      {

        //������ǰ�ļ���
        entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));  //���� ��/�� �Żᵱ�����ļ��д���
        s.PutNextEntry(entry);
        s.Flush();


        //��ѹ���ļ����ٵݹ�ѹ���ļ��� 
        filenames = Directory.GetFiles(FolderToZip);
        foreach (string file in filenames)
        {
          //��ѹ���ļ�
          fs = File.OpenRead(file);

          byte[] buffer = new byte[fs.Length];
          fs.Read(buffer, 0, buffer.Length);
          entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file)));

          entry.DateTime = DateTime.Now;
          entry.Size = fs.Length;
          fs.Close();

          crc.Reset();
          crc.Update(buffer);

          entry.Crc = crc.Value;

          s.PutNextEntry(entry);

          s.Write(buffer, 0, buffer.Length);
        }
      }
      catch
      {
        res = false;
      }
      finally
      {
        if (fs != null)
        {
          fs.Close();
          fs = null;
        }
        if (entry != null)
        {
          entry = null;
        }
        GC.Collect();
        GC.Collect(1);
      }


      folders = Directory.GetDirectories(FolderToZip);
      foreach (string folder in folders)
      {
        if (!ZipFileDictory(folder, s, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))
        {
          return false;
        }
      }

      return res;
    }

    /// <summary>
    /// ѹ��Ŀ¼
    /// </summary>
    /// <param name="FolderToZip">��ѹ�����ļ��У�ȫ·����ʽ</param>
    /// <param name="ZipedFile">ѹ������ļ�����ȫ·����ʽ</param>
    /// <returns></returns>
    private static bool ZipFileDictory(string FolderToZip, string ZipedFile, String Password)
    {
      bool res;
      if (!Directory.Exists(FolderToZip))
      {
        return false;
      }

      ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFile));
      s.SetLevel(6);
      s.Password = Password;

      res = ZipFileDictory(FolderToZip, s, "");

      s.Finish();
      s.Close();

      return res;
    }

    /// <summary>
    /// ѹ���ļ�
    /// </summary>
    /// <param name="FileToZip">Ҫ����ѹ�����ļ���</param>
    /// <param name="ZipedFile">ѹ�������ɵ�ѹ���ļ���</param>
    /// <returns></returns>
    private static bool ZipFile(string FileToZip, string ZipedFile, String Password)
    {
      //����ļ�û���ҵ����򱨴�
      if (!File.Exists(FileToZip))
      {
        throw new System.IO.FileNotFoundException("ָ��Ҫѹ�����ļ�: " + FileToZip + " ������!");
      }
      //FileStream fs = null;
      FileStream ZipFile = null;
      ZipOutputStream ZipStream = null;
      ZipEntry ZipEntry = null;

      bool res = true;
      try
      {
        ZipFile = File.OpenRead(FileToZip);
        byte[] buffer = new byte[ZipFile.Length];
        ZipFile.Read(buffer, 0, buffer.Length);
        ZipFile.Close();

        ZipFile = File.Create(ZipedFile);
        ZipStream = new ZipOutputStream(ZipFile);
        ZipStream.Password = Password;
        ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));
        ZipStream.PutNextEntry(ZipEntry);
        ZipStream.SetLevel(6);

        ZipStream.Write(buffer, 0, buffer.Length);
      }
      catch
      {
        res = false;
      }
      finally
      {
        if (ZipEntry != null)
        {
          ZipEntry = null;
        }
        if (ZipStream != null)
        {
          ZipStream.Finish();
          ZipStream.Close();
        }
        if (ZipFile != null)
        {
          ZipFile.Close();
          ZipFile = null;
        }
        GC.Collect();
        GC.Collect(1);
      }

      return res;
    }

    /// <summary>
    /// ѹ���ļ� �� �ļ���
    /// </summary>
    /// <param name="FileToZip">��ѹ�����ļ����ļ��У�ȫ·����ʽ</param>
    /// <param name="ZipedFile">ѹ�������ɵ�ѹ���ļ�����ȫ·����ʽ</param>
    /// <returns></returns>
    public static bool Zip(String FileToZip, String ZipedFile, String Password)
    {
      if (Directory.Exists(FileToZip))
      {
        return ZipFileDictory(FileToZip, ZipedFile, Password);
      }
      else if (File.Exists(FileToZip))
      {
        return ZipFile(FileToZip, ZipedFile, Password);
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// ��ѹ����(��ѹѹ���ļ���ָ��Ŀ¼)
    /// </summary>
    /// <param name="FileToUpZip">����ѹ���ļ�</param>
    /// <param name="ZipedFolder">ָ����ѹĿ��Ŀ¼</param>
    public static void UnZip(string FileToUpZip, string ZipedFolder, string Password)
    {
      if (!File.Exists(FileToUpZip))
      {
        return;
      }

      if (!Directory.Exists(ZipedFolder))
      {
        Directory.CreateDirectory(ZipedFolder);
      }

      ZipInputStream s = null;
      ZipEntry theEntry = null;

      string fileName;
      FileStream streamWriter = null;
      try
      {
        s = new ZipInputStream(File.OpenRead(FileToUpZip));
        s.Password = Password;
        while ((theEntry = s.GetNextEntry()) != null)
        {
          if (theEntry.Name != String.Empty)
          {
            fileName = Path.Combine(ZipedFolder, theEntry.Name);
            ///�ж��ļ�·���Ƿ����ļ���
            if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
            {
              Directory.CreateDirectory(fileName);
              continue;
            }
            streamWriter = File.Create(fileName);
            int size = 2048;
            byte[] data = new byte[2048];
            while (true)
            {
              size = s.Read(data, 0, data.Length);
              if (size > 0)
              {
                streamWriter.Write(data, 0, size);
              }
              else
              {
                break;
              }
            }
          }
        }
      }
      finally
      {
        if (streamWriter != null)
        {
          streamWriter.Close();
          streamWriter = null;
        }
        if (theEntry != null)
        {
          theEntry = null;
        }
        if (s != null)
        {
          s.Close();
          s = null;
        }
        GC.Collect();
        GC.Collect(1);
      }
    }
  }
}
