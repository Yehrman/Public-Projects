using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompuskillsMvcProject.Models
{
    public class ErrorMessages
    {
      public string IdError()
        {
            string error = "Please put a  number at the end of the url";
            return error;
        }
        public string NullError(string type)
        {
            string error = "We don't have a " + type + " with that id in our system";
            return error;
        }
    }
}