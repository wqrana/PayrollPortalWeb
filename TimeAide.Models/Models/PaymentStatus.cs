using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("PaymentStatus")]
    public partial class PaymentStatus : BaseEntity
    {
        public PaymentStatus()
        {

        }
        [Column("PaymentStatusId")]
        public override int Id { get; set; }
        public string PaymentStatusName { get; set; }

    }
}
