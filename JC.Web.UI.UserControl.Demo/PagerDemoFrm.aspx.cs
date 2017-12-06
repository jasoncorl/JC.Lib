using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JC.Lib.Data;

namespace JC.Web.UI.UserControl.Demo
{
  public partial class PagerDemoFrm : System.Web.UI.Page
  {
    private string sConnOa = ConfigurationManager.ConnectionStrings["DataConnectionString"].ToString();
    private string sSql = "";
    private CommData _ComData;

    private string sqUserName = "%";
    private string sqLogin_Name = "%";
    private string sqDeptCode = "%";

    protected void Page_Load(object sender, EventArgs e)
    {
      //SysSecurity.CheckUserPurview("0-1-2");
      //WPager1.PageSize = 2;
      if (!IsPostBack)
      {
        BindData();
      }

      sSql = "select SeqId, Name, ParentID from t_group";
      this.Dept_CodeRes.SelectCommand = sSql;
    }
    /// <summary>
    /// 数据绑定
    /// </summary>
    protected void BindData()
    {
      sqUserName = "%" + UserName.Text.Trim() + "%";
      sqLogin_Name = "%" + Login_Name.Text.Trim() + "%";
      sqDeptCode = this.Dept_Code.SelectedValue + "%";
      SearchStates();

      int iCurPageNo = WPager1.CurrentPageIndex;
      sSql = @"WITH OrderedOrders AS (select ROW_NUMBER() OVER (ORDER BY Uid) AS 'RowNumber', Uid, Login_name, UserName, Password, Mobile, Contact, Dept, Dept_code, Position_name, Status, Addtime, 
      Email, TaskText, Purview, DeptPurview, Sex, Duty, LoginStatus, DtLastLogin, t_group.Name As GroupName from t_sysuser 
      Left join t_group on t_group.SeqId = t_sysuser.Dept_code Where UserName Like @P3 And Login_Name Like @P4 
      And Dept_code Like @P5) SELECT * FROM OrderedOrders Where RowNumber BETWEEN @p1 AND @p2; ; 
      Select Count(Uid) From t_sysuser Where UserName Like @P3 And Login_Name Like @P4 And Dept_code Like @P5 ";
      object[] arrParas = new object[5] { (iCurPageNo - 1) * WPager1.PageSize + 1, iCurPageNo * WPager1.PageSize, sqUserName, sqLogin_Name, sqDeptCode };
      _ComData = new CommData(sConnOa);
      DataSet _ds = _ComData.GetDs(sSql, arrParas);
      _ComData.CloseConn();

      GridView1.DataSource = _ds.Tables[0];
      WPager1.RecordCount = Convert.ToInt32((_ds.Tables[1].Rows[0][0]));
      GridView1.DataBind();
    }

    protected void WPager1_PageChanged(object src, JC.Web.UI.UserControl.PageChangedEventArgs e)
    {
      WPager1.CurrentPageIndex = e.NewPageIndex;

      BindData();
    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
      BindData();
    }
    protected void RowCmd(object sender, GridViewCommandEventArgs e)
    {
      if (e.CommandName == "Detelte")
      {
        sSql = "Delete from t_sysuser Where Uid = @P1";
        string[] arrParas = new string[1] { (string)e.CommandArgument };
        _ComData = new CommData(sConnOa);
        _ComData.ExecuteNonQuery(sSql, arrParas);
        _ComData.CloseConn();
        BindData();
      }
    }

    protected void SearchStates()
    {
      string[] arrStates = new string[6];
      arrStates[0] = UserName.Text.Trim();
      arrStates[1] = Login_Name.Text.Trim();
      arrStates[3] = Dept_Code.SelectedValue;
      ViewState["Search"] = arrStates;
    }
  }
}