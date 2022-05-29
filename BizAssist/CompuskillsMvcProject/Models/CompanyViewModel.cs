using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CompuskillsMvcProject.Models
{
    public class CompanyViewModel
    {
        [Required]
        public string CompanyName { get; set; }
       
        [Required(ErrorMessage = "Phone number required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "The phone number must be 10 digits.Please add a 0 at the begining of the number if it's under 10 digits")]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [Display (Name ="Billing Address")]
        public string Address { get; set; }
  
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
      
        [Display(Name ="Please select the time interval that you want to pay employees at")]
        public string EmployeePayInterval { get; set; }
        [Display(Name ="Please select the time interval you want to bill clients at")]
        public string ClientBillInterval { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        public List<string> Interval()
        {
            List<string> time = new List<string>();
            time.Add("daily");
            time.Add("weekly");
            time.Add("Every other week");
            time.Add("monthly");
            time.Add("On a per project basis");
            return time;
        }
        public string TimeZone { get; set; }
        public List<string> TimeZones()
        {
            List<string> timeZones = new List<string>();
            var zones = TimeZoneInfo.GetSystemTimeZones();
            foreach (var item in zones)
            {
                timeZones.Add(item.Id);
            }
            return timeZones;
        }
    }
}