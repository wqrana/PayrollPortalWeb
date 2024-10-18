using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;
using TimeAide.Reports;
using TimeAide.Data;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class ReportsController : BaseAuthorizedController
    {       
        private TimeAidePayrollContext payrollDBConetext = new TimeAidePayrollContext();
        // GET: Reports
        public ActionResult Index(int? id)
        {
           
            ViewBag.PayrollFromDD = int.Parse(ConfigurationManager.AppSettings["PayrollFromDD"]);
            ViewBag.PayrollToDD = int.Parse(ConfigurationManager.AppSettings["PayrollToDD"]);           
            ViewBag.CompanyId = new SelectList(DataHelper.GetUserCompanies(), "Id", "CompanyName");
            ViewBag.ReportId = new SelectList(TimeAideReports.GetReportDDL(), "RptIntId", "RptName");
            return PartialView();
        }
       
         public PartialViewResult LocalReportView(ReportViewModel reportViewModel)
         {
            // Local Microsoft RDLC report
             var rptId = (TimeAide_REPORTS)reportViewModel.ReportId;
             var rptFileName = TimeAideReports.ReportFileNames[rptId];
             var rptDataTableName = TimeAideReports.ReportDataTableNames[rptId];
             var reportpath = "Reports" + "/" + rptFileName;
             ReportViewer rv = new ReportViewer();
            IList<ReportParameter> reportParams = new List<ReportParameter>(); 
             DataTable reportDataTable = null;
             rv.ProcessingMode = ProcessingMode.Local;
             rv.SizeToReportContent = true;
             rv.AsyncRendering = false;
             ReportDataHelper reptDataHelper = new ReportDataHelper(reportViewModel.CompanyName);
             if (rptId == TimeAide_REPORTS.REP_PAYROLL_QRTCURADRE)
             {
                ReportParameter reptParam = new ReportParameter("CompanyName", reportViewModel.CompanyName);
                reportParams.Add(reptParam);               
                reportDataTable = ToDataTable(reptDataHelper.getCuadreQuarterlyRptData(reportViewModel), rptDataTableName);
                
              }         
           
             rv.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportpath;            
             rv.LocalReport.DataSources.Add(new ReportDataSource(rptDataTableName, reportDataTable));
            rv.LocalReport.SetParameters(reportParams);
            ViewBag.ReportView = rv;

             return PartialView("LocalReportView");
         }
        private static DataTable ToDataTable<T>(List<T> l_oItems, string dataTableName)
        {
            try
            {
                DataTable oReturn = new DataTable(dataTableName);
                object[] a_oValues;
                int i;
                PropertyInfo[] a_oProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo oProperty in a_oProperties)
                {
                    oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType));
                }

                foreach (T oItem in l_oItems)
                {
                    a_oValues = new object[a_oProperties.Length];

                    for (i = 0; i < a_oProperties.Length; i++)
                    {
                        a_oValues[i] = a_oProperties[i].GetValue(oItem, null);

                    }

                    oReturn.Rows.Add(a_oValues);
                }

                return oReturn;
            }
            catch (Exception ex)
            {
                //ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ToDataTable");
                throw ex;
            }
        }
        private static Type BaseType(Type oType)
        {
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            else
            {
                return oType;
            }
        }
    }

}


