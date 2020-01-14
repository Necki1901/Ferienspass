﻿<%@ Page Title="AdminView" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Project_View.aspx.cs" Inherits="Ferienspaß.Pages.Admin" %>


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
    <div class="form-group">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <asp:Label Text="Name der Veranstaltung" runat="server"></asp:Label>
                    <asp:TextBox ID="txtEventName" runat="server"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <asp:Label Text="Verantwortlicher" runat="server"></asp:Label>
                    <asp:TextBox ID="txtOrganizerName" runat="server"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <asp:Label Text="Datum der Veranstaltung" runat="server"></asp:Label>
                    <asp:TextBox ID="datepicker" placeholder="Datum" cssclass="datepicker-field" TextMode="Date" runat="server"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnSearch" Text="Suchen" class="btn btn-primary" runat="server" OnClick="btnSearch_Click"/>
        </div>
    <br />
    <div>
            <asp:GridView ID="gvAdminProjects" runat="server" Height="400px" Width="1015px" AutoGenerateColumns="False" DataKeyNames="PID" EnableViewState="False" OnRowEditing="gvAdminProjects_RowEditing" OnRowDataBound="gvAdminProjects_RowDataBound1" OnRowUpdating="gvAdminProjects_RowUpdating" OnRowCancelingEdit="gvAdminProjects_RowCancelingEdit" OnRowCommand="gvAdminProjects_RowCommand" OnRowDeleted="gvAdminProjects_RowDeleted" OnRowDeleting="gvAdminProjects_RowDeleting" CssClass="table table-bordered" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvAdminProjects_PageIndexChanging" PageSize="7">
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
                    <EditItemTemplate>
                            <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" ImageUrl="~/App_Themes/default/ok.png" Height="50" Width="50"/>                          
                            <br />
                            <br />
                             <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" ImageUrl="~/App_Themes/default/cancel.png" Height="50" Width="50"/>
                        </EditItemTemplate>
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
        <asp:Panel ID="pnlBlockBg" runat="server" Visible="true" Width="100%" Height="100%" ForeColor="Black" ></asp:Panel><%--Panel um den Hintergrund während Add oder Update Vorgängen zu Blockieren--%>            
   
        <asp:panel ID="pnlUpdateInsert" runat="server" Visible="false"></asp:Panel>
    
              <asp:Label ID="lblInfo" runat="server"></asp:Label>
        </div>
    <p>
        DATE Format: YYYY.MM.DD</p>
    <p>
        TIME Format: HH:MM:SS</p>
    <p>
</asp:Content>
