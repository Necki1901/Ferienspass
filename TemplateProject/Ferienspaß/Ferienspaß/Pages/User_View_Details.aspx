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
        <asp:GridView ID="gv_User_View_Details" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_User_View_Details_RowCommand">
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
                        <asp:Button ID="btnRegister" runat="server" Text="Anmelden" CommandName="register"/>
                        <asp:Button ID="btnQueue" runat="server" Text="In die Warteschlange" CommandName="queue" Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
               
             </Columns>
        </asp:GridView>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>

        <asp:GridView ID="gv_Children" runat="server" AutoGenerateColumns="False" Visible="False">
            <columns>
                 <asp:TemplateField HeaderText="ChildID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblChildID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Vorname">
                    <ItemTemplate>
                        <asp:Label ID="lblGivenname" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nachname">
                    <ItemTemplate>
                        <asp:Label ID="lblSurname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Geburtsdatum">
                    <ItemTemplate>
                        <asp:Label ID="lblBirthday" runat="server" Text='<%# Convert.ToDateTime(Eval("BD")).ToString("dd/MM/yyyy") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Aktion">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkUseChildren" runat="server" CommandName="checked" />
                    </ItemTemplate>
                </asp:TemplateField>


            </columns>
        </asp:GridView>

        <asp:Label ID="lblChildrenMessage" runat="server"></asp:Label>

        <asp:Button ID="btnAddChildren" runat="server" OnClick="btnAddChildren_Click" Text="Kinder hinzufügen" Visible="False" />
    </form>
</body>
</html>
