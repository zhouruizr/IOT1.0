using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT1._0.Controllers.Finance
{
    public class FinanceController : Controller
    {
        //
        // GET: /Finance/

        public ActionResult DailyReport()
        {
            return View();
        }

    }
}
