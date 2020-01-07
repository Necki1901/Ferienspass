<%@ Page Title="Anmeldung hinzufügen" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Project_View_Add.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Project_View_Add" %>


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

     <asp:GridView CssClass="table table-bordered" ID="gv_add_child" runat="server" AutoGenerateColumns="False">
        <Columns>

        </Columns>
     </asp:GridView>


    <asp:Label runat="server" ID="lbl_Message"></asp:Label>
</asp:Content>