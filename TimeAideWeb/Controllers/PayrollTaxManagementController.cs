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
    public class PayrollTaxManagementController : TimeAidePayrollControllers<Payroll>
    {
        // GET: PayrollTaxManagement
        private IDictionary<int, string> StatusTypeList = new Dictionary<int, string>() { { 0, "Fed Tax Status" }, { 1, "HACIENDA Tax Status" }, { 2, "FUTA Tax Status" } };
        private IDictionary<int, string> ReportTypeList = new Dictionary<int, string>() { { 0, "Summary Report" }, {1, "Fed Rpt" }, { 2, "HACIENDA Rpt" }, { 3, "FUTA Rpt" } };
        public override ActionResult Index()
        {
            ViewBag.PayrollTaxesFromDD = int.Parse(ConfigurationManager.AppSettings["PayrollTaxesFromDD"]);
            ViewBag.PayrollTaxesToDD = int.Parse(ConfigurationManager.AppSettings["PayrollTaxesToDD"]);
            ViewBag.CompanyId = new SelectList(DataHelper.GetUserCompanies(), "Id", "CompanyName");
            //ViewBag.CompanyId = new SelectList(payrollDBConetext.GetAll<Company>().OrderBy(o => o.CompanyName), "Id", "CompanyName");

            // ViewBag.PayrollStatusId = new SelectList(payrollDBConetext.GetAll<PayrollStatus>(), "Id", "PayrollStatusName");
            // ViewBag.PaymentStatusId = new SelectList(payrollDBConetext.GetAll<PaymentStatus>(), "Id", "PaymentStatusName");

            ViewBag.FederalTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<FederalTaxDepositStatus>(), "Id", "FederalTaxDepositStatusName");
            ViewBag.FederalTaxDepositScheduleId = new SelectList(payrollDBConetext.GetAll<FederalTaxDepositSchedule>(), "Id", "FederalTaxDepositScheduleName");

            ViewBag.HaciendaTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<HaciendaTaxDepositStatus>(), "Id", "HaciendaTaxDepositStatusName");
            ViewBag.HaciendaTaxDepositScheduleId = new SelectList(payrollDBConetext.GetAll<HaciendaTaxDepositSchedule>(), "Id", "HaciendaTaxDepositScheduleName");

            ViewBag.FUTATaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<FUTATaxDepositStatus>(), "Id", "FUTATaxDepositStatusName");
            ViewBag.FUTATaxDepositScheduleId = new SelectList(payrollDBConetext.GetAll<FUTATaxDepositSchedule>(), "Id", "FUTATaxDepositScheduleName");

            //ViewBag.SUTATaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<SUTATaxDepositStatus>(), "Id", "SUTATaxDepositStatusName");
            //ViewBag.SINOTTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<SINOTTaxDepositStatus>(), "Id", "SINOTTaxDepositStatusName");
            //ViewBag.CHOFERILTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<CHOFERILTaxDepositStatus>(), "Id", "CHOFERILTaxDepositStatusName");
            return PartialView();
        }
        public ActionResult IndexByFilter(PayrollFilterViewModel model)
        {
            //  var payrollList = payrollDBConetext.Payroll.ToList();
            var payrollTaxList = getPayrollTaxList(model);

            return PartialView(payrollTaxList);
        }
        private List<Payroll> getPayrollTaxList(PayrollFilterViewModel model)
        {
            var companyName = "";
            // var retList = payrollDBConetext.PayrollInformation<Payroll>(model.CompanyId, model.PayDate, model.PayrollStatusId, model.PaymentStatusId);
            var companyInfo = payrollDBConetext.Company.Find(model.CompanyId);
            if (companyInfo != null) companyName = companyInfo.CompanyName;
            var userCompaniesList = DataHelper.GetUserCompanies().Select(s => s.CompanyName).ToArray();
            var retList = payrollDBConetext.Payroll
                            .Where(w => w.DataEntryStatus == 1)
                            .Where(w => (companyName == "" ? (userCompaniesList.Count() == 0 ? true : userCompaniesList.Contains(w.CompanyName)) : (w.CompanyName == companyName)))
                            .Where(w => (w.PayDate >= model.FromPayDate) && (w.PayDate <= model.ToPayDate))
                            .Where(w => model.FederalTaxDepositScheduleId == 0 ? true : (w.FederalTaxDepositScheduleId == model.FederalTaxDepositScheduleId))
                            .Where(w => model.FederalTaxDepositStatusId == 0 ? true : (w.FederalTaxDepositStatusId == model.FederalTaxDepositStatusId))
                            .Where(w => model.HaciendaTaxDepositScheduleId == 0 ? true : (w.HaciendaTaxDepositScheduleId == model.HaciendaTaxDepositScheduleId))
                            .Where(w => model.HaciendaTaxDepositStatusId == 0 ? true : (w.HaciendaTaxDepositStatusId == model.HaciendaTaxDepositStatusId))
                            .Where(w => model.FUTATaxDepositScheduleId == 0 ? true : (w.FUTATaxDepositScheduleId == model.FUTATaxDepositScheduleId))
                            .Where(w => model.FUTATaxDepositStatusId == 0 ? true : (w.FUTATaxDepositStatusId == model.FUTATaxDepositStatusId))
                            .ToList();

            return retList;
        }
        public ActionResult AjaxEditTaxStatus(int payrollId, int statusTypeId)
        {
            var model = payrollDBConetext.Payroll.Find(payrollId);
            if (model != null)
            {
                ViewBag.StatusTypeName = StatusTypeList[statusTypeId];
                ViewBag.StatusTypeId = statusTypeId;
                ViewBag.FederalTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<FederalTaxDepositStatus>(), "Id", "FederalTaxDepositStatusName", model.FederalTaxDepositStatusId);
                ViewBag.HaciendaTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<HaciendaTaxDepositStatus>(), "Id", "HaciendaTaxDepositStatusName", model.HaciendaTaxDepositStatusId);
                ViewBag.FUTATaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<FUTATaxDepositStatus>(), "Id", "FUTATaxDepositStatusName", model.FUTATaxDepositStatusId);
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult SaveEditTaxStatus(PayrollUpdateViewModel model)
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
                if (tAWinDbContext != null)
                {
                    batchEntity = tAWinDbContext.tblBatch.Find(payrollEntity.PayrollExternalId);
                }

                payrollEntity.ModifiedBy = SessionHelper.LoginId;
                payrollEntity.ModifiedDate = DateTime.Now;
                if (model.StatusTypeId == 0) //Fed tax
                {
                    payrollEntity.FederalTaxDepositStatusId = model.FederalTaxDepositStatusId.Value;
                    payrollEntity.FederalTaxEFTPSNo = model.TaxReferenceNo;
                    payrollEntity.FederalTaxDepositAmount = model.TaxDepositAmount;
                    payrollEntity.FederalTaxDepositDate = model.TaxDepositDate;
                    payrollEntity.FederalTaxComments = model.Comments;
                    payrollEntity.FederalTaxStatusDate = DateTime.Now;
                    payrollEntity.FederalTaxStatusByName = SessionHelper.UserName;

                    if (batchEntity != null)
                    {
                        batchEntity.intFederalTaxDepositStatusId = model.FederalTaxDepositStatusId.Value;
                        batchEntity.strFederalTaxEFTPSNo = model.TaxReferenceNo;
                        batchEntity.decFederalTaxDepositAmount = model.TaxDepositAmount;
                        batchEntity.dtFederalTaxDepositDate = model.TaxDepositDate;
                        batchEntity.dtFederalTaxStatusDate = DateTime.Now;
                        batchEntity.strFederalTaxStatusByName = SessionHelper.UserName;

                        tAWinDbContext.SaveChanges();
                    }
                }
                else if (model.StatusTypeId == 1)//Hacienda tax
                {
                    payrollEntity.HaciendaTaxDepositStatusId = model.HaciendaTaxDepositStatusId.Value;
                    payrollEntity.HaciendaTaxReceiptNo = model.TaxReferenceNo;
                    payrollEntity.HaciendaTaxDepositAmount = model.TaxDepositAmount;
                    payrollEntity.HaciendaTaxDepositDate = model.TaxDepositDate;
                    payrollEntity.HaciendaTaxComments = model.Comments;
                    payrollEntity.HaciendaTaxStatusDate = DateTime.Now;
                    payrollEntity.HaciendaTaxStatusByName = SessionHelper.UserName;

                    if (batchEntity != null)
                    {
                        batchEntity.intHaciendaTaxDepositStatusId = model.HaciendaTaxDepositStatusId.Value;
                        batchEntity.strHaciendaTaxReceiptNo = model.TaxReferenceNo;
                        batchEntity.decHaciendaTaxDepositAmount = model.TaxDepositAmount;
                        batchEntity.dtHaciendaTaxDepositDate = model.TaxDepositDate;
                        batchEntity.dtHaciendaTaxStatusDate = DateTime.Now;
                        batchEntity.strHaciendaTaxStatusByName = SessionHelper.UserName;

                        tAWinDbContext.SaveChanges();
                    }
                }
                else if (model.StatusTypeId == 2)
                {
                    payrollEntity.FUTATaxDepositStatusId = model.FUTATaxDepositStatusId.Value;
                    payrollEntity.FUTATaxReceiptNo = model.TaxReferenceNo;
                    payrollEntity.FUTATaxDepositAmount = model.TaxDepositAmount;
                    payrollEntity.FUTATaxDepositDate = model.TaxDepositDate;
                    payrollEntity.FUTATaxComments = model.Comments;
                    payrollEntity.FUTATaxStatusDate = DateTime.Now;
                    payrollEntity.FUTATaxStatusByName = SessionHelper.UserName;

                    if (batchEntity != null)
                    {
                        batchEntity.intFUTATaxDepositStatusId = model.FUTATaxDepositStatusId.Value;
                        batchEntity.strFUTATaxReceiptNo = model.TaxReferenceNo;
                        batchEntity.decFUTATaxDepositAmount = model.TaxDepositAmount;
                        batchEntity.dtFUTATaxDepositDate = model.TaxDepositDate;
                        batchEntity.dtFUTATaxStatusDate = DateTime.Now;
                        batchEntity.strFUTATaxStatusByName = SessionHelper.UserName;

                        tAWinDbContext.SaveChanges();
                    }
                }
                payrollDBConetext.SaveChanges();
                switch (model.StatusTypeId)
                {
                    case 0:
                        newReturnVal = payrollEntity.FederalTaxDepositStatus.FederalTaxDepositStatusName;
                        newRetStatusId = payrollEntity.FederalTaxDepositStatusId;
                        break;
                    case 1:
                        newReturnVal = payrollEntity.HaciendaTaxDepositStatus.HaciendaTaxDepositStatusName;
                        newRetStatusId = payrollEntity.HaciendaTaxDepositStatusId;
                        break;
                    case 2:
                        newReturnVal = payrollEntity.FUTATaxDepositStatus.FUTATaxDepositStatusName;
                        newRetStatusId = payrollEntity.FUTATaxDepositStatusId??0;
                        break;
                }

            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, retVal = newReturnVal, retStatusId = newRetStatusId });
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

                        switch (reportTypeId)
                        {
                            case 0: //summary rep
                                payrollEntity.CompanyPayrollSummary = fileData;
                                payrollEntity.PayrollSummaryRptName = fileName;
                                if (batchEntity != null)
                                {
                                    batchEntity.varCompanyPayrollSummary = fileData;
                                    batchEntity.strPayrollSummaryRptName = fileName;
                                    tAWinDbContext.SaveChanges();
                                }
                                break;
                            case 1: //fed rep
                                payrollEntity.FederalTaxConfirmation = fileData;
                                payrollEntity.FederalTaxRptName = fileName;
                                if (batchEntity != null)
                                {
                                    batchEntity.varFederalTaxConfirmation = fileData;
                                    batchEntity.strFederalTaxRptName = fileName;
                                    tAWinDbContext.SaveChanges();
                                }
                                break;
                            case 2: //Hac report
                                payrollEntity.HaciendaTaxConfirmation = fileData;
                                payrollEntity.HaciendaTaxRptName = fileName;
                                if (batchEntity != null)
                                {
                                    batchEntity.varHaciendaTaxConfirmation = fileData;
                                    batchEntity.strHaciendaTaxRptName = fileName;
                                    tAWinDbContext.SaveChanges();
                                }
                                break;
                            case 3:
                                payrollEntity.FUTATaxConfirmation = fileData;
                                payrollEntity.FUTATaxRptName = fileName;
                                if (batchEntity != null)
                                {
                                    batchEntity.varFUTATaxConfirmation = fileData;
                                    batchEntity.strFUTATaxRptName = fileName;
                                    tAWinDbContext.SaveChanges();
                                }
                                break;
                        }

                        payrollEntity.ModifiedDate = DateTime.Now;
                        payrollEntity.ModifiedBy = SessionHelper.LoginId;
                        payrollDBConetext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("No report file found.");
                    }
                }
                else
                {
                    throw new Exception("No Payroll tax record found.");
                }

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

            byte[] fileBytes = null;
            string fileName = "";
            var payrollEntity = payrollDBConetext.Payroll.Find(payrollId);
            if (payrollEntity != null)
            {
                switch (reportTypeId)
                {
                    case 0: //summary
                        fileBytes = payrollEntity.CompanyPayrollSummary;
                        fileName = "SummaryRpt_" + payrollEntity.PayrollSummaryRptName;
                        break;
                    case 1: //Fed
                        fileBytes = payrollEntity.FederalTaxConfirmation;
                        fileName = "FederalTaxRpt_" + payrollEntity.FederalTaxRptName;
                        break;
                    case 2: //Hac
                        fileBytes = payrollEntity.HaciendaTaxConfirmation;
                        fileName = "HaciendaTaxRpt_" + payrollEntity.HaciendaTaxRptName;
                        break;
                    case 3:
                        fileBytes = payrollEntity.FUTATaxConfirmation;
                        fileName = "FUTATaxRpt_" + payrollEntity.FUTATaxRptName;
                        break;
                }
                if (fileBytes != null)
                {
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
            return null;
        }

    }
}