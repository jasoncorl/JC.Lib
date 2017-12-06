<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadFrm.aspx.cs" Inherits="JC.Web.UI.UserControl.Demo.UploadFrm" %>

<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <title>上传测试</title> 
</head> 
<body> 
    <form id="form1" runat="server"> 
    <div>
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
      </asp:Timer>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
          <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
          <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
        <ContentTemplate>
          last update:<asp:Literal ID="txtLastUpdate" runat="server"></asp:Literal>
        </ContentTemplate>
      </asp:UpdatePanel>
      <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"></asp:UpdateProgress>
     
          <asp:FileUpload ID="FileUpload1" runat="server" />
          <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    </div> 
    </form> 
</body> 