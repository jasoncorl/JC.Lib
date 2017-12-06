using System;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace JC.Web.UI.UserControl
{
	public enum DateOrder
	{
		start,end
	}
	/// <summary>
	/// MyCalendar 的摘要说明。
	/// </summary>
	[DefaultProperty("Text"),
  ToolboxData("<{0}:WCTextBox runat=server></{0}:WCTextBox>"), ToolboxBitmap(typeof(ImgRes), "Resources.WCTextBox.bmp")]	
	public class WCTextBox : System.Web.UI.WebControls.TextBox
	{
		private bool cannull = true;
		private bool imgvisible = true;
		private string comparectlname = "";
		private DateOrder ordertype = DateOrder.end;
		private string imgurl = @"/Image/UserControl/open_b.gif";

		public bool NullOr
		{
			set 
			{
				cannull = value;
			}
		}

		public DateOrder Dateorder
		{
			get {		return ordertype;   }
			set	{	ordertype = value;  }
		}

		public string CompareCtl
		{
			get {		return comparectlname;	  }
			set	{	comparectlname = value;  }
		}

		public string ImgUrl 
		{
			get {	return imgurl;	  }
			set	{	imgurl = value;  }
		}

		public bool ImgVisible
		{
			get   {	return imgvisible;   }
			set	{	imgvisible = value;  }
		}

		//控件初始化
		protected override void OnInit(EventArgs e)
		{
			this.BorderWidth=1;
			this.BorderColor=Color.FromName("#6B799C");
			base.OnInit (e);
			this.Attributes["onblur"]="CheckDataCtl(this,'dt');";
		}

	
		//要写出到的 HTML 编写器
		protected override void Render(HtmlTextWriter output)
		{
			if(comparectlname != "")
			{
				System.Web.UI.Control Control = this.Page.FindControl(comparectlname);
				if(Control == null)
					Control = this.Parent.FindControl(comparectlname);
				if(Control!=null)
				{
					this.Attributes["onblur"] = "if(CheckDataCtl(this,'dt')) CompareDate(this,'"+Control.ClientID+"','"+ordertype+"')";
					this.Attributes["onpropertychange"] = "this.focus()";
					//this.Attributes["onpropertychange"] = "CompareDate(this,'"+Control.ClientID+"','"+ordertype+"')";
				}
			}
			if(this.Text.Trim().EndsWith("0:00:00"))
				this.Text = this.Text.Replace("0:00:00","");
			base.Render(output);			
			output.Write("<IMG src='"+this.imgurl+"' OnMouseOver=\"this.style.cursor='hand';\" ");
			if( ! this.imgvisible || this.Enabled==false)
				output.Write("  style='VISIBILITY: hidden'");
			output.Write(" width='16' height='16' align='absMiddle' onclick=\"show_mycalendar('"+this.ClientID+"');\">");			
		}
	}
}
