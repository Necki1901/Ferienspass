 <%@ Page Title="Admin_Clubs_View" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Clubs_View.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Clubs_View" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
    
    </asp:Content>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Panel ID="pnlBlockBg" CssClass="pnlBlockBg" runat="server" Visible="false"></asp:Panel><%--Panel um den Hintergrund während Add oder Update Vorgängen zu Blockieren--%>            
    <div class="form-group">                          
                    <asp:Panel ID="pnlInsert" CssClass="pnlUpdateInsert" runat="server" Visible="false"><%--Panel in welches Textboxes und Dropdownlists zum Inserten eines Eintrages eingefügt werden.--%>                       
                        <div class="form-row"><asp:Label ID="lblOrganisationName" Text="Vereinsname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtOrganisationName" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblOrganisationStreet" Text="Straße:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtOrganisationStreet" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblOrganisationStreetNumber" Text="Hausnummer:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtOrganisationStreetNumber" runat="server" CssClass="txtPanel"></asp:TextBox></div>
                        <div class="form-row">
                        <asp:Label ID="lblDescription" Text="Vereinsbeschreibung:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblSpokesPerson" Text="Ansprechpartner:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlSpokesPerson" runat="server" CssClass="lblPanel"></asp:DropDownList>   </div>
                        <div class="form-row">
                        <asp:Button ID="btnBack" Text="Zurück" CssClass="btnspace" runat="server" OnClick="btnBack_Click"></asp:Button>
                        <asp:Button ID="btnAdd" Text="Hinzufügen" CssClass="btnspace" runat="server" OnClick="btnAdd_Click1"></asp:Button>
                        <asp:Label ID="lblInfo" runat="server"></asp:Label>
                            </div>
                        </asp:Panel>
                   

                    <asp:Panel ID="pnlUpdate" CssClass="pnlUpdateInsert" runat="server" Visible="false"><%--Panel in welches Textboxes und Dropdownlists zum Updaten eines Eintrages eingefügt werden.--%>                       
                        <div class="form-row"><asp:Label ID="lblOrganisationName2" Text="Vereinsname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtOrganisationName2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblOrganisationStreet2" Text="Straße" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtOrganisationStreet2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblOrganisationStreetNumber2" Text="Hausnummer:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtOrganisationStreetNumber2" runat="server" CssClass="txtPanel"></asp:TextBox></div>
                        <div class="form-row"> <asp:Label ID="lblDescription2" Text="Vereinsbeschreibung" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtDescription2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblSpokesPerson2" Text="Ansprechpartner:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlSpokesPerson2" runat="server" CssClass="lblPanel"></asp:DropDownList>  </div>                                       
                        <div class="form-row"> <asp:Button ID="btnBack2" Text="Zurück" CssClass="btnspace" runat="server" OnClick="btnBack2_Click"></asp:Button>
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
   <%-- <div>
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
    </div>--%>
    <div>
         <asp:GridView ID="gvAdminClubs" runat="server" Height="400px" Width="1015px" AutoGenerateColumns="False" DataKeyNames="ORGID" EnableViewState="False" CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" OnRowCommand="gvAdminClubs_RowCommand" OnRowEditing="gvAdminClubs_RowEditing" >
                <Columns>
                    <asp:TemplateField HeaderText="Vereinsname" SortExpression="NAME">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateClubName" runat="server" Text='<%# Bind("NAME") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateClubName" runat="server" Text='<%# Eval("NAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Straße" SortExpression="SN">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateClubStreet" runat="server" Text='<%# Bind("STREET") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateClubStreet" runat="server" Text='<%# Eval("STREET") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Hausnummer">
                         <EditItemTemplate>
                          <asp:TextBox ID="txtEditItemTemplateClubNumber" runat="server" Text='<%# Bind("NUMBER") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateClubNumber" runat="server" Text='<%#Eval("NUMBER")%>'></asp:Label>                                                  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Beschreibung">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateClubDescription" runat="server" Text='<%# Bind("DESCRIPTION") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateClubDescription" runat="server" Text='<%# Eval("DESCRIPTION")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                   
                    <asp:TemplateField HeaderText="Ansprechperson:">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditItemTemplateClubSpokesPerson" runat="server">
                              <%--  <asp:ListItem>0</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>--%>
                            </asp:DropDownList>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateClubSpokesPerson" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
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
                    <asp:TemplateField HeaderText="ORGID" Visible="False">
                     <ItemTemplate>
                          <asp:Label ID="lblItemTemplateClubID" runat="server" Text='<%# Eval("ORGID").ToString() %>'></asp:Label>
                        </ItemTemplate>   
                  <%--  <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateUserID" runat="server" Text='<%# Bind("UID").ToString() %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                        </asp:TemplateField>                   
                </Columns>
            </asp:GridView>
      
        
        <asp:Label ID="lblInfoBottom" runat="server"></asp:Label>
        </div>
        </asp:Content>


