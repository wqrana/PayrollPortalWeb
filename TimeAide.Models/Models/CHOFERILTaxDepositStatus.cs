using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("CHOFERILTaxDepositStatus")]
    public partial class CHOFERILTaxDepositStatus : BaseEntity
    {
        public CHOFERILTaxDepositStatus()
        {

        }

        [Column("CHOFERILTaxDepositStatusId")]
        public override int Id { get; set; }
        public string CHOFERILTaxDepositStatusName { get; set; }

    }
}
