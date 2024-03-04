using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNails.Transactions.Models;

namespace WebNails.Transactions.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User != null && User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult _Index(string search = "", DateTime? DateFrom = null, DateTime? DateTo = null)
        {
            using (var sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ContextDatabase"].ConnectionString))
            {
                var intSkip = Utilities.PagingHelper.Skip;

                var param = new DynamicParameters();
                param.Add("@intSkip", intSkip);
                param.Add("@intTake", Utilities.PagingHelper.CountSort);
                param.Add("@strDateFrom", DateFrom);
                param.Add("@strDateTo", DateTo);
                param.Add("@strValue", search);
                param.Add("@intTotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var objResult = sqlConnect.Query<InfoPaypal>("spInfoPaypal_GetInfoPaypal", param, commandType: CommandType.StoredProcedure);

                ViewBag.Count = param.Get<int>("@intTotalRecord");

                return PartialView(objResult);
            }
        }
    }
}