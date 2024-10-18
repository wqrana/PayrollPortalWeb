using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("PayrollStatus")]
    public partial class PayrollStatus : BaseEntity
    {
        public PayrollStatus()
        {

        }
        [Column("PayrollStatusId")]
        public override int Id { get; set; }
        public string PayrollStatusName { get; set; }

    }
}
