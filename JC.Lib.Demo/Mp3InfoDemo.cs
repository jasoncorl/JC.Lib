using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using JC.Lib.mp3;

namespace JC.Lib.Demo
{
  public partial class Mp3InfoDemo : Form
  {
    public Mp3InfoDemo()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Mp3FileID3 _mp3 = new Mp3FileID3(@"C:\Users\Administrator\Desktop\2iQYt0dEUr.mp3");
      //Console.WriteLine("TAGID:" + _mp3.ID3.TAGID);
      //Console.WriteLine("Title:" + _mp3.ID3.Title);
      //Console.WriteLine("Album:" + _mp3.ID3.Album);
      //Console.WriteLine("Artist:" + _mp3.ID3.Artist);
      //Console.WriteLine("Comment:" + _mp3.ID3.Comment);
      //Console.WriteLine("Year:" + _mp3.ID3.Year);
      //Console.WriteLine("Genre:" + _mp3.ID3.Genre);
    }
  }
}
