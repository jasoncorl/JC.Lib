using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace JC.Lib.mp3
{
  /// <summary>
  /// Mp3信息结构 
  /// </summary>
  public struct Mp3ID3Str
  {
    public string TAGID;     //TAG，三个字节 
    public string Title;        //歌曲名,30个字节 
    public string Artist;       //歌手名,30个字节 
    public string Album;        //所属唱片,30个字节 
    public string Year;         //年,4个字符 
    public string Comment;      //注释,30个字节 
    public string Genre;      //注释,1个字节 
  }

  public class MusicID3Tag
  {
    public byte[] TAGID = new byte[3];      //  3
    public byte[] Title = new byte[30];     //  30
    public byte[] Artist = new byte[30];    //  30 
    public byte[] Album = new byte[30];     //  30 
    public byte[] Year = new byte[4];       //  4 
    public byte[] Comment = new byte[30];   //  30 
    public byte[] Genre = new byte[1];      //  1
  }


  /// <summary>
  /// mp3文件信息类
  /// </summary>
  public class Mp3FileID3
  {
    public Mp3ID3Str ID3;

    /// <summary>
    /// 构造函数,输入文件名即得到信息
    /// </summary>
    /// <param name="FilePath"></param>
    public Mp3FileID3(String FilePath)
    {
      //GetID3V1(FilePath);
      GetID3V2(FilePath);
    }
    
    /// <summary>
    /// 获取mp3的ID3V1信息
    /// </summary>
    /// <param name="filePath"></param>
    private void GetID3V2(string filePath)
    {
      Encoding myEncoding = Encoding.GetEncoding("GB2312");

      using (FileStream fs = File.OpenRead(filePath))
      {
        byte[] TempByte;
        TempByte = new byte[3];
        fs.Read(TempByte, 0, TempByte.Length);
        Console.WriteLine(myEncoding.GetString(TempByte).Trim("\0".ToCharArray()));
        TempByte = new byte[1];
        fs.Read(TempByte, 0, TempByte.Length);
        Console.WriteLine(myEncoding.GetString(TempByte).Trim("\0".ToCharArray()));
        TempByte = new byte[1];
        fs.Read(TempByte, 0, TempByte.Length);
        Console.WriteLine(myEncoding.GetString(TempByte).Trim("\0".ToCharArray()));
        TempByte = new byte[1];
        fs.Read(TempByte, 0, TempByte.Length);
        Console.WriteLine(myEncoding.GetString(TempByte).Trim("\0".ToCharArray()));
        TempByte = new byte[4];
        fs.Read(TempByte, 0, TempByte.Length);
        Console.WriteLine(myEncoding.GetString(TempByte).Trim("\0".ToCharArray()));
      }
    }
    /// <summary>
    /// 获取mp3的ID3V1信息
    /// </summary>
    /// <param name="filePath"></param>
    private void GetID3V1(string filePath)
    {
      Encoding myEncoding = Encoding.GetEncoding("GB2312");

      using (FileStream fs = File.OpenRead(filePath))
      {
        if (fs.Length >= 128)
        {
          MusicID3Tag tag = new MusicID3Tag();

          fs.Seek(-128, SeekOrigin.End);
          fs.Read(tag.TAGID, 0, tag.TAGID.Length);
          fs.Read(tag.Title, 0, tag.Title.Length);
          fs.Read(tag.Artist, 0, tag.Artist.Length);
          fs.Read(tag.Album, 0, tag.Album.Length);
          fs.Read(tag.Year, 0, tag.Year.Length);
          fs.Read(tag.Comment, 0, tag.Comment.Length);
          fs.Read(tag.Genre, 0, tag.Genre.Length);

          ID3.TAGID = myEncoding.GetString(tag.TAGID);
          if (ID3.TAGID.Equals("TAG"))
          {
            ID3.Title = myEncoding.GetString(tag.Title).Trim("\0".ToCharArray());
            ID3.Artist = myEncoding.GetString(tag.Artist).Trim("\0".ToCharArray());
            ID3.Album = myEncoding.GetString(tag.Album).Trim("\0".ToCharArray());
            ID3.Year = myEncoding.GetString(tag.Year).Trim("\0".ToCharArray());
            ID3.Comment = myEncoding.GetString(tag.Comment).Trim("\0".ToCharArray());
            ID3.Genre = myEncoding.GetString(tag.Genre).Trim("\0".ToCharArray());
          }
        }
      }
    }
  }
}

