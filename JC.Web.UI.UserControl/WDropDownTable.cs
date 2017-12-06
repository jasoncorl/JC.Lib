using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Web.UI.Design;
using System.Globalization;

using System.Collections.Specialized;

namespace JC.Web.UI.UserControl
{
  /// <summary>
  /// 带联想功能的下拉上下文表格，客户端分页
  /// </summary>
  [DefaultProperty("SelectedValue"), System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust"),
    ToolboxData("<{0}:WDropDownTable runat=server></{0}:WDropDownTable>"), ToolboxBitmap(typeof(ImgRes), "Resources.WDropDownTable.bmp")]
  public class WDropDownTable : DataBoundControl, INamingContainer,  IPostBackDataHandler  //System.Web.UI.WebControls.TextBox, WebControl, IDesignerFilter
  {
    private ListItemCollection items;
    private string cachedSelectedValue;
    private int cachedSelectedIndex; 
    private bool _stateLoaded;
    private static readonly object EventTextChanged = new object();

    /// <summary>
    /// 构造
    /// </summary>
    public WDropDownTable()
    {
      cachedSelectedIndex = -1;
      this.SelectedIndex = -1;
    }

    /// <summary>
    /// Events
    /// </summary>
    public event EventHandler TextChanged
    {
      add
      {
        base.Events.AddHandler(EventTextChanged, value);
      }
      remove
      {
        base.Events.RemoveHandler(EventTextChanged, value);
      }
    }

    protected virtual void OnTextChanged(EventArgs e)
    {
      EventHandler handler = (EventHandler)base.Events[EventTextChanged];
      if (handler != null)
      {
        handler(this, e);
      }
    }

    public virtual void ClearSelection()
    {
      for (int i = 0; i < this.Items.Count; i++)
      {
        this.Items[i].Selected = false;
      }
    }

    /// <summary>
    /// 等于SelectedIndex，设计时不可用
    /// </summary>
    [Browsable(false)]
    public string Text
    {
      get
      {
        return (string)this.ViewState["Text"];
      }
      set
      {
        this.ViewState["Text"] = value;
        if (base.Initialized)
        {
          base.RequiresDataBinding = true;
        }
      }
    }

    public int SelectedIndex
    {
      get
      {
        try
        {
          return Convert.ToInt32(this.Text);
        }
        catch { }
        return -1;
      }
      set
      {
        if (value < -1)
        {
          if (this.Items.Count != 0)
          {
            throw new ArgumentOutOfRangeException("value", string.Format("{0}{1}索引越界。",this.ID,"SelectedIndex"));
          }
          value = -1;
        }
        if (((this.Items.Count != 0) && (value < this.Items.Count)) || (value == -1))
        {
          this.ClearSelection();
          if (value >= 0)
          {
            this.Items[value].Selected = true;
          }
        }
        this.Text = value.ToString();
        //else if (this._stateLoaded)
        //{
        //  throw new ArgumentOutOfRangeException("value", string.Format("{0}{1}索引越界。", this.ID, "SelectedIndex"));
        //}
        this.cachedSelectedIndex = value;
      }
    }

    public ListItem SelectedItem
    {
      get
      {
        int selectedIndex = this.SelectedIndex;
        if (selectedIndex >= 0)
        {
          return this.Items[selectedIndex];
        }
        return null;
        //return new ListItem("", "", true);
      }
    }

    [Browsable(true), Category("Appearance")]
    public string SelectedValue
    {
      get
      {
        int selectedIndex = this.SelectedIndex;
        if (selectedIndex >= 0)
        {
          return this.Items[selectedIndex].Value;
        }
        return string.Empty;
      }
      set
      {
        if (this.Items.Count != 0)
        {
          if ((value == null) || (base.DesignMode && (value.Length == 0)))
          {
            this.ClearSelection();
            return;
          }
          ListItem item = this.Items.FindByValue(value);
          if ((((this.Page != null) && this.Page.IsPostBack) && this._stateLoaded) && (item == null))
          {
            this.cachedSelectedValue = string.Empty;
            //throw new ArgumentOutOfRangeException("value", string.Format("{0}{1}索引越界。", this.ID, "SelectedValue"));
          }
          if (item != null)
          {
            this.ClearSelection();
            item.Selected = true;
          }
        }
        this.cachedSelectedValue = value;
      }
    }

    [Browsable(true), Category("Appearance")]
    public string SelectedText
    {
      get
      {
        int selectedIndex = this.SelectedIndex;
        if (selectedIndex >= 0)
        {
          return this.Items[selectedIndex].Text;
        }
        return string.Empty;
      }
      set
      {
        if (this.Items.Count != 0)
        {
          if ((value == null) || (base.DesignMode && (value.Length == 0)))
          {
            this.ClearSelection();
            return;
          }
          ListItem item = this.Items.FindByText(value);
          if ((((this.Page != null) && this.Page.IsPostBack) && this._stateLoaded) && (item == null))
          {
            this.cachedSelectedValue = string.Empty;
            //throw new ArgumentOutOfRangeException("value", string.Format("{0}{1}索引越界。", this.ID, "SelectedValue"));
          }
          if (item != null)
          {
            this.ClearSelection();
            item.Selected = true;
          }
        }
        //this.cachedSelectedValue = value;
      }
    }

    public string DataTextField
    {
      get
      {
        object obj2 = this.ViewState["DataTextField"];
        if (obj2 != null)
        {
          return (string)obj2;
        }
        return string.Empty;
      }
      set
      {
        this.ViewState["DataTextField"] = value;
        if (base.Initialized)
        {
          base.RequiresDataBinding = true;
        }
      }
    }

    public string DataValueField
    {
      get
      {
        object obj2 = this.ViewState["DataValueField"];
        if (obj2 != null)
        {
          return (string)obj2;
        }
        return string.Empty;
      }
      set
      {
        this.ViewState["DataValueField"] = value;
        if (base.Initialized)
        {
          base.RequiresDataBinding = true;
        }
      }
    }
    
    public string DataTextFormatString
    {
      get
      {
        object obj2 = this.ViewState["DataTextFormatString"];
        if (obj2 != null)
        {
          return (string)obj2;
        }
        return string.Empty;
      }
      set
      {
        this.ViewState["DataTextFormatString"] = value;
        if (base.Initialized)
        {
          base.RequiresDataBinding = true;
        }
      }
    }

    public bool AppendDataBoundItems
    {
      get
      {
        object obj2 = this.ViewState["AppendDataBoundItems"];
        return ((obj2 != null) && ((bool)obj2));
      }
      set
      {
        this.ViewState["AppendDataBoundItems"] = value;
        if (base.Initialized)
        {
          base.RequiresDataBinding = true;
        }
      }
    }

    internal bool SaveSelectedIndicesViewState
    {
      get
      {
        return true;
        //if ((((base.Events[EventSelectedIndexChanged] != null) || (base.Events[EventTextChanged] != null)) || (!base.IsEnabled || !this.Visible)) || ((this.AutoPostBack && (this.Page != null)) && !this.Page.ClientSupportsJavaScript))
        //{
        //  return true;
        //}
        //foreach (ListItem item in this.Items)
        //{
        //  if (!item.Enabled)
        //  {
        //    return true;
        //  }
        //}
        //Type type = base.GetType();
        //return (((type != typeof(DropDownList)) && (type != typeof(ListBox))) && ((type != typeof(CheckBoxList)) && (type != typeof(RadioButtonList))));
      }
    }

    private ArrayList cachedSelectedIndices;
    internal virtual ArrayList SelectedIndicesInternal
    {
      get
      {
        this.cachedSelectedIndices = new ArrayList(3);
        for (int i = 0; i < this.Items.Count; i++)
        {
          if (this.Items[i].Selected)
          {
            this.cachedSelectedIndices.Add(i);
          }
        }
        return this.cachedSelectedIndices;
      }
    }

    internal void ItemsLoadViewState(Object State)
    {
      Triplet tp = (Triplet)State;
      string[] t = (string[])tp.First;
      string[] v = (string[])tp.Second;
      bool[] e = (bool[])tp.Third;

      if (this.items == null) this.items = new ListItemCollection();
      for (int i = 0; i < t.Length; i++)
      {
        this.items.Add(new ListItem(t[i], v[i], e[i]));
      }
    }

    internal object ItemsSaveViewState()
    {
      if(true)// (this.saveAll)
      {
        int count = this.Items.Count;
        object[] objArray = new string[count];
        object[] objArray2 = new string[count];
        bool[] z = new bool[count];
        for (int j = 0; j < count; j++)
        {
          objArray[j] = this.Items[j].Text;
          objArray2[j] = this.Items[j].Value;
          z[j] = this.Items[j].Enabled;
        }
        return new Triplet(objArray, objArray2, z);
      }
      //ArrayList x = new ArrayList(4);
      //ArrayList y = new ArrayList(4);
      //for (int i = 0; i < this.Items.Count; i++)
      //{
      //  object obj2 = this.Items[i].SaveViewState();
      //  if (obj2 != null)
      //  {
      //    x.Add(i);
      //    y.Add(obj2);
      //  }
      //}
      //if (x.Count > 0)
      //{
      //  return new Pair(x, y);
      //}
      //return null;
    }

    //internal object ItemSaveViewState(ListItem item)
    //{
    //  string x = null;
    //  string y = null;
    //  if (item.textisdirty)
    //  {
    //    x = item.Text;
    //  }
    //  if (item.valueisdirty)
    //  {
    //    y = item.Value;
    //  }
    //  if (item.enabledisdirty)
    //  {
    //    return new Triplet(x, y, this.Enabled);
    //  }
    //  if (item.valueisdirty)
    //  {
    //    return new Pair(x, y);
    //  }
    //  if (item.textisdirty)
    //  {
    //    return x;
    //  }
    //  return null;
    //}

    protected override object SaveViewState()
    {
      //this.ViewState.Add("Text",this.Text);
      //this.ViewState.SetItemDirty("Text", false);
      //return base.SaveViewState(); 
      object x = base.SaveViewState();
      object y = ItemsSaveViewState();
      object z = null;
      if (this.SaveSelectedIndicesViewState)
      {
        z = this.SelectedIndicesInternal;
      }
      if (((z == null) && (y == null)) && (x == null))
      {
        return null;
      }
      return new Triplet(x, y, z);

    }

    protected override void LoadViewState(object savedState)
    {
      if (savedState != null)
      {
        Triplet myState = (Triplet)savedState;
        if (myState.First != null)
          base.LoadViewState(myState.First);
        if (myState.Second != null)
          ItemsLoadViewState(myState.Second);
      }
      this._stateLoaded = true;
    }

    protected override void PerformDataBinding(IEnumerable dataSource)
    {
      //base.PerformDataBinding(dataSource);
      if (dataSource != null)
      {
        bool flag = false;
        bool flag2 = false;
        string dataTextField = this.DataTextField;
        string dataValueField = this.DataValueField;
        string dataTextFormatString = this.DataTextFormatString;
        if (!this.AppendDataBoundItems)
        {
          this.Items.Clear();
        }
        ICollection is2 = dataSource as ICollection;
        if (is2 != null)
        {
          this.Items.Capacity = is2.Count + this.Items.Count;
        }
        if ((dataTextField.Length != 0) || (dataValueField.Length != 0))
        {
          flag = true;
        }
        if (dataTextFormatString.Length != 0)
        {
          flag2 = true;
        }
        foreach (object obj2 in dataSource)
        {
          ListItem item = new ListItem();
          if (flag)
          {
            if (dataTextField.Length > 0)
            {
              item.Text = DataBinder.GetPropertyValue(obj2, dataTextField, dataTextFormatString);
            }
            if (dataValueField.Length > 0)
            {
              item.Value = DataBinder.GetPropertyValue(obj2, dataValueField, null);
            }
          }
          else
          {
            if (flag2)
            {
              item.Text = string.Format(CultureInfo.CurrentCulture, dataTextFormatString, new object[] { obj2 });
            }
            else
            {
              item.Text = obj2.ToString();
            }
            item.Value = obj2.ToString();
          }
          this.Items.Add(item);
        }
      }
      if (this.cachedSelectedValue != null)
      {
        int num = -1;
        num = this.FindByValueInternal(this.cachedSelectedValue, true);
        if (-1 == num)
        {
          //throw new ArgumentOutOfRangeException("value", this.ID+"的SelectedValue越界");
        }
        if ((this.cachedSelectedIndex != -1) && (this.cachedSelectedIndex != num))
        {
          //throw new ArgumentException(this.ID + "的SelectedIndex错误");
        }
        this.SelectedIndex = num;
        this.cachedSelectedValue = null;
        this.cachedSelectedIndex = -1;
      }
      else if (this.cachedSelectedIndex != -1)
      {
        this.SelectedIndex = this.cachedSelectedIndex;
        this.cachedSelectedIndex = -1;
      }
    }

    int FindByValueInternal( string value, bool includeDisabled)
    {
      int num = 0;
      foreach (ListItem item in this.Items)
      {
        if (item.Value.Equals(value) && (includeDisabled || item.Enabled))
        {
          return num;
        }
        num++;
      }
      return -1;
    }

    [Browsable(true),
    DefaultValue(true),
    Description("控件是否允许用户输入")]
    public bool AllowEdit
    {
      get
      {
        object obj = ViewState["AllowEdit"];
        return (obj == null) ? true : (bool)obj;
      }
      set { ViewState["AllowEdit"] = value; }
    }


    [Browsable(true),
    DefaultValue(4),
    Description("每行的列数")]
    public int ColsEachRows
    {
      get
      {
        object obj = ViewState["ColsEachRows"];
        return (obj == null) ? 4 : (int)obj;
      }
      set { ViewState["ColsEachRows"] = value; }
    }

    public virtual ListItemCollection Items
    {
      get
      {
        if (this.items == null)
        {
          this.items = new ListItemCollection();
          //if (base.IsTrackingViewState)
          //{
            //this.items.TrackViewState();
          //}
        }
        return this.items;
      }
    }
    
    protected override HtmlTextWriterTag TagKey
    {
      get
      {
        return HtmlTextWriterTag.Input;
      }
    }

    public override void DataBind()
    {
      base.DataBind();
    }
    /*
     * */
    /// <summary>
    /// 将控件的 HTML 开始标记呈现到指定的编写器中
    /// </summary>
    /// <param name="writer"></param>
    public override void RenderBeginTag(HtmlTextWriter writer)
    {
      //Attributes.Add("SelectedText", "");
      //Attributes.Add("SelectedValue", "");
      //Attributes.Add("SelectedIndex", "");
      base.RenderBeginTag(writer);
    }

    protected override void AddAttributesToRender(HtmlTextWriter writer)
    {
      writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
      writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID);
      writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Text);
      writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
      base.AddAttributesToRender(writer);
    }

    /// <summary>
    /// 将控件的 HTML 结束标记呈现到指定的编写器中
    /// </summary>
    /// <param name="writer"></param>
    public override void RenderEndTag(HtmlTextWriter writer)
    {
      base.RenderEndTag(writer);
    }

    /// <summary>
    /// 将控件的内容呈现到指定的编写器中
    /// </summary>
    /// <param name="writer"></param>
    protected override void RenderContents(HtmlTextWriter writer)
    {
      string sDataDivName = this.ClientID + "_DataDiv";
      //int SelectedIndex = 0;
      //string TextUi = "";
      //try 
      //{ 
      //  SelectedIndex=Convert.ToInt32(this.Text);
      //  TextUi = ;
      //}
      //catch{}
      //用户UI框
      writer.AddAttribute("onclick", "showDivUi(" + sDataDivName + ");event.cancelBubble=true;");//显示浮动层
      writer.AddAttribute("onkeyup", "Filter(this);event.cancelBubble=true;");//过滤
      writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_UI");
      writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + "_UI");
      writer.AddAttribute(HtmlTextWriterAttribute.Value, (this.SelectedItem==null)?string.Empty:this.SelectedItem.Text);
      writer.RenderBeginTag(HtmlTextWriterTag.Input);
      writer.RenderEndTag();
      //下拉按钮
      writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
      writer.AddAttribute(HtmlTextWriterAttribute.Value, "▼");
      writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align:center;width:24px;");//text-indent:-6px，IE兼容性视图偏移
      writer.AddAttribute("onclick", "popDivUi(" + sDataDivName + ");event.cancelBubble=true;");//显示浮动层
      writer.RenderBeginTag(HtmlTextWriterTag.Input);
      writer.RenderEndTag();
      //数据浮动层,显示为table
      writer.AddAttribute(HtmlTextWriterAttribute.Id, sDataDivName);
      writer.AddAttribute(HtmlTextWriterAttribute.Name, sDataDivName);
      writer.AddAttribute(HtmlTextWriterAttribute.Style, "position:static;display:none; width:280px; height:auto; border:solid 1px gray; background-color:Yellow; table-layout:fixed;");
      writer.AddAttribute("onclick", "event.cancelBubble=true;");
      writer.RenderBeginTag(HtmlTextWriterTag.Div);
      writer.RenderBeginTag(HtmlTextWriterTag.Table);
      for (int i = 0; i < this.Items.Count; i++)
      {
        if (i % this.ColsEachRows == 0) writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        writer.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:hand; border-bottom:buttonshadow 1px solid; border-right:buttonshadow 1px solid; border-left:buttonhighlight 1px solid; border-top:buttonhighlight 1px solid");
        writer.AddAttribute("onmouseover", "mouseover(this)");
        writer.AddAttribute("onmouseout", "mouseout(this)");
        writer.AddAttribute("onmousedown", "mousedown(this)");
        writer.AddAttribute("onmouseup", "mouseup(this)");
        writer.AddAttribute("onclick", "setSelected(this)");
        writer.AddAttribute("Text", this.Items[i].Text);
        writer.AddAttribute("Value", this.Items[i].Value);
        writer.AddAttribute("Index", i.ToString());
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        writer.Write(this.Items[i].Text);
        writer.RenderEndTag();
        if (i % this.ColsEachRows == this.ColsEachRows - 1) writer.RenderEndTag();
      }
      for (int i = 0; i < ColsEachRows - (this.Items.Count % ColsEachRows); i++)
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        writer.Write("&nbsp;");
        writer.RenderEndTag();
      }
      writer.RenderEndTag();  //最后一个HtmlTextWriterTag.Tr
      writer.RenderEndTag();  //HtmlTextWriterTag.Table
      //增加一个iframe可以解决被select遮挡的问题
      writer.WriteLine("<iframe frameBorder=0 width=0 scrolling=no height=0></iframe>");
      writer.RenderEndTag();  //HtmlTextWriterTag.Div

      if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "clientScript"))
      {
        writer.WriteLine("<script type=\"text/javascript\" language=\"javascript\">");
        writer.WriteLine("var oAllDiv = new Array();");

        #region 鼠标异动到某对象上的一系列函数
        writer.WriteLine("function mouseover(obj){");
        writer.WriteLine("  obj.style.borderTop = 'buttonshadow 1px solid';");
        writer.WriteLine("  obj.style.borderLeft = 'buttonshadow 1px solid';");
        writer.WriteLine("  obj.style.borderRight = 'buttonhighlight 1px solid';");
        writer.WriteLine("  obj.style.borderBottom = 'buttonhighlight 1px solid';");
        writer.WriteLine("}");
        
        writer.WriteLine("function mouseout(obj){");
        writer.WriteLine("  obj.style.borderTop = 'buttonhighlight 1px solid';");
        writer.WriteLine("  obj.style.borderLeft = 'buttonhighlight 1px solid';");
        writer.WriteLine("  obj.style.borderRight = 'buttonshadow 1px solid';");
        writer.WriteLine("  obj.style.borderBottom = 'buttonshadow 1px solid';");
        writer.WriteLine("}");

        writer.WriteLine("function mousedown(obj){");
        writer.WriteLine("  obj.style.borderTop = 'buttonshadow 1px solid';");
        writer.WriteLine("  obj.style.borderLeft = 'buttonshadow 1px solid';");
        writer.WriteLine("  obj.style.borderRight = 'buttonhighlight 1px solid';");
        writer.WriteLine("  obj.style.borderBottom = 'buttonhighlight 1px solid';");
        writer.WriteLine("}");

        writer.WriteLine("function mouseup(obj){");
        writer.WriteLine("  obj.style.borderTop = 'buttonhighlight 1px solid';");
        writer.WriteLine("  obj.style.borderLeft = 'buttonhighlight 1px solid';");
        writer.WriteLine("  obj.style.borderRight = 'buttonshadow 1px solid';");
        writer.WriteLine("  obj.style.borderBottom = 'buttonshadow 1px solid';");
        writer.WriteLine("}");

        
        writer.WriteLine("function hiddenAllDiv(){");
        writer.WriteLine("	for(var i=0;i<oAllDiv.length;i++){");
        writer.WriteLine("		hiddDivUi(oAllDiv[i]);");
        writer.WriteLine("	}");
        writer.WriteLine("}");
        #endregion

        #region 操作系列函数
        writer.WriteLine("function popDivUi(popDiv){");
        writer.WriteLine("  if(popDiv.style.display==''){popDiv.style.display = 'none'} else showDivUi(popDiv);");
        writer.WriteLine("}");

        writer.WriteLine("function showDivUi(popDiv){");
        writer.WriteLine("  popDiv.style.display = '';");
        writer.WriteLine("	for(var i=0;i<oAllDiv.length;i++){"); //隐藏其他同类DIV
        writer.WriteLine("		if(oAllDiv[i]!=popDiv) hiddDivUi(oAllDiv[i]);");
        writer.WriteLine("	}");
        writer.WriteLine("}");

        writer.WriteLine("function hiddDivUi(popDiv){");
        writer.WriteLine("  popDiv.style.display = 'none';");
        writer.WriteLine("}");

        writer.WriteLine("function setSelected(obj){");
        writer.WriteLine("  var o = obj.parentElement.parentElement.parentElement.parentElement.previousSibling.previousSibling;");
        writer.WriteLine("  o.value = obj.Text;");
        writer.WriteLine("  o.previousSibling.value = obj.Index;");
        writer.WriteLine("  hiddDivUi(obj.parentElement.parentElement.parentElement.parentElement);");
        writer.WriteLine("}");

        writer.WriteLine("function Filter(obj){");
        writer.WriteLine("//alert(obj.value);");
        writer.WriteLine("}");
        #endregion

        writer.WriteLine("</script>");
        writer.WriteLine("<script event='onclick()' for='document' type=\"text/javascript\" language=\"javascript\">hiddenAllDiv();</script>");
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "");
      }

      writer.WriteLine("<script type=\"text/javascript\" language=\"javascript\">");
      writer.WriteLine("  oAllDiv.push(" + sDataDivName + ")");
      writer.WriteLine("</script>");
    }

    #region 实现接口IPostBackDataHandler的方法
    /// <summary>
    /// 为 ASP.NET 服务器控件处理回发数据
    /// </summary>
    /// <param name="postDataKey"></param>
    /// <param name="postCollection"></param>
    /// <returns></returns>
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

    public virtual void RaisePostDataChangedEvent() {}
    #endregion

    #region 实现接口IPostBackEventHandler的方法
    /// <summary>
    /// 使服务器控件能够处理将窗体发送到服务器时引发的事件。
    /// </summary>
    /// <param name="eventArgument"></param>
    public void RaisePostBackEvent(string args)
    {
      //string s = args;
      //int pageIndex = CurrentPageIndex;
      //try
      //{
      //  if (args == null || args == "")
      //    args = inputPageIndex;
      //  pageIndex = int.Parse(args);
      //}
      //catch { }
    }
    #endregion
  }
}
