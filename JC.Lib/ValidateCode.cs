using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Drawing;

namespace JC.Lib
{
  /// <summary>
  /// 验证码生成类，使用一个页面调用本类函数，然后在image空间的url指向该页面
  /// </summary>
  public static class ValidateCode
  {
    /// <summary>
    /// 生成一个（0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z）的随机字符串
    /// </summary>
    /// <param name="codeCount"></param>
    /// <returns></returns>
    public static string CreateRandomCode(int codeCount)
    {
      string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
      string[] allCharArray = allChar.Split(',');
      string randomCode = "";
      int temp = -1;

      Random rand = new Random();
      for (int i = 0; i < codeCount; i++)
      {
        if (temp != -1)
        {
          rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
        }
        int t = rand.Next(35);
        if (temp == t)
        {
          return CreateRandomCode(codeCount);
        }
        temp = t;
        randomCode += allCharArray[t];
      }
      return randomCode;
    }

    /// <summary>
    /// 将输入的字符串生成为图片
    /// </summary>
    /// <param name="checkCode"></param>
    public static void CreateImage(string checkCode)
    {
      int iwidth = (int)(checkCode.Length * 11.5);
      Bitmap image = new Bitmap(iwidth, 20);
      Graphics g = Graphics.FromImage(image);
      Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
      Brush b = new System.Drawing.SolidBrush(Color.White);
      //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
      g.Clear(Color.Blue);
      g.DrawString(checkCode, f, b, 3, 3);

      Pen blackPen = new Pen(Color.Black, 0);
      //Random rand = new Random();
      //for (int i = 0; i < 5; i++)
      //{
      //  int y = rand.Next(image.Height);
      //  g.DrawLine(blackPen, 0, y, image.Width, y);
      //}

      System.IO.MemoryStream ms = new System.IO.MemoryStream();
      image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
      HttpContext.Current.Response.ClearContent();
      HttpContext.Current.Response.ContentType = "image/Jpeg";
      HttpContext.Current.Response.BinaryWrite(ms.ToArray());
      g.Dispose();
      image.Dispose();
    }
  }
}
