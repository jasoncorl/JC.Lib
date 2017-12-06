using System;
using System.IO;
using System.Web;
using System.Text;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace JC.Web.UI.UserControl
{
	/// <summary>
	/// WbsUpFile ��ժҪ˵����
	/// </summary>
	[DefaultProperty("Text"),
    ToolboxData("<{0}:WUpFile runat=server></{0}:WUpFile>"), ToolboxBitmap(typeof(ImgRes), "Resources.WUpFile.bmp")]
	public class WUpFile : System.Web.UI.WebControls.TextBox,INamingContainer
	{
		private StringBuilder doc;	
		private const String REGEXP_IS_VALID_INT  = @"^\d{1,}$";			//����У�鳣��
		private TextBox filename = new TextBox();		

		[Browsable(true),
		DefaultValue(""),
		Description("�ļ����Ƽ�·��")]
		public string FilePathName
		{
			get
			{				
				return (this.Text == "" )?String.Empty:this.Text;
			}
			set
			{
				this.Text = value;
				filename.Text = ExtractFileShow( this.Text );				
			}
		}

		[Browsable(true),
		DefaultValue(""),
		Description("����ģ����ϴ��ļ����ļ���")]
		public string VirtualPath
		{
			get
			{
				object obj=ViewState["VirtualPath"];
				return (obj==null)?String.Empty:(string)obj;
			}
			set
			{
				ViewState["VirtualPath"] = value;
			}
		}

		[Browsable(true),
		DefaultValue("/UpFilePath/"),
		Description("�ϴ��ļ�����·��")]
		public string UpFilePath
		{
			get
			{
				object obj=ViewState["UpFilePath"];
				return (obj==null)? "/UpFilePath/":(string)obj;
			}
			set
			{
				ViewState["UpFilePath"] = value;			
			}
		}

		[Browsable(true),
		DefaultValue("100px"),
		Description("��ʾ���")]
		public  Unit ShowWidth
		{
			get 
			{
				return filename.Width;
			}
			set
			{
				filename.Width = value;
			}
		}

		protected override void CreateChildControls()
		{
			filename.ReadOnly=true;		  
			filename.BorderWidth=1;
			filename.BorderColor=Color.FromName("#6B799C");			
			filename.Attributes.Add("align","absMiddle");
			
			Controls.Add(filename);
	
			ImageButton deleteFile = new ImageButton();
			deleteFile.ImageUrl =  @"/Image/UserControl/aboutlist.gif";
			deleteFile.Width = 16;
			deleteFile.Height = 16;
			deleteFile.ToolTip = "ɾ���ļ�";
			deleteFile.Attributes.Add("align","absMiddle");
			deleteFile.Attributes.Add("onclick","if(window.document.all['"+this.ClientID+"'].value==''){return false;}");// alert('���ļ���ɾ');
			deleteFile.Click += new System.Web.UI.ImageClickEventHandler(this.deleteFile_Click);
			Controls.Add(deleteFile);
			base.CreateChildControls ();
		}

		private void deleteFile_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string FilePath = "";
			if( FilePathName != "")
			{
				FilePath = HttpContext.Current.Server.MapPath( UpFilePath + FilePathName );
				if ( File.Exists( FilePath ))
				{
					File.Delete(FilePath);
					FilePathName = "";
				}
				else
				{
					filename.Text = "";
					this.Text = "";
					return ;
				}
//				{
//					HttpContext.Current.Response.Write("<script language=jscript>alert('���ļ�������! δ�ܳɹ�ɾ��!');</script>");
//				}
			}	
		}

		public bool IsValidInt(string str_Num )
		{
			if (( str_Num != null ) && ( str_Num != "" ) )
			{
				if( (new Regex(REGEXP_IS_VALID_INT)).IsMatch(str_Num) )
					return true;
			}
			return false;
		}

		public string ExtractFileShow(string fileName)
		{
			int nPotIndex;
			fileName = Path.GetFileName( fileName );
			nPotIndex = fileName.LastIndexOf("$!$");
		
			if (( nPotIndex > 0) && ( nPotIndex < fileName.Length - 5 ))
			{
				if ( IsValidInt( fileName.Substring( nPotIndex + 3 ,3 )))
					fileName = fileName.Remove( nPotIndex, 6);	
			}
			return fileName;			
		}

		protected override void Render(HtmlTextWriter output)
		{
			this.Width=0;
			string vitualFilter = Page.Request.Path.TrimStart('/');
			this.filename.ToolTip = vitualFilter;
			this.VirtualPath = vitualFilter.Substring(0,vitualFilter.IndexOf("/"));
			base.Render(output);
			this.RenderChildren(output);
			doc = new StringBuilder("<IMG src='/Image/UserControl/advise.gif' OnMouseOver=\"this.style.cursor='hand';\" width='16' height='16' align='absMiddle' alt='�ϴ��ļ�'");
			doc.Append("onclick=\"var cSearchValue=showModalDialog('/SysManage/UserControl/upfile.htm','?VirtualPath=" + VirtualPath + "','dialogWidth:450px;dialogHeight:146px;help:0;status:0');");
			doc.Append("if (cSearchValue==''||cSearchValue == -1 || cSearchValue == null) return false;");
			doc.Append("else { window.document.all['"+this.ClientID+"'].value=cSearchValue.split('�S')[1];");
			doc.Append("window.document.all['"+filename.UniqueID+"'].value=cSearchValue.split('�S')[0];}\">");
			output.Write(doc.ToString());	
		
			
		}
	}
}
