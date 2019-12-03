
<%@ Page Title="Registration" Language="C#" MasterPageFile="~/MasterPage/Login.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Ferienspaß.Registration" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-center h-100">
        <div class="card">
            <div class="card-header d-flex">
                <h3 class="align-content-center ml-auto mr-auto">Registrieren</h3>

            </div>
            <div class="card-body">
                <div class="" style="width:650px">
                   
                    
                    <div id="div_userdata" runat="server" class="card mb-3">
                      <h4>Ihre Daten</h4>
                      <div class="card-body">
                        <div>
                          <div class="form-row">
                            <div class="form-group col-md-6">
                              <label for="inputEmail4">Vorname</label>
                              <asp:TextBox ID="tbx_firstname" CssClass="form-control" runat="server" Text="Michale"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                              <label for="inputPassword4">Nachname</label>
                               <asp:TextBox ID="tbx_lastname" CssClass="form-control" runat="server" Text="Plasser"></asp:TextBox>
                            </div>
                          </div>
                          <div class="form-row">
                            <div class="form-group col-md-6">
                              <label for="inputEmail4">E-Mail Adresse</label>
                              <asp:TextBox ID="tbx_email" CssClass="form-control" runat="server" Text="müchplas"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                              <label for="inputPassword4">Telefonnummer</label>
                               <asp:TextBox ID="tbx_phone" CssClass="form-control" runat="server" Text="242424"></asp:TextBox>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <div id="div_pwd" runat="server" class="card mb-1">
                      <div class="card-body">
                        <div>
                          <div class="form-row">
                            <div class="form-group col-md-6">
                              <label for="inputEmail4">Passwort</label>
                              <asp:TextBox ID="tbx_pwd1" CssClass="form-control" runat="server" Text="Soek1234"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                              <label for="inputPassword4">Passwort wiederholen</label>
                               <asp:TextBox ID="tbx_pwd2" CssClass="form-control" runat="server" Text="Soek1234"></asp:TextBox>
                            </div>
                          </div>
                          <small class="form-text text-muted text-center">Mind. 8 Zeichen, Mind. einen Großbuchstaben, Mind. eine Zahl</small>
                        </div>
                      </div>
                    </div>
                    <hr />
                    

                    <div id="div_children" runat="server" visible="false" class="card mb-3 mt-3">
                       <h4 class="text-center">Ihre Kinder</h4>
                      <div class="card-body">
                        <div>
                            <asp:GridView CssClass="table table-bordered mt-2" CellSpacing="0" HorizontalAlign="Center"  ID="gvChildren" runat="server" AutoGenerateColumns="false" OnRowEditing="gvChildren_RowEditing" DataKeyNames="CID" OnRowUpdating="gvChildren_RowUpdating" OnRowCancelingEdit="gvChildren_RowCancelingEdit" OnRowDeleting="gvChildren_RowDeleting" OnSelectedIndexChanging="gvChildren_SelectedIndexChanging">
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
                                             <asp:LinkButton ID="btn_delete" UseSubmitBehavior="false" runat="server" CommandName="Delete"><i class="fa fa-times" style="font-size:22px;"></i></asp:LinkButton>
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
                      </div>
                    </div>
                    

                   
                    <div class="form-group ml-auto mr-auto d-flex">
                        <asp:Button ID="btn_register" OnClick="btn_register_Click" Text="Registration abschließen" CssClass="btn btn-primary col-md-5 ml-auto mr-auto align-content-center login_btn"  runat="server" /><br />
                    </div>
                    <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
                </div>
            </div>



            <div class="card-footer">
                <div class="d-flex justify-content-center links">
                    <a href="login.aspx">Zurück zum Login</a>
                </div>
                <div class="d-flex justify-content-center">
                    <a href="PasswordReset.aspx">Passwort vergessen?</a>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
