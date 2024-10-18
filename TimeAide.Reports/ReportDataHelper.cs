using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Data;
using TimeAide.Web.ViewModel;

namespace TimeAide.Reports
{
   public class ReportDataHelper
    {
      
        private TimeAideWindowContext tAWindowContext = null;
        
        public ReportDataHelper(string companyName)
        {
            tAWindowContext = DataHelper.GetSelectedCompTAWinEFContext(companyName);
        }
        public List<spPay_rpt_CuadreQuarterlyReport_Result> getCuadreQuarterlyRptData(ReportViewModel model)
        {
            List<spPay_rpt_CuadreQuarterlyReport_Result> retRptData = null;
            try
            {
                tAWindowContext.Database.CommandTimeout=180;

                retRptData = tAWindowContext.spPay_rpt_CuadreQuarterlyReport(model.CompanyName,model.FromDate,model.ToDate)
                                            .OrderBy(o=>o.FECHA_NOMINA)
                                            .ToList<spPay_rpt_CuadreQuarterlyReport_Result>();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                if (tAWindowContext != null)
                    tAWindowContext.Dispose();

            }
            return retRptData;
        }

    }
}
