using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrbanCompanyClone
{
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }


        public Category (int id, String name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    public class SubCategory
    {

        public int Id { get; set; }
        public String Name { get; set; }


        public SubCategory (int id, String name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    public class Service
    {

        public int Id { get; set; }
        public String Name { get; set; }

        public Service (int id, String name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}