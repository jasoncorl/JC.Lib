<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TelerikRadUploadFrm.aspx.cs" Inherits="JC.Web.UI.UserControl.Demo.TelerikRadUploadFrm" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
      <telerik:RadUpload ID="RadUpload1" runat="server" MaxFileInputsCount="1" ControlObjectsVisibility="None"></telerik:RadUpload>
      <telerik:RadProgressArea ID="RadProgressArea1" runat="server">
      </telerik:RadProgressArea>
      <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
      <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    </div>
  </form>
</body>
</html>
