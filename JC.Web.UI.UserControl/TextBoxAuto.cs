using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JC.Web.UI.UserControl
{
  /// <summary>
  /// 根据内容自动伸展高度
  /// </summary>
  [DefaultProperty("Text"),
  ToolboxData("<{0}:TextBoxAuto runat=server></{0}:TextBoxAuto>"), ToolboxBitmap(typeof(ImgRes), "JC.Web.UI.UserControl.Resources.open.png")]
  public class TextBoxAuto : System.Web.UI.WebControls.TextBox
  {
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (this.Text != "")
      {
        //Graphics g = Graphics.FromImage(ImgRes.open);
        //SizeConverter k = new SizeConverter();
        //SizeF s = g.MeasureString(this.Text, new Font(new FontFamily(this.Font.Name == "" ? "Arial" : this.Font.Name), 12.00F), Convert.ToInt32(this.Width.Value));

        //this.Height = Convert.ToInt32(s.Height)+12;
        //统计字符数

        Regex _rg;
        MatchCollection _mathccoll;
        int iWC = 0;
        //汉字字数
        _rg = new Regex(@"([\u4e00-\u9fa5])");
        _mathccoll = _rg.Matches(this.Text);
        iWC += _mathccoll.Count * 2;

        //英文单词，数字串
        _rg = new Regex("([a-zA-Z0-9])");
        _mathccoll = _rg.Matches(this.Text);
        iWC += _mathccoll.Count;

        if (this.Columns == 0)
        {
          this.Columns = 10;
        }
        //当浏览器是netscape的时候列需要另外加2
        if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Browser != null)
        {
          this.Columns += 4;
        }

        this.Rows = iWC / this.Columns + ((iWC % this.Columns > 0) ? 1 : 0);
      }
      //这里加上IE和netscape的显示会不一样,蠢得死的IE滚动条在里面，每行减少了2个字符的显示
      writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowY, "hidden");
      base.Render(writer);
    }
  }
}
