using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrbanCompanyClone.Exceptions
{
    public class DuplicateSubCategoryException :Exception
    {
        public Int32 SubCategoryId { get; set; }
        public DuplicateSubCategoryException() { }
    }
}