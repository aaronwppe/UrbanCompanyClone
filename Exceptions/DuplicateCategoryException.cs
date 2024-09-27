using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrbanCompanyClone.Exceptions
{
    public class DuplicateCategoryException : Exception
    {
        public Int32 CategoryId { get; set; }
        public DuplicateCategoryException() { }
    }
}