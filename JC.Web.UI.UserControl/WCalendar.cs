using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JC.Web.UI.UserControl
{
  /// <summary>
  /// WCalendar ��ժҪ˵����
  /// ����ѡ��ؼ������ܳ�������ҳjavascript����������Id�ظ������⣩
  /// 2.0.0������˻ᱻselect�ڵ������⣩
  /// </summary>
  [DefaultProperty("Text"),
    ToolboxData("<{0}:WCalendar runat=server></{0}:WCalendar>")]
  public class WCalendar : System.Web.UI.WebControls.WebControl, IPostBackDataHandler, INamingContainer
  {
    //private string size;
    private string size;

    [Bindable(true),
      Category("Appearance"),
      DefaultValue("")]
    public string Text
    {
      get
      {
        if (ViewState["Text"] != null)
        {
          return ViewState["Text"].ToString();
        }
        else
        {
          return null;
        }
      }

      set
      {
        ViewState["Text"] = value;
      }
    }

    [Bindable(true),
    Category("Appearance"),
    DefaultValue("")]
    public string Size
    {
      get
      {
        return size;
      }

      set
      {
        size = value;
      }
    }

    /// <summary> 
    /// ���˿ؼ����ָ�ָ�������������
    /// </summary>
    /// <param name="output"> Ҫд������ HTML ��д�� </param>
    protected override void Render(HtmlTextWriter output)
    {
      output.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID);
      output.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
      if (this.CssClass != "") { output.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass); }
      if (this.Text != "" && this.Text != null) { output.AddAttribute(HtmlTextWriterAttribute.Value, this.Text); }
      if (this.Size != "" && this.Size != null) { output.AddAttribute(HtmlTextWriterAttribute.Size, this.Size); }
      output.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:hand");
      output.AddAttribute(HtmlTextWriterAttribute.Type, "text");
      output.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true");
      output.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:this.focus()");
      output.AddAttribute("onFocus", "fPopCalendar(this,this,PopCal); return false;");
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(),"WCalendarClientScript"))
      {
        output.WriteLine("<script language=\"JavaScript\">");
        output.WriteLine("var gdCtrl = new Object();");
        output.WriteLine("var gcGray = \"#808080\";");			//�ǵ�ǰ��Ӧ�������ֵ���ɫ
        output.WriteLine("var gcToggle = \"highlight\";");		//����������ڵ�Ԫ��ĵ�ɫ
        output.WriteLine("var gcBG = \"threedface\";");			//��������ɫ
        output.WriteLine("var gMonths = new Array(\"һ��\",\"����\",\"����\",\"����\",\"����\",\"����\",\"����\",\"����\",\"����\",\"ʮ��\",\"ʮһ��\",\"ʮ����\");");

        output.WriteLine("var gdCurDate = new Date();");				//gdCurDate������ǰ����
        output.WriteLine("var giYear = gdCurDate.getFullYear();");		//giYear������ǰ���
        output.WriteLine("var giMonth = gdCurDate.getMonth()+1;");		//giMonth������ǰ�·ݣ���ΪgetMonth()���ص���0-11����������ʵ�ǰ��Ҫ��1��
        output.WriteLine("var giDay = gdCurDate.getDate();");			//giDay������ǰ��
        output.WriteLine("var sxYear = giYear;");						//sxYear������ѡ���
        output.WriteLine("var sxMonth = giMonth;");						//sxMonth������ѡ�·�
        output.WriteLine("var sxDay = giDay;");							//sxDay������ѡ��
        output.WriteLine("var sxDatestr = gdCtrl.value;");				//��ǰ��ѡ����

        output.WriteLine("if (sxDatestr != \"\"){");
        output.WriteLine("var sxDate = new Date(sxDatestr);");
        output.WriteLine("sxYear = sxDate.getFullYear();");
        output.WriteLine("}");

        output.WriteLine("var VicPopCal = new Object();");

        #region ����춯��ĳ�����ϵ�һϵ�к���
        output.WriteLine("function mouseover(obj){");
        output.WriteLine("obj.style.borderTop = 'buttonshadow 1px solid';");
        output.WriteLine("obj.style.borderLeft = 'buttonshadow 1px solid';");
        output.WriteLine("obj.style.borderRight = 'buttonhighlight 1px solid';");
        output.WriteLine("obj.style.borderBottom = 'buttonhighlight 1px solid';");
        output.WriteLine("}");


        output.WriteLine("function mouseout(obj){");
        output.WriteLine("obj.style.borderTop = 'buttonhighlight 1px solid';");
        output.WriteLine("obj.style.borderLeft = 'buttonhighlight 1px solid';");
        output.WriteLine("obj.style.borderRight = 'buttonshadow 1px solid';");
        output.WriteLine("obj.style.borderBottom = 'buttonshadow 1px solid';");
        output.WriteLine("}");

        output.WriteLine("function mousedown(obj){");
        output.WriteLine("obj.style.borderTop = 'buttonshadow 1px solid';");
        output.WriteLine("obj.style.borderLeft = 'buttonshadow 1px solid';");
        output.WriteLine("obj.style.borderRight = 'buttonhighlight 1px solid';");
        output.WriteLine("obj.style.borderBottom = 'buttonhighlight 1px solid';");
        output.WriteLine("}");

        output.WriteLine("function mouseup(obj){");
        output.WriteLine("obj.style.borderTop = 'buttonhighlight 1px solid';");
        output.WriteLine("obj.style.borderLeft = 'buttonhighlight 1px solid';");
        output.WriteLine("obj.style.borderRight = 'buttonshadow 1px solid';");
        output.WriteLine("obj.style.borderBottom = 'buttonshadow 1px solid';");
        output.WriteLine("}");
        #endregion

        #region ����������һϵ�к���

        output.WriteLine("function fPopCalendar(popCtrl, dateCtrl, popCal){");
        //output.WriteLine("parent.event.cancelBubble=true;");			//2005-09-28
        output.WriteLine("VicPopCal = popCal;");
        output.WriteLine("gdCtrl = dateCtrl;");
        output.WriteLine("fSetYearMon(giYear, giMonth);");
        output.WriteLine("var point = fGetXY(popCtrl);");
        output.WriteLine("with (VicPopCal.style) {left = point.x;top  = point.y+popCtrl.offsetHeight+1;visibility = 'visible';}");
        output.WriteLine("VicPopCal.focus();");
        output.WriteLine("}");

        output.WriteLine("function fSetDate(iYear, iMonth, iDay){");
        output.WriteLine("if ((iYear == 0) && (iMonth == 0) && (iDay == 0)){");
        output.WriteLine("gdCtrl.value = \"\";");
        output.WriteLine("}");
        output.WriteLine("else{");
        output.WriteLine("if (iMonth < 10){iMonth = \"0\"+iMonth;}"); //���ʱ��
        output.WriteLine("if (iDay < 10){iDay = \"0\"+iDay;}");
        output.WriteLine("gdCtrl.value = iYear+\"-\"+iMonth+\"-\"+iDay;");
        output.WriteLine("}");
        output.WriteLine("VicPopCal.style.visibility = \"hidden\";");
        output.WriteLine("}");

        output.WriteLine("function fSetSelected(aCell){");
        output.WriteLine("var iOffset = 0;");
        output.WriteLine("var iYear = parseInt(document.all.tbSelYear.value);");
        output.WriteLine("var iMonth = parseInt(document.all.tbSelMonth.value);");
        output.WriteLine("aCell.bgColor = gcBG;");
        output.WriteLine("with (aCell.children[\"cellText\"]){");
        output.WriteLine("var iDay = parseInt(innerText);");
        output.WriteLine("if (color==gcGray){iOffset = (Victor<10)?-1:1;}");
        output.WriteLine("iMonth += iOffset;");
        output.WriteLine("if (iMonth<1) {	iYear--; iMonth = 12;}else{if (iMonth>12){iYear++;iMonth = 1;}}");
        output.WriteLine("}");
        output.WriteLine("fSetDate(iYear, iMonth, iDay);");
        output.WriteLine("}");

        output.WriteLine("function Point(iX, iY){this.x = iX;this.y = iY;}");

        output.WriteLine("function fBuildCal(iYear, iMonth){");
        output.WriteLine("var aMonth=new Array();");
        output.WriteLine("for(i=1;i<7;i++){aMonth[i]=new Array(i);}");
        output.WriteLine("var dCalDate=new Date(iYear, iMonth-1, 1);");
        output.WriteLine("var iDayOfFirst=dCalDate.getDay();");
        output.WriteLine("var iDaysInMonth=new Date(iYear, iMonth, 0).getDate();");
        output.WriteLine("var iOffsetLast=new Date(iYear, iMonth-1, 0).getDate()-iDayOfFirst+1;");
        output.WriteLine("var iDate = 1;");
        output.WriteLine("var iNext = 1;");
        output.WriteLine("for (d = 0; d < 7; d++){aMonth[1][d] = (d<iDayOfFirst)?-(iOffsetLast+d):iDate++;}");
        output.WriteLine("for (w = 2; w < 7; w++){for (d = 0; d < 7; d++){aMonth[w][d] = (iDate<=iDaysInMonth)?iDate++:-(iNext++);}}");
        output.WriteLine("return aMonth;");
        output.WriteLine("}");


        output.WriteLine("function fDrawCal(iYear, iMonth, iDay, iCellWidth, iDateTextSize) {");
        output.WriteLine("var WeekDay = new Array(\"��\",\"һ\",\"��\",\"��\",\"��\",\"��\",\"��\");");
        output.WriteLine("var styleTD = \" bgcolor='\"+gcBG+\"' width='\"+iCellWidth+\"' bordercolor='\"+gcBG+\"' valign='middle' align='center' style='font-size: 12px;background: buttonface;border-top: buttonhighlight 1px solid;border-left: buttonhighlight 1px solid;border-right: buttonshadow 1px solid;	border-bottom: buttonshadow 1px solid;\";");
        output.WriteLine("with (document) {");
        output.WriteLine("write(\"<tr align='center'>\");");
        output.WriteLine("for(i=0; i<7; i++){write(\"<td height='20' \"+styleTD+\"color:#990099' >\" + WeekDay[i] + \"</td>\");}");
        output.WriteLine("write(\"</tr>\");");
        output.WriteLine("for (w = 1; w < 7; w++) {");
        output.WriteLine("write(\"<tr align='center'>\");");
        output.WriteLine("for (d = 0; d < 7; d++) {");
        output.WriteLine("write(\"<td width='10%' height='15' id=calCell \"+styleTD+\"cursor:hand;' onmouseover='mouseover(this)' onmouseout='mouseout(this)' onmousedown='mousedown(this)' onmouseup='mouseup(this)' onclick='fSetSelected(this)'>\");");
        output.WriteLine("write(\"<font style='font-size: 13px;' id=cellText Victor='Liming Weng'> </font>\");");
        output.WriteLine("write(\"</td>\");");
        output.WriteLine("}");
        output.WriteLine("write(\"</tr>\");");
        output.WriteLine("}");
        output.WriteLine("}");
        output.WriteLine("}");


        output.WriteLine("function fUpdateCal(iYear, iMonth) {");
        output.WriteLine("sxYear = iYear;");
        output.WriteLine("sxMonth = iMonth;");
        output.WriteLine("yeartd1.innerText = sxYear + \"��\";");
        output.WriteLine("monthtd1.innerText = gMonths[sxMonth-1];");
        output.WriteLine("myMonth = fBuildCal(iYear, iMonth);");
        output.WriteLine("var i = 0;");
        output.WriteLine("for (w = 0; w < 6; w++){");
        output.WriteLine("for (d = 0; d < 7; d++){");
        output.WriteLine("with (cellText[(7*w)+d]) {");
        output.WriteLine("Victor = i++;");
        output.WriteLine("if (myMonth[w+1][d]<0) {");
        output.WriteLine("color = gcGray;");
        output.WriteLine("innerText = -myMonth[w+1][d];");
        output.WriteLine("}else{");
        output.WriteLine("color = ((d==0)||(d==6))?\"red\":\"black\";");
        output.WriteLine("innerText = myMonth[w+1][d];");
        output.WriteLine("}");
        output.WriteLine("}");
        output.WriteLine("}");
        output.WriteLine("}");
        output.WriteLine("}");

        //�����б���е���ݺ��·�
        output.WriteLine("function fSetYearMon(iYear, iMon){");
        output.WriteLine("sxYear = iYear;");
        output.WriteLine("sxMonth = iMon;");
        output.WriteLine("yeartd1.innerText = sxYear + \"��\";");
        output.WriteLine("monthtd1.innerText = gMonths[sxMonth-1];");
        output.WriteLine("document.all.tbSelMonth.options[iMon-1].selected = true;");
        output.WriteLine("for (i = 0; i < document.all.tbSelYear.length; i++){");
        output.WriteLine("if (document.all.tbSelYear.options[i].value == iYear){");
        output.WriteLine("document.all.tbSelYear.options[i].selected = true;");
        output.WriteLine("}");
        output.WriteLine("}");
        output.WriteLine("fUpdateCal(iYear, iMon);");
        output.WriteLine("}");

        output.WriteLine("function fPrevMonth(){");
        output.WriteLine("var iMon = document.all.tbSelMonth.value;");
        output.WriteLine("var iYear = document.all.tbSelYear.value;");
        output.WriteLine("if (--iMon<1) {");
        output.WriteLine("iMon = 12;");
        output.WriteLine("iYear--;");
        output.WriteLine("}");
        output.WriteLine("fSetYearMon(iYear, iMon);");
        output.WriteLine("}");


        output.WriteLine("function fNextMonth(){");
        output.WriteLine("var iMon = document.all.tbSelMonth.value;");
        output.WriteLine("var iYear = document.all.tbSelYear.value;");
        output.WriteLine("if (++iMon>12) {");
        output.WriteLine("iMon = 1;");
        output.WriteLine("iYear++;");
        output.WriteLine("}");
        output.WriteLine("fSetYearMon(iYear, iMon);");
        output.WriteLine("}");


        output.WriteLine("function fGetXY(aTag){");
        output.WriteLine("var oTmp = aTag;");
        output.WriteLine("var pt = new Point(0,0);");
        output.WriteLine("do {");
        output.WriteLine("pt.x += oTmp.offsetLeft;");
        output.WriteLine("pt.y += oTmp.offsetTop;");
        output.WriteLine("oTmp = oTmp.offsetParent;");
        output.WriteLine("} while(oTmp.tagName!=\"BODY\");");
        output.WriteLine("return pt;");
        output.WriteLine("}");

        #endregion

        #region ������
        output.WriteLine("with (document){");
        output.WriteLine("write(\"<Div id='PopCal' onclick='event.cancelBubble=true' style='POSITION:absolute; VISIBILITY: hidden; bordercolor:#000000;border:2px ridge;width:10;z-index:100;'>\");");

        //����һ��iframe���Խ����select�ڵ�������
        output.WriteLine("write(\"<iframe frameBorder=0 width=180 scrolling=no height=170></iframe>\")");
        //�޸�table
        output.WriteLine("write(\"<table id='popTable' border='1' bgcolor='#eeede8' cellpadding='0' cellspacing='0' style='font-size:12px;Z-INDEX:202;position:absolute;top:0;left:0;'>\");");
        //ԭtable
        //output.WriteLine("write(\"<table id='popTable' border='1' bgcolor='#eeede8' cellpadding='0' cellspacing='0' style='font-size:12px'>\");");

        output.WriteLine("write(\"<TR>\");");
        output.WriteLine("write(\"<td valign='middle' align='center' style='cursor:default'>\");");
        //����ͷ
        output.WriteLine("write(\"<table width='176' border='0' cellpadding='0' cellspacing='0'>\");");
        output.WriteLine("write(\"<tr align='center'>\");");
        //��һ����
        output.WriteLine("write(\"<td height='22' width='20' name='PrevMonth' style='font-family:\\\"webdings\\\";font-size:15px' onClick='fPrevMonth()' onmouseover='this.style.color=\\\"#ff9900\\\"' onmouseout='this.style.color=\\\"\\\"'>3</td>\");");
        //��ʾ��ѡ�����----------

        //��ʾ���
        output.WriteLine("write(\"<td width='64' id='yeartd1' style='font-size:12px' onmouseover='yeartd1.style.display=\\\"none\\\";yeartd2.style.display=\\\"\\\";' onmouseout='this.style.background=\\\"\\\"'>\");");
        output.WriteLine("write(sxYear + \"��\");");
        output.WriteLine("write(\"</td>\");");
        //���ѡ��
        //output.WriteLine("write(\"<td width='64' id='yeartd2' style='display:none' onmouseout='yeartd2.style.display=\\\"none\\\";yeartd1.style.display=\\\"\\\";'>\");");
        output.WriteLine("write(\"<td width='64' id='yeartd2' style='display:none'>\");");
        output.WriteLine("write(\"<SELECT style='width:64px;font-size: 12px;font-family: ����;' id='tbSelYear' onChange='fUpdateCal(document.all.tbSelYear.value, document.all.tbSelMonth.value);yeartd2.style.display=\\\"none\\\";yeartd1.style.display=\\\"\\\";' Victor='Won'>\");");
        output.WriteLine("for(i=" + (DateTime.Now.Year - 5).ToString() + ";i<" + (DateTime.Now.Year + 5).ToString() + ";i++){");
        output.WriteLine("write(\"<OPTION value='\"+i+\"'>\"+i+\"��</OPTION>\");");
        output.WriteLine("}");
        output.WriteLine("write(\"</SELECT>\");");
        output.WriteLine("write(\"</td>\");");
        //��ʾ��ѡ���·�----------
        //��ʾ�·�
        output.WriteLine("write(\"<td width='72' id='monthtd1' style='font-size:12px' onmouseover='monthtd1.style.display=\\\"none\\\";monthtd2.style.display=\\\"\\\";' onmouseout='this.style.background=\\\"\\\"'>\");");
        output.WriteLine("write(gMonths[sxMonth-1]);");
        output.WriteLine("write(\"</td>\");");
        //�·�ѡ��
        //output.WriteLine("write(\"<td width='72' id='monthtd2' style='display:none' onmouseout='monthtd2.style.display=\\\"none\\\";monthtd1.style.display=\\\"\\\";'>\");");
        output.WriteLine("write(\"<td width='72' id='monthtd2' style='display:none'>\");");
        output.WriteLine("write(\"<select style='width:72px;font-size: 12px;font-family: ����;' id='tbSelMonth' onChange='fUpdateCal(document.all.tbSelYear.value, document.all.tbSelMonth.value);monthtd2.style.display=\\\"none\\\";monthtd1.style.display=\\\"\\\";' Victor='Won'>\");");

        output.WriteLine("for (i=0; i<12; i++){");
        output.WriteLine("write(\"<option value='\"+(i+1)+\"'>\"+gMonths[i]+\"</option>\");");
        output.WriteLine("}");
        output.WriteLine("write(\"</SELECT>\");");
        output.WriteLine("write(\"</td>\");");
        //��һ����
        output.WriteLine("write(\"<td width='20' name='PrevMonth' style='font-family:\\\"webdings\\\";font-size:15px' onclick='fNextMonth()' onmouseover='this.style.color=\\\"#ff9900\\\"' onmouseout='this.style.color=\\\"\\\"'>4</td>\");");

        output.WriteLine("write(\"</tr>\");");
        output.WriteLine("write(\"</table>\");");
        //----------------------------����ͷ����----------------
        output.WriteLine("write(\"</td></TR><TR><td align='center'>\");");
        output.WriteLine("write(\"<DIV style='background-color:teal;'><table width='100%' border='0' bgcolor='threedface' cellpadding='0' cellspacing='0'>\");");
        output.WriteLine("fDrawCal(giYear, giMonth, giDay, 19, 14);");
        output.WriteLine("write(\"</table></DIV>\");");
        output.WriteLine("write(\"</td></TR><TR><TD height='20' align='center' valign='bottom'>\");");
        output.WriteLine("write(\"<font style='cursor:hand;font-size:12px' onclick='fSetDate(0,0,0)' onMouseOver='this.style.color=\\\"#0033FF\\\"' onMouseOut='this.style.color=0'>���</font>\");");
        output.WriteLine("write(\"&nbsp;&nbsp;&nbsp;&nbsp;\");");
        output.WriteLine("write(\"<font style='cursor:hand;font-size:12px' onclick='fSetDate(giYear,giMonth,giDay)' onMouseOver='this.style.color=\\\"#0033FF\\\"' onMouseOut='this.style.color=0'>����: \"+giYear+\"-\"+giMonth+\"-\"+giDay+\"</font>\");");
        output.WriteLine("write(\"</TD></TR></TD></TR></TABLE>\");");
        output.WriteLine("write(\"</Div>\");");
        output.WriteLine("}");
        #endregion

        output.WriteLine("</script>");
        output.WriteLine("<SCRIPT event=onclick() for=document>PopCal.style.visibility = 'hidden';</SCRIPT>");
        //this.Page.RegisterClientScriptBlock("clientScript", "");
        this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "WCalendarClientScript", "");
      }
    }

    #region IPostBackDataHandler ��Ա

    public event EventHandler TextChanged;


    /// <summary>
    /// ������ʵ��ʱ��Ϊ ASP.NET �������ؼ�����ط����ݡ�
    /// </summary>
    /// <param name="postDataKey">�ؼ�����Ҫ��ʶ��</param>
    /// <param name="postCollection">���д�������ֵ�ļ���</param>
    /// <returns>����������ؼ���״̬�ڻط���������ģ���Ϊ true������Ϊ false��</returns>
    public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
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


    /// <summary>
    /// ������ʵ��ʱ�����ź�Ҫ��������ؼ�����֪ͨ ASP.NET Ӧ�ó���ÿؼ���״̬�Ѹ��ġ�
    /// </summary>
    public virtual void RaisePostDataChangedEvent()
    {
      OnTextChanged(EventArgs.Empty);
    }


    protected virtual void OnTextChanged(EventArgs e)
    {
      if (TextChanged != null)
        TextChanged(this, e);
    }
    #endregion
  }


}
