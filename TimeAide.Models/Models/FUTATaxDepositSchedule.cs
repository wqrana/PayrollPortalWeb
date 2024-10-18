using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("FUTATaxDepositSchedule")]
    public partial class FUTATaxDepositSchedule : BaseEntity
    {
        public FUTATaxDepositSchedule()
        {

        }

        [Column("FUTATaxDepositScheduleId")]
        public override int Id { get; set; }
        public string FUTATaxDepositScheduleName { get; set; }

    }
}
