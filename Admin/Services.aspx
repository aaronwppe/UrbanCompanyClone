<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="UrbanCompanyClone.Services" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="row">
            <h1><asp:Label ID="Label1" runat="server" Text="Services"></asp:Label></h1>
        </div>

        <br />

        <h5>Category</h5>
        <div class="row">
            <asp:DropDownList ID="CategoryDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CategoryDropDownList_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <h5>Sub Category</h5>
        <div class="row">
            <asp:DropDownList ID="SubCategoryDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubCategoryDropDownList_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <br />

        <h6>Filtered Results</h6>
        <div class="row">
            <asp:GridView ID="ServiceGridView" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="ServiceGridView_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField ItemStyle-Width="150px" HeaderText="No." />
                    <asp:BoundField ItemStyle-Width="200px" HeaderText="Category" />
                    <asp:BoundField ItemStyle-Width="300px" HeaderText="Sub Category" />
                    <asp:BoundField ItemStyle-Width="400px" HeaderText="Service" />

                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
            </asp:GridView>
        </div>
        

       
    </main>

</asp:Content>
