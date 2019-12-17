<%@ Page Title="Participants" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Participants_View.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Participants_View" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:DropDownList CssClass="table-bordered" ID="ddl_Projects" runat="server" OnSelectedIndexChanged="ddl_Projects_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <asp:Label runat="server" ID="lbl_Project_Info"></asp:Label>

    <asp:GridView CssClass="table table-bordered" ID="gv_Participants" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:TemplateField HeaderText="ID" Visible = "false">
                <ItemTemplate>
                    <asp:Label ID="lblSurname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vorname">
                <ItemTemplate>
                    <asp:Label ID="lblSurname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nachname">
                <ItemTemplate>
                    <asp:Label ID="lblGivenname" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Geburtsdatum">
                <ItemTemplate>
                    <asp:Label ID="lblBirthdate" runat="server" Text='<%# Convert.ToDateTime(Eval("BD")).ToString("dd/MM/yyyy") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Telefonnummer">
                <ItemTemplate>
                    <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("phone") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

</asp:Content>
