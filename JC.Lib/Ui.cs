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
    /// 仅仅提示
    /// </summary>
    /// <param name="strMsg"></param>
    public static void alert(string strMsg)
    {
      HttpContext.Current.Response.Write("<script language=javascript>alert('" + strMsg + "');</script>");
    }

    /// <summary>
    /// 提示功能，并跳转页面
    /// </summary>
    /// <param name="strMsg"></param>
    /// <param name="sUrl"></param>
    public static void alert(string strMsg, string sUrl)
    {
      HttpContext.Current.Response.Write("<script language=javascript>alert('" + strMsg + "');location.href='" + sUrl + "';</script>");
    }

    /// <summary>
    /// 向客户端输出流
    /// </summary>
    /// <param name="strMsg"></param>
    public static void write(string strMsg)
    {
      HttpContext.Current.Response.Write(strMsg);
    }

    /// <summary>
    /// 页面回退，history.back()的功能
    /// </summary>
    public static void winBack()
    {
      HttpContext.Current.Response.Write("<script language=javascript>history.back();</script>");
    }

    /// <summary>
    /// 服务器端重定向 
    /// </summary>
    /// <param name="sUrl">转向URL</param>
    public static void redirect(string sUrl)
    {
      HttpContext.Current.Response.Write("<script language=javascript>location.href='" + sUrl + "';</script>");
    }

    /// <summary>
    /// 关闭当前窗口对象
    /// </summary>
    public static void close()
    {
      HttpContext.Current.Response.Write("<script language=javascript>self.close();</script>");
    }

    /// <summary>
    /// 服务器端重定向 
    /// </summary>
    /// <param name="sUrl">转向URL</param>
    /// <param name="sTarget">目标对象RedirectTargetOption值</param>
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
    /// 得到DataGrid中checkbox选择的值字串
    /// </summary>
    /// <param name="dgName">DataGrid对象</param>
    /// <param name="chkSelName">checkbox名称</param>
    /// <param name="indexSelInDg">checkbox在DataGrid中的位置Index</param>
    /// <returns>返回已选择的checkbox的值串</returns>
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
