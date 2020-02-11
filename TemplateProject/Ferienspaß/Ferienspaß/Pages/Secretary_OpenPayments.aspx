<%@ Page Title="Offene Zahlungen"  Language="C#" MasterPageFile="~/MasterPage/Secretary.Master" AutoEventWireup="true" CodeBehind="Secretary_OpenPayments.aspx.cs" Inherits="Ferienspaß.Pages.Secretary_OpenPayments" %>

<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden" OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <script type="text/javascript">
        function ConfirmSetParticipationPaid() {
            if (confirm("Als bezahlt markieren?")) {
                return true;
            }
            return false;
        }
    </script>



    <asp:Literal ID="lit_msg" runat="server"></asp:Literal>

     <asp:GridView CssClass="table table-bordered" ID="gvOpenPayments" runat="server" AutoGenerateColumns="false" OnRowCommand="gvOpenPayments_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="PID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPID" runat="server" Text='<%# Eval("PID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="CID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Kind">
                    <ItemTemplate>
                        <asp:Label ID="lblChildName" runat="server" Text='<%# Eval("childname") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Elternteil">
                    <ItemTemplate>
                        <asp:Label ID="lblParentName" runat="server" Text='<%# Eval("username") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate>
                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("email") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Telefonnummer">
                    <ItemTemplate>
                        <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("phone") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Projektname">
                    <ItemTemplate>
                        <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Betrag">
                    <ItemTemplate>
                        <asp:Label ID="lblPriceAmount" runat="server" Text='<%# Eval("price") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Zahlungsziel">
                    <ItemTemplate>
                        <asp:Label ID="lblPaymentDeadline" runat="server" Text='<%# Eval("payment_deadline") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Aktion">
                    <ItemTemplate>
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btn_participationPaid" Text="Als Bezahlt markieren" CommandName="setParticipationPaid" OnClientClick="return ConfirmSetParticipationPaid()"/>
                    </ItemTemplate>
                </asp:TemplateField> 
             </Columns>
        </asp:GridView>


</asp:Content>