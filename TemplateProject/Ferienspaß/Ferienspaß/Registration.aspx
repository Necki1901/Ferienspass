
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
                      <div class="card-header d-flex" style="height:40px;">
                         <h3 class="align-content-center ml-auto mr-auto" style="margin-top:-10px;" >Ihre Daten</h3>
                      </div>
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
                              <asp:TextBox ID="tbx_pwd1" TextMode="Password" CssClass="form-control" runat="server" Text="Soek1234"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                              <label for="inputPassword4">Passwort wiederholen</label>
                               <asp:TextBox ID="tbx_pwd2" TextMode="Password" CssClass="form-control" runat="server" Text="Soek1234"></asp:TextBox>
                            </div>
                          </div>
                          <small class="form-text text-muted text-center">Mind. 8 Zeichen, Mind. einen Großbuchstaben, Mind. eine Zahl</small>
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
