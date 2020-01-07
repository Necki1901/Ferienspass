
<%@ Page Title="Registration" Language="C#" MasterPageFile="~/MasterPage/Login.Master" AutoEventWireup="true" CodeBehind="RegistrationChildren.aspx.cs" Inherits="Ferienspaß.RegistrationChildren" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-center h-100">
        <div class="card">
            <div class="card-header d-flex">
                <h3 class="align-content-center ml-auto mr-auto">Ihre Kinder</h3>

            </div>
            <div class="card-body col-12" style="width:700px;">
                
                <div>
                    <asp:GridView CssClass="table table-bordered mt-2" CellSpacing="0" HorizontalAlign="Center"  ID="gvChildren" runat="server" OnRowUpdated="gvChildren_RowUpdated" AutoGenerateColumns="false" OnRowEditing="gvChildren_RowEditing" DataKeyNames="CID" OnRowUpdating="gvChildren_RowUpdating" OnRowCancelingEdit="gvChildren_RowCancelingEdit" OnRowDeleting="gvChildren_RowDeleting" OnSelectedIndexChanging="gvChildren_SelectedIndexChanging">
                        <Columns>
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
                                        <asp:LinkButton ID="btn_delete" UseSubmitBehavior="false" runat="server" CommandName="Delete" ><i class="fa fa-times" style="font-size:22px;"></i></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btn_update" UseSubmitBehavior="true" runat="server" CommandName="Update"><i class="fa fa-check-circle" style="font-size:22px;"></i></asp:LinkButton><br />
                                    <asp:LinkButton ID="btn_cancel" runat="server" CommandName="Cancel"><i class="fa fa-times-circle" style="font-size:22px;"></i></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>

                                   

                        </Columns>
                    </asp:GridView>

                    <asp:Button ID="btn_addChild" OnClick="btn_addChild_Click" runat="server" CssClass="btn btn-info" Text="Kind anlegen" />
                </div>
               
                  
                <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
            </div>
          



            <div class="card-footer">
                <div class="form-group ml-auto mr-auto d-flex">
                    <asp:Button ID="btn_back" OnClick="btn_back_Click" Text="Abschließen" CssClass="btn btn-primary col-md-5 ml-auto mr-auto align-content-center login_btn"  runat="server" /><br />
                </div>
            </div>
        </div>
    </div>



</asp:Content>
