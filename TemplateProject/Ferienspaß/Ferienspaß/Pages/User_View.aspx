<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_View.aspx.cs" Inherits="Ferienspaß.Pages.User_View" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtSuchen" placeholder="Name der Veranstaltung" runat="server"></asp:TextBox>
            <asp:TextBox ID="datepicker" placeholder="Datum" cssclass="datepicker-field" TextMode="Date" runat="server"></asp:TextBox>
            <br />
            <asp:CheckBox ID="cbNoParticipants" runat="server" Text="Keine leeren Projekte"/>
            <asp:CheckBox ID="cbTooManyParticipants" runat="server" Text="Keine vollen Projekte"/>
            <asp:Button ID="btnFilter" Text="Filtern" runat="server" OnClick="btnFilter_Click" />
            
            
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
                <asp:TemplateField HeaderText="Projektnummer">
                    <ItemTemplate>
                        <asp:Label ID="lblFrage" runat="server" Text='<%# Eval("Number") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Freie Plätze">
                    <ItemTemplate>
                        <asp:Label ID="lblFrage" runat="server" Text='<%# Eval("participants") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <%-- Details --%>
                <asp:TemplateField HeaderText="Details">
                    <ItemTemplate>
                        <asp:Button ID="btnShowDetails" runat="server" Text="Zur Anmeldung" CommandName="details" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            </asp:GridView>
        </div>
    </form>
</body>
</html>
