﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeAide.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TimeAideWindowContext : DbContext
    {
        public TimeAideWindowContext()
            : base("name=TimeAideWindowContext")
        {
        }
        public TimeAideWindowContext(string con)
               : base(con)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tCompany> tCompanies { get; set; }
        public virtual DbSet<tblBatch> tblBatch { get; set; }
    
        public virtual ObjectResult<spPay_rpt_CuadreQuarterlyReport_Result> spPay_rpt_CuadreQuarterlyReport(string payrollCompany, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate)
        {
            var payrollCompanyParameter = payrollCompany != null ?
                new ObjectParameter("PayrollCompany", payrollCompany) :
                new ObjectParameter("PayrollCompany", typeof(string));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spPay_rpt_CuadreQuarterlyReport_Result>("spPay_rpt_CuadreQuarterlyReport", payrollCompanyParameter, startDateParameter, endDateParameter);
        }
    }
}
