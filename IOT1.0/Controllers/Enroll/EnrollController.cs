using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT1._0.Controllers.Enroll
{
    public class EnrollController : Controller
    {
        //
        // GET: /Enroll/
        /// <summary>
        /// 报名记录
        /// </summary>
        /// <returns></returns>
        public ActionResult EnrollList()
        {
            return View();
        }
        /// <summary>
        /// 回访记录
        /// </summary>
        /// <returns></returns>
        public ActionResult FollowList()
        {
            return View();
        }
        /// <summary>
        /// 票据打印
        /// </summary>
        /// <returns></returns>
        public ActionResult InvoicePrint()
        {
            return View();
        }
    }
}
