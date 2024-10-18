using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("HaciendaTaxDepositStatus")]
    public partial class HaciendaTaxDepositStatus : BaseEntity
    {
        public HaciendaTaxDepositStatus()
        {

        }
        [Column("HaciendaTaxDepositStatusId")]
        public override int Id { get; set; }
        public string HaciendaTaxDepositStatusName { get; set; }

    }
}
