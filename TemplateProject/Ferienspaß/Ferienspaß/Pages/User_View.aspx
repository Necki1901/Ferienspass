<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_View.aspx.cs" Inherits="Ferienspaß.Pages.User_View" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtSuchen" placeholder="Name der Veranstaltung" AutoPostBack="true" OnTextChanged="txtSuchen_TextChanged" runat="server"></asp:TextBox> <%--<asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>--%>
        </div>
        <div>
            <asp:GridView ID="gv_UserView" runat="server" 
            AutoGenerateColumns="False" 
            AllowPaging="True" 
            AllowSorting="True" >

            <Columns>
                <asp:TemplateField HeaderText="Projekt">
                    <ItemTemplate>
                        <asp:Label ID="lblFragenID" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Beschreibung">
                    <ItemTemplate>
                        <asp:Label ID="lblFrage" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Datum">
                    <ItemTemplate>
                        <asp:Label ID="lblFrage" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToString("dd/MM/yyyy") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>    
                <asp:TemplateField HeaderText="Straße">
                    <ItemTemplate>
                        <asp:Label ID="lblFrage" runat="server" Text='<%# Eval("Place") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nr.">
                    <ItemTemplate>
                        <asp:Label ID="lblFrage" runat="server" Text='<%# Eval("Number") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Nr.">
                    <ItemTemplate>
                        <asp:Label ID="lblFrage" runat="server" Text='<%# Eval("participants") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <%-- Details --%>
                <asp:TemplateField HeaderText="Details">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnCancelInsert" runat="server" CommandName="Details" ImageUrl="~/App_Themes/default/details.jpg" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            </asp:GridView>
        </div>
    </form>
</body>
</html>
