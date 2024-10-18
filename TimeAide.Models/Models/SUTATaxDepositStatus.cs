using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("SUTATaxDepositStatus")]
    public partial class SUTATaxDepositStatus : BaseEntity
    {
        public SUTATaxDepositStatus()
        {

        }

        [Column("SUTATaxDepositStatusId")]
        public override int Id { get; set; }
        public string SUTATaxDepositStatusName { get; set; }

    }
}
