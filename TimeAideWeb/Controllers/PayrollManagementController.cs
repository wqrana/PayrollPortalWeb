using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{    

    public class PayrollManagementController : TimeAidePayrollControllers<Payroll>
    {
        private IDictionary<int, string> StatusTypeList = new Dictionary<int, string>() { { 0, "Payment Status" }, { 1, "Payroll Status" } };
        private IDictionary<int, string> ReportTypeList = new Dictionary<int, string>() { { 0, "Payment Report" }, { 1, "Summary Report" } };
        // GET: Payroll
        public override ActionResult Index()
        {
            ViewBag.PayrollFromDD=int.Parse(ConfigurationManager.AppSettings["PayrollFromDD"]);
            ViewBag.PayrollToDD = int.Parse(ConfigurationManager.AppSettings["PayrollToDD"]);
            // ViewBag.CompanyId = new SelectList(payrollDBConetext.GetAll<Company>().OrderBy(o=>o.CompanyName), "Id", "CompanyName");
            ViewBag.CompanyId = new SelectList(DataHelper.GetUserCompanies(), "Id", "CompanyName");
            ViewBag.PayrollStatusId = new SelectList(payrollDBConetext.GetAll<PayrollStatus>(), "Id", "PayrollStatusName");
            ViewBag.PaymentStatusId = new SelectList(payrollDBConetext.GetAll<PaymentStatus>(), "Id", "PaymentStatusName");
            return PartialView();
        }
        public ActionResult IndexByFilter(PayrollFilterViewModel model)
        {
            //  var payrollList = payrollDBConetext.Payroll.ToList();
            var payrollList = getPayrollList(model);

            return PartialView(payrollList);
        }

        public ActionResult AjaxEditStatus(int payrollId, int statusTypeId)
        {
            var model = payrollDBConetext.Payroll.Find(payrollId);
            if (model != null)
            {
                ViewBag.StatusTypeName = StatusTypeList[statusTypeId];
                ViewBag.StatusTypeId = statusTypeId;
                ViewBag.PaymentStatusId = new SelectList(payrollDBConetext.GetAll<PaymentStatus>(), "Id", "PaymentStatusName", model.PaymentStatusId);
                ViewBag.PayrollStatusId = new SelectList(payrollDBConetext.GetAll<PayrollStatus>(), "Id", "PayrollStatusName", model.PayrollStatusId);
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult SaveEditStatus(PayrollUpdateViewModel model)
        {
            string status = "Success";
            string message = "Successfully Updated!";
            string newReturnVal = "";
            int newRetStatusId = 0;
            var payrollEntity = payrollDBConetext.Payroll.Find(model.PayrollId);
            try
            {
                var tAWinDbContext = DataHelper.GetSelectedCompTAWinEFContext(payrollEntity.CompanyName);                
                tblBatch batchEntity = null;
                if (tAWinDbContext!=null)
                {
                    batchEntity = tAWinDbContext.tblBatch.Find(payrollEntity.PayrollExternalId);
                }

                payrollEntity.PaymentStatusId = model.PaymentStatusId.Value;
              //  payrollEntity.PayrollStatusId = model.PayrollStatusId.Value;
                payrollEntity.ModifiedBy = SessionHelper.LoginId;
                payrollEntity.ModifiedDate = DateTime.Now;
                if (model.StatusTypeId == 0)
                {
                    payrollEntity.PaymentStatusByName = SessionHelper.UserName;
                    payrollEntity.PaymentStatusDate = DateTime.Now;

                    if (batchEntity != null)
                    {
                        batchEntity.intPaymentStatusId = model.PaymentStatusId.Value;
                        batchEntity.strPaymentStatusByName = SessionHelper.UserName;
                        batchEntity.dtPaymentStatusDate = DateTime.Now;
                        tAWinDbContext.SaveChanges();
                    }
                }
                
                payrollDBConetext.SaveChanges();
                switch (model.StatusTypeId)
                {
                    case 0:
                        newReturnVal = payrollEntity.PaymentStatus.PaymentStatusName;
                        newRetStatusId = payrollEntity.PaymentStatusId;
                        break;
                    case 1:
                        newReturnVal = payrollEntity.PayrollStatus.PayrollStatusName;
                        break;
                }
                                              
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, retVal= newReturnVal, retStatusId= newRetStatusId });
        }
        public ActionResult AjaxUploadReportFile(int payrollId, int reportTypeId)
        {
            var model = payrollDBConetext.Payroll.Find(payrollId);
            ViewBag.ReportTypeName = ReportTypeList[reportTypeId];
            ViewBag.ReportTypeId = reportTypeId;

            return PartialView("UploadReport", model);
        }
      
        [HttpPost]
        public JsonResult UploadReportFile()
        {
            string status = "Success";
            string message = "Report file is Successfully uploaded!";

            int payrollId = int.Parse(Request.Form["PayrollId"]);
            int reportTypeId = int.Parse(Request.Form["ReportTypeId"]);

            try
            {
                var payrollEntity = payrollDBConetext.Payroll.Find(payrollId);
                var tAWinDbContext = DataHelper.GetSelectedCompTAWinEFContext(payrollEntity.CompanyName);
                tblBatch batchEntity = null;
                if (tAWinDbContext != null)
                {
                    batchEntity = tAWinDbContext.tblBatch.Find(payrollEntity.PayrollExternalId);
                }
                if (payrollEntity != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase rptFile = Request.Files[0];
                        byte[] fileData = null;
                        string fileName = Path.GetFileName(rptFile.FileName);
                        using (var binaryReader = new BinaryReader(rptFile.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(rptFile.ContentLength);
                        }

                        if (reportTypeId == 0) //payment report
                        {
                            payrollEntity.PaymentConfirmation = fileData;
                            payrollEntity.PaymentConfirmationRptName = fileName;
                            if (batchEntity != null)
                            {
                                batchEntity.varPaymentConfirmation = fileData;
                                batchEntity.strPaymentConfirmationRptName = fileName;
                                tAWinDbContext.SaveChanges();
                            }
                        }
                        else if (reportTypeId == 1)// summary report
                        {
                            payrollEntity.CompanyPayrollSummary = fileData;
                            payrollEntity.PayrollSummaryRptName = fileName;
                            if (batchEntity != null)
                            {
                                batchEntity.varCompanyPayrollSummary = fileData;
                                batchEntity.strPayrollSummaryRptName = fileName;
                                tAWinDbContext.SaveChanges();
                            }
                        }
                        payrollEntity.ModifiedDate = DateTime.Now;
                        payrollEntity.ModifiedBy = SessionHelper.LoginId;
                    }
                    else
                    {
                        throw new Exception("No report file found.");
                    }
                }
                else
                {
                    throw new Exception("No Payroll record found.");
                }
                payrollDBConetext.SaveChanges();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                //retResult = new { status = "Error", message = ex.Message };
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        public ActionResult DownloadReportFile(int payrollId, int reportTypeId)
        {

            byte[] fileBytes;
            var payrollEntity = payrollDBConetext.Payroll.Find(payrollId);
            if (payrollEntity != null)
            {
                fileBytes = reportTypeId == 0 ? payrollEntity.PaymentConfirmation : payrollEntity.CompanyPayrollSummary;
                var fileName = reportTypeId == 0 ? "PaymentRpt_" + payrollEntity.PaymentConfirmationRptName : "SummaryRpt_" + payrollEntity.PayrollSummaryRptName;

                if (fileBytes != null)
                {

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                }

            }

            return null;

        }
        private List<Payroll> getPayrollList(PayrollFilterViewModel model)
        {
            var companyName = "";
            // var retList = payrollDBConetext.PayrollInformation<Payroll>(model.CompanyId, model.PayDate, model.PayrollStatusId, model.PaymentStatusId);
           var companyInfo = payrollDBConetext.Company.Find(model.CompanyId);
            if (companyInfo != null) companyName = companyInfo.CompanyName;
            var userCompaniesList = DataHelper.GetUserCompanies().Select(s=>s.CompanyName).ToArray();
            var retList = payrollDBConetext.Payroll
                            .Where(w => w.DataEntryStatus == 1)
                            .Where(w => (companyName == "" ? (userCompaniesList.Count()==0?true: userCompaniesList.Contains(w.CompanyName)) : (w.CompanyName == companyName)))
                            .Where(w => (w.PayDate >= model.FromPayDate) && (w.PayDate <= model.ToPayDate))
                            .Where(w => model.PayrollStatusId == 0 ? true : (w.PayrollStatusId == model.PayrollStatusId))
                            .Where(w => model.PaymentStatusId == 0 ? true : (w.PaymentStatusId == model.PaymentStatusId))
                           
                            .ToList();
                           

            return retList;
        }
    }
}