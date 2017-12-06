<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxComponentsFrm.aspx.cs" Inherits="JC.Web.UI.UserControl.Demo.AjaxComponentsFrm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title></title>
</head>
<body>
  <form id="form1" runat="server">
    <div>
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
      </asp:Timer>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <Triggers>
          <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
          last update:<asp:Literal ID="txtLastUpdate" runat="server"></asp:Literal>
          <asp:Button ID="btnUpdate1" runat="server" Text="Update" OnClick="btnUpdate1_Click" />
        </ContentTemplate>
      </asp:UpdatePanel>
      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
          last update:<asp:Literal ID="Literal1" runat="server"></asp:Literal>
          <asp:Button ID="Button1" runat="server" Text="Update" OnClick="Button1_Click" />
        </ContentTemplate>
      </asp:UpdatePanel>
      <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
          <asp:Label runat="server" ID="Label1">ddd</asp:Label>
          <asp:Label runat="server" ID="lblMsg1"></asp:Label>
        </ProgressTemplate>
      </asp:UpdateProgress>
      <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel2" runat="server">
        <ProgressTemplate>
          <asp:Label runat="server" ID="Label2">ddd</asp:Label>
          <asp:Label runat="server" ID="lblMsg2"></asp:Label>
        </ProgressTemplate>
      </asp:UpdateProgress>
      ssssssssssss
    </div>
  </form>
</body>
</html>
