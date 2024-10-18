namespace TimeAide.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Collections.Generic;
    using TimeAide.Common.Helpers;
    using System.Data.SqlClient;
    using TimeAide.Models.ViewModel;


    public partial class TimeAidePayrollContext : DbContext
    {
        public TimeAidePayrollContext() : base("name=TimeAidePayrollContext")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<TimeAideContext, TimeAide.Data.Migrations.Configuration>());
            this.Database.CommandTimeout = 60;
        }
        public TimeAidePayrollContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<TimeAideContext, TimeAide.Data.Migrations.Configuration>());
            this.Database.CommandTimeout = 60;
        }

          public virtual DbSet<UserInformation> UserInformation { get; set; }
          public virtual DbSet<Company> Company { get; set; }

      
        public virtual DbSet<PaymentStatus> PaymentStatus { get; set; }

        public virtual DbSet<Payroll> Payroll { get; set; }
        public virtual DbSet<PayrollQuarterlyTax> PayrollQuarterlyTax { get; set; }
        public virtual DbSet<PayrollStatus> PayrollStatus { get; set; }

        public virtual DbSet<FederalTaxDepositStatus> FederalTaxDepositStatus { get; set; }
        public virtual DbSet<FederalTaxDepositSchedule> FederalTaxDepositSchedule { get; set; }

        public virtual DbSet<HaciendaTaxDepositStatus> HaciendaTaxDepositStatus { get; set; }
        public virtual DbSet<HaciendaTaxDepositSchedule> HaciendaTaxDepositSchedule { get; set; }

        public virtual DbSet<FUTATaxDepositStatus> FUTATaxDepositStatus { get; set; }
        public virtual DbSet<FUTATaxDepositSchedule> FUTATaxDepositSchedule { get; set; }
        public virtual DbSet<SINOTTaxDepositStatus> SINOTTaxDepositStatus { get; set; }
        public virtual DbSet<SUTATaxDepositStatus> SUTATaxDepositStatus { get; set; }
        public virtual DbSet<CHOFERILTaxDepositStatus> CHOFERILTaxDepositStatus { get; set; }
        public virtual DbSet<UserCompany> UserCompany { get; set; }

        public List<T> PayrollInformation<T>(int companyId, DateTime? payDate, int payrollStatusId, int paymentStatusId) where T : Payroll
        {
            var companyIdParameter = new SqlParameter("@companyId", companyId);
            var payDateParameter = new SqlParameter("@payDate", payDate);
            var payrollStatusIdParameter = new SqlParameter("@payrollStatusId", payrollStatusId);           
            var paymentStatusIdParameter = new SqlParameter("@paymentStatusId", paymentStatusId);
         
            try
            {
                var payrollInformationList = this.Database
                   .SqlQuery<T>("sp_PayrollInformation @companyId, @payDate,@payrollStatusId ,@paymentStatusId ",
                                                    companyIdParameter, payDateParameter, payrollStatusIdParameter, paymentStatusIdParameter
                                                    )
                   .ToList<T>();
                return payrollInformationList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<T> GetAll<T>() where T : BaseEntity
        {
            
            return Set<T>().Where(t => t.DataEntryStatus == 1).ToList();
        }
        public T Find<T>(int id) where T : BaseEntity
        {
            return Set<T>().FirstOrDefault(entity => entity.Id == id);
        }
        public void Add<T>(T newItem) where T : BaseEntity
        {
            Set<T>().Add(newItem);
        }
    }
}
