using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("HaciendaTaxDepositSchedule")]
    public partial class HaciendaTaxDepositSchedule : BaseEntity
    {
        public HaciendaTaxDepositSchedule()
        {

        }

        [Column("HaciendaTaxDepositScheduleId")]
        public override int Id { get; set; }
        public string HaciendaTaxDepositScheduleName { get; set; }

    }
}
