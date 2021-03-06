﻿<%@ Page Title="Anmeldung hinzufügen" Language="C#" MasterPageFile="~/MasterPage/Portier.Master" AutoEventWireup="true" CodeBehind="Portier_Cancellations_View.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Cancellations_View" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <script>
        function alertCompeteCancellation() {
            if (confirm("Möchten Sie die Stornierung wirklich abschließen?")) {
                return true;
            }
            return false;
        }
    </script>

    <div>
        <div class="form-group">
            <div class="form-line">
                <asp:Label Text="Projektname" runat="server"></asp:Label>
                <asp:TextBox ID="txtProjectName" runat="server"></asp:TextBox>


                <asp:Label Text="Name der Eltern" runat="server"></asp:Label>
                <asp:TextBox ID="txtParentName" runat="server"></asp:TextBox>
            </div>

            <div class="form-line">
                <asp:Label Text="Stornierdatum" runat="server"></asp:Label>
                <asp:TextBox ID="txtStornoDate" TextMode="Date" runat="server"></asp:TextBox>

                <asp:Label Text="Name des Kindes" runat="server"></asp:Label>
                <asp:TextBox ID="txtChildName" runat="server"></asp:TextBox>
            </div>
            <asp:Button ID="btnSearch" runat="server" Text="Suchen" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
        </div>
    </div>

     <div>
        <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
    </div>

     <asp:GridView CssClass="table table-bordered" ID="gv_cancellations" Autopost ="true" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_cancellations_RowCommand">
         <Columns>
             <asp:TemplateField HeaderText="ID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("cancel_id") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Projektname">
                <ItemTemplate>
                    <asp:Label ID="lblProject" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Kindname">
                <ItemTemplate>
                     <asp:Label ID="lblChild" runat="server" Text='<%#  $"{Eval("childName")}" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Elternname">
                <ItemTemplate>
                    <asp:Label ID="lblParent" runat="server" Text='<%# $"{Eval("userName")}" %>'></asp:Label>
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
                    <asp:Button runat="server" OnClientClick="return alertCompeteCancellation()" CssClass="btn btn-primary" CommandName="completed" Text="Stornierung abschließen"/>
                </ItemTemplate>
            </asp:TemplateField>
         </Columns>
     </asp:GridView>

</asp:Content>