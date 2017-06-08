using CCSH.DataProvider;
using DataProvider;
using DataProvider.SqlServer;
using IOT1._0.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace IOT1._0.Controllers
{
    public class AccountController : BaseController
    {
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        [HttpPost]
        public Flexigride GetJson(JObject json)
        {
            SearchMod<SYS_Account> searchModel = new SearchMod<SYS_Account>();
            searchModel.page = Convert.ToInt32(json["page"].ToString());//当前页
            searchModel.rp = Convert.ToInt32(json["rp"].ToString());//页面大小
            searchModel.sortorder = json["sortorder"].ToString();//排序字段
            searchModel.sortname = json["sortname"].ToString();//排序方式

            SYS_Account model = JsonToObject<SYS_Account>(json);
            IQueryable<SYS_Account> query = DPBase.db.SYS_Account;
            query = string.IsNullOrEmpty(searchModel.sortorder) ? query.OrderByDescending(c => searchModel.sortorder) : query.OrderBy(c => searchModel.sortorder);
            if (!string.IsNullOrEmpty(model.ACC_Account))
            {
                query = query.Where(c => c.ACC_Account.Contains(model.ACC_Account));
            }
            if (!string.IsNullOrEmpty(model.ACC_Email))
            {
                query = query.Where(c => c.ACC_Email.Contains(model.ACC_Email));
            }
            searchModel.query = query;
            Flexigride grid = new Flexigride();
            grid.rows = DPBase.DPGetQueryLst(searchModel, out searchModel);
            grid.page = searchModel.page;
            grid.total = searchModel.total;
            return grid;
        }

        ///<summary>
        ///获取单个按钮的信息
        ///</summary>
        [HttpPost]
        public SYS_Account Get(JObject json)
        {
            var model = JsonToObject<SYS_Account>(json);
            SYS_Account _model = DPBase.Get<SYS_Account>(model.ACC_Id);
            return _model;
        }

        ///<summary>
        ///验证button属性方法
        ///</summary>
        [HttpPost]
        public string ValACCAttribute(JObject json)
        {
            SYS_Account model = JsonToObject<SYS_Account>(json);
            using (NERPEntities context= new NERPEntities())
            {
               IQueryable<SYS_Account> _Account = context.SYS_Account.Where(c => c.ACC_Account == model.ACC_Account);
               if (_Account.Count()>0)
               {
                   return "1";
               }
               return "0";
            }
        }

        ///<summary>
        ///新增
        ///</summary>
        ///<param name="userName">用户名</param>
        [HttpPost]
        public int Add(JObject json)
        {
            SYS_Account model = JsonToObject<SYS_Account>(json);
            model.ACC_CreatedBy = UserSession.userid;  //当前用户
            model.ACC_CreatedOn = DateTime.Now;
            return DPBase.Add(model);
        }

        ///<summary>
        ///修改
        ///</summary>
        [HttpPost]
        public string Edit(JObject json)
        {
            SYS_Account model = JsonToObject<SYS_Account>(json);
            model.ACC_Password = model.ACC_Password == "" ? "" : ANTPower.Security.Encrypt.StrToMd5Encrypt(model.ACC_Password);
            return DPBase.Update(model) ? "新增成功！" : "新增失败";
        }

        ///<summary>
        ///删除
        ///</summary>
        [HttpPost]
        public string Delete(JObject json)
        {
            var model = JsonToObject<SYS_Account>(json);
            return DPBase.Delete<SYS_Account>(model.ACC_Id) ? "删除成功！" : "删除失败";
        }



        ///<summary>
        ///给角色分配权限
        ///</summary>
        [HttpPost]
        public string Allocate(JObject json)
        {
            try 
	        {
                int AR_AccountId = Convert.ToInt32(json["ACC_ID"].ToString());
                using (NERPEntities context = new NERPEntities())
                {
                    //删除已有的数据集合
                    List<SYS_AccountRole> tempList = context.SYS_AccountRole.Where(c => c.AR_AccountId == AR_AccountId).ToList();
                    context.SYS_AccountRole.RemoveRange(tempList);
                    context.SaveChanges();

                    //循环添加
                    for (int i = 0; i < json["RoleValueArray"].Count(); i++)
                    {
                        string tempRoleName = json["RoleValueArray"][i].ToString();
                        SYS_AccountRole _role = new SYS_AccountRole();
                        _role.AR_AccountId = AR_AccountId;
                        int AR_SystemRoleId = context.SYS_SystemRole.Where(c => c.ROLE_Name == tempRoleName).FirstOrDefault().ROLE_Id;
                        _role.AR_SystemRoleId = AR_SystemRoleId;
                        DPBase.Add(_role);
                    }
                    return "保存成功！";
                }
	        }
	        catch (Exception ex)
	        {
		        throw new Exception(ex.Message + "转换的过程中发生了错误！");
	        }
           
        }


        ///<summary>
        ///拉取整个角色的信息
        ///</summary>
        [HttpPost]
        public List<SYS_SystemRole> GetAllRoleInfo()
        {
            List<SYS_SystemRole> _list = DPBase.Get<SYS_SystemRole>().ToList();
            return _list;
        }


        ///<summary>
        ///拉取选取的角色信息
        ///</summary>
        [HttpGet]
        public Array GetRoleChecked(string accout)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                //    var _list = context.vw_AccountRole.Where(c => c.staffid == accout).Select(o => new { ROLE_Name = o.ROLE_Name }).ToArray();
                    var _list = context.vw_AccountRole.Where(c => c.staffid == accout).Select(o =>o.ROLE_Name).ToArray();
                    //return JsonConvert.SerializeObject(_list);
                    return _list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }
    }
}
