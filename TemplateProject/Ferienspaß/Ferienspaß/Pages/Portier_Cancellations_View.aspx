<%@ Page Title="Anmeldung hinzufügen" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Portier_Cancellations_View.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Cancellations_View" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <asp:GridView CssClass="table table-bordered" ID="gv_cancellations" Autopost ="true" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_cancellations_RowCommand">
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
              <asp:TemplateField HeaderText="Projektname" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblProject" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Kindname">
                <ItemTemplate>
                    <asp:Label ID="lblChild" runat="server" Text='<%#  $"{Eval("gn")} {Eval("sn")}" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Elternname">
                <ItemTemplate>
                    <asp:Label ID="lblParent" runat="server" Text='<%# $"{Eval("gnUser")} {Eval("snUser")}" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Telefonnummer">
                <ItemTemplate>
                    <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("phone")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Preis">
                <ItemTemplate>
                    <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("price") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Stornierdatum">
                <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("date")).ToString("dd/MM/yyyy") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Aktion">
                <ItemTemplate>
                    <asp:Button runat="server" CssClass="btn btn-primary" CommandName="completed" Text="Stornierung abgeschlossen"/>
                </ItemTemplate>
            </asp:TemplateField>
         </Columns>
     </asp:GridView>

</asp:Content>