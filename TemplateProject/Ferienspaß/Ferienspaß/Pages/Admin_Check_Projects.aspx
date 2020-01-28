<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Check_Projects.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Check_Projects" %>

<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden" OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <script type="text/javascript">
        function Delete() {
            if (confirm("Datensatz löschen?")) {
                return true;
            }
            return false;
        }
    </script>

    <asp:Button runat="server" Text="Test" ID="btnTwoWeeks" OnClick="btnTwoWeeks_Click"/>


     <asp:GridView CssClass="table table-bordered" ID="gvProjects" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="PID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPID" runat="server" Text='<%# Eval("PID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Projektname">
                    <ItemTemplate>
                        <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Datum">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("date")).ToString("dd/MM/yyyy") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Aktion">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="Testbutton" CommandName="two_weeks"/>
                    </ItemTemplate>
                </asp:TemplateField>
             </Columns>
        </asp:GridView>

        <asp:Literal ID="lit_msg" runat="server"></asp:Literal>

</asp:Content>