using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentoring.Core.Data.Models
{
    public partial class Territories
    {
        public Territories()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritories>();
        }

        [Key]
        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

        public Region Region { get; set; }
        public ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}
