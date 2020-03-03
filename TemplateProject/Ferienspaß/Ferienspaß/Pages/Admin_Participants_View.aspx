<%@ Page Title="Participants" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="Admin_Participants_View.aspx.cs" Inherits="Ferienspaß.Pages.Admin_Participants_View" %>


<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function Delete() {
            if (confirm("Datensatz löschen?")) {
                return true;
            }
            return false;
        }
    
    </script>

    <h1><asp:Label runat="server" ID="lbl_projectname"></asp:Label></></h1>

    <div class="settings">
      
        Jahr: <asp:DropDownList ID="ddl_Years" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Years_SelectedIndexChanged"> </asp:DropDownList>

        Projekte:<asp:DropDownList CssClass="table-bordered" ID="ddl_Projects" runat="server" OnSelectedIndexChanged="ddl_Projects_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
       
        <br />
        <br />
        <asp:Button runat="server" ID="print" CssClass="btn btn-primary" OnClick="print_Click" Text="Druckansicht"/>
        <br />

    </div>

    <asp:GridView CssClass="table table-bordered" ID="gv_Participants" runat="server" AutoGenerateColumns="False" OnRowDeleting="gv_Participants_RowDeleting" OnRowEditing="gv_Participants_RowEditing" OnRowUpdating="gv_Participants_RowUpdating" OnRowDataBound="gv_Participants_RowDataBound" OnRowCancelingEdit="gv_Participants_RowCancelingEdit" OnRowCommand="gv_Participants_RowCommand" OnPageIndexChanging="gv_Participants_PageIndexChanging" AllowSorting="true" OnSorting="gv_Participants_Sorting">
        <Columns>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <asp:Label ID="lblChildID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vorname" SortExpression="GN">
                <ItemTemplate>
                    <asp:Label ID="lblSurname" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nachname" SortExpression="SN">
                <ItemTemplate>
                    <asp:Label ID="lblGivenname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Geburtsdatum">
                <ItemTemplate>
                    <asp:Label ID="lblBirthdate" runat="server" Text='<%# Convert.ToDateTime(Eval("BD")).ToString("dd/MM/yyyy") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Telefonnummer">
                <ItemTemplate>
                    <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("phone") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Bezahlt">
                <ItemTemplate>
                    <asp:Label ID="lblPaid" runat="server" Text='<%# ChangePaidType(Convert.ToInt32(Eval("paid"))) %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddl_Paid" runat="server" EnableViewState="true"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
             <%-- Buttons --%>
                <asp:TemplateField HeaderText="Aktion">
                     <HeaderTemplate>
                            <asp:Button ID="btn_add_participation" CssClass="btn btn-primary" runat="server" Text="Anmeldung Hinzufügen" CommandName="add" EnableViewState="false"/>
                        </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEditParticipant" runat="server" CommandName="edit" ImageUrl="~/App_Themes/default/edit.png" Height="50px" Width="50px" EnableViewState="false" />
                        <asp:ImageButton ID="btnDeleteParticipant" runat="server" CommandName="delete" ImageUrl="~/App_Themes/default/trash.png" Height="50px" Width="50px" EnableViewState="false" OnClientClick="return Delete()" />
                    </ItemTemplate>
                     <EditItemTemplate>     
                        <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" ImageUrl="~/App_Themes/default/ok.png"  />
                        <asp:ImageButton ID="btnCancel"  runat="server" CommandName="Cancel" ImageUrl="~/App_Themes/default/Cancel.png" />
                    </EditItemTemplate>
                </asp:TemplateField>
        </Columns>
    </asp:GridView>




    <asp:Label runat="server" ID="lbl_Message"></asp:Label>
</asp:Content>
