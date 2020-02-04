0<%@ Page Title="Anmeldungen drucken" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Participants_Print.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Participants_Print" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function Open_print() {
            window.print();  
        }

    </script>


    <h1><asp:Label runat="server" ID="lbl_project"></asp:Label></h1>
    <asp:Label runat="server" ID="lbl_description"></asp:Label><br />
    <br />

    <asp:GridView CssClass="table table-bordered" ID="gv_participants" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <asp:Label ID="lblChildID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
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
            <asp:TemplateField HeaderText="Bezahlt">
                <ItemTemplate>
                    <asp:Label ID="lblPaid" runat="server" Text='<%# Eval("paid") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddl_Paid" runat="server" EnableViewState="true"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

     <asp:Button runat="server" ID="print" CssClass="btn btn-primary"  OnClientClick="Open_print()" Text="Drucken"/>

    <asp:Label runat="server" ID="lbl_Message"></asp:Label>
</asp:Content>