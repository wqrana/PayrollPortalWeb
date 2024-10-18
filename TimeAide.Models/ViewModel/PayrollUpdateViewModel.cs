using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;


namespace TimeAide.Models.ViewModel
{
    public class PayrollUpdateViewModel
    {
        public int PayrollId { get; set; }   
        public int StatusTypeId { get; set; }
        public int? PayrollStatusId { get; set; }
        public int? PaymentStatusId { get; set; }
        public int? FederalTaxDepositStatusId { get; set; }      
        public int? HaciendaTaxDepositStatusId { get; set; }       
        public int? FUTATaxDepositStatusId { get; set; }
        public int? SUTATaxDepositStatusId { get; set; }
        public int? SINOTTaxDepositStatusId { get; set; }
        public int? CHOFERILTaxDepositStatusId { get; set; }
        public decimal? TaxDepositAmount { get; set; }
        public string TaxReferenceNo { get; set; }
        public DateTime? TaxDepositDate { get; set; }
        public string Comments { get; set; }
    }

    public class QuarterlyTaxViewModel
    {
        public int Id { get; set; }
        public int StatusTypeId { get; set; }       
        public int? FUTATaxDepositStatusId { get; set; }
        public int? SUTATaxDepositStatusId { get; set; }
        public int? SINOTTaxDepositStatusId { get; set; }
        public int? CHOFERILTaxDepositStatusId { get; set; }
        public decimal? TaxDepositAmount { get; set; }
        public string TaxReferenceNo { get; set; }
        public DateTime? TaxDepositDate { get; set; }
        public string Comments { get; set; }
    }
}
