using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeAide.Web.ViewModel
{
    public class ReportViewModel
    {
        public int? ReportId { get; set; }       
        public string ReportName { get; set; }              
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }       
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        
    }
   
}