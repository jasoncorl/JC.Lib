using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;
using System.Reflection;

[assembly: WebResource("JC.Web.UI.UserControl.Resources.close.png", "image/png")]
[assembly: WebResource("JC.Web.UI.UserControl.Resources.open.png", "image/png")]
namespace JC.Web.UI.UserControl
{
  /// <summary>
  /// 控制TextBox高度+-的两个按钮
  /// </summary>
  [DefaultProperty("Text"),
  ToolboxData("<{0}:TextBoxPlus runat=server></{0}:TextBoxPlus>"), ToolboxBitmap(typeof(ImgRes), "JC.Web.UI.UserControl.Resources.close.png")]
  public class TextBoxPlus : System.Web.UI.WebControls.Panel
  {
    protected override void OnInit(EventArgs e)
    {
      //base.OnInit(e);

      //注册脚本
      if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "TextBoxPlusControl"))
      {
      StringBuilder sb = new StringBuilder("<script type=\"text/javascript\" language=\"javascript\">\n");
      sb.Append("function openTextBox(o) {o.previousSibling.previousSibling.rows += 5;}\n");
      sb.Append("function closeTextBox(o) {o.previousSibling.rows -= 5;}\n");
      sb.Append("</script>");
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "TextBoxPlusControl", sb.ToString());
      }
    }
    protected override void Render(HtmlTextWriter writer)
    {
      //base.Render(writer);
      writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ClientScript.GetWebResourceUrl(typeof(ImgRes), "JC.Web.UI.UserControl.Resources.close.png"));
      writer.AddAttribute(HtmlTextWriterAttribute.Alt, "收缩文本框");
      writer.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:pointer;");
      writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:closeTextBox(this)");
      writer.RenderBeginTag(HtmlTextWriterTag.Img);
      writer.RenderEndTag();

      writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ClientScript.GetWebResourceUrl(typeof(ImgRes), "JC.Web.UI.UserControl.Resources.open.png"));
      writer.AddAttribute(HtmlTextWriterAttribute.Alt, "展开文本框");
      writer.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:pointer;");
      writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:openTextBox(this)");
      writer.RenderBeginTag(HtmlTextWriterTag.Img);
      writer.RenderEndTag();
    }
  }
}
