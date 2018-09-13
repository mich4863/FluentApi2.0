namespace FluentApi.Ef
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        public int? TeamId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmploymentDate { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        [StringLength(50)]
        public string CPRNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal? Salary { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }

        public virtual Team Team { get; set; }

        public string DisplayName
        {
            get
            {
                return this.Firstname + " " + this.Lastname;
            }
        }
    }
}
