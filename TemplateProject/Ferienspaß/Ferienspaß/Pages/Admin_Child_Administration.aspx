<%@ Page Title="Child Administration" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="Admin_Child_Administration.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Child_Administration" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden" OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:GridView CssClass="table table-bordered" ID="gv_Children" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_Children_RowCommand" OnRowEditing="gv_Children_RowEditing" OnRowUpdating="gv_Children_RowUpdating" OnRowCancelingEdit="gv_Children_RowCancelingEdit">
            <Columns>
                <asp:TemplateField HeaderText="ID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblChildID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Projekt">
                    <ItemTemplate>
                        <asp:Label ID="lblGivenname" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGivenname" runat="server" Text='<%# Eval("GN") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Projekt">
                    <ItemTemplate>
                        <asp:Label ID="lblSurname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                    </ItemTemplate>
                     <EditItemTemplate>
                        <asp:TextBox ID="txtSurname" runat="server" Text='<%# Eval("SN") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Projekt">
                    <ItemTemplate>
                        <asp:Label ID="lblBirthday" runat="server" Text='<%# Convert.ToDateTime(Eval("BD")).ToString("dd/MM/yyyy")%>'></asp:Label>
                    </ItemTemplate>
                     <EditItemTemplate>
                         <asp:TextBox ID="txtBirthday" runat="server" Text='<%# Convert.ToDateTime(Eval("BD")).ToString("dd/MM/yyyy")%>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <%-- Buttons --%>
                <asp:TemplateField HeaderText="Aktion">
                     <HeaderTemplate>
                            <asp:Button ID="btnAddChild" CssClass="btn btn-primary" runat="server" Text="Kind Hinzufügen" CommandName="add"/>
                        </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEditChild" CssClass="btn btn-primary" runat="server" CommandName="edit" ImageUrl="~/App_Themes/default/edit.png" EnableViewState="false"/>
                        <asp:ImageButton ID="btnDeleteChild" CssClass="btn btn-primary" runat="server" CommandName="delete" ImageUrl="~/App_Themes/default/trash.png" EnableViewState="false"/>
                    </ItemTemplate>
                     <EditItemTemplate>
                        <asp:ImageButton ID="btnUpdate" CssClass="btn btn-primary" runat="server" CommandName="Update" ImageUrl="~/App_Themes/default/ok.png"  />
                        <asp:ImageButton ID="btnCancel" CssClass="btn btn-primary" runat="server" CommandName="Cancel" ImageUrl="~/App_Themes/default/Cancel.png" />
                    </EditItemTemplate>
                </asp:TemplateField>
             </Columns>
        </asp:GridView>


</asp:Content>