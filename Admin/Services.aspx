<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="UrbanCompanyClone.Services" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <main>
         
        <div class="row">
            <h1><asp:Label ID="Label1" runat="server" Text="Services"></asp:Label></h1>
        </div>
        <br />

        <!--Dropdowns and Buttons-->       
        <div class="row">

            <!--Category Drop Down List--> 
            <div class="col-3">
                <h5>Category</h5>
                <asp:DropDownList ID="CategoryDropDownList" 
                                  runat="server" 
                                  AutoPostBack="True" 
                                  OnSelectedIndexChanged="CategoryDropDownList_SelectedIndexChanged"/>
            </div>       

            <!--Sub Category Drop Down List-->
            <div class="col-3">
                <h5>Sub Category</h5>
                <asp:DropDownList ID="SubCategoryDropDownList" 
                                  runat="server" 
                                  AutoPostBack="True"  
                                  OnSelectedIndexChanged="SubCategoryDropDownList_SelectedIndexChanged"/>
            </div>

            <!--Add Category Button--> 
            <div class="col-2">
                <asp:Button ID="AddCategoryButton" 
                            runat="server"
                            Text="Add New Category" />
                <ajaxToolkit:ModalPopupExtender ID="AddCategoryMPE" 
                                runat="server" 
                                TargetControlId="AddCategoryButton" 
                                PopupControlID="AddCategoryModalPanel" 
                                OkControlID="CloseAddCategoryButton" />
            </div>

            <!--Add Sub Category Button--> 
            <div class="col-2">
                <asp:Button ID="AddSubCategoryButton" 
                            runat="server"
                            Text="Add New Sub Category" />
                <ajaxToolkit:ModalPopupExtender 
                                ID="AddSubCategoryMPE" 
                                runat="server"
                                TargetControlId="AddSubCategoryButton" 
                                PopupControlID="AddSubCategoryModalPanel" 
                                OkControlID="CloseAddSubCategoryButton" />
            </div>

            <!--Add Service Button--> 
            <div class="col-2">
                <asp:Button ID="AddServiceButton" 
                            runat="server"
                            Text="Add New Service" />
                <ajaxToolkit:ModalPopupExtender 
                                ID="AddServiceMPE" 
                                runat="server"
                                TargetControlId="AddServiceButton" 
                                PopupControlID="AddServiceModalPanel" 
                                OkControlID="AddServiceMP_CloseButton" />
            </div>
        <br />

        <!--Service Table-->
        <div class="row">
            <asp:GridView ID="ServiceGridView" runat="server" OnRowCommand="ServiceGridView_RowCommand" AutoGenerateColumns="False" OnSelectedIndexChanged="ServiceGridView_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField ItemStyle-Width="150px" HeaderText="No." />
                    <asp:BoundField ItemStyle-Width="200px" HeaderText="Category" />
                    <asp:BoundField ItemStyle-Width="300px" HeaderText="Sub Category" />
                    <asp:BoundField ItemStyle-Width="400px" HeaderText="Service" />

                    <asp:TemplateField>
                        <ItemTemplate>                                 
                            <asp:Button runat="server" 
                                        CommandName="Add" 
                                        CommandArgument='<%# Eval("service_id") %>' 
                                        Text="Add" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </main>

    <!--Select Modal-->
    <!--
    <asp:Button ID="SelectModalButton" 
                runat="server" 
                Style="display:none;"/>

    <ajaxToolkit:ModalPopupExtender ID="SelectMPE" 
                                    runat="server" 
                                    TargetControlId="SelectModalButton" 
                                    PopupControlID="ModalPanel" 
                                    OkControlID="OKButton" />

    <asp:Panel ID="ModalPanel" runat="server" Width="500px" BackColor="White">
        <header>
            Selected Service
        </header>

        <asp:TextBox ID="SelectModalCategoryTextBox" runat="server" Enabled="false"></asp:TextBox>
        <asp:TextBox ID="SelectModalSubCategoryTextBox" runat="server" Enabled="false"></asp:TextBox>
        <asp:TextBox ID="SelectModalServiceTextBox" runat="server" Enabled="false"></asp:TextBox>

        <footer>
            <asp:Button ID="OKButton" runat="server" Text="Close" />
        </footer>
    </asp:Panel>-->

    <!--Add Category Modal-->
    <asp:Panel ID="AddCategoryModalPanel" runat="server" Width="500" BackColor="White">
 
        <header>
            Add New Category
        </header>

        <table>
            <tr>
                <td>Category</td>
                <td>
                    <asp:TextBox ID="NewCategoryNameTextBox" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        runat="server"
                        ErrorMessage="*Required" 
                        ControlToValidate="NewCategoryNameTextBox"
                        EnableClientScript="true"
                        ValidationGroup="AddCategoryMPVG">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                    <td>Icon</td>
                    <td>
                        <asp:FileUpload ID="NewCategoryIconFileUpload" runat="server" />
                        <asp:RequiredFieldValidator 
                            runat="server"
                            ErrorMessage="*Required" 
                            ControlToValidate="NewCategoryIconFileUpload"
                            EnableClientScript="true"
                            ValidationGroup="AddCategoryMPVG">
                        </asp:RequiredFieldValidator>
                    </td>
            </tr>
        </table>

        <footer>
            <asp:Button ID="CloseAddCategoryButton" runat="server" Text="Close" />
            <asp:Button 
                ID="AddNewCategoryButton" 
                runat="server" 
                Text="Add" 
                OnClick="AddCategoryMP_Submit"
                ValidationGroup="AddCategoryMPVG"/>
        </footer>
        
    </asp:Panel>

    <!--Add Sub Category Modal-->
    <asp:Panel ID="AddSubCategoryModalPanel" runat="server" Width="500" BackColor="White">
        <header>
            Add New Sub Category
        </header>

        <table>
            <tr>
                <td>Category</td>
                <td>
                    <asp:DropDownList 
                        ID="AddSubCategoryMPCategoryDDL" 
                        runat="server" 
                        AutoPostBack="True"/>
                </td>
            </tr>
            <tr>
                <td>Sub Category</td>
                <td>
                    <asp:TextBox ID="NewSubCategoryNameTextBox" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        runat="server"
                        ErrorMessage="*Required" 
                        ControlToValidate="NewSubCategoryNameTextBox"
                        EnableClientScript="true"
                        ValidationGroup="AddSubCategoryMPVG">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                    <td>Icon</td>
                    <td>
                    <asp:FileUpload ID="NewSubCategoryIconFileUpload" runat="server" />
                    <asp:RequiredFieldValidator 
                        runat="server"
                        ErrorMessage="*Required" 
                        ControlToValidate="NewSubCategoryIconFileUpload"
                        EnableClientScript="true"
                        ValidationGroup="AddSubCategoryMPVG">
                    </asp:RequiredFieldValidator>
                    </td>
            </tr>
        </table>

        <footer>
            <asp:Button ID="CloseAddSubCategoryButton" runat="server" Text="Close" />
            <asp:Button 
                ID="AddNewSubCategoryButton" 
                runat="server" 
                Text="Add" 
                OnClick="AddSubCategoryMP_Submit" 
                ValidationGroup="AddSubCategoryMPVG"/>
        </footer>
    </asp:Panel> 

    <!--Add Service Modal-->
    <asp:Panel ID="AddServiceModalPanel" runat="server" Width="500" BackColor="White">
        <header>
            Add New Service
        </header>

        <table>
            <tr>
                <td>Sub Category</td>
                <td>
                    <asp:DropDownList 
                        ID="AddServiceMPSubCategoryDDL" 
                        runat="server" 
                        AutoPostBack="True"/>
                </td>
            </tr>
            <tr>
                <td>Service</td>
                <td>
                    <asp:TextBox ID="AddServiceMP_ServiceNameTextBox" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        runat="server"
                        ErrorMessage="*Required" 
                        ControlToValidate="AddServiceMP_ServiceNameTextBox"
                        EnableClientScript="true"
                        ValidationGroup="AddServiceMPVG">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Image</td>
                <td>
                    <asp:FileUpload ID="AddServiceMP_ImageFileUpload" runat="server" />
                    <asp:RequiredFieldValidator 
                        runat="server"
                        ErrorMessage="*Required" 
                        ControlToValidate="AddServiceMP_ImageFileUpload"
                        EnableClientScript="true"
                        ValidationGroup="AddServiceMPVG">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Description</td>
                <td>
                    <asp:TextBox ID="AddServiceMP_DescriptionTextBox" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        runat="server"
                        ErrorMessage="*Required" 
                        ControlToValidate="AddServiceMP_DescriptionTextBox"
                        EnableClientScript="true"
                        ValidationGroup="AddServiceMPVG">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Cost</td>
                <td>
                    <asp:TextBox ID="AddServiceMP_CostTextBox" runat="server" type="number"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        runat="server"
                        ErrorMessage="*Required" 
                        ControlToValidate="AddServiceMP_CostTextBox"
                        EnableClientScript="true"
                        ValidationGroup="AddServiceMPVG">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>

        </table>

        <footer>
            <asp:Button ID="AddServiceMP_CloseButton" runat="server" Text="Close" />
            <asp:Button 
                ID="AddServiceMP_SubmitButton" 
                runat="server" 
                Text="Add" 
                OnClick="AddServiceMP_Submit" 
                ValidationGroup="AddServiceMPVG"/>
        </footer>
    </asp:Panel> 

</asp:Content>