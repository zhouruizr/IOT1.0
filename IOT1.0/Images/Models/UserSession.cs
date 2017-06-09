using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOT1._0.Models
{
    public class UserSession
    {
        public static string username
        {
            set
            {
                HttpContext.Current.Session["username"] = value;
            }
            get
            {
                if (HttpContext.Current.Session["username"] == null || HttpContext.Current.Session["username"].ToString().Trim() == "")
                {
                    HttpContext.Current.Response.Redirect("~/login.html", true);
                    throw new Exception("登录超时，请重新登录！");
                }
                else
                {
                    return HttpContext.Current.Session["username"].ToString();
                }
            }
        }

        public static string userid
        {
            set
            {
               
                HttpContext.Current.Session["userid"] = value;
            }
            get
            {
                if (HttpContext.Current.Session["userid"] == null || HttpContext.Current.Session["userid"].ToString().Trim() == "")
                {
                    HttpContext.Current.Response.Redirect("~/login.html", true);
                    throw new Exception("登录超时，请重新登录！");
                }
                else
                {
                    return  HttpContext.Current.Session["userid"].ToString();
                }
            }
        }

        public static string UserLoginTime
        {
            set
            {

                HttpContext.Current.Session["UserLoginTime"] = value;
            }
            get
            {
                if (HttpContext.Current.Session["UserLoginTime"] == null || HttpContext.Current.Session["UserLoginTime"].ToString().Trim() == "")
                {
                    throw new Exception("登录超时，请重新登录！");
                }
                else
                {
                    return HttpContext.Current.Session["UserLoginTime"].ToString();
                }
            }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        public static string ValidateCode
        {
            set
            {
                HttpContext.Current.Session["ValidateCode"] = value;
            }
            get
            {
                return HttpContext.Current.Session["ValidateCode"].ToString();
            }
        }

        ////模拟错误
        //public static string _userid
        //{
        //    set
        //    {

        //        HttpContext.Current.Session["userid"] = value;
        //    }
        //    get
        //    {

        //            //HttpContext.Current.Response.Redirect("~/login.html", true);
        //            throw new Exception("登录超时，请重新登录！");

        //    }
        //}
    }
}