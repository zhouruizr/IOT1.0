using CCSH.DataProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCSH.DataProvider.SqlServer;
using CCSH.DataProvider.SqlServer.Model;
using ANTPower.Security;
using DataProvider.SqlServer;

namespace DataProvider.SqlServer
{
    public class DPAuthority : DPBase
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param> 
        /// <returns></returns>
        public static List<SYS_Account> Login(string userName, string password)
        {
            try
            {
                var newPassWord = StringHelper.CreateMD5(password);//md5加密
                using (NERPEntities context = new NERPEntities())
                {
                    var result = context.SYS_Account.Where(c => c.ACC_Account == userName).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int ChangePassWord(string userName, string password)
        {
            var newPassWord = StringHelper.CreateMD5(password);//md5加密
            using (NERPEntities context = new NERPEntities())
            {
                var result = context.SYS_Account.First(c => c.ACC_Account == userName);
                result.ACC_Password = newPassWord;
                var d = context.SaveChanges();
                return d;
            }
        }


        /// <summary>
        /// 获取所有手机端菜单-调用存储过程sp_AuthorityWap实现-20150402
        /// </summary>
        /// <param name="staffId">工号</param>
        /// <returns></returns>
        public static IList<Menu> GetFunctionMenuWap(string staffId)
        {
            using (NERPEntities context = new NERPEntities())
            {
                var query = context.sp_AuthorityWap(staffId).ToList();
                var menus = GetChildMenu(query, 0);
                return menus;
            }
        }

        /// <summary>
        /// 获取子级--手机端专用--20150402
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static IList<Menu> GetChildMenu(IList<sp_AuthorityWap_Result> list, int id)
        {
            IList<Menu> menus = new List<Menu>();
            foreach (var item in list.Where(it => it.ParentId == id))
            {
                Menu menu = new Menu();
                menu.ID = item.ID;
                menu.Name = item.Name;
                menu.URL = item.URL;
                menu.OrderIndex = item.OrderIndex;
                menu.IsSuspended = item.IsSuspended;
                menu.Childs = GetChildMenu(list, item.ID);
                menus.Add(menu);
            }
            return menus;
        }

    }
}
