<%@ Page Title="Login" Language="C#" MasterPageFile="~/MasterPage/Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ferienspaß.Login" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-center h-100">
        <div class="card">
            <div class="card-header d-flex">
                <h3 class="align-content-center ml-auto mr-auto">Anmelden</h3>

            </div>
            <div class="card-body">
                <div>
                    <div id="grp_user" runat="server" class="input-group form-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-user"></i></span>
                        </div>
                        <asp:TextBox ID="tbx_user" CssClass="form-control" runat="server" Text="stefan.pohn@htlvb.at"></asp:TextBox>

                        <%--<input type="text" class="form-control" placeholder="E-Mail Adresse">--%>
                    </div>
                    <div id="grp_pwd"  runat="server" class="input-group form-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-key"></i></span>
                        </div>
                        <asp:TextBox ID="tbx_pass" CssClass="form-control" runat="server" Text="Soek"></asp:TextBox>     
                    </div>

                    <div class="form-group ml-auto mr-auto d-flex">
                        <asp:Button ID="btn_login" Text="Anmelden" CssClass="btn btn-primary col-md-5 ml-auto mr-auto align-content-center login_btn" OnClick="Login_Click" runat="server" /><br />
                    </div>
                    <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="card-footer">
                <div class="d-flex justify-content-center links">
                    Noch nicht registriert?&nbsp;&nbsp;<a href="#">Jetzt registrieren</a>
                </div>
                <div class="d-flex justify-content-center">
                    <a href="#">Passwort vergessen?</a>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
