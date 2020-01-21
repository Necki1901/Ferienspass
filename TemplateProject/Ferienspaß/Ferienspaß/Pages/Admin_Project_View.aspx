<%@ Page Title="AdminView" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Project_View.aspx.cs" Inherits="Ferienspaß.Pages.Admin" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>

    

</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<asp:Panel ID="pnlBlockBg" CssClass="pnlBlockBg" runat="server" Visible="false"></asp:Panel><%--Panel um den Hintergrund während Add oder Update Vorgängen zu Blockieren--%>            
    <div class="form-group">                          
                    <asp:Panel ID="pnlInsert" CssClass="pnlUpdateInsert" runat="server" Visible="false"><%--Panel in welches Textboxes und Dropdownlists zum Inserten eines Eintrages eingefügt werden.--%>                       
                        <div class="form-row"><asp:Label ID="lblDate" Text="Datum:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblStart" Text="Start:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtStart" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblEnd" Text="Ende:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtEnd" runat="server" CssClass="txtPanel"></asp:TextBox></div>
                        <div class="form-row"> <asp:Label ID="lblName" Text="Projektname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblDesc" Text="Beschreibung:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtDesc" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblPlace" Text="Ort/Straße:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtPlace" runat="server" CssClass="txtPanel"></asp:TextBox> </div>
                        <div class="form-row"><asp:Label ID="lblNumber" Text="Hausnummer:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtNumber" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblCapacity" Text="Kapazität:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtCapacity" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblGuide" Text="Ansprechperson:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlGuide" runat="server"></asp:DropDownList></div>
                        <asp:Button ID="btnBack" Text="Zurück" CssClass="btnspace" runat="server" OnClick="btnBack_Click"></asp:Button>
                         <asp:Button ID="btnAdd" Text="Hinzufügen" CssClass="btnspace" runat="server" OnClick="btnAdd_Click1"></asp:Button>
                    </asp:Panel>
                    <asp:Panel ID="pnlUpdate" CssClass="pnlUpdateInsert" runat="server" Visible="false"><%--Panel in welches Textboxes und Dropdownlists zum Updaten eines Eintrages eingefügt werden.--%>                       
                        <div class="form-row"><asp:Label ID="Label1" Text="Datum:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtDate2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblStart2" Text="Start:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtStart2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblEnd2" Text="Ende:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtEnd2" runat="server" CssClass="txtPanel"></asp:TextBox></div>
                        <div class="form-row"> <asp:Label ID="lblName2" Text="Projektname:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtName2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblDesc2" Text="Beschreibung:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtDesc2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblPlace2" Text="Ort/Straße:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtPlace2" runat="server" CssClass="txtPanel"></asp:TextBox> </div>
                        <div class="form-row"><asp:Label ID="lblNumber2" Text="Hausnummer:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtNumber2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblCapacity2" Text="Kapazität:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:TextBox ID="txtCapacity2" runat="server" CssClass="txtPanel"></asp:TextBox>
                        <asp:Label ID="lblGuide2" Text="Ansprechperson:" runat="server" CssClass="lblPanel"></asp:Label>
                        <asp:DropDownList ID="ddlGuide2" runat="server"></asp:DropDownList></div>
                        <asp:Button ID="btnBack2" Text="Zurück" CssClass="btnspace" runat="server" OnClick="btnBack2_Click"></asp:Button>
                        <asp:Button ID="btnUpdate" Text="Ändern" CssClass="btnspace" runat="server" OnClick="btnUpdate_Click"></asp:Button>
                    </asp:Panel>         
                </div>   
    

 <script type="text/javascript">
        function Delete() {
            if (confirm("Datensatz wirklich löschen?")) {
            }
            return false;
        }
    </script>

    <div class="container">        
                <div class="eventname">
                    <asp:Label Text="Name der Veranstaltung" runat="server"></asp:Label>
                    <asp:TextBox ID="txtEventName" runat="server"></asp:TextBox>
                </div>
                <div class="guidename">
                    <asp:Label Text="Verantwortlicher" runat="server"></asp:Label>
                    <asp:TextBox ID="txtOrganizerName" runat="server"></asp:TextBox>
                </div>
                <div class="date">
                    <asp:Label Text="Datum der Veranstaltung" runat="server"></asp:Label>
                    <asp:TextBox ID="datepicker" placeholder="Datum" cssclass="datepicker-field" TextMode="Date" runat="server"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnSearch" Text="Suchen" CssClass="btn btn-primary fa-amazon" runat="server" OnClick="btnSearch_Click"/>
        
    <br />
    <div>
            <asp:GridView ID="gvAdminProjects" runat="server" Height="400px" Width="1015px" AutoGenerateColumns="False" DataKeyNames="PID" EnableViewState="False" OnRowEditing="gvAdminProjects_RowEditing" OnRowCommand="gvAdminProjects_RowCommand" OnRowDeleted="gvAdminProjects_RowDeleted" OnRowDeleting="gvAdminProjects_RowDeleting" CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvAdminProjects_PageIndexChanging" PageSize="7" OnSorting="gvAdminProjects_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateProjectName" runat="server" Text='<%# Bind("NAME") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectName" runat="server" Text='<%# Eval("NAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Beschreibung">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateProjectDesc" runat="server" Text='<%# Bind("DESCRIPTION") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectDesc" runat="server" Text='<%# Eval("DESCRIPTION") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Datum">
                         <EditItemTemplate>
                          <asp:TextBox ID="txtEditItemTemplateProjectDate" runat="server" Text='<%# Bind("DATE", "{0:yyyy/MM/dd}") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectDate" runat="server" Text='<%#Convert.ToDateTime(Eval("DATE")).ToString("yyyy/MM/dd")%>'></asp:Label>                                                  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Beginn">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateProjectStartTime" runat="server" Text='<%# Bind("START") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectStartTime" runat="server" Text='<%# Eval("START")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ende">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateProjectEndTime" runat="server" Text='<%# Bind("END") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectEndTime" runat="server" Text='<%#Eval("END")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Straße">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateProjectStreet" runat="server" Text='<%# Bind("PLACE") %>'></asp:TextBox>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectStreet" runat="server" Text='<%# Eval("PLACE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nummer">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateProjectStreetNumber" runat="server" Text='<%# Bind("NUMBER") %>'></asp:TextBox>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectStreetNumber" runat="server" Text='<%# Eval("NUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kapazität"> 
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditItemTemplateProjectCapacity" runat="server" Text='<%# Bind("CAPACITY") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectCapacity" runat="server" Text='<%# Eval("CAPACITY") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="ID_Verantw.">
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectGuideID" runat="server" Text='<%# Eval("GID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Verantwortlicher">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditItemTemplateProjectGuide" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectGuide" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ändern">
                    <HeaderTemplate>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CommandName="Add" OnClick="btnAdd_Click" CssClass="btn btn-primary" />
                    </HeaderTemplate>
                    <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ImageUrl="~/App_Themes/default/edit.png" Height="50" Width="50"  />
                        </ItemTemplate>                   
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Löschen">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/App_Themes/default/trash.png" Visible= Height="50" Width="50" OnClientClick="return Delete()"/>
                        </ItemTemplate>                   
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PID" Visible="false">                         
                        <ItemTemplate>
                            <asp:Label ID="lblItemTemplateProjectID" runat="server" Text='<%# Eval("PID").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>       
       &nbsp;<asp:Label ID="lblInfo" runat="server"></asp:Label>
        </div>
    <p>
        DATE Format: YYYY.MM.DD</p>
    <p>
        TIME Format: HH:MM:SS</p>
    
        
       <%-- <asp:Panel ID="pnlUpdateInsert" CssClass="pnlUpdateInsert" runat="server" Visible="true"></asp:Panel>--%>
</asp:Content>
