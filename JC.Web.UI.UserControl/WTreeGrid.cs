using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace JC.Web.UI.UserControl
{
	/// <summary>
	/// TreeGrid ��ժҪ˵����
	/// </summary>
  [DefaultProperty("Text"), ToolboxData("<{0}:TreeGrid runat=server></{0}:TreeGrid>"), ToolboxBitmap(typeof(ImgRes), "Resources.WTreeGrid.bmp")]
	[Designer("TreeGidLib.TreeGridDesigner, TreeGidLib")]
	public class WTreeGrid : System.Web.UI.WebControls.DataGrid 
	{
		private bool gridState = false;
		private string scriptSrc ="/ComScript/TreeDataGrid.js";
		private DataTable dt = new DataTable();

		#region �Զ�������
		[Bindable(true),Category("Default"),DefaultValue("/ComScript/TreeDataGrid.js")]
		[Description("��λ TreeDataGrid.js")]
		[EditorAttribute(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public string ScriptSrc
		{
			get { return scriptSrc; }
			set { scriptSrc = value; }
		}

		[Bindable(true),Category("Default"),DefaultValue(false)]
		[Description("���ñ༭״̬��")]
		public bool EditState
		{
			get { return gridState; }
			set { gridState = value; }
		}

		public override object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;

				object dataSource = value;

				if (dataSource == null) return;
				if (dataSource.GetType().Name == "DataSet")
					dt = ((DataSet)dataSource).Tables[0];
				if (dataSource.GetType().Name == "DataTable")
					dt = (DataTable)dataSource;
			}
		}

		#endregion

		/// <summary>
		/// ���˿ؼ����ָ�ָ�������������
		/// </summary>
		/// <param name="output"> Ҫд������ HTML ��д�� </param>
		protected override void Render(HtmlTextWriter output)
		{
			string hideValue = "";
			if (System.Web.HttpContext.Current != null)
				hideValue = System.Web.HttpContext.Current.Request[this.ClientID +"Nodes"];

			output.Write("<script src='"+ scriptSrc +"'></script>\n");
			output.Write("<INPUT type='hidden' name='"+ this.ClientID +"Nodes' value='"+ hideValue +"'>\n");
			base.Render(output);
			if (gridState)
				output.Write("\n<script>try {TreeDataGrid("+ this.ClientID +",2);} catch(e){}</script>");
			else
				output.Write("\n<script>try {TreeDataGrid("+ this.ClientID +",0);} catch(e){}</script>");
		}

		#region �Զ��庯��
		protected override void OnItemDataBound(DataGridItemEventArgs e)
		{
			base.OnItemDataBound (e);
			
			if (dt.Columns.Contains("ReStr") && e.Item.ItemIndex >-1)
			{
				e.Item.Attributes["ID"] = DataBinder.Eval(e.Item.DataItem,"ReStr").ToString();
			}
		}

		#endregion

	}
}
