using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using DataProvider.SqlServer;
using CCSH.DataProvider.SqlServer;
using IOT1._0.Models;
using CCSH.DataProvider.SqlServer.Model;
using DataProvider;
using System.IO;
using System.Drawing;
using System.Net.Http.Headers;

namespace IOT1._0.Controllers
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public int Login(JObject param)
        {
            try
            {
                string userName = param["userName"].ToString();
                string password = param["password"].ToString();
                if (string.IsNullOrEmpty(userName)||string.IsNullOrEmpty(password))//用户名或者密码为空
                {
                    return -1;
                }

                var dt = DPAuthority.Login(userName, password);//执行查询

                if (dt.Count == 0)//用户或者密码不正确
                {
                    return -2;
                }

                foreach (var d in dt)//设置session
                {
                    UserSession.userid = d.ACC_Account;
                    UserSession.username = d.ACC_Account;
                    UserSession.UserLoginTime = DateTime.Now.ToString();
                }
                return dt.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            };
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        [HttpGet]
        public void Logout()
        {
            UserSession.userid = "";
            UserSession.username = "";
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public int ChangePassWord(JObject param)
        {
            if (UserSession.userid == "")
            {
                throw new Exception("请重新登录！");
            }
           
            //判断当前用户是否与传进来的值一致
            string userName = UserSession.username;
            string oldpassword = param["oldpassword"].ToString();
            string newpassword = param["newpassword"].ToString();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(oldpassword))//用户名或者密码为空
            {
                return -1;
            }

            //先查询旧的密码和用户名是否在库中存在
            var dt = DPAuthority.Login(userName, oldpassword);//执行查询
            if (dt.Count == 0)
            {
                return -2;//用户名和密码不正确。 
            }
            //正确，则更新密码
            var d = DPAuthority.ChangePassWord(userName, newpassword);
            return d; //结果为0，更新失败；大于0，则更新成功。
        }

        [HttpGet]
        public dynamic GetUserInfo()
        {
            throw new Exception();
        }

        /// <summary>
        /// 获取当前版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetVersion()
        {
            var version = ConfigurationManager.AppSettings["Version"].ToString();
            return version;
        }

        /// <summary>
        /// 获取手机端菜单
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<Menu> GetMenuWap(string staffId)
        {
            ////临时模拟一个登陆
            //UserSession.userid = "01447";
            //UserSession.username = "周瑞";
            if (UserSession.userid == "")
            {
                throw new Exception("请重新登录！");
            }
            var ret = DPAuthority.GetFunctionMenuWap(staffId);
            return ret;
        }

        /// <summary>
        /// 获得验证码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResponseMessage getCode()
        {
            //生成验证码
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            UserSession.ValidateCode = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return result;
        
        }
    }
   
}
