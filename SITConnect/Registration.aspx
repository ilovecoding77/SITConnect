<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <script type ="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;
            if (str.length < 8) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length Must be at least 8 Characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number")
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password must contain one uppercase character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase")
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password must contain one lowercase character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase")
            }
            else if (str.search(/[^A-Za-z0-9]/)== -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password must contain one uppercase character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase")
            }

            document.getElementById("lbl_pwdchecker").innerHTML = "Excellent!";
            document.getElementById("lbl_pwdchecker").style.color = "Blue";

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Account Registration"></asp:Label>
            <br />
            <asp:Label ID="Label6" runat="server" Text="First Name:"></asp:Label>
            <asp:TextBox ID="tb_firstname" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label7" runat="server" Text="Last Name:"></asp:Label>
            <asp:TextBox ID="tb_lastname" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_emailcheck" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Password:"></asp:Label>
            <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Label ID="lbl_pwdchecker" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Confirm Password:"></asp:Label>
            <asp:TextBox ID="tb_cfpwd" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <asp:Label ID="Label8" runat="server" Text="Credit Card:"></asp:Label>
            <asp:TextBox ID="tb_creditNo" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label9" runat="server" Text="Date Of Birth:"></asp:Label>

            <asp:TextBox ID="tb_dob" runat="server" placeholder="DD/MM/YYYY" TextMode="Date"></asp:TextBox>

            <br />
            <asp:Button ID="btn_submit" runat="server" OnClick="Button1_Click" Text="Submit" />
            <asp:Label ID="lbl_registration" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
