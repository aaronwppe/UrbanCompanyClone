using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Google.Protobuf.WellKnownTypes;

namespace UrbanCompanyClone
{
    public partial class Services : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            ServiceDataManager dataManager = new ServiceDataManager();

            BindCategoryDropDownList(dataManager.GetCategoryList());

            BindSubCategoryDropDownList(dataManager.GetSubCategoryList());

            BindServiceGridView(dataManager.GetServiceDataTable());

            ViewState["AllSubCategoriesLoaded"] = true;
            ViewState["AllServicesLoaded"] = true;
        }

        void BindCategoryDropDownList(List<Category> categories)
        {
            
            CategoryDropDownList.DataSource = categories;

            //[CHANGE] value is hardcoded
            CategoryDropDownList.DataTextField = "Name";
            CategoryDropDownList.DataValueField = "Id";

            CategoryDropDownList.DataBind();

            // select 'All'
            CategoryDropDownList.SelectedValue = "0";

            return;
        }

        void BindSubCategoryDropDownList(List<SubCategory> subCategories)
        {
            SubCategoryDropDownList.DataSource = subCategories;

            //[CHANGE] value is hardcoded
            SubCategoryDropDownList.DataTextField = "Name";
            SubCategoryDropDownList.DataValueField = "Id";

            SubCategoryDropDownList.DataBind();

            // select 'All'
            SubCategoryDropDownList.SelectedValue = "0";

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
                    BindSubCategoryDropDownList(new ServiceDataManager().GetSubCategoryList());
                    ViewState["AllSubCategoriesLoaded"] = true;
                }

                if (!AllServicesLoaded)
                {
                    BindServiceGridView(new ServiceDataManager().GetServiceDataTable());
                    ViewState["AllServicesLoaded"] = true;
                }

                return;
            }
                 
            BindSubCategoryDropDownList(new ServiceDataManager().GetSubCategoryList(selectedCategory));
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
    }
}