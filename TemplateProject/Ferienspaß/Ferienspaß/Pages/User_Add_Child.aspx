<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="User_Add_Child.aspx.cs" Inherits="Ferienspaß.Pages.User_Add_Child" %>
<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden"  OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <script>
        function alertDelete() {
            if (confirm("Möchten Sie den Datensatz wirklich löschen?")) {
                return true;
            }
            return false;
        }
    </script>

     <div>
        <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
    </div>

    <asp:GridView CssClass="table table-bordered mt-2" CellSpacing="0" HorizontalAlign="Center"  ID="gvChildren" runat="server" AutoGenerateColumns="false" OnRowEditing="gvChildren_RowEditing" DataKeyNames="CID" OnRowUpdating="gvChildren_RowUpdating" OnRowCancelingEdit="gvChildren_RowCancelingEdit" OnRowDeleting="gvChildren_RowDeleting">
        <Columns>
             <asp:TemplateField HeaderText="CID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblCID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>    
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Vorname">
                <ItemTemplate>
                    <asp:Label ID="lbl_cFirstname" runat="server" Text='<%# Eval("GN") %>'></asp:Label>    
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox Rows="4" ID="tbx_cFirstname" Width="100%" runat="server" Text='<%# Bind("GN") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Nachname">
                <ItemTemplate>
                    <asp:Label ID="lbl_cLastname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>    
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox Rows="4" ID="tbx_cLastname" Width="100%" runat="server" Text='<%# Bind("SN") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Geburtsdatum">
                <ItemTemplate>
                    <asp:Label ID="lbl_cBirth" runat="server" Text='<%# (DateTime.Parse(Eval("BD").ToString()).ToShortDateString()) %>'></asp:Label>    
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox Rows="4" ID="tbx_cBirth" TextMode="Date" Width="100%" runat="server" Text='<%#  Bind("BD") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Aktion">
                <ItemTemplate>
                    <asp:LinkButton ID="btn_edit" UseSubmitBehavior="true" runat="server" CommandName="Edit"><i class="fa fa-edit" style="font-size:18px;"></i></asp:LinkButton>
                        <asp:LinkButton ID="btn_delete" OnClientClick="return alertDelete()" UseSubmitBehavior="false" runat="server" CommandName="Delete" ><i class="fa fa-times" style="font-size:22px;"></i></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="btn_update" UseSubmitBehavior="true" runat="server" CommandName="Update"><i class="fa fa-check-circle" style="font-size:22px;"></i></asp:LinkButton><br />
                    <asp:LinkButton ID="btn_cancel" runat="server" CommandName="Cancel"><i class="fa fa-times-circle" style="font-size:22px;"></i></asp:LinkButton>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

     <asp:Button ID="btn_addChild" OnClick="btn_addChild_Click" runat="server" CssClass="btn btn-info" Text="Kind anlegen" />


</asp:Content>