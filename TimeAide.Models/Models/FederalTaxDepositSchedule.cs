using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("FederalTaxDepositSchedule")]
    public partial class FederalTaxDepositSchedule : BaseEntity
    {
        public FederalTaxDepositSchedule()
        {

        }

        [Column("FederalTaxDepositScheduleId")]
        public override int Id { get; set; }
        public string FederalTaxDepositScheduleName { get; set; }

    }
}
