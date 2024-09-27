using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrbanCompanyClone.Exceptions
{
    public class DuplicateServiceException : Exception
    {
        public Int32 ServiceId { get; set; }
        public DuplicateServiceException() { }
    }
}