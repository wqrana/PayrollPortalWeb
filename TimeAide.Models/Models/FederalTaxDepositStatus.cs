using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("FederalTaxDepositStatus")]
    public partial class FederalTaxDepositStatus : BaseEntity
    {
        public FederalTaxDepositStatus()
        {

        }

        [Column("FederalTaxDepositStatusId")]
        public override int Id { get; set; }
        public string FederalTaxDepositStatusName { get; set; }
      
    }
}
