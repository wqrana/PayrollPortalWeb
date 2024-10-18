using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("Company")]
    public partial class Company : BaseEntity
    {
        public Company()
        {

        }

        [Column("CompanyId")]
        public override int Id { get; set; }       
        public string CompanyName { get; set; }
        public string DBServerName { get; set; }
        public string DBName { get; set; }
        public string DBUser { get; set; }
        public string DBPassword { get; set; }

    }
}
