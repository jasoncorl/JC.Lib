using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace JC.Web.UI.UserControl
{
//	enum Type{status,alert,confirm,close}
	/// <summary>
	/// WLabel ��ժҪ˵����
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:WLabel runat=server></{0}:WLabel>")]
	public class WLabel : System.Web.UI.WebControls.Label
	{
		private string strscript="";
	
		[Bindable(true), 
			Category("Appearance"), 
			DefaultValue("")] 
		public string Script 
		{
			get	{	return strscript;	}
			set	{	strscript = value;	}
		}

       // ���˿ؼ����ָ�ָ�������������
		protected override void Render(HtmlTextWriter output)
		{
			base.Text = "<script language='javascript'>"+strscript+"</script>";
			base.Render(output);
		}
	}
}
