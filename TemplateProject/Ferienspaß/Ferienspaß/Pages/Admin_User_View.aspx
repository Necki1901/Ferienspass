<%@ Page Title="AdminUserView" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_User_View.aspx.cs" Inherits="Ferienspaß.Admin_User_View" %>

<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <script type="text/javascript">
        function Delete() {
            if (confirm("Datensatz wirklich löschen?")) {
                return true;               
            }
            return false;
        }
    </script>
    <div>
         <asp:GridView ID="gvAdminUsers" runat="server" Height="400px" Width="1015px" AutoGenerateColumns="False" DataKeyNames="UID" EnableViewState="False" CssClass="table table-bordered" OnRowCommand="gvAdminUsers_RowCommand" OnRowEditing="gvAdminUsers_RowEditing" OnRowCancelingEdit="gvAdminUsers_RowCancelingEdit" OnRowUpdating="gvAdminUsers_RowUpdating" OnRowDataBound="gvAdminUsers_RowDataBound" OnRowDeleting="gvAdminUsers_RowDeleting" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvAdminUsers_PageIndexChanging" PageSize="7">
                <Columns>
                    <asp:TemplateField HeaderText="Vorname">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserGN" runat="server" Text='<%# Bind("GN") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserGN" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nachname">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserSN" runat="server" Text='<%# Bind("SN") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserSN" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Telefon">
                         <EditItemTemplate>
                          <asp:TextBox ID="txtEditItemTemplateUserPhone" runat="server" Text='<%# Bind("PHONE") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserPhone" runat="server" Text='<%#Eval("PHONE")%>'></asp:Label>                                                  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EMail">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserMail" runat="server" Text='<%# Bind("EMAIL") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserMail" runat="server" Text='<%# Eval("EMAIL")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                   
                    <asp:TemplateField HeaderText="User_Gruppe">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditItemTemplateUserGroup" runat="server">
                              <%--  <asp:ListItem>0</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>--%>
                            </asp:DropDownList>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserGroup" runat="server" Text='<%# Eval("DESCRIPTION") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Zustand">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserState" runat="server" Text='<%# Bind("LOCKED") %>'></asp:TextBox>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserState" runat="server" Text='<%# Eval("LOCKED") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email_Zustand">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserMailState" runat="server" Text='<%# Bind("EmailConfirmed") %>'></asp:TextBox>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserMailState" runat="server" Text='<%# Eval("EmailConfirmed") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ändern">
                    <HeaderTemplate>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CommandName="Add" CssClass="btn btn-primary" />
                    </HeaderTemplate>
                    <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ImageUrl="~/App_Themes/default/edit.png" Height="50" Width="50"  />
                        </ItemTemplate>
                    <EditItemTemplate>
                            <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" ImageUrl="~/App_Themes/default/ok.png" Height="50" Width="50"/>                          
                            <br />
                            <br />
                             <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" ImageUrl="~/App_Themes/default/cancel.png" Height="50" Width="50"/>
                        </EditItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Löschen">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/App_Themes/default/trash.png" Height="50" Width="50" OnClientClick="return Delete()" />
                        </ItemTemplate>                   
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UID" Visible="False">
                     <ItemTemplate>
                          <asp:Label ID="lblItemTemplateUserID" runat="server" Text='<%# Eval("UID").ToString() %>'></asp:Label>
                        </ItemTemplate>   
                  <%--  <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserID" runat="server" Text='<%# Bind("UID").ToString() %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kinder">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnChildren" runat="server" ImageUrl="~/App_Themes/default/child.png" Height="50" Width="50" CommandName="Children" />
                        </ItemTemplate>       
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


         <asp:Label ID="lblInfo" runat="server"></asp:Label>


         <br />
         Zustand: 0 = nicht gesperrt, 1 = gesperrt<br />
         Email_Zustand: 0 = nicht bestätigt, 1 = bestätigt<br />
         <br />


        </div>

   </asp:Content>

