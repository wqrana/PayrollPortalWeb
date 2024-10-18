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
    public class PayrollQuarterlyTaxMangementController : TimeAidePayrollControllers<PayrollQuarterlyTax>
    {
        // GET: PayrollQuarterlyTaxMangement
        private IDictionary<int, string> StatusTypeList = new Dictionary<int, string>() { { 0, "FUTA Tax Status" }, { 1, "SUTA Tax Status" }, { 2, "SINOT Tax Status" }, { 3, "CHOFERIL Tax Status" } };
        private IDictionary<int, string> ReportTypeList = new Dictionary<int, string>() { { 0, "FUTA Report" }, { 1, "SUTA Rpt" }, { 2, "SINOT Rpt" }, { 3, "CHOFERIL Rpt" } };
        private readonly DateTime DefaultTaxesFromDate;

        public PayrollQuarterlyTaxMangementController()
        {
            int quaterlyTaxesFromDD;
            int.TryParse(ConfigurationManager.AppSettings["QuaterlyTaxesFromDD"],out quaterlyTaxesFromDD);
            DefaultTaxesFromDate = DateTime.Today.AddDays(quaterlyTaxesFromDD * -1);
        }
        public override ActionResult Index()
        {
            var currentDate = DateTime.Today;
            ViewBag.PayrollTaxesFromDD = int.Parse(ConfigurationManager.AppSettings["QuaterlyTaxesFromDD"]);

            var quarterList = payrollDBConetext.GetAll<PayrollQuarterlyTax>()
                                                .Where(W => W.QuarterStartDate >= DefaultTaxesFromDate)
                                                .Select(s => s.Quarter)
                                                .Distinct().Select(s => new { Id = s, Name = s });

            var selectedQuater = payrollDBConetext.GetAll<PayrollQuarterlyTax>()
                                                .Where(w => (currentDate >= w.QuarterStartDate && currentDate <= w.QuarterEndDate))
                                                .Select(w => w.Quarter).Distinct().FirstOrDefault();

            // ViewBag.CompanyId = new SelectList(payrollDBConetext.GetAll<Company>().OrderBy(o => o.CompanyName), "Id", "CompanyName");
            ViewBag.CompanyId = new SelectList(DataHelper.GetUserCompanies(), "Id", "CompanyName");
            ViewBag.QuarterId = new SelectList(quarterList, "Id", "Name", selectedQuater);
            ViewBag.FUTATaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<FUTATaxDepositStatus>(), "Id", "FUTATaxDepositStatusName");
            ViewBag.FUTATaxDepositScheduleId = new SelectList(payrollDBConetext.GetAll<FUTATaxDepositSchedule>(), "Id", "FUTATaxDepositScheduleName");

            ViewBag.SUTATaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<SUTATaxDepositStatus>(), "Id", "SUTATaxDepositStatusName");
            ViewBag.SINOTTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<SINOTTaxDepositStatus>(), "Id", "SINOTTaxDepositStatusName");
            ViewBag.CHOFERILTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<CHOFERILTaxDepositStatus>(), "Id", "CHOFERILTaxDepositStatusName");

            return PartialView();
        }
        public ActionResult IndexByFilter(PayrollFilterViewModel model)
        {
            //  var payrollList = payrollDBConetext.Payroll.ToList();
            var payrollTaxList = getQuarterlyTaxList(model);

            return PartialView(payrollTaxList);
        }
        private List<PayrollQuarterlyTax> getQuarterlyTaxList(PayrollFilterViewModel model)
        {
            var companyName = "";
            // var retList = payrollDBConetext.PayrollInformation<Payroll>(model.CompanyId, model.PayDate, model.PayrollStatusId, model.PaymentStatusId);
            var companyInfo = payrollDBConetext.Company.Find(model.CompanyId);
            if (companyInfo != null) companyName = companyInfo.CompanyName;
            var userCompaniesList = DataHelper.GetUserCompanies().Select(s => s.CompanyName).ToArray();
            var retList = payrollDBConetext.PayrollQuarterlyTax
                            .Where(w => w.DataEntryStatus == 1)
                            .Where(W => W.QuarterStartDate >= DefaultTaxesFromDate)
                            .Where(w => (companyName == "" ? (userCompaniesList.Count() == 0 ? true : userCompaniesList.Contains(w.CompanyName)) : (w.CompanyName == companyName)))
                            .Where(w => (model.Quarter== null ? true:(w.Quarter== model.Quarter)))                           
                            .Where(w => model.FUTATaxDepositScheduleId == 0 ? true : (w.FUTATaxDepositScheduleId == model.FUTATaxDepositScheduleId))
                            .Where(w => model.FUTATaxDepositStatusId == 0 ? true : (w.FUTATaxDepositStatusId == model.FUTATaxDepositStatusId))
                            .Where(w => model.SUTATaxDepositStatusId == 0 ? true : (w.SUTATaxDepositStatusId == model.SUTATaxDepositStatusId))
                            .Where(w => model.SINOTTaxDepositStatusId == 0 ? true : (w.SINOTTaxDepositStatusId == model.SINOTTaxDepositStatusId))
                            .Where(w => model.CHOFERILTaxDepositStatusId == 0 ? true : (w.CHOFERILTaxDepositStatusId == model.CHOFERILTaxDepositStatusId))
                            .ToList();

            return retList;
        }
        public ActionResult AjaxEditTaxStatus(int id, int statusTypeId)
        {
            var model = payrollDBConetext.PayrollQuarterlyTax.Find(id);
            if (model != null)
            {
                ViewBag.StatusTypeName = StatusTypeList[statusTypeId];
                ViewBag.StatusTypeId = statusTypeId;               
                ViewBag.FUTATaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<FUTATaxDepositStatus>(), "Id", "FUTATaxDepositStatusName", model.FUTATaxDepositStatusId);
                ViewBag.SUTATaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<SUTATaxDepositStatus>(), "Id", "SUTATaxDepositStatusName",model.SUTATaxDepositStatusId);
                ViewBag.SINOTTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<SINOTTaxDepositStatus>(), "Id", "SINOTTaxDepositStatusName",model.SINOTTaxDepositStatusId);
                ViewBag.CHOFERILTaxDepositStatusId = new SelectList(payrollDBConetext.GetAll<CHOFERILTaxDepositStatus>(), "Id", "CHOFERILTaxDepositStatusName",model.CHOFERILTaxDepositStatusId);
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult SaveEditTaxStatus(QuarterlyTaxViewModel model)
        {
            string status = "Success";
            string message = "Successfully Updated!";
            string newReturnVal = "";
            int newRetStatusId = 0;
            decimal? depositAmt = 0;
            var taxEntity = payrollDBConetext.PayrollQuarterlyTax.Find(model.Id);
            try
            {

                taxEntity.ModifiedBy = SessionHelper.LoginId;
                taxEntity.ModifiedDate = DateTime.Now;
                if (model.StatusTypeId == 0) //Futa tax
                {
                    taxEntity.FUTATaxDepositStatusId = model.FUTATaxDepositStatusId.Value;
                    taxEntity.FUTATaxReceiptNo = model.TaxReferenceNo;
                    taxEntity.FUTATaxDepositAmount = model.TaxDepositAmount;
                    taxEntity.FUTATaxDepositDate = model.TaxDepositDate;
                    taxEntity.FUTATaxComments = model.Comments;
                    taxEntity.FUTATaxStatusDate = DateTime.Now;
                    taxEntity.FUTATaxStatusByName = SessionHelper.UserName;
                    
                }
                else if (model.StatusTypeId == 1)//Suta tax
                {
                    taxEntity.SUTATaxDepositStatusId = model.SUTATaxDepositStatusId.Value;
                    taxEntity.SUTATaxReceiptNo = model.TaxReferenceNo;
                    taxEntity.SUTATaxDepositAmount = model.TaxDepositAmount;
                    taxEntity.SUTATaxDepositDate = model.TaxDepositDate;
                    taxEntity.SUTATaxComments = model.Comments;
                    taxEntity.SUTATaxStatusDate = DateTime.Now;
                    taxEntity.SUTATaxStatusByName = SessionHelper.UserName;
                }
                else if (model.StatusTypeId == 2)
                {
                    taxEntity.SINOTTaxDepositStatusId = model.SINOTTaxDepositStatusId.Value;
                    taxEntity.SINOTTaxReceiptNo = model.TaxReferenceNo;
                    taxEntity.SINOTTaxDepositAmount = model.TaxDepositAmount;
                    taxEntity.SINOTTaxDepositDate = model.TaxDepositDate;
                    taxEntity.SINOTTaxComments = model.Comments;
                    taxEntity.SINOTTaxStatusDate = DateTime.Now;
                    taxEntity.SINOTTaxStatusByName = SessionHelper.UserName;
                }
                else if (model.StatusTypeId == 3)
                {
                    taxEntity.CHOFERILTaxDepositStatusId = model.CHOFERILTaxDepositStatusId.Value;
                    taxEntity.CHOFERILTaxReceiptNo = model.TaxReferenceNo;
                    taxEntity.CHOFERILTaxDepositAmount = model.TaxDepositAmount;
                    taxEntity.CHOFERILTaxDepositDate = model.TaxDepositDate;
                    taxEntity.CHOFERILTaxComments = model.Comments;
                    taxEntity.CHOFERILTaxStatusDate = DateTime.Now;
                    taxEntity.CHOFERILTaxStatusByName = SessionHelper.UserName;
                }
                payrollDBConetext.SaveChanges();
                switch (model.StatusTypeId)
                {
                    case 0:
                        newReturnVal = taxEntity.FUTATaxDepositStatus.FUTATaxDepositStatusName;
                        newRetStatusId = taxEntity.FUTATaxDepositStatusId.Value;
                        depositAmt = taxEntity.FUTATaxDepositAmount;
                        break;
                    case 1:
                        newReturnVal = taxEntity.SUTATaxDepositStatus.SUTATaxDepositStatusName;
                        newRetStatusId = taxEntity.SUTATaxDepositStatusId.Value;
                        depositAmt = taxEntity.SUTATaxDepositAmount;
                        break;
                    case 2:
                        newReturnVal = taxEntity.SINOTTaxDepositStatus.SINOTTaxDepositStatusName;
                        newRetStatusId = taxEntity.SINOTTaxDepositStatusId??0;
                        depositAmt = taxEntity.SINOTTaxDepositAmount;
                        break;
                    case 3:
                        newReturnVal = taxEntity.CHOFERILTaxDepositStatus.CHOFERILTaxDepositStatusName;
                        newRetStatusId = taxEntity.CHOFERILTaxDepositStatusId ?? 0;
                        depositAmt = taxEntity.CHOFERILTaxDepositAmount;
                        break;
                }

            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, retVal = newReturnVal, retStatusId = newRetStatusId, retDepositAmt= depositAmt });
        }
        public ActionResult AjaxUploadReportFile(int id, int reportTypeId)
        {
            var model = payrollDBConetext.PayrollQuarterlyTax.Find(id);
            ViewBag.ReportTypeName = ReportTypeList[reportTypeId];
            ViewBag.ReportTypeId = reportTypeId;

            return PartialView("UploadReport", model);
        }
        [HttpPost]
        public JsonResult UploadReportFile()
        {
            string status = "Success";
            string message = "Report file is Successfully uploaded!";

            int id = int.Parse(Request.Form["Id"]);
            int reportTypeId = int.Parse(Request.Form["ReportTypeId"]);

            try
            {
                var payrollEntity = payrollDBConetext.PayrollQuarterlyTax.Find(id);
              
               
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
                            case 0: //futa rep
                                payrollEntity.FUTATaxConfirmation = fileData;
                                payrollEntity.FUTATaxRptName = fileName;                              
                                break;
                            case 1: //suta rep
                                payrollEntity.SUTATaxConfirmation = fileData;
                                payrollEntity.SUTATaxRptName = fileName;                                
                                break;
                            case 2: //sinot report
                                payrollEntity.SINOTTaxConfirmation = fileData;
                                payrollEntity.SINOTTaxRptName = fileName;                               
                                break;
                            case 3: //CHOFERIL report
                                payrollEntity.CHOFERILTaxConfirmation = fileData;
                                payrollEntity.CHOFERILTaxRptName = fileName;                              
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
        public ActionResult DownloadReportFile(int id, int reportTypeId)
        {

            byte[] fileBytes = null;
            string fileName = "";
            var payrollEntity = payrollDBConetext.PayrollQuarterlyTax.Find(id);
            if (payrollEntity != null)
            {
                switch (reportTypeId)
                {
                    case 0: //futa
                        fileBytes = payrollEntity.FUTATaxConfirmation;
                        fileName = "FUTARpt_" + payrollEntity.FUTATaxRptName;
                        break;
                    case 1: //suta
                        fileBytes = payrollEntity.SUTATaxConfirmation;
                        fileName = "SUTATaxRpt_" + payrollEntity.SUTATaxRptName;
                        break;
                    case 2: //sinot 
                        fileBytes = payrollEntity.SINOTTaxConfirmation;
                        fileName = "SINOTTaxRpt_" + payrollEntity.SINOTTaxRptName;
                        break;
                    case 3: //CHOFERIL
                        fileBytes = payrollEntity.CHOFERILTaxConfirmation;
                        fileName = "CHOFERILTaxRpt_" + payrollEntity.CHOFERILTaxRptName;
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