using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;


namespace TimeAide.Models.ViewModel
{
   public class PayrollFilterViewModel
    {
        public int Id { get; set; }
        public DateTime? FromPayDate { get; set; }
        public DateTime? ToPayDate { get; set; }
        public int CompanyId { get; set; }
        public string Quarter { get; set; }
        public int PayrollStatusId { get; set; }
        public int PaymentStatusId { get; set; }
        public int FederalTaxDepositStatusId { get; set; }
        public int FederalTaxDepositScheduleId { get; set; }
        public int HaciendaTaxDepositStatusId { get; set; }
        public int HaciendaTaxDepositScheduleId { get; set; }
        public int FUTATaxDepositStatusId { get; set; }
        public int FUTATaxDepositScheduleId { get; set; }
        public int SUTATaxDepositStatusId { get; set; }
        public int SINOTTaxDepositStatusId { get; set; }
        public int CHOFERILTaxDepositStatusId { get; set; }
    }
}
