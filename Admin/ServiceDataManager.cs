using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using UrbanCompanyClone.Exceptions;

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
        
        //Queries
        static readonly String query_selectIdNameFromCategories = $"SELECT {categoryFeature_id}, {categoryFeature_name} FROM {categoryTable}";

        static readonly String query_selectAllSubCategories = $"SELECT * FROM {subCategoryTable}";
        static readonly String query_selectSubCategories = $"SELECT * FROM {subCategoryTable} WHERE {subCategoryFeature_categoryId} = @categoryId";

        //Procedures
        /*
         * Adds a new category record
         * PARAMS => ( IN category_name VARCHAR(50), IN category_icon_url VARCHAR(255), 
	     *             OUT category_id INT, OUT duplicate_category_name TINYINT )
         */
        static readonly String procedure_addNewCategory = "Add_New_Category";

        /*
         * Adds a new sub category record
         * PARAMS => ( IN sub_category_name VARCHAR(50), IN sub_category_icon_url VARCHAR(255), IN category_id INT,
         *             OUT sub_category_id INT, OUT duplicate_sub_category_name TINYINT, OUT category_dne TINYINT )
         */
        static readonly String procedure_addNewSubCategory = "Add_New_Sub_Category";

        /*
         * Adds new service record
         * PARAMS => ( IN service_name VARCHAR(50), IN service_image_url VARCHAR(255), IN service_description TEXT,
         *             IN service_cost DECIMAL(4, 2), IN sub_category_id INT,
         *             OUT service_id INT, OUT duplicate_service_name TINYINT, OUT sub_category_dne TINYINT )
         */
        static readonly String procedure_addNewService = "Add_New_Service";

        /*
         * Get_Services
         * PARAMS => (IN category_id, IN sub_category_id) 
         */
        static readonly String procedure_getServices = "Get_Services";

        /*
         * Add_Service_Tuple
         * PARAMS => (IN category_name, IN sub_category_name, IN service_name,
                      OUT category_id, OUT sub_category_id, OUT service_id)
        */
        static readonly String procedure_addServiceTuple = "Add_Service_Tuple";


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

                    using (MySqlCommand command = new MySqlCommand(query_selectIdNameFromCategories, connection))
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

        public List<SubCategory> GetSubCategoryList(int categoryId = 0)
        {
            String query;

            if (categoryId == 0)
                query = query_selectAllSubCategories;
            else
                query = query_selectSubCategories;



            List<SubCategory> subCategoryList = new List<SubCategory>
            {
                new SubCategory(0, "All")
            };

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
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
        private DataTable CallProcedureGetServices (Int32 categoryId = 0, Int32 subCategoryId = 0)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(procedure_getServices, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new MySqlParameter("@category_id", MySqlDbType.Int32)).Value = categoryId;
                        command.Parameters.Add(new MySqlParameter("@sub_category_id", MySqlDbType.Int32)).Value = subCategoryId;

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

            }
            catch (MySqlException) { }

            return dataTable;
        }
        public DataTable GetServiceDataTable()
        {
            return CallProcedureGetServices();
        }

        public DataTable GetServiceDataTableByCategory(int categoryId)
        {
            return CallProcedureGetServices(categoryId);
        }

        public DataTable GetServiceDataTableBySubCategory(int subCategoryId)
        {
           return CallProcedureGetServices(0, subCategoryId);
        }

        private void AddInputParameter(MySqlCommand command, String name, String value)
        {
            MySqlDbType type = MySqlDbType.VarChar;
            int size = 50;

            MySqlParameter parameter = new MySqlParameter(name, type, size);
            parameter.Value = value;

            command.Parameters.Add(parameter);
        }

        private MySqlParameter AddOutputParameter(MySqlCommand command, String name)
        {
            MySqlDbType type = MySqlDbType.Int32;

            MySqlParameter parameter = new MySqlParameter(name, type)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(parameter);

            return parameter;
        }

        public Int32 AddNewCategory(String categoryName, String iconUrl)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(procedure_addNewCategory, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new MySqlParameter("@category_name", MySqlDbType.VarChar, 50)).Value = categoryName;
                    command.Parameters.Add(new MySqlParameter("@category_icon_url", MySqlDbType.VarChar, 255)).Value = iconUrl;

                    MySqlParameter categoryIdParameter = new MySqlParameter("@category_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                    MySqlParameter duplicateCategoryNameParameter = new MySqlParameter("@duplicate_category_name", MySqlDbType.Bit) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(categoryIdParameter);
                    command.Parameters.Add(duplicateCategoryNameParameter);

                    command.ExecuteNonQuery();

                    Int32 categoryId = Convert.ToInt32(categoryIdParameter.Value);

                    // category name not unique 
                    if (Convert.ToBoolean(duplicateCategoryNameParameter.Value))
                        throw new DuplicateCategoryException() { CategoryId = categoryId };

                    return categoryId;
                }
            }
        }

        public Int32 AddNewSubCategory(String subCategoryName, String iconUrl, Int32 categoryId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(procedure_addNewSubCategory, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //input paramters
                    command.Parameters.Add(new MySqlParameter("@sub_category_name", MySqlDbType.VarChar, 50)).Value = subCategoryName;
                    command.Parameters.Add(new MySqlParameter("@sub_category_icon_url", MySqlDbType.VarChar, 255)).Value = iconUrl;
                    command.Parameters.Add(new MySqlParameter("@category_id", MySqlDbType.Int32)).Value = categoryId;

                    //output parameters
                    MySqlParameter subCategoryIdParameter = new MySqlParameter("@sub_category_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                    MySqlParameter duplicateSubCategoryNameParameter = new MySqlParameter("@duplicate_sub_category_name", MySqlDbType.Bit) { Direction = ParameterDirection.Output };
                    MySqlParameter categoryDneParameter = new MySqlParameter("@category_dne", MySqlDbType.Bit) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(subCategoryIdParameter);
                    command.Parameters.Add(duplicateSubCategoryNameParameter);
                    command.Parameters.Add(categoryDneParameter);

                    command.ExecuteNonQuery();

                    Int32 subCategoryId = Convert.ToInt32(subCategoryIdParameter.Value);

                    //subcategory name not unique - not inserted
                    if (Convert.ToBoolean(duplicateSubCategoryNameParameter.Value))
                        throw new DuplicateSubCategoryException() { SubCategoryId = subCategoryId };

                    //category does not exist 
                    if (Convert.ToBoolean(categoryDneParameter.Value))
                        throw new InvalidCategoryException();

                    return subCategoryId;
                }
            }
        }

        public Int32 AddNewService(String serviceName, String imageUrl, String description, Decimal cost, Int32 subCategoryId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(procedure_addNewService, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //input paramters
                    command.Parameters.Add(new MySqlParameter("@service_name", MySqlDbType.VarChar, 50)).Value = serviceName;
                    command.Parameters.Add(new MySqlParameter("@service_image_url", MySqlDbType.VarChar, 255)).Value = imageUrl;
                    command.Parameters.Add(new MySqlParameter("@service_description", MySqlDbType.Text)).Value = description;
                    command.Parameters.Add(new MySqlParameter("@service_cost", MySqlDbType.Decimal) { Precision = 4, Scale = 2 }).Value = cost;
                    command.Parameters.Add(new MySqlParameter("@sub_category_id", MySqlDbType.Int32)).Value = subCategoryId;

                    //output parameters
                    MySqlParameter serviceIdParameter = new MySqlParameter("@service_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                    MySqlParameter duplicateServiceNameParameter = new MySqlParameter("@duplicate_service_name", MySqlDbType.Bit) { Direction = ParameterDirection.Output };
                    MySqlParameter subCategoryDneParamter = new MySqlParameter("@sub_category_dne", MySqlDbType.Bit) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(serviceIdParameter);
                    command.Parameters.Add(duplicateServiceNameParameter);
                    command.Parameters.Add(subCategoryDneParamter);

                    command.ExecuteNonQuery();

                    Int32 serviceId = Convert.ToInt32(serviceIdParameter.Value);

                    //service name not unique
                    if (Convert.ToBoolean(duplicateServiceNameParameter.Value))
                        throw new DuplicateServiceException() { ServiceId = serviceId };

                    //sub category does not exist 
                    if (Convert.ToBoolean(subCategoryDneParamter.Value))
                        throw new InvalidSubCategoryException();

                    return serviceId;
                }
            }
        }
    }
}