<%@ Page Title="AdminUserView" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_User_View.aspx.cs" Inherits="Ferienspaß.Admin_User_View" %>

<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlBlockBg" CssClass="pnlBlockBg" runat="server" Visible="false"></asp:Panel><%--Panel um den Hintergrund während Add oder Update Vorgängen zu Blockieren--%>            
    <div class="form-group">                          
                    <asp:Panel ID="pnlInsert" CssClass="pnlUpdateInsert" runat="server" Visible="false"><%--Panel in welches Textboxes und Dropdownlists zum Inserten eines Eintrages eingefügt werden.--%>                       
                        <div class="form-row"><asp:Label ID="lblGivenName" Text="Vorname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtGivenName" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblSurName" Text="Nachname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtSurName" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblPhone" Text="Telefon:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="txtPanel"></asp:TextBox></div>
                        <div class="form-row"> <asp:Label ID="lblEMail" Text="EMail:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtEMail" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblUserGroup" Text="User-Gruppe" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlUserGroup" runat="server" CssClass="lblPanel"></asp:DropDownList>
                        <asp:Label ID="lblLocked" Text="Zustand:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlLocked" runat="server" CssClass="txtPanel"></asp:DropDownList> </div>
                        <div class="form-row"><asp:Label ID="lblEmailConfirmed" Text="EMail-Zustand:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlEmailConfirmed" runat="server" CssClass="txtPanel"></asp:DropDownList>                       
                        <asp:Button ID="btnBack" Text="Zurück" CssClass="btnspace" runat="server" OnClick="btnBack_Click"></asp:Button>
                         <asp:Button ID="btnAdd" Text="Hinzufügen" CssClass="btnspace" runat="server" OnClick="btnAdd_Click1"></asp:Button>
                              <asp:Label ID="lblInfo" runat="server"></asp:Label>
                            </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlUpdate" CssClass="pnlUpdateInsert" runat="server" Visible="false"><%--Panel in welches Textboxes und Dropdownlists zum Updaten eines Eintrages eingefügt werden.--%>                       
                        <div class="form-row"><asp:Label ID="lblGivenName2" Text="Vorname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtGivenName2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblSurName2" Text="Nachname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtSurName2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblPhone2" Text="Telefon:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtPhone2" runat="server" CssClass="txtPanel"></asp:TextBox></div>
                        <div class="form-row"> <asp:Label ID="lblMail2" Text="EMail:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtEMail2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblUserGroup2" Text="User-Gruppe" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlUserGroup2" runat="server" CssClass="lblPanel"></asp:DropDownList>
                        <asp:Label ID="lblLocked2" Text="Zustand:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlLocked2" runat="server" CssClass="txtPanel"></asp:DropDownList> </div>
                        <div class="form-row"><asp:Label ID="lblEmailConfirmed2" Text="EMail-Zustand:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlEmailConfirmed2" runat="server" CssClass="txtPanel"></asp:DropDownList>                       
                        <asp:Button ID="btnBack2" Text="Zurück" CssClass="btnspace" runat="server" OnClick="btnBack2_Click"></asp:Button>
                        <asp:Button ID="btnUpdate" Text="Ändern" CssClass="btnspace" runat="server" OnClick="btnUpdate_Click"></asp:Button>
                              <asp:Label ID="lblInfo2" runat="server"></asp:Label>
                            </div>
                    </asp:Panel>         
                </div>   
    
        <script type="text/javascript">
        function Delete() {
            if (confirm("Datensatz wirklich löschen?")) {
                return true;               
            }
            return false;
        }
    </script>
    <div>
        <div>

        </div>
        <div class="form-group">
            <div>
                <asp:Label Text="Vorname: " runat="server"></asp:Label>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                <asp:Label Text="Nachname: " runat="server"></asp:Label>
                <asp:TextBox ID="txtSurname3" runat="server"></asp:TextBox>
                <div class="form-row">
                    <asp:Label Text="User-Group: " runat="server"></asp:Label>
                    <asp:DropDownList ID="ddlUserGroup3" runat="server"></asp:DropDownList>
                </div>
                <div>
                    <asp:CheckBox ID="cbxConditionLocked" runat="server" Text="User gesperrt" />
                </div>
                <div>
                    <asp:CheckBox ID="cbxConditionConfirmed" runat="server" Text="Email bestätigt" /> 
                </div>
            </div>
            <asp:Button ID="btnSearch" runat="server" Text="Suchen" OnClick="btnSearch_Click" CssClass="btn btn-primary" />
        </div>
    </div>
    <div>
         <asp:GridView ID="gvAdminUsers" runat="server" Height="400px" Width="1015px" AutoGenerateColumns="False" DataKeyNames="UID" EnableViewState="False" CssClass="table table-bordered" OnRowCommand="gvAdminUsers_RowCommand" OnRowEditing="gvAdminUsers_RowEditing" OnRowDeleting="gvAdminUsers_RowDeleting" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvAdminUsers_PageIndexChanging" PageSize="7" OnSorting="gvAdminUsers_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Vorname" SortExpression="GN">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserGN" runat="server" Text='<%# Bind("GN") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateUserGN" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nachname" SortExpression="SN">
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
      
         <br />
         Zustand: 0 = nicht gesperrt, 1 = gesperrt<br />
         Email_Zustand: 0 = nicht bestätigt, 1 = bestätigt<br />
         <br />

        <asp:Label ID="lblInfoBottom" runat="server"></asp:Label>
        </div>
       
   </asp:Content>

