using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentoring.Core.Data.Models
{
    public partial class CustomerDemographics
    {
        public CustomerDemographics()
        {
            CustomerCustomerDemo = new HashSet<CustomerCustomerDemo>();
        }


        [Key]
        public string CustomerTypeId { get; set; }
        public string CustomerDesc { get; set; }

        public ICollection<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
    }
}
