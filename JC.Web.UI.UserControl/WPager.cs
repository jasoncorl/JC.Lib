using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;

using System.Resources;
using System.Drawing;

namespace JC.Web.UI.UserControl
{
  #region WPager Server Control

  [DefaultProperty("PageSize")]
  [DefaultEvent("PageChanged")]
  [ParseChildren(false)]
  [PersistChildren(false)]
  [Description("ר����ASP.Net WebӦ�ó���ķ�ҳ�ؼ�")]
  [Designer(typeof(PagerDesigner))]
  [ToolboxData("<{0}:WPager runat=server></{0}:WPager>"), ToolboxBitmap(typeof(ImgRes), "Resources.WPager.bmp")]
  public class WPager : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
  {
    private string cssClassName;
    private string urlPageIndexName = "page";
    private bool urlPaging = false;
    private string inputPageIndex;
    private string currentUrl = null;
    private NameValueCollection urlParams = null;

    #region Properties

    #region Navigation Buttons

    /// <summary>
    /// ��ȡ������һ��ֵ����ֵ��ʾ�����ָ����ͣ�ڵ�����ť��ʱ�Ƿ���ʾ������ʾ��
    /// </summary>
    [Browsable(true),
    Category("������ť"),
    DefaultValue(true),
    Description("ָ�������ͣ���ڵ�����ť��ʱ���Ƿ���ʾ������ʾ")]
    public bool ShowNavigationToolTip
    {
      get
      {
        object obj = ViewState["ShowNavigationToolTip"];
        return (obj == null) ? true : (bool)obj;
      }
      set
      {
        ViewState["ShowNavigationToolTip"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�����õ�����ť������ʾ�ı��ĸ�ʽ��
    /// </summary>
    [Browsable(true),
    Category("������ť"),
    DefaultValue("ת����{0}ҳ"),
    Description("ҳ������ť������ʾ�ı��ĸ�ʽ")]
    public string NavigationToolTipTextFormatString
    {
      get
      {
        object obj = ViewState["NavigationToolTipTextFormatString"];
        return (obj == null) ? "ת����{0}ҳ" : (string)obj;
      }
      set
      {
        string tip = value;
        if (tip.Trim().Length < 1 && tip.IndexOf("{0}") < 0)
          tip = "{0}";
        ViewState["NavigationToolTipTextFormatString"] = tip;
      }
    }

    /// <summary>
    /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ�ҳ������ť���������ִ��档
    /// </summary>
    /// <remarks>
    /// ����ֵ��Ϊtrue����δʹ��ͼƬ��ťʱ��ҳ������ť�е���ֵ1��2��3�Ƚ��ᱻ�����ַ�һ���������ȴ��档
    /// </remarks>
    [Browsable(true),
    Category("������ť"),
    DefaultValue(false),
    Description("�Ƿ�ҳ������ֵ��ť����������һ���������ȴ���")]
    public bool ChinesePageIndex
    {
      get
      {
        object obj = ViewState["ChinesePageIndex"];
        return (obj == null) ? false : (bool)obj;
      }
      set
      {
        ViewState["ChinesePageIndex"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������ҳ������ֵ������ť�����ֵ���ʾ��ʽ��
    /// </summary>
    /// <value>
    /// �ַ�����ָ��ҳ������ֵ��ť�����ֵ���ʾ��ʽ��Ĭ��ֵΪ<see cref="String.Empty"/>����δ���ø����ԡ�</value>
    /// <remarks>
    /// ʹ��NumericButtonTextFormatString����ָ��ҳ������ֵ��ť����ʾ��ʽ����δ���ø�ֵʱ������ť�ı������ǣ�1 2 3 ...�����ø�ֵ���ı�������ť�ı�����ʾ��ʽ��
    /// �罫��ֵ��Ϊ��[{0}]���������ı�����ʾΪ��[1] [2] [3] ...������ֵ��Ϊ��-{0}-�����ʹ�����ı���Ϊ��-1- -2- -3- ...��
    /// </remarks>
    [Browsable(true),
    DefaultValue(""),
    Category("������ť"),
    Description("ҳ������ֵ��ť�����ֵ���ʾ��ʽ")]
    public string NumericButtonTextFormatString
    {
      get
      {
        object obj = ViewState["NumericButtonTextFormatString"];
        return (obj == null) ? String.Empty : (string)obj;
      }
      set
      {
        ViewState["NumericButtonTextFormatString"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�����÷�ҳ������ť�����ͣ���ʹ�����ֻ���ͼƬ��
    /// </summary>
    /// <remarks>
    /// Ҫʹ��ͼƬ��ť������Ҫ׼������ͼƬ����0��9��ʮ����ֵͼƬ����ShowPageIndex��Ϊtrueʱ������һҳ����һҳ����һҳ�����һҳ������ҳ��...�������ťͼƬ����ShowFirstLast��ShowPrevNext����Ϊtrueʱ����
    /// ����Ҫʹ��ǰҳ��������ֵ��ť��ͬ�ڱ��ҳ������ֵ��ť������׼����ǰҳ�����İ�ťͼƬ��
    /// ����Ҫʹ�ѽ��õĵ�һҳ����һҳ����һҳ�����һҳ��ťͼƬ��ͬ�������İ�ťͼƬ������׼�����ĸ���ť�ڽ���״̬�µ�ͼƬ��
    /// <p><b>ͼƬ�ļ��������������£�</b></p>
    /// <p>��0��9ʮ����ֵ��ťͼƬ��������Ϊ����ֵ+ButtonImageNameExtension+ButtonImageExtension�������е�ButtonImageNameExtension���Բ������ã�
    /// ButtonImageExtension��ͼƬ�ļ��ĺ�׺������ .gif�� .jpg�ȿ��������������ʾ���κ�ͼƬ�ļ����͡���ҳ������1����ͼƬ�ļ�������Ϊ��1.gif����1.jpg����
    /// ���������׻������ͼƬ�ļ�ʱ������ͨ��ָ��ButtonImageNameExtension����ֵ�����ֲ�ͬ�׵�ͼƬ�����һ��ͼƬ���Բ�����ButtonImageNameExtension����ͼƬ�ļ��������ڡ�1.gif������2.gif���ȵȣ����ڶ���ͼƬ������ButtonImageNameExtensionΪ��f����ͼƬ�ļ��������ڡ�1f.gif������2f.gif���ȵȡ�</p>
    /// <p>��һҳ��ť��ͼƬ�ļ����ԡ�first����ͷ����һҳ��ťͼƬ���ԡ�prev����ͷ����һҳ��ťͼƬ���ԡ�next����ͷ�����һҳ��ťͼƬ���ԡ�last����ͷ������ҳ��ťͼƬ���ԡ�more����ͷ���Ƿ�ʹ��ButtonImageNameExtensionȡ������ֵ��ť�����ü��Ƿ��и�����ͼƬ��</p>
    /// </remarks>
    /// <example>
    /// ���´���Ƭ��ʾ�����ʹ��ͼƬ��ť��
    /// <p>
    /// <code><![CDATA[
    /// <Webdiyer:WPager runat="server" 
    ///		id="pager1" 
    ///		OnPageChanged="ChangePage"  
    ///		PagingButtonType="image" 
    ///		ImagePath="images" 
    ///		ButtonImageNameExtension="n" 
    ///		DisabledButtonImageNameExtension="g" 
    ///		ButtonImageExtension="gif" 
    ///		CpiButtonImageNameExtension="r" 
    ///		PagingButtonSpacing=5/>
    /// ]]>
    /// </code>
    /// </p>
    /// </example>
    [Browsable(true),
    DefaultValue(PagingButtonType.Text),
    Category("������ť"),
    Description("��ҳ������ť�����ͣ���ʹ�����ֻ���ͼƬ")]
    public PagingButtonType PagingButtonType
    {
      get
      {
        object obj = ViewState["PagingButtonType"];
        return (obj == null) ? PagingButtonType.Text : (PagingButtonType)obj;
      }
      set
      {
        ViewState["PagingButtonType"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������ҳ������ֵ��ť�����ͣ���ֵ����PagingButtonType��ΪImageʱ����Ч��
    /// </summary>
    /// <remarks>
    /// ������PagingButtonType��ΪImage���ֲ�����ҳ������ֵ��ťʹ��ͼƬʱ�����Խ���ֵ��ΪText�����ʹҳ�������ݰ�ťʹ���ı�������ͼƬ��ť��
    /// </remarks>
    [Browsable(true),
    DefaultValue(PagingButtonType.Text),
    Category("������ť"),
    Description("ҳ������ֵ��ť������")]
    public PagingButtonType NumericButtonType
    {
      get
      {
        object obj = ViewState["NumericButtonType"];
        return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
      }
      set
      {
        ViewState["NumericButtonType"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�����õ�һҳ����һҳ����һҳ�����һҳ��ť�����ͣ���ֵ����PagingButtonType��ΪImageʱ����Ч��
    /// </summary>
    /// <remarks>
    /// ������PagingButtonType��ΪImage���ֲ����õ�һҳ����һҳ����һҳ�����һҳ��ťʹ��ͼƬ������Խ���ֵ��ΪText�����ʹǰ����ĸ���ťʹ���ı�������ͼƬ��ť��
    /// </remarks>
    [Browsable(true),
    Category("������ť"),
    DefaultValue(PagingButtonType.Text),
    Description("��һҳ����һҳ����һҳ�����һҳ��ť������")]
    public PagingButtonType NavigationButtonType
    {
      get
      {
        object obj = ViewState["NavigationButtonType"];
        return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
      }
      set
      {
        ViewState["NavigationButtonType"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�����á�����ҳ����...����ť�����ͣ���ֵ����PagingButtonType��ΪImageʱ����Ч��
    /// </summary>
    /// <remarks>
    /// ������PagingButtonType��ΪImage���ֲ����ø���ҳ��...����ťʹ��ͼƬʱ�����Խ���ֵ��ΪText�����ʹ����ҳ��ťʹ���ı�������ͼƬ��ť��
    /// </remarks>
    [Browsable(true),
    Category("������ť"),
    DefaultValue(PagingButtonType.Text),
    Description("������ҳ����...����ť������")]
    public PagingButtonType MoreButtonType
    {
      get
      {
        object obj = ViewState["MoreButtonType"];
        return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
      }
      set
      {
        ViewState["MoreButtonType"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�����÷�ҳ������ť֮��ļ�ࡣ
    /// </summary>
    [Browsable(true),
    Category("������ť"),
    DefaultValue(typeof(Unit), "5px"),
    Description("��ҳ������ť֮��ļ��")]
    public Unit PagingButtonSpacing
    {
      get
      {
        object obj = ViewState["PagingButtonSpacing"];
        return (obj == null) ? Unit.Pixel(5) : (Unit.Parse(obj.ToString()));
      }
      set
      {
        ViewState["PagingButtonSpacing"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ���ҳ����Ԫ������ʾ��һҳ�����һҳ��ť��
    /// </summary>
    [Browsable(true),
    Description("�Ƿ���ҳ����Ԫ������ʾ��һҳ�����һҳ��ť"),
    Category("������ť"),
    DefaultValue(true)]
    public bool ShowFirstLast
    {
      get
      {
        object obj = ViewState["ShowFirstLast"];
        return (obj == null) ? true : (bool)obj;
      }
      set { ViewState["ShowFirstLast"] = value; }
    }

    /// <summary>
    /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ���ҳ����Ԫ������ʾ��һҳ����һҳ��ť��
    /// </summary>
    [Browsable(true),
    Description("�Ƿ���ҳ����Ԫ������ʾ��һҳ����һҳ��ť"),
    Category("������ť"),
    DefaultValue(true)]
    public bool ShowPrevNext
    {
      get
      {
        object obj = ViewState["ShowPrevNext"];
        return (obj == null) ? true : (bool)obj;
      }
      set { ViewState["ShowPrevNext"] = value; }
    }

    /// <summary>
    /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ���ҳ����Ԫ������ʾҳ������ֵ��ť��
    /// </summary>
    [Browsable(true),
    Description("�Ƿ���ҳ����Ԫ������ʾ��ֵ��ť"),
    Category("������ť"),
    DefaultValue(true)]
    public bool ShowPageIndex
    {
      get
      {
        object obj = ViewState["ShowPageIndex"];
        return (obj == null) ? true : (bool)obj;
      }
      set { ViewState["ShowPageIndex"] = value; }
    }

    /// <summary>
    /// ��ȡ������Ϊ��һҳ��ť��ʾ���ı���
    /// </summary>
    [Browsable(true),
    Description("��һҳ��ť����ʾ���ı�"),
    Category("������ť"),
    DefaultValue("<font face=\"webdings\">9</font>")]
    public string FirstPageText
    {
      get
      {
        object obj = ViewState["FirstPageText"];
        return (obj == null) ? "<font face=\"webdings\">9</font>" : (string)obj;
      }
      set { ViewState["FirstPageText"] = value; }
    }

    /// <summary>
    /// ��ȡ������Ϊ��һҳ��ť��ʾ���ı���
    /// </summary>
    [Browsable(true),
    Description("��һҳ��ť����ʾ���ı�"),
    Category("������ť"),
    DefaultValue("<font face=\"webdings\">3</font>")]
    public string PrevPageText
    {
      get
      {
        object obj = ViewState["PrevPageText"];
        return (obj == null) ? "<font face=\"webdings\">3</font>" : (string)obj;
      }
      set { ViewState["PrevPageText"] = value; }
    }

    /// <summary>
    /// ��ȡ������Ϊ��һҳ��ť��ʾ���ı���
    /// </summary>
    [Browsable(true),
    Description("��һҳ��ť����ʾ���ı�"),
    Category("������ť"),
    DefaultValue("<font face=\"webdings\">4</font>")]
    public string NextPageText
    {
      get
      {
        object obj = ViewState["NextPageText"];
        return (obj == null) ? "<font face=\"webdings\">4</font>" : (string)obj;
      }
      set { ViewState["NextPageText"] = value; }
    }

    /// <summary>
    /// ��ȡ������Ϊ���һҳ��ť��ʾ���ı���
    /// </summary>
    [Browsable(true),
    Description("���һҳ��ť����ʾ���ı�"),
    Category("������ť"),
    DefaultValue("<font face=\"webdings\">:</font>")]
    public string LastPageText
    {
      get
      {
        object obj = ViewState["LastPageText"];
        return (obj == null) ? "<font face=\"webdings\">:</font>" : (string)obj;
      }
      set { ViewState["LastPageText"] = value; }
    }

    /// <summary>
    /// ��ȡ�������� <see cref="WPager"/> �ؼ���ҳ����Ԫ����ͬʱ��ʾ����ֵ��ť����Ŀ��
    /// </summary>
    [Browsable(true),
    Description("Ҫ��ʾ��ҳ������ֵ��ť����Ŀ"),
    Category("������ť"),
    DefaultValue(10)]
    public int NumericButtonCount
    {
      get
      {
        object obj = ViewState["NumericButtonCount"];
        return (obj == null) ? 10 : (int)obj;
      }
      set { ViewState["NumericButtonCount"] = value; }
    }

    /// <summary>
    /// ��ȡ������һ��ֵ����ֵָ���Ƿ���ʾ�ѽ��õİ�ť��
    /// </summary>
    /// <remarks>
    /// ��ֵ����ָ���Ƿ���ʾ�ѽ��õķ�ҳ������ť������ǰҳΪ��һҳʱ����һҳ����һҳ��ť�������ã�����ǰҳΪ���һҳʱ����һҳ�����һҳ��ť�������ã������õİ�ťû�����ӣ��ڰ�ť�ϵ��Ҳ�������κ����á�
    /// </remarks>
    [Browsable(true),
    Category("������ť"),
    Description("�Ƿ���ʾ�ѽ��õİ�ť"),
    DefaultValue(true)]
    public bool ShowDisabledButtons
    {
      get
      {
        object obj = ViewState["ShowDisabledButtons"];
        return (obj == null) ? true : (bool)obj;
      }
      set
      {
        ViewState["ShowDisabledButtons"] = value;
      }
    }

    #endregion

    #region Image Buttons

    /// <summary>
    /// ��ȡ�����õ�ʹ��ͼƬ��ťʱ��ͼƬ�ļ���·����
    /// </summary>
    [Browsable(true),
    Category("ͼƬ��ť"),
    Description("��ʹ��ͼƬ��ťʱ��ָ��ͼƬ�ļ���·��"),
    DefaultValue(null)]
    public string ImagePath
    {
      get
      {
        string imgPath = (string)ViewState["ImagePath"];
        if (imgPath != null)
          imgPath = this.ResolveUrl(imgPath);
        return imgPath;
      }
      set
      {
        string imgPath = value.Trim().Replace("\\", "/");
        ViewState["ImagePath"] = (imgPath.EndsWith("/")) ? imgPath : imgPath + "/";
      }
    }

    /// <summary>
    /// ��ȡ�����õ�ʹ��ͼƬ��ťʱ��ͼƬ�����ͣ���gif��jpg����ֵ��ͼƬ�ļ��ĺ�׺����
    /// </summary>
    [Browsable(true),
    Category("ͼƬ��ť"),
    DefaultValue(".gif"),
    Description("��ʹ��ͼƬ��ťʱ��ͼƬ�����ͣ���gif��jpg����ֵ��ͼƬ�ļ��ĺ�׺��")]
    public string ButtonImageExtension
    {
      get
      {
        object obj = ViewState["ButtonImageExtension"];
        return (obj == null) ? ".gif" : (string)obj;
      }
      set
      {
        string ext = value.Trim();
        ViewState["ButtonImageExtension"] = (ext.StartsWith(".")) ? ext : ("." + ext);
      }
    }

    /// <summary>
    /// ��ȡ�������Զ���ͼƬ�ļ����ĺ�׺�ַ����������ֲ�ͬ���͵İ�ťͼƬ��
    /// </summary>
    /// <remarks><note>ע�⣺</note>��ֵ�����ļ���׺��������Ϊ���ֲ�ͬ��ͼƬ�ļ�����ͼƬ���м�����ַ������磺
    /// ��ǰ�����װ�ťͼƬ������һ���еġ�1����ͼƬ����Ϊ��1f.gif������һ���еġ�1����ͼƬ������Ϊ��1n.gif�������е�f��n��ΪButtonImageNameExtension��</remarks>
    [Browsable(true),
    DefaultValue(null),
    Category("ͼƬ��ť"),
    Description("�Զ���ͼƬ�ļ����ĺ�׺�ַ��������ļ���׺��������ͼƬ��1f.gif����ButtonImageNameExtension��Ϊ��f��")]
    public string ButtonImageNameExtension
    {
      get
      {
        return (string)ViewState["ButtonImageNameExtension"];
      }
      set
      {
        ViewState["ButtonImageNameExtension"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�����õ�ǰҳ������ť��ͼƬ����׺��
    /// </summary>
    /// <remarks>
    /// �� <see cref="PagingButtonType"/> ��Ϊ Image ʱ�����������������õ�ǰҳ������ֵ��ťʹ�õ�ͼƬ����׺�ַ�����˿���ʹ��ǰҳ������ť������ҳ������ťʹ�ò�ͬ��ͼƬ����δ���ø�ֵ����Ĭ��ֵΪ<see cref="ButtonImageNameExtension"/>������ǰҳ������ť������ҳ������ťʹ����ͬ��ͼƬ��
    /// </remarks>
    [Browsable(true),
    DefaultValue(null),
    Category("ͼƬ��ť"),
    Description("��ǰҳ������ť��ͼƬ����׺�ַ���")]
    public string CpiButtonImageNameExtension
    {
      get
      {
        object obj = ViewState["CpiButtonImageNameExtension"];
        return (obj == null) ? ButtonImageNameExtension : (string)obj;
      }
      set
      {
        ViewState["CpiButtonImageNameExtension"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�������ѽ��õ�ҳ������ťͼƬ����׺�ַ�����
    /// </summary>
    /// <remarks>
    /// �� <see cref="PagingButtonType"/> ��Ϊ Image ʱ�� ��ֵ�����������ѽ��ã���û�����ӣ����������޷�Ӧ����ҳ������ť��������һҳ����һҳ����һҳ�����һҳ�ĸ���ť����ͼƬ�ļ�����׺�ַ�������˿���ʹ�ѽ��õ�ҳ������ť��ͬ��������ҳ������ť����δ���ø�ֵ����Ĭ��ֵΪ<see cref="ButtonImageNameExtension"/>�����ѽ��õ�ҳ������ť��������ҳ������ťʹ����ͬ��ͼƬ��
    /// </remarks>
    [Browsable(true),
    DefaultValue(null),
    Category("ͼƬ��ť"),
    Description("�ѽ��õ�ҳ������ť��ͼƬ����׺�ַ���")]
    public string DisabledButtonImageNameExtension
    {
      get
      {
        object obj = ViewState["DisabledButtonImageNameExtension"];
        return (obj == null) ? ButtonImageNameExtension : (string)obj;
      }
      set
      {
        ViewState["DisabledButtonImageNameExtension"] = value;
      }
    }
    /// <summary>
    /// ָ����ʹ��ͼƬ��ťʱ��ͼƬ�Ķ��뷽ʽ��
    /// </summary>

    [Browsable(true),
    Description("ָ����ʹ��ͼƬ��ťʱ��ͼƬ�Ķ��뷽ʽ"),
    DefaultValue(ImageAlign.Baseline),
    Category("ͼƬ��ť")]
    public ImageAlign ButtonImageAlign
    {
      get
      {
        object obj = ViewState["ButtonImageAlign"];
        return (obj == null) ? ImageAlign.Baseline : (ImageAlign)obj;
      }
      set { ViewState["ButtonImageAlign"] = value; }
    }


    #endregion

    #region Paging

    /// <summary>
    /// ��ȡ�������Ƿ�����url�����ݷ�ҳ��Ϣ��
    /// </summary>
    /// <remarks>
    /// ����Url��ҳ��ʽ�ǽ��û������ʵ�ҳ����ͨ��Url�����ݣ����ڸ÷�ҳ��ʽ��ʹ��ҳ��������ط����������ݣ�
    /// ����ÿ�η�ҳʱ���е����ݶ��ָ�Ϊ��ʼֵ����Ҫ���»�ȡ��ʹ��Url��ҳ��ʽ��֧�ֶ�̬�ı��ҳ�ؼ�������ֵ��
    /// ����ʱ�޷����µ�����ֵͨ��Url�����ݸ���һҳ��
    /// </remarks>
    /// <example>����ʾ��˵�������WPager��Url��ҳ��ʽ��DataGrid���з�ҳ��ʹ��Access���ݿ⣩��
    /// <code><![CDATA[
    ///<%@Register TagPrefix="Webdiyer" Namespace="Wuqi.Webdiyer" Assembly="WPager"%>
    ///<%@Import Namespace="System.Data.OleDb"%>
    ///<%@ Import Namespace="System.Data"%>
    ///<%@ Page Language="C#" debug=true%>
    ///<HTML>
    ///	<HEAD>
    ///		<TITLE>Welcome to Webdiyer.com </TITLE>
    ///		<script runat="server">
    ///		OleDbConnection conn;
    ///		OleDbCommand cmd;
    ///		void Page_Load(object src,EventArgs e){
    ///		conn=new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+Server.MapPath("access/WPager.mdb"));
    ///		if(!Page.IsPostBack){
    ///		cmd=new OleDbCommand("select count(newsid) from wqnews",conn);
    ///		conn.Open();
    ///		pager.RecordCount=(int)cmd.ExecuteScalar();
    ///		conn.Close();
    ///		BindData();
    ///		}
    ///		}
    ///
    ///		void BindData(){
    ///		cmd=new OleDbCommand("select newsid,heading,source,addtime from wqnews order by addtime desc",conn);
    ///		OleDbDataAdapter adapter=new OleDbDataAdapter(cmd);
    ///		DataSet ds=new DataSet();
    ///		adapter.Fill(ds,pager.PageSize*(pager.CurrentPageIndex-1),pager.PageSize,"news");
    ///		dg.DataSource=ds.Tables["news"];
    ///		dg.DataBind();
    ///		}
    ///
    ///		void ChangePage(object src,PageChangedEventArgs e){
    ///		pager.CurrentPageIndex=e.NewPageIndex;
    ///		BindData();
    ///		}
    ///
    ///		</script>
    ///		<meta http-equiv="Content-Language" content="zh-cn">
    ///		<meta http-equiv="content-type" content="text/html;charset=gb2312">
    ///		<META NAME="Generator" CONTENT="EditPlus">
    ///		<META NAME="Author" CONTENT="Webdiyer(yhaili@21cn.com)">
    ///	</HEAD>
    ///	<body>
    ///		<form runat="server" ID="Form1">
    ///			<h2 align="center">WPager��ҳʾ��</h2>
    ///			<asp:DataGrid id="dg" runat="server" 
    ///			Width="760" CellPadding="4" Align="center" />
    ///			
    ///			<Webdiyer:WPager runat="server" id="pager" 
    ///			OnPageChanged="ChangePage" 
    ///			HorizontalAlign="center" 
    ///			style="MARGIN-TOP:10px;FONT-SIZE:16px" 
    ///			PageSize="8" 
    ///			ShowInputBox="always" 
    ///			SubmitButtonStyle="border:1px solid #000066;height:20px;width:30px" 
    ///			InputBoxStyle="border:1px #0000FF solid;text-align:center" 
    ///			SubmitButtonText="ת��" 
    ///			UrlPaging="true" 
    ///			UrlPageIndexName="pageindex" />
    ///		</form>
    ///	</body>
    ///</HTML>
    /// ]]></code>
    /// </example>
    [Browsable(true),
    Category("��ҳ"),
    DefaultValue(false),
    Description("�Ƿ�ʹ��url���ݷ�ҳ��Ϣ�ķ�ʽ����ҳ")]
    public bool UrlPaging
    {
      get
      {
        return urlPaging;
      }
      set
      {
        urlPaging = value;
      }
    }

    /// <summary>
    /// ��ȡ�����õ�����Url��ҳ��ʽʱ����url�б�ʾҪ���ݵ�ҳ�����Ĳ��������ơ�
    /// </summary>
    /// <remarks>
    /// �������������Զ���ͨ��Url����ҳ����ʱ��ʾҪ���ݵ�ҳ�����Ĳ��������ƣ��Ա��������еĲ������ظ���
    /// <p>�����Ե�Ĭ��ֵ�ǡ�page������ͨ��Url��ҳʱ����ʾ���������ַ���е�Url�����ڣ�</p>http://www.webdiyer.com/WPager/samples/datagrid_url.aspx?page=2 
    /// <p>�罫��ֵ��Ϊ��pageindex�����������Url����Ϊ��</p><p>http://www.webdiyer.com/WPager/samples/datagrid_url.aspx?pageindex=2 </p>
    /// </remarks>
    [Browsable(true),
    DefaultValue("page"),
    Category("��ҳ"),
    Description("������Url��ҳ��ʽʱ����ʾ��url�б�ʾҪ���ݵ�ҳ�����Ĳ���������")]
    public string UrlPageIndexName
    {
      get { return urlPageIndexName; }
      set { urlPageIndexName = value; }
    }

    /// <summary>
    /// ��ȡ�����õ�ǰ��ʾҳ��������
    /// </summary>
    ///<remarks>ʹ�ô�������ȷ���� WPager �ؼ��е�ǰ��ʾ��ҳ����ǰ��ʾ��ҳ�������������Ժ�ɫ����Ӵ���ʾ�������Ի������Ա�̵ķ�ʽ��������ʾ��ҳ��
    ///<p>��<b>ע�⣺</b>��ͬ��DataGrid�ؼ���CurrentPageIndex��WPager��CurrentPageIndex�����Ǵ�1��ʼ�ġ�</p></remarks>
    [ReadOnly(true),
    Browsable(false),
    Description("��ǰ��ʾҳ������"),
    Category("��ҳ"),
    DefaultValue(1),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CurrentPageIndex
    {
      get
      {
        object cpage = ViewState["CurrentPageIndex"];
        int pindex = (cpage == null) ? 1 : (int)cpage;
        if (pindex > PageCount && PageCount > 0)
          return PageCount;
        else if (pindex < 1)
          return 1;
        return pindex;
      }
      set
      {
        int cpage = value;
        if (cpage < 1)
          cpage = 1;
        else if (cpage > this.PageCount)
          cpage = this.PageCount;
        ViewState["CurrentPageIndex"] = cpage;
      }
    }

    /// <summary>
    /// ��ȡ��������Ҫ��ҳ�����м�¼��������
    /// </summary>
    /// <remarks>
    /// ��ҳ���һ�μ���ʱ��Ӧ�Ա�̷�ʽ���Ӵ洢���̻�Sql����з��ص����ݱ�������Ҫ��ҳ�ļ�¼��������������ԣ�WPager�Ὣ�䱣���ViewState�в���ҳ��ط�ʱ��ViewState�л�ȡ��ֵ����˱�����ÿ�η�ҳ��Ҫ�������ݿ��Ӱ���ҳ���ܡ�WPager����Ҫ��ҳ���������ݵ��������� <see cref="PageSize"/> ������������ʾ����������Ҫ����ҳ������ <see cref="PageCount"/>��ֵ��
    /// </remarks>
    /// <example>
    /// �����ʾ����ʾ����Ա�̷�ʽ����Sql��䷵�صļ�¼�������������ԣ�
    /// <p>
    /// <code><![CDATA[
    /// <HTML>
    /// <HEAD>
    /// <TITLE>Welcome to Webdiyer.com </TITLE>
    /// <script runat="server">
    ///		SqlConnection conn;
    ///		SqlCommand cmd;
    ///		void Page_Load(object src,EventArgs e)
    ///		{
    ///			conn=new SqlConnection(ConfigurationSettings.AppSettings["ConnStr"]);
    ///			if(!Page.IsPostBack)
    ///			{
    ///				cmd=new SqlCommand("select count(id) from news",conn);
    ///				conn.Open();
    ///				pager.RecordCount=(int)cmd.ExecuteScalar();
    ///				conn.Close();
    ///				BindData();
    ///			}
    ///		}
    ///
    ///		void BindData()
    ///		{
    ///			cmd=new SqlCommand("GetPagedNews",conn);
    ///			cmd.CommandType=CommandType.StoredProcedure;
    ///			cmd.Parameters.Add("@pageindex",pager.CurrentPageIndex);
    ///			cmd.Parameters.Add("@pagesize",pager.PageSize);
    ///			conn.Open();
    ///			dataGrid1.DataSource=cmd.ExecuteReader();
    ///			dataGrid1.DataBind();
    ///			conn.Close();
    ///		}
    ///		void ChangePage(object src,PageChangedEventArgs e)
    ///		{
    ///			pager.CurrentPageIndex=e.NewPageIndex;
    ///			BindData();
    ///		}
    ///		</script>
    ///		<meta http-equiv="Content-Language" content="zh-cn">
    ///		<meta http-equiv="content-type" content="text/html;charset=gb2312">
    ///		<META NAME="Generator" CONTENT="EditPlus">
    ///		<META NAME="Author" CONTENT="Webdiyer(yhaili@21cn.com)">
    ///	</HEAD>
    ///	<body>
    ///		<form runat="server" ID="Form1">
    ///			<asp:DataGrid id="dataGrid1" runat="server" />
    ///
    ///			<Webdiyer:WPager id="pager" runat="server" 
    ///			PageSize="8" 
    ///			NumericButtonCount="8" 
    ///			ShowCustomInfoSection="before" 
    ///			ShowInputBox="always" 
    ///			CssClass="mypager" 
    ///			HorizontalAlign="center" 
    ///			OnPageChanged="ChangePage" />
    ///
    ///		</form>
    ///	</body>
    ///</HTML>
    /// ]]>
    /// </code></p>
    /// <p>��ʾ��ʹ�õĴ洢���̴������£�</p>
    /// <code><![CDATA[
    ///CREATE procedure GetPagedNews
    ///		(@pagesize int,
    ///		@pageindex int)
    ///		as
    ///		set nocount on
    ///		declare @indextable table(id int identity(1,1),nid int)
    ///		declare @PageLowerBound int
    ///		declare @PageUpperBound int
    ///		set @PageLowerBound=(@pageindex-1)*@pagesize
    ///		set @PageUpperBound=@PageLowerBound+@pagesize
    ///		set rowcount @PageUpperBound
    ///		insert into @indextable(nid) select id from news order by addtime desc
    ///		select O.id,O.title,O.source,O.addtime from news O,@indextable t where O.id=t.nid
    ///		and t.id>@PageLowerBound and t.id<=@PageUpperBound order by t.id
    ///		set nocount off
    ///GO
    /// ]]>
    /// </code>
    /// </example>
    [Browsable(false),
    Description("Ҫ��ҳ�����м�¼����������ֵ���ڳ�������ʱ���ã�Ĭ��ֵΪ225��Ϊ���ʱ֧�ֶ����õĲ���ֵ��"),
    Category("Data"),
    DefaultValue(225)]
    public int RecordCount
    {
      get
      {
        object obj = ViewState["Recordcount"];
        return (obj == null) ? 0 : (int)obj;
      }
      set { ViewState["Recordcount"] = value; }
    }

    /// <summary>
    /// ��ȡ��ǰҳ֮��δ��ʾ��ҳ��������
    /// </summary>
    [Browsable(false),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int PagesRemain
    {
      get
      {
        return PageCount - CurrentPageIndex;
      }
    }

    /// <summary>
    /// �Ƿ�ÿҳ��ʾ�ļ�¼�����嵽Cookie
    /// </summary>
    [Browsable(true),
    Description("�Ƿ�ÿҳ��ʾ�ļ�¼�����嵽Cookie"),
    Category("��ҳ"),
    DefaultValue(true)]
    public bool PageSizeInCookie
    {
      get
      {
        object obj = ViewState["PageSizeInCookie"];
        return (obj == null) ? true : (bool)obj;
      }
      set
      {
        ViewState["PageSizeInCookie"] = value;
      }
    }

    /// <summary>
    /// �����ÿҳ��ʾ�ļ�¼���Ƿ�˽��
    /// </summary>
    [Browsable(true),
    Description("�����ÿҳ��ʾ�ļ�¼���Ƿ�˽��"),
    Category("��ҳ"),
    DefaultValue(false)]
    public bool PageSizeCookiePrivate
    {
      get
      {
        object obj = ViewState["PageSizeCookiePrivate"];
        return (obj == null) ? false : (bool)obj;
      }
      set
      {
        ViewState["PageSizeCookiePrivate"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������ÿҳ��ʾ��������
    /// </summary>
    /// <remarks>
    /// ��ֵ��ȡ���������ݳ��ֿؼ�ÿ��Ҫ��ʾ���ݱ��еĵ����ݵ�������WPager���ݸ�ֵ�� <see cref="RecordCount"/> ��������ʾ����������Ҫ����ҳ������ <see cref="PageCount"/>��ֵ��</remarks>
    /// <example>����ʾ���� <see cref="WPager"/> ����Ϊ����ÿҳ��ʾ8�����ݣ�
    /// <code>
    /// <![CDATA[
    ///  ...
    ///  <Webdiyer:WPager id="pager" runat="server" PageSize=8 OnPageChanged="ChangePage"/>
    ///  ...
    /// ]]></code></example>
    [Browsable(true),
    Description("ÿҳ��ʾ�ļ�¼��"),
    Category("��ҳ"),
    DefaultValue(20)]
    public int PageSize
    {
      get
      {
        if (PageSizeInCookie)
        {
          try
          {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["PageSize"];
            return (cookie == null) ? 20 : Convert.ToInt32(cookie.Value);
          }
          catch
          {
            return 20;
          }
        }
        else
        {
          object obj = ViewState["PageSize"];
          return (obj == null) ? 20 : (int)obj;
        }
      }
      set
      {
        if (PageSizeInCookie)
        {
          //�ķ�ʽ����������Ч
          HttpCookie cookie = new HttpCookie("Page");
          cookie.Values.Add("PageSize",value.ToString());
          HttpContext.Current.Response.Cookies.Add(cookie);
          if (PageSizeCookiePrivate)
          {
            cookie.Path = HttpContext.Current.Request.ServerVariables["URL"];
          }
          cookie.Expires = DateTime.Now.AddDays(365);
        }
        else
        {
          ViewState["PageSize"] = value;
        }
      }
    }

    /// <summary>
    /// ��ȡ�ڵ�ǰҳ֮��δ��ʾ��ʣ���¼��������
    /// </summary>
    [Browsable(false),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int RecordsRemain
    {
      get
      {
        if (CurrentPageIndex < PageCount)
          return RecordCount - (CurrentPageIndex * PageSize);
        return 0;
      }
    }


    /// <summary>
    /// ��ȡ����Ҫ��ҳ�ļ�¼��Ҫ����ҳ����
    /// </summary>
    [Browsable(false),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int PageCount
    {
      get { return (int)Math.Ceiling((double)RecordCount / (double)PageSize); }
    }


    #endregion

    #region TextBox and Submit Button

    /// <summary>
    /// ��ȡ������ҳ�����ı������ʾ��ʽ��
    /// </summary>
    /// <remarks>
    /// ҳ�����ļ��������û���ʽ����Ҫ���ʵ�ҳ����������ҳ���ǳ���ʱ����ʾҳ�����ı���ǳ������û���ת��ָ����ҳ��Ĭ������£����ı���ֻ������ҳ�����ڻ���� <see cref="ShowBoxThreshold"/> ��ֵʱ����ʾ��������ʾ��Ҫ����ı����κ�ʱ����ʾ���뽫��ֵ��ΪAlways����ϣ���κ�ʱ�򶼲���ʾ����Ӧ��ΪNever��
    ///</remarks>
    [Browsable(true),
    Description("ָ��ҳ�����ı������ʾ��ʽ"),
    Category("�ı����ύ��ť"),
    DefaultValue(ShowInputBox.Always)]
    public ShowInputBox ShowInputBox
    {
      get
      {
        object obj = ViewState["ShowInputBox"];
        return (obj == null) ? ShowInputBox.Always : (ShowInputBox)obj;
      }
      set { ViewState["ShowInputBox"] = value; }
    }

    /// <summary>
    /// ��ȡ������Ӧ����ҳ���������ı����CSS������
    /// </summary>
    [Browsable(true),
    Category("�ı����ύ��ť"),
    DefaultValue(null),
    Description("Ӧ����ҳ���������ı����CSS����")]
    public string InputBoxClass
    {
      get
      {
        return (string)ViewState["InpubBoxClass"];
      }
      set
      {
        if (value.Trim().Length > 0)
          ViewState["InputBoxClass"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������ҳ���������ı����CSS��ʽ�ı���
    /// </summary>

    [Browsable(true),
    Category("�ı����ύ��ť"),
    DefaultValue(null),
    Description("Ӧ����ҳ���������ı����CSS��ʽ�ı�")]
    public string InputBoxStyle
    {
      get
      {
        return (string)ViewState["InputBoxStyle"];
      }
      set
      {
        if (value.Trim().Length > 0)
          ViewState["InputBoxStyle"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������ҳ����ҳ���������ı���ǰ���ı��ַ���ֵ��
    /// </summary>
    [Browsable(true),
    Category("�ı����ύ��ť"),
    DefaultValue(null),
    Description("ҳ���������ı���ǰ���ı������ַ���")]
    public string TextBeforeInputBox
    {
      get
      {
        return (string)ViewState["TextBeforeInputBox"];
      }
      set
      {
        ViewState["TextBeforeInputBox"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������ҳ�����ı���������ı������ַ���ֵ��
    /// </summary>
    [Browsable(true),
    DefaultValue(null),
    Category("�ı����ύ��ť"),
    Description("ҳ���������ı������ı������ַ���")]
    public string TextAfterInputBox
    {
      get
      {
        return (string)ViewState["TextAfterInputBox"];
      }
      set
      {
        ViewState["TextAfterInputBox"] = value;
      }
    }


    /// <summary>
    /// ��ȡ�������ύ��ť�ϵ��ı���
    /// </summary>
    [Browsable(true),
    Category("�ı����ύ��ť"),
    DefaultValue("����"),
    Description("�ύ��ť�ϵ��ı�")]
    public string SubmitButtonText
    {
      get
      {
        object obj = ViewState["SubmitButtonText"];
        return (obj == null) ? "GO" : (string)obj;
      }
      set
      {
        //if (value.Trim().Length > 0) ȥ�������ݲ���Ҫ��ť���ֵ�����
          ViewState["SubmitButtonText"] = value;
      }
    }
    /// <summary>
    /// ��ȡ������Ӧ�����ύ��ť��CSS������
    /// </summary>
    [Browsable(true),
    Category("�ı����ύ��ť"),
    DefaultValue(null),
    Description("Ӧ�����ύ��ť��CSS����")]
    public string SubmitButtonClass
    {
      get
      {
        return (string)ViewState["SubmitButtonClass"];
      }
      set
      {
        ViewState["SubmitButtonClass"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������Ӧ�����ύ��ť��CSS��ʽ��
    /// </summary>
    [Browsable(true),
    Category("�ı����ύ��ť"),
    DefaultValue(null),
    Description("Ӧ�����ύ��ť��CSS��ʽ")]
    public string SubmitButtonStyle
    {
      get
      {
        return (string)ViewState["SubmitButtonStyle"];
      }
      set
      {
        ViewState["SubmitButtonStyle"] = value;
      }
    }
    /// <summary>
    /// ��ȡ�������Զ���ʾҳ���������ı���������ʼҳ����
    /// </summary>
    /// <remarks>
    /// �� <see cref="ShowInputBox"/> ��ΪAuto��Ĭ�ϣ�����Ҫ��ҳ�����ݵ���ҳ���ﵽ��ֵʱ���Զ���ʾҳ���������ı���Ĭ��ֵΪ30����ѡ� <see cref="ShowInputBox"/> ��ΪNever��Alwaysʱû���κ����á�
    /// </remarks>
    [Browsable(true),
    Description("ָ����ShowInputBox��ΪShowInputBox.Autoʱ������ҳ���ﵽ����ʱ����ʾҳ���������ı���"),
    Category("�ı����ύ��ť"),
    DefaultValue(30)]
    public int ShowBoxThreshold
    {
      get
      {
        object obj = ViewState["ShowBoxThreshold"];
        return (obj == null) ? 30 : (int)obj;
      }
      set { ViewState["ShowBoxThreshold"] = value; }
    }


    #endregion

    #region CustomInfoSection

    /// <summary>
    /// ��ȡ��������ʾ�û��Զ�����Ϣ���ķ�ʽ��
    /// </summary>
    /// <remarks>
    /// ������ֵ��ΪLeft��Rightʱ���ڷ�ҳ����Ԫ����߻��ұ߻���һ��ר�ŵ���������ʾ�й��û��Զ�����Ϣ����ΪNeverʱ����ʾ��
    /// </remarks>
    [Browsable(true),
    Description("��ʾ��ǰҳ����ҳ����Ϣ��Ĭ��ֵΪ����ʾ��ֵΪShowCustomInfoSection.Leftʱ����ʾ��ҳ����ǰ��ΪShowCustomInfoSection.Rightʱ����ʾ��ҳ������"),
    DefaultValue(ShowCustomInfoSection.Left),
    Category("�Զ�����Ϣ��")]
    public ShowCustomInfoSection ShowCustomInfoSection
    {
      get
      {
        object obj = ViewState["ShowCustomInfoSection"];
        return (obj == null) ? ShowCustomInfoSection.Left : (ShowCustomInfoSection)obj;
      }
      set { ViewState["ShowCustomInfoSection"] = value; }
    }

    /// <summary>
    /// ��ȡ�������û��Զ�����Ϣ���ı��Ķ��뷽ʽ��
    /// </summary>
    [Browsable(true),
    Category("�Զ�����Ϣ��"),
    DefaultValue(HorizontalAlign.Left),
    Description("�û��Զ�����Ϣ���ı��Ķ��뷽ʽ")]
    public HorizontalAlign CustomInfoTextAlign
    {
      get
      {
        object obj = ViewState["CustomInfoTextAlign"];
        return (obj == null) ? HorizontalAlign.Left : (HorizontalAlign)obj;
      }
      set
      {
        ViewState["CustomInfoTextAlign"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�������û��Զ�����Ϣ���Ŀ�ȡ�
    /// </summary>
    [Browsable(true),
    Category("�Զ�����Ϣ��"),
    DefaultValue(typeof(Unit), "40%"),
    Description("�û��Զ�����Ϣ���Ŀ��")]
    public Unit CustomInfoSectionWidth
    {
      get
      {
        object obj = ViewState["CustomInfoSectionWidth"];
        return (obj == null) ? Unit.Percentage(40) : (Unit)obj;
      }
      set
      {
        ViewState["CustomInfoSectionWidth"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������Ӧ�����û��Զ�����Ϣ���ļ�����ʽ��������
    /// </summary>
    [Browsable(true),
    Category("�Զ�����Ϣ��"),
    DefaultValue(null),
    Description("Ӧ�����û��Զ�����Ϣ���ļ�����ʽ������")]
    public string CustomInfoClass
    {
      get
      {
        object obj = ViewState["CustomInfoClass"];
        return (obj == null) ? CssClass : (string)obj;
      }
      set
      {
        ViewState["CustomInfoClass"] = value;
      }
    }

    /// <summary>
    /// ��ȡ������Ӧ�����û��Զ�����Ϣ����CSS��ʽ�ı���
    /// </summary>
    /// <value>�ַ���ֵ��ҪӦ�����û��Զ�����Ϣ����CSS��ʽ�ı���</value>
    [Browsable(true),
    Category("�Զ�����Ϣ��"),
    DefaultValue(null),
    Description("Ӧ�����û��Զ�����Ϣ����CSS��ʽ�ı�")]
    public string CustomInfoStyle
    {
      get
      {
        object obj = ViewState["CustomInfoStyle"];
        return (obj == null) ? GetStyleString() : (string)obj;
      }
      set
      {
        ViewState["CustomInfoStyle"] = value;
      }
    }

    /// <summary>
    /// ��ȡ����������ʾ���û��Զ�����Ϣ�����û��Զ����ı���
    /// </summary>
    [Browsable(true),
    Category("�Զ�����Ϣ��"),
    DefaultValue(null),
    Description("Ҫ��ʾ���û��Զ�����Ϣ�����û��Զ�����Ϣ�ı�")]
    public string CustomInfoText
    {
      get
      {
        return (string)ViewState["CustomInfoText"];
      }
      set
      {
        ViewState["CustomInfoText"] = value;
      }
    }

    #endregion

    #region Others

    /// <summary>
    /// ��ȡ������һ��ֵ����ֵָ���Ƿ�������ʾWPager��ҳ��������ʹҪ��ҳ������ֻ��һҳ��
    /// </summary>
    /// <remarks>
    /// Ĭ������£���Ҫ��ҳ������С����ҳʱ��WPager������ҳ������ʾ�κ����ݣ���������ֵ��Ϊtrueʱ����ʹ��ҳ��ֻ��һҳ��WPagerҲ����ʾ��ҳ����Ԫ�ء�
    /// </remarks>
    [Browsable(true),
    Category("Behavior"),
    DefaultValue(false),
    Description("������ʾ��ҳ�ؼ�����ʹҪ��ҳ������ֻҪһҳ")]
    public bool AlwaysShow
    {
      get
      {
        object obj = ViewState["AlwaysShow"];
        return (obj == null) ? false : (bool)obj;
      }
      set
      {
        ViewState["AlwaysShow"] = value;
      }
    }


    /// <summary>
    /// ��ȡ�������� WPager �������ؼ��ڿͻ��˳��ֵļ�����ʽ�� (CSS) �ࡣ
    /// </summary>
    [Browsable(true),
    Description("Ӧ���ڿؼ���CSS����"),
    Category("Appearance"),
    DefaultValue(null)]
    public override string CssClass
    {
      get { return base.CssClass; }
      set
      {
        base.CssClass = value;
        cssClassName = value;
      }
    }


    /// <summary>
    /// ��ȡ������һ��ֵ����ֵָʾ WPager �������ؼ��Ƿ��򷢳�����Ŀͻ��˱����Լ�����ͼ״̬�������Ծ���д��������Ϊfalse��
    /// </summary>
    /// <remarks><see cref="WPager"/> �������ؼ���һЩ��Ҫ�ķ�ҳ��Ϣ������ViewState�У���ʹ��Url��ҳ��ʽʱ����Ȼ��ͼ״̬�ڷ�ҳ������û���κ����ã�������ǰҳ��Ҫ�ط��������������ͼ״̬�Ա��ҳ�ؼ�����ҳ��ط����ȡ�ط�ǰ�ķ�ҳ״̬����ͨ��ҳ��ط���PostBack���ķ�ʽ����ҳʱ��ҪʹWPager��������������������ͼ״̬��
    /// <p><note>�����Բ����ܽ�ֹ�û���<![CDATA[<%@Page EnableViewState=false%> ]]>ҳָ������������ҳ�����ͼ״̬����ʹ�ô�ָ�������WPagerͨ��ҳ��ط�����ҳʱ��WPager��Ϊ�޷���ȡ�������Ϣ����������������</note></p></remarks>
    [Browsable(false),
    Description("�Ƿ����ÿؼ�����ͼ״̬�������Ե�ֵ����Ϊtrue���������û����á�"),
    DefaultValue(true),
    Category("Behavior")]
    public override bool EnableViewState
    {
      get
      {
        return base.EnableViewState;
      }
      set
      {
        base.EnableViewState = true;
      }
    }

    /// <summary>
    /// ��ȡ�����õ��û������ҳ����������Χ���������ҳ������С����Сҳ������ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��
    /// </summary>
    [Browsable(true),
    Description("���û������ҳ����������Χ���������ҳ������С����Сҳ������ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��"),
    DefaultValue("ҳ��������Χ��"),
    Category("Data")]
    public string PageIndexOutOfRangeErrorString
    {
      get
      {
        object obj = ViewState["PageIndexOutOfRangeErrorString"];
        return (obj == null) ? "ҳ��������Χ��" : (string)obj;
      }
      set
      {
        ViewState["PageIndexOutOfRangeErrorString"] = value;
      }
    }

    /// <summary>
    /// ��ȡ�����õ��û�������Ч��ҳ��������ֵ������֣�ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��
    /// </summary>
    [Browsable(true),
    Description("���û�������Ч��ҳ��������ֵ������֣�ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��"),
    DefaultValue("ҳ������Ч��"),
    Category("Data")]
    public string InvalidPageIndexErrorString
    {
      get
      {
        object obj = ViewState["InvalidPageIndexErrorString"];
        return (obj == null) ? "ҳ������Ч��" : (string)obj;
      }
      set
      {
        ViewState["InvalidPageIndexErrorString"] = value;
      }
    }




    #endregion

    #endregion

    #region Control Rendering Logic

    /// <summary>
    /// ��д <see cref="System.Web.UI.Control.OnLoad"/> ������
    /// </summary>
    /// <param name="e">�����¼����ݵ� <see cref="EventArgs"/> ����</param>
    protected override void OnLoad(EventArgs e)
    {
      if (urlPaging)
      {
        currentUrl = Page.Request.Path;
        urlParams = Page.Request.QueryString;
        string pageIndex = Page.Request.QueryString[urlPageIndexName];
        int index = 1;
        try
        {
          index = int.Parse(pageIndex);
        }
        catch { }
        OnPageChanged(new PageChangedEventArgs(index));
      }
      else
      {
        inputPageIndex = Page.Request.Form[this.UniqueID + "_input"];
      }
      //			this.Attributes.Add("style","WIDTH:(javascript:screen.width)");
      base.OnLoad(e);
    }
    /// <summary>
    /// ��д<see cref="System.Web.UI.Control.OnPreRender"/>������
    /// </summary>
    /// <param name="e">�����¼����ݵ� <see cref="EventArgs"/> ����</param>
    protected override void OnPreRender(EventArgs e)
    {
      if (PageCount > 1)
      {
        if ((ShowInputBox == ShowInputBox.Always) || (ShowInputBox == ShowInputBox.Auto && PageCount >= ShowBoxThreshold))
        {
          string checkscript = "<script language=\"Javascript\">function doCheck(el){var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(r.test(el.value)){if(RegExp.$1<1||RegExp.$1>" + PageCount.ToString() + "){alert(\"" + PageIndexOutOfRangeErrorString + "\");document.all[\'" + this.UniqueID + "_input\'].select();return false;}return true;}alert(\"" + InvalidPageIndexErrorString + "\");document.all[\'" + this.UniqueID + "_input\'].select();return false;}</script>";
          #region ������ʱ by J.C.
          //if (!Page.IsClientScriptBlockRegistered("checkinput"))
          //  Page.RegisterClientScriptBlock("checkinput", checkscript);
          //string script = "<script language=\"javascript\" > <!-- \nfunction BuildUrlString(key,value){ var loc=window.location.search.substring(1); var params=loc.split(\"&\"); if(params.length<=1||(params.length==2&&params[0].toLowerCase()==key)) return location.pathname+\"?\"+key+\"=\"+value; var newparam=\"\"; var flag=false; for(i=0;i<params.length;i++){ if(params[i].split(\"=\")[0].toLowerCase()==key.toLowerCase()){ params[i]=key+\"=\"+value; flag=true; break; } } for(i=0;i<params.length;i++){ newparam+=params[i]+\"&\"; } if(flag) newparam=newparam.substring(0,newparam.length-1); else newparam+=key+\"=\"+value; return location.pathname+\"?\"+newparam; } \n//--> </script>";
          //if (!Page.IsClientScriptBlockRegistered("BuildUrlScript"))
          //  Page.RegisterClientScriptBlock("BuildUrlScript", script);
          #endregion
          if (!Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "checkinput"))
            Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "checkinput", checkscript, false);
          string script = "<script language=\"javascript\" > <!-- \nfunction BuildUrlString(key,value){ var loc=window.location.search.substring(1); var params=loc.split(\"&\"); if(params.length<=1||(params.length==2&&params[0].toLowerCase()==key)) return location.pathname+\"?\"+key+\"=\"+value; var newparam=\"\"; var flag=false; for(i=0;i<params.length;i++){ if(params[i].split(\"=\")[0].toLowerCase()==key.toLowerCase()){ params[i]=key+\"=\"+value; flag=true; break; } } for(i=0;i<params.length;i++){ newparam+=params[i]+\"&\"; } if(flag) newparam=newparam.substring(0,newparam.length-1); else newparam+=key+\"=\"+value; return location.pathname+\"?\"+newparam; } \n//--> </script>";
          if (!Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "BuildUrlScript"))
            Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "BuildUrlScript", script, false);
        }
      }
      string cookieScript = "<script language=\"Javascript\">function setPageCookie(name, value, path) {var exdate = new Date();exdate.setDate(exdate.getDate() + 365);if (path == null) {document.cookie = name + \"=\" + value + \";expires=\" + exdate.toGMTString() + \"; path=/;\";}else {document.cookie = name + \"=\" + value + \";expires=\" + exdate.toGMTString() + \"; path=\" + path + \";\";}if (document.all.net) net_color();}</script>";
      if (!Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "PagerCookie"))
        Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "PagerCookie", cookieScript, false);
          
      base.OnPreRender(e);
    }

    /// <summary>
    /// ��д<see cref="System.Web.UI.WebControls.WebControl.AddAttributesToRender"/> ����������Ҫ���ֵ� HTML ���Ժ���ʽ��ӵ�ָ���� <see cref="System.Web.UI.HtmlTextWriter"/> ��
    /// </summary>
    /// <param name="writer"></param>
    protected override void AddAttributesToRender(HtmlTextWriter writer)
    {
      if (this.Page != null)
        this.Page.VerifyRenderingInServerForm(this);
      base.AddAttributesToRender(writer);
    }

    ///<summary>
    ///��д <see cref="System.Web.UI.WebControls.WebControl.RenderBeginTag"/> �������� <see cref="WPager"/> �ؼ��� HTML ��ʼ��������ָ���� <see cref="System.Web.UI.HtmlTextWriter"/> ��д���С�
    ///</summary>
    ///<param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    public override void RenderBeginTag(HtmlTextWriter writer)
    {
      bool showPager = (PageCount > 1 || (PageCount <= 1 && AlwaysShow));
      base.RenderBeginTag(writer);
      if (!showPager)
      {
        writer.Write("<!-----��Ϊ��ҳ��ֻ��һҳ������AlwaysShow������Ϊfalse��WPager����ʾ�κ����ݣ���Ҫ����ҳ��ֻ��һҳ���������ʾWPager���뽫AlwaysShow������Ϊtrue��");
        writer.Write("----->");
      }
      if ((ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right) && showPager)
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Name, "PagerTable");
        writer.AddAttribute(HtmlTextWriterAttribute.Id, "PagerTable");
        writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
        writer.AddAttribute(HtmlTextWriterAttribute.Style, GetStyleString());
        if (Height != Unit.Empty)
          writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
        writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
        writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
        writer.RenderBeginTag(HtmlTextWriterTag.Table);
        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        WriteCellAttributes(writer, true);
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
      }
    }

    ///<summary>
    ///��д <see cref="System.Web.UI.WebControls.WebControl.RenderEndTag"/> �������� <see cref="WPager"/> �ؼ��� HTML ������������ָ���� <see cref="System.Web.UI.HtmlTextWriter"/> ��д���С�
    ///</summary>
    ///<param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>

    public override void RenderEndTag(HtmlTextWriter writer)
    {
      if ((ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right) && (PageCount > 1 || (PageCount <= 1 && AlwaysShow)))
      {
        writer.RenderEndTag();
        writer.RenderEndTag();
        writer.RenderEndTag();
      }
      base.RenderEndTag(writer);
    }


    /// <summary>
    /// ��д <see cref="System.Web.UI.WebControls.WebControl.RenderContents"/> ���������ؼ������ݳ��ֵ�ָ�� <see cref="System.Web.UI.HtmlTextWriter"/> �ı�д���С�
    /// </summary>
    /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    protected override void RenderContents(HtmlTextWriter writer)
    {
      this.Width = Unit.Parse("100%");
      int previouspage = this.PageCount - this.PagesRemain;
      this.CustomInfoText = "�ܼ�¼��:" + this.RecordCount.ToString().Trim() + "&nbsp;&nbsp;&nbsp;�� " + previouspage.ToString() + "/" + this.PageCount.ToString() + " ҳ";
      if (PageCount <= 1 && !AlwaysShow)
        return;
      //writer.Write("<FONT style='COLOR: #0000cc' face='����'>");
      if (ShowCustomInfoSection == ShowCustomInfoSection.Left)
      {
        writer.Write(CustomInfoText);
        RenderCustomizePageSizeControl(writer);
        writer.RenderEndTag();
        WriteCellAttributes(writer, false);
        writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
        //				writer.AddAttribute(HtmlTextWriterAttribute.Valign,"middle");middle
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
      }

      int midpage = (int)((CurrentPageIndex - 1) / NumericButtonCount);
      int pageoffset = midpage * NumericButtonCount;
      int endpage = ((pageoffset + NumericButtonCount) > PageCount) ? PageCount : (pageoffset + NumericButtonCount);
      this.CreateNavigationButton(writer, "first");
      this.CreateNavigationButton(writer, "prev");
      if (ShowPageIndex)
      {
        if (CurrentPageIndex > NumericButtonCount)
          CreateMoreButton(writer, pageoffset);
        for (int i = pageoffset + 1; i <= endpage; i++)
        {
          CreateNumericButton(writer, i);
        }
        if (PageCount > NumericButtonCount && endpage < PageCount)
          CreateMoreButton(writer, endpage + 1);
      }
      this.CreateNavigationButton(writer, "next");
      this.CreateNavigationButton(writer, "last");
      if ((ShowInputBox == ShowInputBox.Always) || (ShowInputBox == ShowInputBox.Auto && PageCount >= ShowBoxThreshold))
      {
        //writer.Write("<FONT style='COLOR: #0000cc' face='����'>");
        writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;ת��");
        //writer.Write("</FONT>");
        if (TextBeforeInputBox != null)
          writer.Write(TextBeforeInputBox);
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "30px");
        //writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "18px");
        writer.AddAttribute(HtmlTextWriterAttribute.Value, CurrentPageIndex.ToString());
        if (InputBoxStyle != null && InputBoxStyle.Trim().Length > 0)
          writer.AddAttribute(HtmlTextWriterAttribute.Style, InputBoxStyle);
        if (InputBoxClass != null && InputBoxClass.Trim().Length > 0)
          writer.AddAttribute(HtmlTextWriterAttribute.Class, InputBoxClass);
        if (PageCount <= 1 && AlwaysShow)
          writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true");
        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "_input");
        string scriptRef = "doCheck(document.all[\'" + this.UniqueID + "_input\'])";
        string postRef = "if(event.keyCode==13){if(" + scriptRef + ")__doPostBack(\'" + this.UniqueID + "\',document.all[\'" + this.UniqueID + "_input\'].value);else{event.returnValue=false;}}";
        string keydownScript = "if(event.keyCode==13){if(" + scriptRef + "){event.returnValue=false;document.all[\'" + this.UniqueID + "\'][1].click();}else{event.returnValue=false;}}";
        string clickScript = "if(" + scriptRef + "){location.href=BuildUrlString(\'" + urlPageIndexName + "\',document.all[\'" + this.UniqueID + "_input\'].value)}";
        writer.AddAttribute("onkeydown", (urlPaging == true) ? keydownScript : postRef);
        writer.RenderBeginTag(HtmlTextWriterTag.Input);
        writer.RenderEndTag();
        if (TextAfterInputBox != null)
          writer.Write(TextAfterInputBox);
        writer.AddAttribute(HtmlTextWriterAttribute.Type, (urlPaging == true) ? "Button" : "submit");
        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
        writer.AddAttribute(HtmlTextWriterAttribute.Value, SubmitButtonText);

        //ResourceManager rm = new ResourceManager("JC.Web.UI.UserControl.ImgRes", typeof(ImgRes).Assembly);
        //System.Web.UI.WebResourceAttribute wr = new WebResourceAttribute("ImgRes.PagerSubmit.ico", "image/gif");
        //writer.AddAttribute(HtmlTextWriterAttribute.Background, wr.WebResource);
        //ResourceManager rm = new ResourceManager("JC.Web.UI.UserControl.ImgRes", typeof(ImgRes).Assembly);
        // (System.Drawing.Icon)rm.GetObject("PagerSubmit")
        //global::JC.Web.UI.UserControl.ImgRes.PagerSubmit
        //System.Web.UI.WebResourceAttribute wr = new WebResourceAttribute("ImgRes.PagerSubmit.gif","image/gif");

        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "BACKGROUND-ATTACHMENT: fixed; font-size:9pt; BACKGROUND-IMAGE: url(/Images/System/Search.gif); WIDTH: 30px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; POSITION: static; HEIGHT: 14pt; BORDER-BOTTOM-STYLE: none");
        //writer.AddAttribute(HtmlTextWriterAttribute.Style, "BACKGROUND-ATTACHMENT: fixed; ");
        //writer.AddAttribute(HtmlTextWriterAttribute.Value, "");//SubmitButtonText
        //				if(SubmitButtonClass!=null&&SubmitButtonClass.Trim().Length>0)
        //					writer.AddAttribute(HtmlTextWriterAttribute.Class,SubmitButtonClass);
        //				if(SubmitButtonStyle!=null&&SubmitButtonStyle.Trim().Length>0)
        //					writer.AddAttribute(HtmlTextWriterAttribute.Style,SubmitButtonStyle);
        if (PageCount <= 1 && AlwaysShow)
          writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, (urlPaging == true) ? clickScript : "return " + scriptRef);
        writer.RenderBeginTag(HtmlTextWriterTag.Input);
        writer.RenderEndTag();
      }

      if (ShowCustomInfoSection == ShowCustomInfoSection.Right)
      {
        writer.RenderEndTag();
        WriteCellAttributes(writer, false);
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        writer.Write(CustomInfoText);
        RenderCustomizePageSizeControl(writer);
      }
      //writer.Write("</FONT>");

    }

    /// <summary>
    /// ��ʾ�Զ���ÿҳ��ʾ�ļ�¼���ؼ�
    /// </summary>
    /// <param name="writer"></param>
    private void RenderCustomizePageSizeControl(HtmlTextWriter writer)
    {
      writer.Write("&nbsp;&nbsp;&nbsp;ÿҳ��ʾ");
      writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "_pageSize");
      writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
      writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "30px");
      //writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "18px");
      writer.AddAttribute(HtmlTextWriterAttribute.Value, PageSize.ToString());
      if (PageSizeCookiePrivate)
      {
        writer.AddAttribute("onblur", "setPageCookie('pageSize',this.value, '" + HttpContext.Current.Request.ServerVariables["URL"] + "');");
      }
      else
      {
        writer.AddAttribute("onblur", "setPageCookie('pageSize',this.value);");
      }

      writer.RenderBeginTag(HtmlTextWriterTag.Input);
      writer.RenderEndTag();
      writer.Write("��");
    }

    #endregion

    #region Private Helper Functions

    /// <summary>
    /// �����ؼ���Styleת��ΪCSS�ַ�����
    /// </summary>
    /// <returns></returns>
    private string GetStyleString()
    {
      if (Style.Count > 0)
      {
        string stl = null;
        string[] skeys = new string[Style.Count];
        Style.Keys.CopyTo(skeys, 0);
        for (int i = 0; i < skeys.Length; i++)
        {
          stl += String.Concat(skeys[i], ":", Style[skeys[i]], ";");
        }
        return stl;
      }
      return null;
    }

    /// <summary>
    /// Ϊ�û��Զ�����Ϣ����ҳ������ť����td������ԡ�
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="leftCell">�Ƿ�Ϊ��һ��td</param>
    private void WriteCellAttributes(HtmlTextWriter writer, bool leftCell)
    {
      string customUnit = CustomInfoSectionWidth.ToString();
      if (ShowCustomInfoSection == ShowCustomInfoSection.Left && leftCell || ShowCustomInfoSection == ShowCustomInfoSection.Right && !leftCell)
      {
        if (CustomInfoClass != null && CustomInfoClass.Trim().Length > 0)
          writer.AddAttribute(HtmlTextWriterAttribute.Class, CustomInfoClass);
        if (CustomInfoStyle != null && CustomInfoStyle.Trim().Length > 0)
          writer.AddAttribute(HtmlTextWriterAttribute.Style, CustomInfoStyle);
        writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
        writer.AddAttribute(HtmlTextWriterAttribute.Align, CustomInfoTextAlign.ToString().ToLower());
      }
      else
      {
        if (CustomInfoSectionWidth.Type == UnitType.Percentage)
        {
          customUnit = (Unit.Percentage(100 - CustomInfoSectionWidth.Value)).ToString();
          writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
        }
        writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
        writer.AddAttribute(HtmlTextWriterAttribute.Align, HorizontalAlign.Right.ToString().ToLower());
      }
      writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
    }

    /// <summary>
    /// ��ȡ��ҳ������ť�ĳ������ַ�����
    /// </summary>
    /// <param name="pageIndex">�÷�ҳ��ť���Ӧ��ҳ������</param>
    /// <returns>��ҳ������ť�ĳ������ַ�����</returns>
    private string GetHrefString(int pageIndex)
    {
      if (urlPaging)
      {
        NameValueCollection col = new NameValueCollection();
        col.Add(urlPageIndexName, pageIndex.ToString());
        return BuildUrlString(col);
      }
      //������ʱ by J.C.
      //return Page.GetPostBackClientHyperlink(this, pageIndex.ToString());
      return Page.ClientScript.GetPostBackClientHyperlink(this, pageIndex.ToString());
    }

    /// <summary>
    /// ��ʹ��Url��ҳ��ʽʱ���ڵ�ǰUrl�ϼ����ҳ���������ò������ڣ���ı���ֵ��
    /// </summary>
    /// <param name="col">Ҫ���뵽��Url�еĲ�������ֵ�ļ��ϡ�</param>
    /// <returns>��ҳ������ť�ĳ������ַ�����������ҳ������</returns>
    private string BuildUrlString(NameValueCollection col)
    {
      int i;
      string tempstr = "";
      if (urlParams == null || urlParams.Count <= 0)
      {
        for (i = 0; i < col.Count; i++)
        {
          tempstr += String.Concat("&", col.Keys[i], "=", col[i]);
        }
        return String.Concat(currentUrl, "?", tempstr.Substring(1));
      }
      NameValueCollection newCol = new NameValueCollection(urlParams);
      string[] newColKeys = newCol.AllKeys;
      for (i = 0; i < newColKeys.Length; i++)
      {
        newColKeys[i] = newColKeys[i].ToLower();
      }
      for (i = 0; i < col.Count; i++)
      {
        if (Array.IndexOf(newColKeys, col.Keys[i].ToLower()) < 0)
          newCol.Add(col.Keys[i], col[i]);
        else
          newCol[col.Keys[i]] = col[i];
      }
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      for (i = 0; i < newCol.Count; i++)
      {
        sb.Append("&");
        sb.Append(newCol.Keys[i]);
        sb.Append("=");
        sb.Append(newCol[i]);
      }
      return String.Concat(currentUrl, "?", sb.ToString().Substring(1));
    }

    /// <summary>
    /// ������һҳ����һҳ����һҳ�����һҳ��ҳ��ť��
    /// </summary>
    /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    /// <param name="btnname">��ҳ��ť����</param>
    private void CreateNavigationButton(HtmlTextWriter writer, string btnname)
    {
      if (!ShowFirstLast && (btnname == "first" || btnname == "last"))
        return;
      if (!ShowPrevNext && (btnname == "prev" || btnname == "next"))
        return;
      string linktext = "";
      bool disabled;
      int pageIndex;
      bool imgButton = (PagingButtonType == PagingButtonType.Image && NavigationButtonType == PagingButtonType.Image);
      if (btnname == "prev" || btnname == "first")
      {
        disabled = (CurrentPageIndex <= 1);
        if (!ShowDisabledButtons && disabled)
          return;
        pageIndex = (btnname == "first") ? 1 : (CurrentPageIndex - 1);
        if (imgButton)
        {
          if (!disabled)
          {
            writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
            AddToolTip(writer, pageIndex);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
          }
          else
          {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
          }
        }
        else
        {
          linktext = (btnname == "prev") ? PrevPageText : FirstPageText;
          if (disabled)
            writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
          else
          {
            WriteCssClass(writer);
            AddToolTip(writer, pageIndex);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
          }
          writer.RenderBeginTag(HtmlTextWriterTag.A);
          writer.Write(linktext);
          writer.RenderEndTag();
        }
      }
      else
      {
        disabled = (CurrentPageIndex >= PageCount);
        if (!ShowDisabledButtons && disabled)
          return;
        pageIndex = (btnname == "last") ? PageCount : (CurrentPageIndex + 1);
        if (imgButton)
        {
          if (!disabled)
          {
            writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
            AddToolTip(writer, pageIndex);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
          }
          else
          {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
          }
        }
        else
        {
          linktext = (btnname == "next") ? NextPageText : LastPageText;
          if (disabled)
            writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
          else
          {
            WriteCssClass(writer);
            AddToolTip(writer, pageIndex);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
          }
          writer.RenderBeginTag(HtmlTextWriterTag.A);
          writer.Write(linktext);
          writer.RenderEndTag();
        }
      }
      WriteButtonSpace(writer);
    }

    /// <summary>
    /// д��CSS������
    /// </summary>
    /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    private void WriteCssClass(HtmlTextWriter writer)
    {
      if (cssClassName != null && cssClassName.Trim().Length > 0)
        writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClassName);
    }

    /// <summary>
    /// ���뵼����ť��ʾ�ı���
    /// </summary>
    /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    /// <param name="pageIndex">������ť��Ӧ��ҳ������</param>
    private void AddToolTip(HtmlTextWriter writer, int pageIndex)
    {
      if (ShowNavigationToolTip)
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format(NavigationToolTipTextFormatString, pageIndex));
      }
    }

    /// <summary>
    /// ������ҳ��ֵ������ť��
    /// </summary>
    /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    /// <param name="index">Ҫ������ť��ҳ������ֵ��</param>
    private void CreateNumericButton(HtmlTextWriter writer, int index)
    {
      bool isCurrent = (index == CurrentPageIndex);
      if (PagingButtonType == PagingButtonType.Image && NumericButtonType == PagingButtonType.Image)
      {
        if (!isCurrent)
        {
          writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
          AddToolTip(writer, index);
          writer.RenderBeginTag(HtmlTextWriterTag.A);
          CreateNumericImages(writer, index, isCurrent);
          writer.RenderEndTag();
        }
        else
          CreateNumericImages(writer, index, isCurrent);
      }
      else
      {
        if (isCurrent)
        {
          writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold");
          writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
          writer.RenderBeginTag(HtmlTextWriterTag.Font);
          if (NumericButtonTextFormatString.Length > 0)
            writer.Write(String.Format(NumericButtonTextFormatString, (ChinesePageIndex == true) ? GetChinesePageIndex(index) : (index.ToString())));
          else
            writer.Write((ChinesePageIndex == true) ? GetChinesePageIndex(index) : index.ToString());
          writer.RenderEndTag();
        }
        else
        {
          WriteCssClass(writer);
          AddToolTip(writer, index);
          writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
          writer.RenderBeginTag(HtmlTextWriterTag.A);
          if (NumericButtonTextFormatString.Length > 0)
            writer.Write(String.Format(NumericButtonTextFormatString, (ChinesePageIndex == true) ? GetChinesePageIndex(index) : (index.ToString())));
          else
            writer.Write((ChinesePageIndex == true) ? GetChinesePageIndex(index) : index.ToString());
          writer.RenderEndTag();
        }
      }
      WriteButtonSpace(writer);
    }

    /// <summary>
    /// �ڷ�ҳ����Ԫ�ؼ����ո�
    /// </summary>
    /// <param name="writer"></param>
    private void WriteButtonSpace(HtmlTextWriter writer)
    {
      if (PagingButtonSpacing.Value > 0)
      {
        writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, PagingButtonSpacing.ToString());
        writer.RenderBeginTag(HtmlTextWriterTag.Span);
        writer.Write("");
        writer.RenderEndTag();
      }
    }

    /// <summary>
    /// ��ȡ����ҳ�����ַ���
    /// </summary>
    /// <param name="index">�����ַ���Ӧ��ҳ������ֵ</param>
    /// <returns>��Ӧ��ҳ������ֵ�������ַ�</returns>
    private string GetChinesePageIndex(int index)
    {
      Hashtable cnChars = new Hashtable();
      cnChars.Add("0", "��");
      cnChars.Add("1", "һ");
      cnChars.Add("2", "��");
      cnChars.Add("3", "��");
      cnChars.Add("4", "��");
      cnChars.Add("5", "��");
      cnChars.Add("6", "��");
      cnChars.Add("7", "��");
      cnChars.Add("8", "��");
      cnChars.Add("9", "��");
      string indexStr = index.ToString();
      string retStr = "";
      for (int i = 0; i < indexStr.Length; i++)
      {
        retStr = String.Concat(retStr, cnChars[indexStr[i].ToString()]);
      }
      return retStr;
    }

    /// <summary>
    /// ����ҳ����ͼƬ��ť��
    /// </summary>
    /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    /// <param name="index">ҳ������ֵ��</param>
    /// <param name="isCurrent">�Ƿ��ǵ�ǰҳ������</param>
    private void CreateNumericImages(HtmlTextWriter writer, int index, bool isCurrent)
    {
      string indexStr = index.ToString();
      for (int i = 0; i < indexStr.Length; i++)
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, indexStr[i], (isCurrent == true) ? CpiButtonImageNameExtension : ButtonImageNameExtension, ButtonImageExtension));
        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
        writer.RenderBeginTag(HtmlTextWriterTag.Img);
        writer.RenderEndTag();
      }
    }

    /// <summary>
    /// ����������ҳ����...����ť��
    /// </summary>
    /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
    /// <param name="pageIndex">���ӵ���ť��ҳ��������</param>
    private void CreateMoreButton(HtmlTextWriter writer, int pageIndex)
    {
      WriteCssClass(writer);
      writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
      AddToolTip(writer, pageIndex);
      writer.RenderBeginTag(HtmlTextWriterTag.A);
      if (PagingButtonType == PagingButtonType.Image && MoreButtonType == PagingButtonType.Image)
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, "more", ButtonImageNameExtension, ButtonImageExtension));
        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
        writer.RenderBeginTag(HtmlTextWriterTag.Img);
        writer.RenderEndTag();
      }
      else
        writer.Write("...");
      writer.RenderEndTag();
      writer.AddStyleAttribute(HtmlTextWriterStyle.Width, PagingButtonSpacing.ToString());
      writer.RenderBeginTag(HtmlTextWriterTag.Span);
      writer.RenderEndTag();
    }

    #endregion

    #region IPostBackEventHandler Implementation

    /// <summary>
    /// ʵ��<see cref="IPostBackEventHandler"/> �ӿڣ�ʹ <see cref="WPager"/> �ؼ��ܹ��������巢�͵�������ʱ�������¼���
    /// </summary>
    /// <param name="args"></param>
    public void RaisePostBackEvent(string args)
    {
      int pageIndex = CurrentPageIndex;
      try
      {
        if (args == null || args == "")
          args = inputPageIndex;
        pageIndex = int.Parse(args);
      }
      catch { }
      OnPageChanged(new PageChangedEventArgs(pageIndex));

    }


    #endregion

    #region IPostBackDataHandler Implementation

    /// <summary>
    /// ʵ�� <see cref="IPostBackDataHandler"/> �ӿڣ�Ϊ <see cref="WPager"/> �������ؼ�����ط����ݡ�
    /// </summary>
    /// <param name="pkey">�ؼ�����Ҫ��ʶ����</param>
    /// <param name="pcol">���д�������ֵ�ļ��ϡ�</param>
    /// <returns></returns>
    public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
    {
      string str = pcol[this.UniqueID + "_input"];
      if (str != null && str.Trim().Length > 0)
      {
        try
        {
          int pindex = int.Parse(str);
          if (pindex > 0 && pindex <= PageCount)
          {
            inputPageIndex = str;
            Page.RegisterRequiresRaiseEvent(this);
          }
        }
        catch
        { }
      }
      return false;
    }

    /// <summary>
    /// ʵ�� <see cref="IPostBackDataHandler"/> �ӿڣ����ź�Ҫ��������ؼ�����֪ͨ ASP.NET Ӧ�ó���ÿؼ���״̬�Ѹ��ġ�
    /// </summary>
    public virtual void RaisePostDataChangedEvent() { }

    #endregion

    #region PageChanged Event
    /// <summary>
    /// ��ҳ����Ԫ��֮һ���������û��ֹ�����ҳ�����ύʱ������
    /// </summary>
    /// <example>�����ʾ����ʾ���ΪPageChanged�¼�ָ���ͱ�д�¼���������ڸ��¼�������������°�DataGrid����ʾ�����ݣ�
    /// <code><![CDATA[
    ///<%@ Page Language="C#"%>
    ///<%@ Import Namespace="System.Data"%>
    ///<%@Import Namespace="System.Data.SqlClient"%>
    ///<%@Import Namespace="System.Configuration"%>
    ///<%@Register TagPrefix="Webdiyer" Namespace="Wuqi.Webdiyer" Assembly="WPager"%>
    ///<HTML>
    ///<HEAD>
    ///<TITLE>Welcome to Webdiyer.com </TITLE>
    ///  <script runat="server">
    ///		SqlConnection conn;
    ///		SqlCommand cmd;
    ///		void Page_Load(object src,EventArgs e)
    ///		{
    ///			conn=new SqlConnection(ConfigurationSettings.AppSettings["ConnStr"]);
    ///			if(!Page.IsPostBack)
    ///			{
    ///				cmd=new SqlCommand("GetNews",conn);
    ///				cmd.CommandType=CommandType.StoredProcedure;
    ///				cmd.Parameters.Add("@pageindex",1);
    ///				cmd.Parameters.Add("@pagesize",1);
    ///				cmd.Parameters.Add("@docount",true);
    ///				conn.Open();
    ///				pager.RecordCount=(int)cmd.ExecuteScalar();
    ///				conn.Close();
    ///				BindData();
    ///			}
    ///		}
    ///
    ///		void BindData()
    ///		{
    ///			cmd=new SqlCommand("GetNews",conn);
    ///			cmd.CommandType=CommandType.StoredProcedure;
    ///			cmd.Parameters.Add("@pageindex",pager.CurrentPageIndex);
    ///			cmd.Parameters.Add("@pagesize",pager.PageSize);
    ///			cmd.Parameters.Add("@docount",false);
    ///			conn.Open();
    ///			dataGrid1.DataSource=cmd.ExecuteReader();
    ///			dataGrid1.DataBind();
    ///			conn.Close();
    ///		}
    ///		void ChangePage(object src,PageChangedEventArgs e)
    ///		{
    ///			pager.CurrentPageIndex=e.NewPageIndex;
    ///			BindData();
    ///		}
    ///  </script>
    ///     <meta http-equiv="Content-Language" content="zh-cn">
    ///		<meta http-equiv="content-type" content="text/html;charset=gb2312">
    ///		<META NAME="Generator" CONTENT="EditPlus">
    ///		<META NAME="Author" CONTENT="Webdiyer(yhaili@21cn.com)">
    ///	</HEAD>
    ///	<body>
    ///		<form runat="server" ID="Form1">
    ///			<asp:DataGrid id="dataGrid1" runat="server" />
    ///			<Webdiyer:WPager id="pager" runat="server" PageSize="8" NumericButtonCount="8" ShowCustomInfoSection="before" ShowInputBox="always" CssClass="mypager" HorizontalAlign="center" OnPageChanged="ChangePage" />
    ///		</form>
    ///	</body>
    ///</HTML>
    /// ]]>
    /// </code>
    /// <p>��ʾ�����õ�Sql Server�洢���̴������£�</p>
    /// <code>
    /// <![CDATA[
    ///CREATE procedure GetNews
    /// 	(@pagesize int,
    ///		@pageindex int,
    ///		@docount bit)
    ///		as
    ///		set nocount on
    ///		if(@docount=1)
    ///		select count(id) from news
    ///		else
    ///		begin
    ///		declare @indextable table(id int identity(1,1),nid int)
    ///		declare @PageLowerBound int
    ///		declare @PageUpperBound int
    ///		set @PageLowerBound=(@pageindex-1)*@pagesize
    ///		set @PageUpperBound=@PageLowerBound+@pagesize
    ///		set rowcount @PageUpperBound
    ///		insert into @indextable(nid) select id from news order by addtime desc
    ///		select O.id,O.source,O.title,O.addtime from news O,@indextable t where O.id=t.nid
    ///		and t.id>@PageLowerBound and t.id<=@PageUpperBound order by t.id
    ///		end
    ///		set nocount off
    ///GO
    /// ]]>
    /// </code>
    ///</example>
    public event PageChangedEventHandler PageChanged;

    #endregion

    #region OnPageChanged Method

    /// <summary>
    /// ���� <see cref="PageChanged"/> �¼�����ʹ������Ϊ�¼��ṩ�Զ��崦�����
    /// </summary>
    /// <param name="e">һ�� <see cref="PageChangedEventArgs"/>���������¼����ݡ�</param>
    protected virtual void OnPageChanged(PageChangedEventArgs e)
    {
      if (this.PageChanged != null)
        PageChanged(this, e);
    }

    #endregion
  }


  #endregion

  #region PageChangedEventHandler Delegate
  /// <summary>
  /// ��ʾ���� <see cref="WPager.PageChanged"/> �¼��ķ�����
  /// </summary>
  public delegate void PageChangedEventHandler(object src, PageChangedEventArgs e);

  #endregion

  #region PageChangedEventArgs Class
  /// <summary>
  /// Ϊ <see cref="WPager"/> �ؼ��� <see cref="WPager.PageChanged"/> �¼��ṩ���ݡ��޷��̳д��ࡣ
  /// </summary>
  /// <remarks>
  /// �� <see cref="WPager"/> �ؼ���ҳ����Ԫ��֮һ���������û�����ҳ�����ύʱ���� <see cref="WPager.PageChanged"/> �¼���
  /// <p>�й� PageChangedEventArgs ʵ���ĳ�ʼ����ֵ�б������ PageChangedEventArgs ���캯����</p>
  /// </remarks>
  public sealed class PageChangedEventArgs : EventArgs
  {
    private readonly int _newpageindex;

    /// <summary>
    /// ʹ����ҳ��������ʼ�� PageChangedEventArgs �����ʵ����
    /// </summary>
    /// <param name="newPageIndex">�û��� <see cref="WPager"/> �ؼ���ҳѡ��Ԫ��ѡ���Ļ���ҳ�����ı������ֹ������ҳ��������</param>
    public PageChangedEventArgs(int newPageIndex)
    {
      this._newpageindex = newPageIndex;
    }

    public int NewPageIndex
    {
      get { return _newpageindex; }
    }
  }
  #endregion

  #region ShowInputBox,ShowCustomInfoSection and PagingButtonType Enumerations
  /// <summary>
  /// ָ��ҳ���������ı������ʾ��ʽ���Ա��û������ֹ�����ҳ������
  /// </summary>
  public enum ShowInputBox : byte
  {
    /// <summary>
    /// �Ӳ���ʾҳ���������ı���
    /// </summary>
    Never,
    /// <summary>
    /// �Զ���ѡ����������� <see cref="WPager.ShowBoxThreshold"/> �ɿ��Ƶ���ҳ���ﵽ����ʱ�Զ���ʾҳ���������ı���
    /// </summary>
    Auto,
    /// <summary>
    /// ������ʾҳ���������ı���
    /// </summary>
    Always
  }


  /// <summary>
  /// ָ����ǰҳ��������ҳ����Ϣ����ʾ��ʽ��
  /// </summary>
  public enum ShowCustomInfoSection : byte
  {
    /// <summary>
    /// ����ʾ��
    /// </summary>
    Never,
    /// <summary>
    /// ��ʾ��ҳ����Ԫ��֮ǰ��
    /// </summary>
    Left,
    /// <summary>
    /// ��ʾ��ҳ����Ԫ��֮��
    /// </summary>
    Right
  }

  /// <summary>
  /// ָ��ҳ������ť�����͡�
  /// </summary>
  public enum PagingButtonType : byte
  {
    /// <summary>
    /// ʹ�����ְ�ť��
    /// </summary>
    Text,
    /// <summary>
    /// ʹ��ͼƬ��ť��
    /// </summary>
    Image
  }


  #endregion

  #region WPager Control Designer
  /// <summary>
  /// <see cref="WPager"/> �������ؼ��������
  /// </summary>
  public class PagerDesigner : System.Web.UI.Design.WebControls.PanelDesigner
  {
    /// <summary>
    /// ��ʼ�� PagerDesigner ����ʵ����
    /// </summary>
    public PagerDesigner()
    {
      this.ReadOnly = true;
    }
    private WPager wb;

    /// <summary>
    /// ��ȡ���������ʱ��ʾ�����ؼ��� HTML��
    /// </summary>
    /// <returns>���������ʱ��ʾ�ؼ��� HTML��</returns>
    public override string GetDesignTimeHtml()
    {

      wb = (WPager)Component;
      wb.RecordCount = 225;
      StringWriter sw = new StringWriter();
      HtmlTextWriter writer = new HtmlTextWriter(sw);
      wb.RenderControl(writer);
      return sw.ToString();
    }

    /// <summary>
    /// ��ȡ�ڳ��ֿؼ�ʱ��������������ʱΪָ�����쳣��ʾ�� HTML��
    /// </summary>
    /// <param name="e">ҪΪ����ʾ������Ϣ���쳣��</param>
    /// <returns>���ʱΪָ�����쳣��ʾ�� HTML��</returns>
    protected override string GetErrorDesignTimeHtml(Exception e)
    {
      string errorstr = "�����ؼ�ʱ����" + e.Message;
      return CreatePlaceHolderDesignTimeHtml(errorstr);
    }
  }
  #endregion
}