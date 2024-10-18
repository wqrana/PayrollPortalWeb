using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;
namespace TimeAide.Web.Controllers
{
    public class UserInformationController : TimeAidePayrollControllers<UserInformation>
    {
        public override ActionResult Edit(int? id)
        {
            AllowEdit();
            var model = payrollDBConetext.UserInformation.Find(id ?? 0);
            IEnumerable<SelectListItem> companySelectList = null;
            companySelectList = payrollDBConetext.GetAll<Company>().OrderBy(o => o.CompanyName)
                                    .Select(s => new SelectListItem
                                    {
                                        Text = s.CompanyName,
                                        Value = s.Id.ToString()

                                    });
            ViewBag.UserCompanyItemList = companySelectList;
            var selectedUserCmp = payrollDBConetext.UserCompany.Where(w => w.UserInformationId == model.Id && w.DataEntryStatus == 1)
                                                    .Select(s => s.CompanyId.ToString()).ToArray();
            ViewBag.SelectedUserCompanyList = selectedUserCmp;
            return PartialView(model);
           
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(UserInformation model)
        {
            string status = "Success";
            string message = "User is successfully saved!";
            UserInformation userEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    userEntity = model;
                    payrollDBConetext.UserInformation.Add(userEntity);
                }
                else
                {
                    userEntity = payrollDBConetext.UserInformation.Find(model.Id);
                    if (userEntity != null)
                    {
                        userEntity.EmployeeId = model.EmployeeId;
                        userEntity.FirstName = model.FirstName;
                        userEntity.MiddleInitial = model.MiddleInitial;
                        userEntity.FirstLastName = model.FirstLastName;
                        userEntity.SecondLastName = model.SecondLastName;
                        userEntity.LoginStatus = model.LoginStatus;
                        userEntity.IsAdmin = model.IsAdmin;
                        userEntity.ModifiedBy = SessionHelper.LoginId;
                        userEntity.ModifiedDate = DateTime.Now;
                    }
                }
                payrollDBConetext.SaveChanges();
                //Save User Company
                var selectedUserCompanyList = model.SelectedUserCompanyIds==null?new List<string>(): model.SelectedUserCompanyIds.Split(',').ToList();
                var existingUserCompany = payrollDBConetext.UserCompany.Where(u => u.UserInformationId == userEntity.Id);
                if (existingUserCompany.Count() > 0)
                {
                    payrollDBConetext.UserCompany.RemoveRange(existingUserCompany);
                }
                if (selectedUserCompanyList.Count() > 0)
                {
                    foreach(var userCompany in selectedUserCompanyList)
                    {
                        var companyId = int.Parse(userCompany);
                        var userCompanyEntity = new UserCompany() { UserInformationId = userEntity.Id, CompanyId = companyId };
                        payrollDBConetext.UserCompany.Add(userCompanyEntity);
                    }
                }
                payrollDBConetext.SaveChanges();
            }
            catch(Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            //return PartialView(training);
            return Json(new { status = status, message = message });
        }
        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var userEntity = payrollDBConetext.UserInformation.Find(id);
            try
            {
                userEntity.ModifiedBy = SessionHelper.LoginId;
                userEntity.ModifiedDate = DateTime.Now;
                userEntity.DataEntryStatus = 0;
                payrollDBConetext.SaveChanges();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                payrollDBConetext.Dispose();
            }
            base.Dispose(disposing);
        }
    
}
}