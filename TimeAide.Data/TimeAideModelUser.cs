namespace TimeAide.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.SqlServer;
    using System.Data.Entity.Migrations.Model;

    public partial class TimeAidePayrollContext : DbContext
    {
       

    }

    public class CustomSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddForeignKeyOperation addForeignKeyOperation)
        {
            addForeignKeyOperation.Name = getFkName(addForeignKeyOperation.PrincipalTable,
                addForeignKeyOperation.DependentTable, addForeignKeyOperation.DependentColumns.ToArray());
            base.Generate(addForeignKeyOperation);
        }

        protected override void Generate(DropForeignKeyOperation dropForeignKeyOperation)
        {
            dropForeignKeyOperation.Name = getFkName(dropForeignKeyOperation.PrincipalTable,
                dropForeignKeyOperation.DependentTable, dropForeignKeyOperation.DependentColumns.ToArray());
            base.Generate(dropForeignKeyOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            createTableOperation.PrimaryKey.Name = getPkName(createTableOperation.Name);
            base.Generate(createTableOperation);
        }

        protected override void Generate(AddPrimaryKeyOperation addPrimaryKeyOperation)
        {
            addPrimaryKeyOperation.Name = getPkName(addPrimaryKeyOperation.Table);
            base.Generate(addPrimaryKeyOperation);
        }

        protected override void Generate(DropPrimaryKeyOperation dropPrimaryKeyOperation)
        {
            dropPrimaryKeyOperation.Name = getPkName(dropPrimaryKeyOperation.Table);
            base.Generate(dropPrimaryKeyOperation);
        }

        private static string getFkName(string primaryKeyTable, string foreignKeyTable, params string[] foreignTableFields)
        {
            return "FK_" + primaryKeyTable.Replace("dbo.", "") + "_" + foreignKeyTable.Replace(".dbo", "") + "_" + foreignTableFields[0];
        }
        private static string getPkName(string primaryKeyTable)
        {
            return "PK_" + primaryKeyTable.Replace("dbo.", "");
        }
    }

    //public class CustomDbConfiguration : DbConfiguration
    //{
    //    public CustomDbConfiguration()
    //    {
    //        SetMigrationSqlGenerator(SqlProviderServices.ProviderInvariantName,
    //            () => new CustomSqlGenerator());
    //    }
    //}
}
