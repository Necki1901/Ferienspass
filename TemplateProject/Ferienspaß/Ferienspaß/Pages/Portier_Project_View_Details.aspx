<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Portier.Master" AutoEventWireup="true" CodeBehind="Portier_Project_View_Details.aspx.cs" Inherits="Ferienspaß.Pages.Portier_Project_View_Details" %>

<asp:Content ID="UserContent" ContentPlaceHolderID="LoggedInUserContent" runat="server">
    <asp:Label ID="lbl_loggedInUser" Font-Bold="true" CssClass="mr-1 ml-2 pr-1" runat="server"></asp:Label>
    <asp:LinkButton ID="btnLogout" runat="server" Text="Abmelden" ToolTip="Abmelden" OnClick="btnLogout_Click" CssClass="btn-sm btn-outline-primary my-2 my-sm-0"><i class='fa fa-sign-out' style='font-size:28px'></i></asp:LinkButton>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <asp:GridView CssClass="table table-bordered" ID="gv_User_View_Details" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_User_View_Details_RowCommand">
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

                 <asp:TemplateField HeaderText="Freie Plätze">
                    <ItemTemplate>
                        <asp:Label ID="lblParticipants" runat="server" Text='<%# Eval("remainingCapacity") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Preis">
                    <ItemTemplate>
                        <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("PRICE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                  <%-- Details --%>
                <asp:TemplateField HeaderText="Aktion">
                    <ItemTemplate>
                        <asp:Button ID="btnRegister" CssClass="btn btn-primary" runat="server" Text="Anmelden" CommandName="register"/>
                        <asp:Button ID="btnQueue" CssClass="btn btn-primary" runat="server" Text="In die Warteschlange" CommandName="queue" Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
               
             </Columns>
        </asp:GridView>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>

        <asp:GridView ID="gv_Children" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False" Visible="False">
            <columns>
                 <asp:TemplateField HeaderText="ChildID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblChildID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Vorname">
                    <ItemTemplate>
                        <asp:Label ID="lblGivenname" runat="server" Text='<%# Eval("GN") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nachname">
                    <ItemTemplate>
                        <asp:Label ID="lblSurname" runat="server" Text='<%# Eval("SN") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Geburtsdatum">
                    <ItemTemplate>
                        <asp:Label ID="lblBirthday" runat="server" Text='<%# Convert.ToDateTime(Eval("BD")).ToString("dd/MM/yyyy") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Aktion">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkUseChildren" runat="server" CommandName="checked" />
                    </ItemTemplate>
                </asp:TemplateField>


            </columns>
        </asp:GridView>

        <asp:Label ID="lblChildrenMessage" runat="server"></asp:Label><br />

        <asp:Button ID="btnAddChildren" class="btn btn-primary" runat="server" OnClick="btnAddChildren_Click" Text="Kinder hinzufügen" Visible="False" />
</asp:Content>
