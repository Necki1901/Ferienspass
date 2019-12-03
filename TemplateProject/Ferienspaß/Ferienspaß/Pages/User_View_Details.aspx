<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_View_Details.aspx.cs" Inherits="Ferienspaß.Pages.User_View_Details" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:GridView ID="gv_User_View_Details" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="ID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblProjectID" runat="server" Text='<%# Eval("PID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Projekt">
                    <ItemTemplate>
                        <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Beschreibung">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Datum">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToString("dd/MM/yyyy") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>    
                <asp:TemplateField HeaderText="Straße">
                    <ItemTemplate>
                        <asp:Label ID="lblPlace" runat="server" Text='<%# Eval("Place") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nr.">
                    <ItemTemplate>
                        <asp:Label ID="lblNumber" runat="server" Text='<%# Eval("Number") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Beginn">
                    <ItemTemplate>
                        <asp:Label ID="lblStart" runat="server" Text='<%# Eval("start") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField><asp:TemplateField HeaderText="Ende">
                    <ItemTemplate>
                        <asp:Label ID="lblEnd" runat="server" Text='<%# Eval("end") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Freie Plätze">
                    <ItemTemplate>
                        <asp:Label ID="lblParticipants" runat="server" Text='<%# Eval("remainingCapacity") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                  <%-- Details --%>
                <asp:TemplateField HeaderText="Aktion">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnRegister" runat="server" CommandName="details" ImageUrl="~/App_Themes/default/anmelden.png" Width="400" Height="400" />
                        <asp:ImageButton ID="btnQueue" runat="server" CommandName="details" ImageUrl="~/App_Themes/default/johannes-egger.jpg" Visible="false" Width="400" Height="400" />
                    </ItemTemplate>
                </asp:TemplateField>
               
             </Columns>
        </asp:GridView>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </form>
</body>
</html>
