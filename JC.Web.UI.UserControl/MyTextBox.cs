using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;

namespace JC.Web.UI.UserControl
{
  /// <summary>
  /// 测试空间
  /// </summary>
  [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
  public class MyTextBox : DataBoundControl, IPostBackDataHandler
  {
    public String Text
    {
      get
      {
        return (String)ViewState["Text"];
      }

      set
      {
        ViewState["Text"] = value;
      }
    }


    public event EventHandler TextChanged;


    public virtual bool LoadPostData(string postDataKey,
       NameValueCollection postCollection)
    {

      String presentValue = Text;
      String postedValue = postCollection[postDataKey];

      if (presentValue == null || !presentValue.Equals(postedValue))
      {
        Text = postedValue;
        return true;
      }

      return false;
    }


    public virtual void RaisePostDataChangedEvent()
    {
      OnTextChanged(EventArgs.Empty);
    }


    protected virtual void OnTextChanged(EventArgs e)
    {
      if (TextChanged != null)
        TextChanged(this, e);
    }

    protected override HtmlTextWriterTag TagKey
    {
      get
      {
        return HtmlTextWriterTag.Input;
      }
    }

    public override void RenderBeginTag(HtmlTextWriter writer)
    {
      //Attributes.Add("SelectedText", "");
      //Attributes.Add("SelectedValue", "");
      //Attributes.Add("SelectedIndex", "");
      //Attributes.Add("type", "hidden");
      base.RenderBeginTag(writer);
    }

    public override void RenderEndTag(HtmlTextWriter writer)
    {
      base.RenderEndTag(writer);
    }

    protected override void Render(HtmlTextWriter output)
    {
      output.Write("<INPUT type= text name = " + this.UniqueID
         + " value = " + this.Text + " >");
    }
  }   
}
