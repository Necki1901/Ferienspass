<%@ Page Title="Userview" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="User_Participations.aspx.cs" Inherits="Ferienspaß.Pages.User_Participations" %>

<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden" OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gv_participations" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False">
        <Columns>
             <asp:TemplateField HeaderText="Vorname">
                <ItemTemplate>
                    <asp:Label ID="lbl_givenname" runat="server" Text='<%# Eval("gn") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Nachname">
                <ItemTemplate>
                    <asp:Label ID="lbl_surname" runat="server" Text='<%# Eval("sn") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Geburtsdatum">
                <ItemTemplate>
                    <asp:Label ID="lbl_birthdate" runat="server" Text='<%# Convert.ToDateTime(Eval("bd")).ToString("dd/MM/yyyy") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Projektname">
                <ItemTemplate>
                    <asp:Label ID="lbl_projectname" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Projektdatum">
                <ItemTemplate>
                    <asp:Label ID="lbl_project_date" runat="server" Text='<%# Convert.ToDateTime(Eval("date")).ToString("dd/MM/yyyy") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:Label runat="server" ID="lbl_Message"></asp:Label>
</asp:Content>