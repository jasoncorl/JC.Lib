using System;
using System.Text;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace JC.Web.UI.UserControl
{
	public enum ValidateType
	{
		never,z,z_num,nint,num,pnum,z_nint,strs,values,v,dt,des,time
	}
	/// <summary>
	/// wbsTextBox 的摘要说明。
	/// </summary>
	[DefaultProperty("Text"),
  ToolboxData("<{0}:WNTextBox runat=server></{0}:WNTextBox>"), ToolboxBitmap(typeof(ImgRes), "Resources.WNTextBox.bmp")]
	public class WNTextBox : System.Web.UI.WebControls.TextBox
	{
		private ValidateType valitype = ValidateType.never;
		private string functionStr = "";
		private int number=0;
		
		public WNTextBox() : base()
		{
			Width = Unit.Percentage(100);
		}

		public ValidateType CheckType
		{
			get {	return valitype;   }
			set	{	valitype = value;  }
		}

		public string OnPropertyChange
		{
			get   {	return functionStr;   }
			set	{	functionStr = value;  }
		}

		public int  CheckNumber
		{
			get {	return number;   }
			set	{	number = value;  }
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			this.BorderWidth=1;
			this.BorderColor=Color.FromName("#6B799C");
			this.Attributes.Add("onblur","CheckDataCtl(this,'"+this.valitype+"',"+this.number+");");
			if(functionStr != "")
				this.Attributes.Add("onpropertychange",functionStr);
		}
	}
}
