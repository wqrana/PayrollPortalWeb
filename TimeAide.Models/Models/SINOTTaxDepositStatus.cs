using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("SINOTTaxDepositStatus")]
    public partial class SINOTTaxDepositStatus : BaseEntity
    {
        public SINOTTaxDepositStatus()
        {

        }

        [Column("SINOTTaxDepositStatusId")]
        public override int Id { get; set; }
        public string SINOTTaxDepositStatusName { get; set; }

    }
}
