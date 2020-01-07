<%@ Page Title="Anmeldung hinzufügen" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Participants_Add.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Project_View_Add" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function Delete() {
            if (confirm("Datensatz löschen?")) {
                return true;
            }
            return false;
        }
    </script>

     <asp:GridView CssClass="table table-bordered" ID="gv_add_child" Autopost ="true" runat="server" AutoGenerateColumns="False" OnRowDataBound="gv_add_child_RowDataBound" OnRowCommand="gv_add_child_RowCommand">
        <Columns>
             <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <asp:Label ID="lbl_user_id" runat="server" Text='<%# Eval("UID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vorname">
                <ItemTemplate>
                    <asp:Label ID="lbl_surname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nachname">
                <ItemTemplate>
                    <asp:Label ID="lbl_givenname" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Bezahlt">
                <ItemTemplate>
                    <asp:DropDownList ID="ddl_paid" runat="server" EnableViewState="true"></asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Children">
                <ItemTemplate>
                    <asp:DropDownList ID="ddl_children" runat="server" EnableViewState="true"></asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="Aktion">
                <ItemTemplate>
                    <asp:Button runat="server" ID="btn_save" Text="Kind hinzufügen" CommandName="save" />
                </ItemTemplate>
            </asp:TemplateField> 



        </Columns>
     </asp:GridView>


    <asp:Label runat="server" ID="lbl_Message"></asp:Label>
</asp:Content>