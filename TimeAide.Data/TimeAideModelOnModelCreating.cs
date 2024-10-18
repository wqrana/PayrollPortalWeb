namespace TimeAide.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    //[DbConfigurationType(typeof(CustomDbConfiguration))]
    public partial class TimeAidePayrollContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payroll>().Property(e => e.Compensations)
           .HasColumnType("Decimal")
           .HasPrecision(18, 5);

            modelBuilder.Entity<Payroll>().Property(e => e.Withholdings)
           .HasColumnType("Decimal")
           .HasPrecision(18, 5);

            modelBuilder.Entity<Payroll>().Property(e => e.Contributions)
            .HasColumnType("Decimal")
            .HasPrecision(18, 5);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


    }
}
