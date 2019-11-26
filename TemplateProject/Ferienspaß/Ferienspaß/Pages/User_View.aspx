<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_View.aspx.cs" Inherits="Ferienspaß.Pages.User_View" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gv_UserView" runat="server" 
            AutoGenerateColumns="False" 
            AllowPaging="True" 
            AllowSorting="True" >

            <Columns>
                <asp:TemplateField HeaderText="Projekte">
                    <ItemTemplate>
                        <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            </asp:GridView>
        </div>
    </form>
</body>
</html>
