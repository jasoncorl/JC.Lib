using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
//using System.Web.UI.WebControls;

namespace JC.Lib.Web
{
  public static class Ui
  {
    /// <summary>
    /// ������ʾ
    /// </summary>
    /// <param name="strMsg"></param>
    public static void alert(string strMsg)
    {
      HttpContext.Current.Response.Write("<script language=javascript>alert('" + strMsg + "');</script>");
    }

    /// <summary>
    /// ��ʾ���ܣ�����תҳ��
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="sUrl"></param>
    public static void alert(string strMsg, string sUrl)
    {
      HttpContext.Current.Response.Write("<script language=javascript>alert('" + strMsg + "');location.href='" + sUrl + "';</script>");
    }

    /// <summary>
    /// ��ͻ��������
    /// </summary>
    /// <param name="strMsg"></param>
    public static void write(string strMsg)
    {
      HttpContext.Current.Response.Write(strMsg);
    }

    /// <summary>
    /// ҳ����ˣ�history.back()�Ĺ���
    /// </summary>
    public static void winBack()
    {
      HttpContext.Current.Response.Write("<script language=javascript>history.back();</script>");
    }

    /// <summary>
    /// ���������ض��� 
    /// </summary>
    /// <param name="sUrl">ת��URL</param>
    public static void redirect(string sUrl)
    {
      HttpContext.Current.Response.Write("<script language=javascript>location.href='" + sUrl + "';</script>");
    }

    /// <summary>
    /// �رյ�ǰ���ڶ���
    /// </summary>
    public static void close()
    {
      HttpContext.Current.Response.Write("<script language=javascript>self.close();</script>");
    }

    /// <summary>
    /// ���������ض��� 
    /// </summary>
    /// <param name="sUrl">ת��URL</param>
    /// <param name="sTarget">Ŀ�����RedirectTargetOptionֵ</param>
    public static void redirect(string sUrl,RedirectTargetOption Target)
    {
      HttpContext.Current.Response.Write("<script language=javascript>window." + Convert.ToString(Target) + ".location.href='" + sUrl + "';</script>");
    }

    public enum RedirectTargetOption 
    {
      self,
      top,
      parent
    }

    /// <summary>
    /// �õ�DataGrid��checkboxѡ���ֵ�ִ�
    /// </summary>
    /// <param name="dgName">DataGrid����</param>
    /// <param name="chkSelName">checkbox����</param>
    /// <param name="indexSelInDg">checkbox��DataGrid�е�λ��Index</param>
    /// <returns>������ѡ���checkbox��ֵ��</returns>
    public static string getSelId(System.Web.UI.WebControls.DataGrid dgName, string chkSelName, int indexSelInDg)
    {
      System.Web.UI.WebControls.CheckBox _cb = new System.Web.UI.WebControls.CheckBox();
      System.Text.StringBuilder _sb = new System.Text.StringBuilder();

      int i, j = dgName.Items.Count;
      for (i = 0; i < j; i++)
      {
        _cb = (System.Web.UI.WebControls.CheckBox)dgName.Items[i].Cells[indexSelInDg].FindControl(chkSelName);
        if (_cb.Checked)
        {
          //Response.Write(MyDataGrid.Items[i].Cells[1].Text.Trim());
          _sb.Append(dgName.Items[i].Cells[indexSelInDg].Text.Trim());
          _sb.Append(",");
        }
      }
      string strNames = _sb.ToString();
      strNames = strNames.Substring(0, strNames.Length - 1);
      return strNames;
    }
  }

}
