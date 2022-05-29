using System;

namespace MvcProjectDbConn
{
    public class DeletedCompany
    {
        public int id { get; set; }
        public string CompanyName { get; set; }
        public int AmountOfEmployees { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateLeft { get; set; }
    }
}