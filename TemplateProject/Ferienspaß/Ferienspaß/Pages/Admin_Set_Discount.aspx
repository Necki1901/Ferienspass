<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Set_Discount.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Set_Discount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlDiscount" runat="server">
        Rabatt für alle Projekte ohne Anmeldungen setzen:

        Wert[%]: <asp:TextBox ID="txtDiscount" runat="server"></asp:TextBox>
        <asp:Button ID="btnCommit" runat="server" Text="Bestätigen" OnClick="btnCommit_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Zurück" OnClick="btnCancel_Click" />

        <br />
        <asp:Label ID="lblMessage" runat="server"></asp:Label>

    </asp:Panel>
</asp:Content>
