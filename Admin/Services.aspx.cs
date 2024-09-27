using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using UrbanCompanyClone.Exceptions;

namespace UrbanCompanyClone
{
    public partial class Services : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            ServiceDataManager dataManager = new ServiceDataManager();

            List<Category> categoryList = dataManager.GetCategoryList();
            List<SubCategory> subCategoryList = dataManager.GetSubCategoryList();

            BindDropDownList(CategoryDropDownList, categoryList);
            //'All' Selected
            CategoryDropDownList.SelectedIndex = 0;

            BindDropDownList(SubCategoryDropDownList, subCategoryList);
            //'All' Selected
            SubCategoryDropDownList.SelectedIndex = 0;



            //[TO DO] process these ddls client-side dynamically
            BindDropDownList(AddSubCategoryMPCategoryDDL, categoryList);
            AddSubCategoryMPCategoryDDL.Items.RemoveAt(0);                  //Remove 'All'

            BindDropDownList(AddServiceMPSubCategoryDDL, subCategoryList);
            AddServiceMPSubCategoryDDL.Items.RemoveAt(0);                   //Remove 'All'


            BindServiceGridView(dataManager.GetServiceDataTable());

            ViewState["AllSubCategoriesLoaded"] = true;
            ViewState["AllServicesLoaded"] = true;
        }

        void BindDropDownList(DropDownList dropDownList, IEnumerable<Object> list)
        {
            dropDownList.DataSource = list;

            dropDownList.DataTextField = "Name";
            dropDownList.DataValueField = "Id";

            dropDownList.DataBind();
            return;
        }

        protected void BindServiceGridView(DataTable services)
        {
            String[] columnName = ServiceDataManager.serviceDataTableColumn;
            ServiceGridView.DataSource = services;

            ServiceGridView.DataKeyNames = new String[] { columnName[0] };

            for (int i = 1; i < columnName.Length; i++)
            {
                BoundField boundField = ServiceGridView.Columns[i - 1] as BoundField;
                boundField.DataField = columnName[i];
            }

            ServiceGridView.DataBind();

        }

        protected void CategoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCategory = Convert.ToInt32(CategoryDropDownList.SelectedValue);
            bool AllSubCategoriesLoaded = Convert.ToBoolean(ViewState["AllSubCategoriesLoaded"]);
            bool AllServicesLoaded = Convert.ToBoolean(ViewState["AllServicesLoaded"]);

            // 'All' is selected
            if (selectedCategory == 0)
            {
                if (!AllSubCategoriesLoaded)
                {
                    BindDropDownList(SubCategoryDropDownList, new ServiceDataManager().GetSubCategoryList());
                    ViewState["AllSubCategoriesLoaded"] = true;
                }

                if (!AllServicesLoaded)
                {
                    BindServiceGridView(new ServiceDataManager().GetServiceDataTable());
                    ViewState["AllServicesLoaded"] = true;
                }

                return;
            }

            BindDropDownList(SubCategoryDropDownList, new ServiceDataManager().GetSubCategoryList(selectedCategory));
            ViewState["AllSubCategoriesLoaded"] = false;

            BindServiceGridView(new ServiceDataManager().GetServiceDataTableByCategory(selectedCategory));
            ViewState["AllServicesLoaded"] = false;
            return;
        }

        protected void SubCategoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSubCategory = Convert.ToInt32(SubCategoryDropDownList.SelectedValue);

            // 'All' is selected
            if (selectedSubCategory == 0)
                return;

            BindServiceGridView(new ServiceDataManager().GetServiceDataTableBySubCategory(selectedSubCategory));
            ViewState["AllServicesLoaded"] = false;
            return;
        }

        protected void ServiceGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[] columnName = ServiceDataManager.serviceDataTableColumn;
            int serviceId = Convert.ToInt32(ServiceGridView.DataKeys[ServiceGridView.SelectedIndex].Values[columnName[0]]);

            Response.Write("Selected Service ID: " + serviceId);
        }

        protected void ServiceGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                int selectedID = Convert.ToInt32(e.CommandArgument);
                SelectModalCategoryTextBox.Text = "Category";

                SelectMPE.Show();
            }
        }

        private string SaveImage(FileUpload image)
        {
            String savePath = @"Uploads\" + Server.HtmlEncode(image.FileName);

            image.SaveAs(Request.PhysicalApplicationPath + savePath);

            return savePath;
        }

        protected void AddCategoryMP_Submit(object sender, EventArgs e)
        {
            //Keep Modal Open
            AddCategoryMPE.Show();

            String categoryName = NewCategoryNameTextBox.Text;
            FileUpload iconFile = NewCategoryIconFileUpload;

            if (!iconFile.HasFile)
                return;

            String savePath = SaveImage(iconFile);

            try
            {
                int categoryId = new ServiceDataManager().AddNewCategory(categoryName, savePath);
                NewCategoryNameTextBox.Text = string.Empty;
                AddCategoryMPE.Hide();

                Response.Write("Category Added Successfully!");
            }
            catch (DuplicateCategoryException ex)
            {
                Response.Write($"Category name not unique [ID:{ex.CategoryId}]");
            }
            catch (Exception)
            {
                //[TO-DO] Handle MySqlException
                Response.Write("Category could not be added!");
            }
        }

        protected void AddSubCategoryMP_Submit(object sender, EventArgs e)
        {
            //Keep Modal Open
            AddCategoryMPE.Show();

            Int32 categoryId = Convert.ToInt32(AddSubCategoryMPCategoryDDL.SelectedValue);
            String subCategoryName = NewSubCategoryNameTextBox.Text;
            FileUpload iconFile = NewSubCategoryIconFileUpload;

            if (!iconFile.HasFile)
                return;

            String savePath = SaveImage(iconFile);

            try
            {
                int subCategoryId = new ServiceDataManager().AddNewSubCategory(subCategoryName, savePath, categoryId);
                NewCategoryNameTextBox.Text = string.Empty;
                AddCategoryMPE.Hide();

                Response.Write("SubCategory Added Successfully!");
            }
            catch (InvalidCategoryException)
            {
                Response.Write("Category does not exist!");
            }
            catch (DuplicateSubCategoryException ex)
            {
                Response.Write($"Sub category name not unique [ID:{ex.SubCategoryId}]");
            }
            catch (Exception)
            {
                //[TO-DO] Handle MySqlException
                Response.Write("SubCategory could not be added!");
            }
        }

        protected void AddServiceMP_Submit(object sender, EventArgs e)
        {
            //Keep Modal Open
            AddServiceMPE.Show();

            Int32 subCategoryId = Convert.ToInt32(AddServiceMPSubCategoryDDL.SelectedValue);
            String serviceName = AddServiceMP_ServiceNameTextBox.Text;
            FileUpload imageFile = AddServiceMP_ImageFileUpload;
            String description = AddServiceMP_DescriptionTextBox.Text;
            //(4,2) constraint of decimal needs to be handled
            Decimal cost = Convert.ToDecimal(AddServiceMP_CostTextBox.Text);

            if (!imageFile.HasFile)
                return;

            String savePath = SaveImage(imageFile);

            try
            {
                int serviceId = new ServiceDataManager().AddNewService(serviceName, savePath, description, cost, subCategoryId);
                AddServiceMP_ServiceNameTextBox.Text = string.Empty;
                AddServiceMP_DescriptionTextBox.Text = string.Empty;
                AddServiceMP_CostTextBox.Text = string.Empty;
                AddCategoryMPE.Hide();

                Response.Write("Service Added Successfully!");
            }
            catch (InvalidSubCategoryException)
            {
                Response.Write("Sub Category does not exist!");
            }
            catch (DuplicateServiceException ex)
            {
                Response.Write($"Service name not unique [ID:{ex.ServiceId}]");
            }
            catch (Exception)
            {
                //[TO-DO] Handle MySqlException
                Response.Write("Service could not be added!");
            }
        }
    }
}