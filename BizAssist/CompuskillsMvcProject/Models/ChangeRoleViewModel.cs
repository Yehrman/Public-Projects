using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompuskillsMvcProject.Models
{
    public class ChangeRoleViewModel
    {
        public string EmployeeId { get; set; }
      //  public string Employee { get; set; }
        public string RoleId { get; set; }
       // public string Role { get; set; }
        public static Dictionary<string, string> Roles()
        {
            Dictionary<string, string> roles = new Dictionary<string,string >();
            roles.Add("Employee", "35FB245B-FDE0-464D-9985-CFDD0F2AEB18");
            roles.Add("Finance department", "DA5E99CF-E875-41DD-8300-9E0101D97D39");
            roles.Add("Human resources department", "D2EF7DA3-4C52-4B48-A141-D8E41CE8C7A9");
            roles.Add("Ceo", "602514B5-413F-419D-97A1-2533130D8DC4");
            roles.Add("Senior managment", "DA422AC8-7F2D-46AC-AE2C-C536FC98402F");
            return roles;
        }
    }
}