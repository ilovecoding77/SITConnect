<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LfoeUEaAAAAAF6ciO_EN_h0V7suaSqgujhx1MsA"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" Text="Login"></asp:Label>
            <br />
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="tb_userid" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
            <br />
        <asp:Label ID="lblpw" runat="server" EnableViewState="False">Error Msg</asp:Label>  
        </div>
                  <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        <asp:Label ID="lblCaptchaMsg" runat="server" EnableViewState="False">Error Msg</asp:Label>  
        </form>
       
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute(' SITE KEY ', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
    </body>
</html>
