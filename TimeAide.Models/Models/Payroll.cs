
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("Payroll")]
    public partial class Payroll : BaseWithLoggingEntity
    {
        public Payroll()
        {

        }

        [Column("PayrollId")]
        public override int Id { get; set; }
        public Guid PayrollExternalId { get; set; }
        public string CompanyName { get; set; }
        public string PayrollName { get; set; }
        public DateTime PayDate { get; set; }
        public int PayrollStatusId { get; set; }
        public string BatchTypeName { get; set; }
        public string PayWeekNum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Compensations { get; set; }
        public decimal Withholdings { get; set; }
        public decimal Contributions { get; set; }

        public decimal DirectDepositAmount { get; set; }
        public decimal CheckDepositAmount { get; set; }

        public decimal? SSPayableAmount { get; set; }
        public decimal? MedPayableAmount { get; set; }
        public decimal? MedPlusPayableAmount { get; set; }
        public decimal? HaciendaPayableAmount { get; set; }
        public decimal? FUTAPayableAmount { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentStatusByName { get; set; }
        public DateTime? PaymentStatusDate { get; set; }
        public int? FederalTaxDepositScheduleId { get; set; }
        public int FederalTaxDepositStatusId { get; set; }
        public decimal? FederalTaxDepositAmount { get; set; }
        public DateTime? FederalTaxDepositDate { get; set; }
        public string FederalTaxEFTPSNo { get; set; }
        public DateTime? FederalTaxStatusDate { get; set; }
        public string FederalTaxStatusByName { get; set; }
        public string FederalTaxComments { get; set; }
        public int? HaciendaTaxDepositScheduleId { get; set; }
        public int HaciendaTaxDepositStatusId { get; set; }
        public decimal? HaciendaTaxDepositAmount { get; set; }
        public DateTime? HaciendaTaxDepositDate { get; set; }
        public string HaciendaTaxReceiptNo { get; set; }
        public DateTime? HaciendaTaxStatusDate { get; set; }
        public string HaciendaTaxStatusByName { get; set; }
        public string HaciendaTaxComments { get; set; }
        public int? FUTATaxDepositScheduleId { get; set; }
        public int? FUTATaxDepositStatusId { get; set; }
        public decimal? FUTATaxDepositAmount { get; set; }
        public DateTime? FUTATaxDepositDate { get; set; }
        public string FUTATaxReceiptNo { get; set; }
        public DateTime? FUTATaxStatusDate { get; set; }
        public string FUTATaxStatusByName { get; set; }
        public string FUTATaxComments { get; set; }
        public string PayrollTypeName { get; set; }
        public string TemplateTypeName { get; set; }
        public byte[] CompanyPayrollSummary { get; set; }
        public string PayrollSummaryRptName { get; set; }
        public byte[] PaymentConfirmation { get; set; }
        public string PaymentConfirmationRptName { get; set; }
        public byte[] FederalTaxConfirmation { get; set; }
        public string FederalTaxRptName { get; set; }
        public byte[] HaciendaTaxConfirmation { get; set; }
        public string HaciendaTaxRptName { get; set; }
        public byte[] FUTATaxConfirmation { get; set; }
        public string FUTATaxRptName { get; set; }
        public string CreatedByName { get; set; }
        public int? ClosedBy { get; set; }
        public string ClosedByName { get; set; }
        public DateTime? ClosedDate { get; set; }

        public virtual PayrollStatus PayrollStatus { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual FederalTaxDepositSchedule FederalTaxDepositSchedule { get; set; }
        public virtual FederalTaxDepositStatus FederalTaxDepositStatus { get; set; }
        public virtual HaciendaTaxDepositSchedule HaciendaTaxDepositSchedule { get; set; }
        public virtual HaciendaTaxDepositStatus HaciendaTaxDepositStatus { get; set; }

        public virtual FUTATaxDepositSchedule FUTATaxDepositSchedule { get; set; }
        public virtual FUTATaxDepositStatus FUTATaxDepositStatus { get; set; }
            

    }
}
