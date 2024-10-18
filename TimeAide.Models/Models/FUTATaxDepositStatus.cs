using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("FUTATaxDepositStatus")]
    public partial class FUTATaxDepositStatus : BaseEntity
    {
        public FUTATaxDepositStatus()
        {

        }

        [Column("FUTATaxDepositStatusId")]
        public override int Id { get; set; }
        public string FUTATaxDepositStatusName { get; set; }

    }
}
