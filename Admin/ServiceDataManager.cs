using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace UrbanCompanyClone
{
    public class ServiceDataManager
    {
        //Category Table Constants
        static readonly String categoryTable = "category";
        static readonly String categoryFeature_id = "id";
        static readonly String categoryFeature_name = "name";

        //Sub-Category Table Constants
        static readonly String subCategoryTable = "sub_category";
        static readonly String subCategoryFeature_id = "id";
        static readonly String subCategoryFeature_categoryId = "category_id";
        static readonly String subCategoryFeature_name = "name";

        //Service Table Constants
        static readonly String serviceTable = "service";
        static readonly String serviceFeature_id = "id";
        static readonly String serviceFeature_subCategoryId = "sub_category_id";
        static readonly String serviceFeature_name = "name";

        //Service Data Table Structure
        public static readonly string[] serviceDataTableColumn = { "service_id", "row_num", "category_name", "sub_category_name", "service_name" };
        static readonly String serviceDataTableStructure = $"{serviceTable}.{serviceFeature_id} AS {serviceDataTableColumn[0]}, " +
                                                           $"ROW_NUMBER() OVER (ORDER BY {serviceTable}.{serviceFeature_id}) AS {serviceDataTableColumn[1]}, " + //[inconsistency] db used for view management
                                                           $"{categoryTable}.{categoryFeature_name} AS {serviceDataTableColumn[2]}, " +
                                                           $"{subCategoryTable}.{subCategoryFeature_name} AS {serviceDataTableColumn[3]}, " +
                                                           $"{serviceTable}.{serviceFeature_name} AS {serviceDataTableColumn[4]} ";

        //Queries
        static readonly String query_selectAllCategories = $"SELECT * FROM {categoryTable}";

        static readonly String query_selectAllSubCategories = $"SELECT * FROM {subCategoryTable}";
        static readonly String query_selectSubCategories = $"SELECT * FROM {subCategoryTable} WHERE {subCategoryFeature_categoryId} = @categoryId";

        static readonly String query_selectAllServices = "SELECT " + serviceDataTableStructure + $"FROM {serviceTable} " +
                                                         $"JOIN {subCategoryTable} ON {serviceTable}.{serviceFeature_subCategoryId} = {subCategoryTable}.{subCategoryFeature_id} " +
                                                         $"JOIN {categoryTable} ON {subCategoryTable}.{subCategoryFeature_categoryId} = {categoryTable}.{categoryFeature_id}";

        static readonly String query_selectServicesFromCategory = "SELECT " + serviceDataTableStructure + $"FROM {serviceTable} " +
                                                                  $"JOIN {subCategoryTable} ON {serviceTable}.{serviceFeature_subCategoryId} = {subCategoryTable}.{subCategoryFeature_id} " +
                                                                  $"JOIN {categoryTable} ON {subCategoryTable}.{subCategoryFeature_categoryId} = {categoryTable}.{categoryFeature_id} " +
                                                                  $"WHERE {categoryTable}.{categoryFeature_id} = @categoryId";

        static readonly String query_selectServicesFromSubCategory = "SELECT " + serviceDataTableStructure + $"FROM {serviceTable} " +
                                                                  $"JOIN {subCategoryTable} ON {serviceTable}.{serviceFeature_subCategoryId} = {subCategoryTable}.{subCategoryFeature_id} " +
                                                                  $"JOIN {categoryTable} ON {subCategoryTable}.{subCategoryFeature_categoryId} = {categoryTable}.{categoryFeature_id} " +
                                                                  $"WHERE {subCategoryTable}.{subCategoryFeature_id} = @subCategoryId";


        //use your mysql connection string here instead
        static readonly String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ServiceDataManager() {}

        public List<Category> GetCategoryList()
        {
            List<Category> categoryList = new List<Category>
            {
                new Category(0, "All")
            };

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_selectAllCategories, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category(Convert.ToInt32(reader[categoryFeature_id]), reader[categoryFeature_name].ToString());
                            categoryList.Add(category);
                        }
                    }
                }
            }
            catch (MySqlException) {}

            return categoryList;
        }

        public List<SubCategory> GetSubCategoryList()
        {
            List<SubCategory> subCategoryList = new List<SubCategory>
            {
                new SubCategory(0, "All")
            };

            try
            {

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_selectAllSubCategories, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubCategory subCategory = new SubCategory(Convert.ToInt32(reader[subCategoryFeature_id]), reader[subCategoryFeature_name].ToString());
                            subCategoryList.Add(subCategory);
                        }
                    }
                }
            }
            catch (MySqlException) {}

            return subCategoryList;
        }

        public List<SubCategory> GetSubCategoryList(int categoryId)
        {
            List<SubCategory> subCategoryList = new List<SubCategory>
            {
                new SubCategory(0, "All")
            };

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_selectSubCategories, connection))
                    {
                        command.Parameters.AddWithValue("categoryId", categoryId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SubCategory subCategory = new SubCategory(Convert.ToInt32(reader[subCategoryFeature_id]), reader[subCategoryFeature_name].ToString());
                                subCategoryList.Add(subCategory);
                            }
                        }
                    }
                }
            }
            catch (MySqlException) {}

            return subCategoryList;
        }

        public DataTable GetServiceDataTable()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_selectAllServices, connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (MySqlException) {}

            return dataTable;
        }

        public DataTable GetServiceDataTableByCategory(int categoryId)
        {
            DataTable dataTable = new DataTable();
            
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_selectServicesFromCategory, connection))
                    {
                        command.Parameters.AddWithValue("categoryId", categoryId);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    
                }
            }
            catch (MySqlException) {}

            return dataTable;
        }

        public DataTable GetServiceDataTableBySubCategory(int subCategoryId)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query_selectServicesFromSubCategory, connection))
                    {
                        command.Parameters.AddWithValue("subCategoryId", subCategoryId);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                }
            }
            catch (MySqlException) {}

            return dataTable;
        }
    }
}