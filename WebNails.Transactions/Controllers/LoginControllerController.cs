using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebNails.Transactions.Models;
using WebNails.Transactions.Utilities;

namespace WebNails.Transactions.Controllers
{
    public class LoginControllerController : Controller
    {
        // GET: administrator/Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {

            var strMD5Password = Sercurity.Md5(model.Password);

            var queryString = Request.UrlReferrer.Query;

            var queryDictionary = HttpUtility.ParseQueryString(queryString);

            var checklogin = CheckLogin(new LoginModel { Username = model.Username, Password = strMD5Password });
            if (checklogin)
            {
                var ticket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now, DateTime.Now.AddHours(1), true, "UserLogged", FormsAuthentication.FormsCookiePath);
                var strEncrypt = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEncrypt);
                Response.Cookies.Add(cookie);

                if (queryDictionary.Count > 0)
                {
                    return Json(new { ReturnUrl = queryDictionary.Get("ReturnUrl"), IsLogin = true, Message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ReturnUrl = "/transactions.html", IsLogin = true, Message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { ReturnUrl = queryDictionary.Get("ReturnUrl"), IsLogin = false, Message = "Login Fail" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = System.DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index", "Login");
        }

        private bool CheckLogin(LoginModel model)
        {
            using (var sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ContextDatabase"].ConnectionString))
            {
                var Domain = Request.Url.Host;

                var checklogin = sqlConnect.Query("spUserSite_GetByUsernameAndPassword", new { strUsername = model.Username, strPassword = model.Password, strDomain = Domain }, commandType: CommandType.StoredProcedure).Count() == 1;

                return checklogin;
            }
        }
    }
}