using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;
using System.Data.Entity;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TimeAide.Web.Controllers
{
    public class AuthorizationException : ApplicationException
    {
        public string ErrorMessage
        {
            get { return "You are not authorize to access the requested information. Please contact system admin"; }
        }
    }
    public abstract class TimeAidePayrollBaseControllers : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (String.IsNullOrWhiteSpace(SessionHelper.LoginEmail))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" }, { "ReturnUrl", filterContext.HttpContext.Request.Url.PathAndQuery } });
            }


        }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            //Log the error!!
            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, filterContext.Exception, this.ControllerContext);
            //Redirect to action
            //filterContext.Result = RedirectToAction("Error", "InternalError");

            // OR return specific view
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/_InternalError.cshtml"
            };
        }

        
    }
    public abstract class TimeAidePayrollSecurityBaseControllers: TimeAidePayrollBaseControllers
    {
       public TimeAidePayrollSecurityBaseControllers(string name)
        {

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (String.IsNullOrWhiteSpace(SessionHelper.LoginEmail))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" }, { "ReturnUrl", filterContext.HttpContext.Request.Url.PathAndQuery } });
            }
        }
        public void AllowDelete()
        {
            //if (!privileges.AllowDelete)
            //{
            //    throw new AuthorizationException();
            //}
        }
        public void AllowEdit()
        {
            //if (!privileges.AllowEdit)
            //{
            //    throw new AuthorizationException();
            //}
        }
        public virtual void AllowAdd()
        {
            //if (!privileges.AllowAdd)
            //{
            //    throw new AuthorizationException();
            //}
        }
        public virtual void AllowView()
        {
            //if (!privileges.AllowView)
            //{
            //    throw new AuthorizationException();
            //}
        }
    }
    public abstract class TimeAidePayrollControllers<T> : TimeAidePayrollSecurityBaseControllers where T : BaseWithLoggingEntity, new()
    {
        public TimeAidePayrollContext payrollDBConetext = new TimeAidePayrollContext();
        public TimeAidePayrollControllers() : base(typeof(T).Name)
        {
        }
        public virtual List<T> OnIndex(List<T> model)
        {
            return model;
        }
        public virtual ActionResult Index()
        {
            try
            {
                AllowView();
                var entitySet = payrollDBConetext.GetAll<T>();
                entitySet = OnIndex(entitySet);

                return PartialView(entitySet.OrderByDescending(w=>w.CreatedDate));
              
            }
            catch (AuthorizationException ex)
            {
              
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(T).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        
        public virtual ActionResult Details(int? id)
        {
            try
            {
               AllowView();
               var model= payrollDBConetext.Find<T>(id ?? 0);
               return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "", "");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
       
        public virtual ActionResult Create()
        {
            try
            {
                AllowAdd();
                if (typeof(T) == typeof(UserInformation))
                {
                    IEnumerable<SelectListItem> companySelectList = null;
                    companySelectList = payrollDBConetext.GetAll<Company>().OrderBy(o => o.CompanyName)
                                            .Select(s => new SelectListItem
                                            {
                                                Text = s.CompanyName,
                                                Value = s.Id.ToString()

                                            });
                    ViewBag.UserCompanyList = companySelectList;
                }
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "", "Create");
                return PartialView("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
      
        public virtual ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = payrollDBConetext.Find<T>(id??0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "", "Edit");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual void OnClose(T entity)
        {
        }
       
        public ActionResult Delete(int id)
        {
            try
            {
                AllowDelete();
                var entity = payrollDBConetext.Find<T>(id);
                return PartialView(entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "", "Delete");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }    
                     
    }
}