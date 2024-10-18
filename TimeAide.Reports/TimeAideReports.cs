using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Reports
{
   
    //Reports
    public enum TimeAide_REPORTS
    {
        REP_PAYROLL_QRTCURADRE = 0     /*int value = 0*/       
           
    }
    //Report's Filter
    public enum TimeAide_REPORT_FILTERS
    {        
        RF_NONE = 0,
        RF_ALL        

    }
    //Static Class for reports detail in application
    public static class TimeAideReports
    {
        public const string TimeAide_REPORT_PATH = "/Report Project1/";
        public static string GetReportPath(TimeAide_REPORTS rpt)
        {
            return TimeAide_REPORT_PATH + "" + ReportFileNames[rpt];
        }            
       
        #region Report Names
        /// <summary>
        /// Report Names
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORTS, string> ReportNames = new ReadOnlyDictionary<TimeAide_REPORTS, string>(new Dictionary<TimeAide_REPORTS, string>
        {
            {TimeAide_REPORTS.REP_PAYROLL_QRTCURADRE, " Quarter Cuadre Report"}            
             
        });
        #endregion
        #region Report File Names
        /// <summary>
        /// List of the report file that goes with the tagged report.
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORTS, string> ReportFileNames = new ReadOnlyDictionary<TimeAide_REPORTS, string>(new Dictionary<TimeAide_REPORTS, string>
        {           
            {TimeAide_REPORTS.REP_PAYROLL_QRTCURADRE, "CuadreQuarterlyRpt.rdlc"}           

        });
        #endregion
        #region Report Filter Names
        public static readonly IDictionary<TimeAide_REPORT_FILTERS, string> ReportFilterNames = new ReadOnlyDictionary<TimeAide_REPORT_FILTERS, string>(new Dictionary<TimeAide_REPORT_FILTERS, string>
        {
            
            { TimeAide_REPORT_FILTERS.RF_NONE, "No Filters for this Report."},
            { TimeAide_REPORT_FILTERS.RF_ALL, "All Filters for this Report."},
            
            });
        #endregion
        #region Report DataTable
        /// <summary>
        /// Report DataTable list
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORTS, string> ReportDataTableNames = new ReadOnlyDictionary<TimeAide_REPORTS, string>(new Dictionary<TimeAide_REPORTS, string>
        {
            {TimeAide_REPORTS.REP_PAYROLL_QRTCURADRE, "CuadreQuarterlyReportDS"}
           
        });
        #endregion

        #region Report Menu
        /// <summary>
        /// Report Menu Layout
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORTS, bool> ReportMenu = new ReadOnlyDictionary<TimeAide_REPORTS, bool>(new Dictionary<TimeAide_REPORTS, bool>
                {
                    { TimeAide_REPORTS.REP_PAYROLL_QRTCURADRE, true}
                           
                });
           
        #endregion
        #region Get Report Menu Tree
        /// <summary>
        /// Report Menu Tree
        /// </summary>
        public static IList<TimeAideReport> GetReportDDL()
        {
            IList<TimeAideReport> reptDDList = new List<TimeAideReport>();
            foreach (var rpm in ReportMenu)
            {                
                    TimeAide_REPORTS rept = rpm.Key;
                reptDDList.Add(new TimeAideReport(rept));               
            }           

            return reptDDList;
        }
        #endregion
        
     
        #region Report Filter Assignments
        public static readonly IDictionary<TimeAide_REPORTS, List<TimeAide_REPORT_FILTERS>> ReportFilterAssignments = new ReadOnlyDictionary<TimeAide_REPORTS, List<TimeAide_REPORT_FILTERS>>(new Dictionary<TimeAide_REPORTS, List<TimeAide_REPORT_FILTERS>>
        {
            {TimeAide_REPORTS.REP_PAYROLL_QRTCURADRE, (new List<TimeAide_REPORT_FILTERS>
            {
                TimeAide_REPORT_FILTERS.RF_NONE
             })
        }
        });
        #endregion
    }    

    //Report Class for get report detail
    public class TimeAideReport
    {
       public TimeAide_REPORTS RptId { get; set; }       
        public string RptName { get; set; }       
        public string RptFileName { get; set; }
        public string RptFilePath { get; set; }
        public int RptIntId {
            get { return (int)RptId; }
        }
        public TimeAideReport(TimeAide_REPORTS rptId)
        {
            RptId = rptId;          
            RptName = TimeAideReports.ReportNames[rptId];           
            RptFileName = TimeAideReports.ReportFileNames[rptId];
            RptFilePath = TimeAideReports.GetReportPath(rptId);
        }
        
    }   
 }
