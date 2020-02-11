 <%@ Page Title="Userview" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="User_Project_View.aspx.cs" Inherits="Ferienspaß.Pages.Projectview" %>

<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden" OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div>
        <div class="form-group">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <asp:Label Text="Name der Veranstaltung" runat="server"></asp:Label>
                    <asp:TextBox ID="txtSuchen" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:CheckBox ID="cbNoParticipants" runat="server" Text="Keine leeren Projekte"/>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md-6">
                    <asp:Label Text="Datum der Veranstaltung" runat="server"></asp:Label>
                    <asp:TextBox ID="datepicker" placeholder="Datum" cssclass="datepicker-field" TextMode="Date" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:CheckBox ID="cbTooManyParticipants" runat="server" Text="Keine vollen Projekte"/>
                </div>
            </div>
            <asp:Button ID="btnFilter" Text="Suchen" class="btn btn-primary" runat="server" OnClick="btnFilter_Click" />
        </div>
          <br />
            <asp:GridView ID="gv_UserView" runat="server" 
            AutoGenerateColumns="False" 
            AllowPaging="True" 
            AllowSorting="True" OnRowCommand="gv_UserView_RowCommand" CssClass="table table-bordered" OnPageIndexChanging="gv_UserView_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="ID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblProjectID" runat="server" Text='<%# Eval("PID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Projekt">
                    <ItemTemplate>
                        <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Beschreibung">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Datum">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToString("dd/MM/yyyy") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>    
                <asp:TemplateField HeaderText="Straße">
                    <ItemTemplate>
                        <asp:Label ID="lblPlace" runat="server" Text='<%# Eval("Place") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nr.">
                    <ItemTemplate>
                        <asp:Label ID="lblNumber" runat="server" Text='<%# Eval("Number") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Beginn">
                    <ItemTemplate>
                        <asp:Label ID="lblStart" runat="server" Text='<%# Eval("start") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField><asp:TemplateField HeaderText="Ende">
                    <ItemTemplate>
                        <asp:Label ID="lblEnd" runat="server" Text='<%# Eval("end") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Kapazität">
                    <ItemTemplate>
                        <asp:Label ID="lblRemaining" runat="server" Text='<%# Eval("remainingCapacity") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Preis">
                    <ItemTemplate>
                        <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("PRICE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                    <%-- Details --%>
                <asp:TemplateField HeaderText="Details">
                    <ItemTemplate>
                        <asp:Button class="btn btn-primary" ID="btnShowDetails" runat="server" Text="Zur Anmeldung" CommandName="details" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            </asp:GridView>
        </div>
</asp:Content>
