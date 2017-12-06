<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PagerDemoFrm.aspx.cs" Inherits="JC.Web.UI.UserControl.Demo.PagerDemoFrm" %>

<%@ Register Assembly="UserControl" Namespace="JC.Web.UI.UserControl" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html>
<head id="Head1" runat="server">
  <title>系统用户列表</title>
  <link href="/Includes/BasicLayout.css" type="text/css" rel="stylesheet" />
  <script language="javascript" type="text/javascript" src="/Includes/ComScript.js"></script>
  <style type="text/css">
    #WPager1
    {
      font-family: "tahoma";
      color: #808080;
      font-size: 12px;
      line-height: 120%;
      text-decoration: none;
      background-image: url("/images/PageList_bg.jpg");
    }

      #WPager1 input[name="WPager1"]
      {
        width: 36px;
        height: 28px;
        background-image: url("/images/Main_btn_GO.jpg");
      }

      #WPager1 a
      {
        font-family: "tahoma";
        color: #808080;
        font-size: 12px;
        text-decoration: none;
      }
  </style>

</head>
<body style="margin-top: 2px;">
  <form id="form1" runat="server">
    <table id="PageTitle" cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr onclick="SearchShow(this.childNodes[0].childNodes[0])" style="cursor: pointer">
        <td style="WIDTH: 20px; height: 24px" align="right" valign="middle">
          <img id="imgSearchShow" alt="显示/隐藏查询条件" src="../images/SearchHide.gif" /></td>
        <td class="ptitle" align="center" valign="middle" style="height: 24px">系统用户列表</td>
        <td style="width: 50; height: 24px;">
          <input id="hidSearchShow" type="hidden" value="true" name="hidSearchShow" runat="server" /></td>
      </tr>
    </table>
    <hr style="color: black; height: 1px; width: 100%" />
    <asp:SqlDataSource ID="Dept_CodeRes" runat="server" ProviderName="System.Data.SqlClient" ConnectionString="<%$ ConnectionStrings:DataConnectionString %>"></asp:SqlDataSource>
    <table id="TableSearch" cellpadding="0" width="100%">
      <tr>
        <td align="right">用户姓名：</td>
        <td>
          <asp:TextBox ID="UserName" runat="server"></asp:TextBox></td>
        <td align="right">用户登录名：</td>
        <td>
          <asp:TextBox ID="Login_Name" runat="server"></asp:TextBox></td>
        <td align="right">部门：</td>
        <td>&nbsp;<asp:DropDownList ID="Dept_Code" runat="server" DataSourceID="Dept_CodeRes" DataValueField="SeqId" DataTextField="Name">
          <asp:ListItem Value="">全部</asp:ListItem>
        </asp:DropDownList></td>
      </tr>
      <tr>
        <td align="right"></td>
        <td></td>
        <td align="right"></td>
        <td></td>
        <td align="right"></td>
        <td>&nbsp;<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="../images/Search.gif" OnClick="btnSearch_Click"></asp:ImageButton>
          &nbsp;<asp:ImageButton ID="btnNew" runat="server" ImageUrl="../images/New.gif" OnClientClick='OpenWindow("SysUserEdit.aspx?Op=New");'></asp:ImageButton></td>
      </tr>
    </table>
    <div>
      <asp:GridView ID="GridView1" AutoGenerateColumns="False" Width="100%" runat="server" CssClass="TableList" OnRowCommand="RowCmd">
        <Columns>
          <asp:BoundField DataField="Uid" HeaderText="通道Id" Visible="false" />
          <asp:TemplateField>
            <HeaderTemplate>删除</HeaderTemplate>
            <HeaderStyle Width="40" />
            <ItemTemplate>
              <asp:ImageButton ID="btnDelete" AlternateText="删除本行记录" OnClientClick="if(confirm('确定删除本条记录么？')==false){return false;}" CommandName="Detelte" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.Uid") %>' runat="server" ImageUrl="../Images/DgDelete.gif" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="center" />
          </asp:TemplateField>
          <asp:TemplateField>
            <HeaderTemplate>修改</HeaderTemplate>
            <HeaderStyle Width="40" />
            <ItemTemplate>
              <img src="../images/DgEdit.gif" alt="编辑本行记录" onclick='<%# DataBinder.Eval(Container, "DataItem.Uid", "OpenWindow(\"SysUserEdit.aspx?ID={0}&Op=Edit\");") %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="center" />
          </asp:TemplateField>
          <asp:TemplateField>
            <HeaderTemplate>查看</HeaderTemplate>
            <HeaderStyle Width="40" />
            <ItemTemplate>
              <img src="../images/DgView.gif" alt="查看详细信息" onclick='<%# DataBinder.Eval(Container, "DataItem.Uid", "OpenWindow(\"SysUserEdit.aspx?ID={0}&Op=view\");") %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="center" />
          </asp:TemplateField>
          <asp:BoundField DataField="UserName" HeaderText="用户姓名" />
          <asp:BoundField DataField="Login_Name" HeaderText="登录名" />
          <asp:BoundField DataField="Mobile" HeaderText="手机号码" />
          <asp:BoundField DataField="GroupName" HeaderText="部门" />
          <asp:BoundField DataField="Position_name" HeaderText="职级" />
          <asp:BoundField DataField="Sex" HeaderText="性别" />
          <asp:BoundField DataField="DtLastLogin" HeaderText="最后登录时间" />
          <asp:BoundField DataField="Status" HeaderText="状态" />
        </Columns>
        <RowStyle CssClass="TableRow" />
        <SelectedRowStyle CssClass="TableSelectedRow" />
        <HeaderStyle CssClass="TableHeader" />
        <AlternatingRowStyle CssClass="TableAlternatingRow" />
      </asp:GridView>
      <cc1:WPager ID="WPager1" runat="server" OnPageChanged="WPager1_PageChanged" AlwaysShow="true" PrevPageText="上一页" NextPageText="下一页" SubmitButtonText="" 
        TextAfterInputBox="页" PagingButtonSpacing="2" PageSizeCookiePrivate="true" FirstPageText="首页" LastPageText="尾页">
      </cc1:WPager>
    </div>
  </form>
</body>
</html>
