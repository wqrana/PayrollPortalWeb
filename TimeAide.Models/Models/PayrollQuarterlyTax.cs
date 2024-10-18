using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
 public class PayrollQuarterlyTax :BaseWithLoggingEntity
    {
        [Column("PayrollQuarterlyTaxId")]
        public override int Id { get; set; }
        public string CompanyName { get; set; }
        public string Quarter { get; set; }
        public DateTime? QuarterStartDate { get; set; }
        public DateTime? QuarterEndDate { get; set; }

        public decimal? FUTAWHAmount { get; set; }
        public int? FUTATaxDepositScheduleId { get; set; }
        public int? FUTATaxDepositStatusId { get; set; }
        public decimal? FUTATaxDepositAmount { get; set; }
        public DateTime? FUTATaxDepositDate { get; set; }
        public string FUTATaxReceiptNo { get; set; }
        public DateTime? FUTATaxStatusDate { get; set; }
        public string FUTATaxStatusByName { get; set; }
        public byte[] FUTATaxConfirmation { get; set; }
        public string FUTATaxRptName { get; set; }
        public string FUTATaxComments { get; set; }

        public decimal? SUTAWHAmount { get; set; }      
        public int? SUTATaxDepositStatusId { get; set; }
        public decimal? SUTATaxDepositAmount { get; set; }
        public DateTime? SUTATaxDepositDate { get; set; }
        public string SUTATaxReceiptNo { get; set; }
        public DateTime? SUTATaxStatusDate { get; set; }
        public string SUTATaxStatusByName { get; set; }
        public byte[] SUTATaxConfirmation { get; set; }
        public string SUTATaxRptName { get; set; }
        public string SUTATaxComments { get; set; }

        public decimal? SINOTWHAmount { get; set; }      
        public int? SINOTTaxDepositStatusId { get; set; }
        public decimal? SINOTTaxDepositAmount { get; set; }
        public DateTime? SINOTTaxDepositDate { get; set; }
        public string SINOTTaxReceiptNo { get; set; }
        public DateTime? SINOTTaxStatusDate { get; set; }
        public string SINOTTaxStatusByName { get; set; }
        public byte[] SINOTTaxConfirmation { get; set; }
        public string SINOTTaxRptName { get; set; }
        public string SINOTTaxComments { get; set; }

        public decimal? CHOFERILWHAmount { get; set; }       
        public int? CHOFERILTaxDepositStatusId { get; set; }
        public decimal? CHOFERILTaxDepositAmount { get; set; }
        public DateTime? CHOFERILTaxDepositDate { get; set; }
        public string CHOFERILTaxReceiptNo { get; set; }
        public DateTime? CHOFERILTaxStatusDate { get; set; }
        public string CHOFERILTaxStatusByName { get; set; }
        public byte[] CHOFERILTaxConfirmation { get; set; }
        public string CHOFERILTaxRptName { get; set; }
        public string CHOFERILTaxComments { get; set; }

        public string CreatedByName { get; set; }

        public virtual FUTATaxDepositSchedule FUTATaxDepositSchedule { get; set; }
        public virtual FUTATaxDepositStatus FUTATaxDepositStatus { get; set; }

        public virtual SUTATaxDepositStatus SUTATaxDepositStatus { get; set; }
        public virtual SINOTTaxDepositStatus SINOTTaxDepositStatus { get; set; }

        public virtual CHOFERILTaxDepositStatus CHOFERILTaxDepositStatus { get; set; }
    }
}
