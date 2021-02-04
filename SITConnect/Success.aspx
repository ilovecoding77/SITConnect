<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="SITConnect.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_Click" Text="Logout" />
            <asp:Label ID="lblmessage" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
