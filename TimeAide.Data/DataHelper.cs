using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using TimeAide.Common.Helpers;
using System.Data;
using TimeAide.Web.Models;

namespace TimeAide.Data
{
    public class DataHelper
    {

        private static string ConvertToEFConnectionString(string conStr)
        {
            // Specify the provider name, server and database.
            var providerName = "System.Data.SqlClient";

            //var providerName = "System.Data.EntityClient";

            // Initialize the EntityConnectionStringBuilder.
            var entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = conStr;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/TimeAideWindow.csdl|res://*/TimeAideWindow.ssdl|res://*/TimeAideWindow.msl";

            return entityBuilder.ToString();
        }

        public static TimeAideWindowContext GetSelectedCompTAWinEFContext(string companyName)
        {

            TimeAideWindowContext dbContext = null;
            var payrollDbContext = new TimeAidePayrollContext();
            var companyInfo = payrollDbContext.Company.Where(w => w.CompanyName == companyName).FirstOrDefault();
            if (companyInfo != null)
            {

                if (!String.IsNullOrEmpty(companyInfo.DBServerName) &&
                      !String.IsNullOrEmpty(companyInfo.DBName) &&
                      !String.IsNullOrEmpty(companyInfo.DBUser) &&
                      !String.IsNullOrEmpty(companyInfo.DBPassword))
                {
                    var connStrDb = string.Format("data source={0};initial catalog={1};User ID={2};Password={3};MultipleActiveResultSets=True;App=EntityFramework", companyInfo.DBServerName, companyInfo.DBName, companyInfo.DBUser, companyInfo.DBPassword);
                    var efConnStr = ConvertToEFConnectionString(connStrDb);
                    dbContext = new TimeAideWindowContext(efConnStr);

                }
            }
            return dbContext;
        }
        public static IEnumerable<Company> GetUserCompanies()
        {           
            var payrollDbContext = new TimeAidePayrollContext();
            var loginUserId = SessionHelper.LoginId;
            var userCompanyList = payrollDbContext.UserCompany.Where(w => w.UserInformationId == loginUserId
                                                        && w.DataEntryStatus == 1)
                                                        .Select(s => s.Company.CompanyName).ToArray();

            var userCmpReturnList = payrollDbContext.GetAll<Company>().OrderBy(o => o.CompanyName)
                                          .Where(w => userCompanyList.Count() == 0 ? true : userCompanyList.Contains(w.CompanyName));
            return userCmpReturnList;
        }

    }
}

//  
//  