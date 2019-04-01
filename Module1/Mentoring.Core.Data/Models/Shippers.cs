using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentoring.Core.Data.Models
{
    public partial class Shippers
    {
        public Shippers()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
