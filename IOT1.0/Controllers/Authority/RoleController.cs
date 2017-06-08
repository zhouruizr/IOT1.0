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
    public class RoleController : BaseController
    {
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        [HttpPost]
        public Flexigride GetJson(JObject json)
        {
            SearchMod<SYS_SystemRole> searchModel = new SearchMod<SYS_SystemRole>();
            searchModel.page = Convert.ToInt32(json["page"].ToString());//当前页
            searchModel.rp = Convert.ToInt32(json["rp"].ToString());//页面大小
            searchModel.sortorder = json["sortorder"].ToString();//排序字段
            searchModel.sortname = json["sortname"].ToString();//排序方式

            SYS_SystemRole model = JsonToObject<SYS_SystemRole>(json);
            IQueryable<SYS_SystemRole> query = DPBase.db.SYS_SystemRole;
            query = string.IsNullOrEmpty(searchModel.sortorder) ? query.OrderByDescending(c => searchModel.sortorder) : query.OrderBy(c => searchModel.sortorder);
            if (!string.IsNullOrEmpty(model.ROLE_Name))
            {
                query = query.Where(c => c.ROLE_Name.Contains(model.ROLE_Name));
            }
            if (!string.IsNullOrEmpty(model.ROLE_Level.ToString()))
            {
                query = query.Where(c => c.ROLE_Level == model.ROLE_Level);
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
        public SYS_SystemRole Get(JObject json)
        {
            if (string.IsNullOrEmpty(json["ROLE_Level"].ToString()))
            {
                json["ROLE_Level"] = 0;
            }
            var model = JsonToObject<SYS_SystemRole>(json);
            SYS_SystemRole _model = DPBase.Get<SYS_SystemRole>(model.ROLE_Id);
            return _model;
        }

        ///<summary>
        ///验证button属性方法
        ///</summary>
        [HttpPost]
        public string ValRolAttribute(JObject json)
        {
            if (string.IsNullOrEmpty(json["ROLE_Level"].ToString()) || string.IsNullOrEmpty(json["ROLE_OrderIndex"].ToString()))
            {
                json["ROLE_Level"] = 0;
                json["ROLE_OrderIndex"] = 0;
            }
            SYS_SystemRole model = JsonToObject<SYS_SystemRole>(json);
            using (NERPEntities context = new NERPEntities())
            {
                IQueryable<SYS_SystemRole> _role = context.SYS_SystemRole.Where(c => c.ROLE_Name == model.ROLE_Name);
                if (_role.Count() > 0)
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
            SYS_SystemRole model = JsonToObject<SYS_SystemRole>(json);
            model.ROLE_CreatedBy = UserSession.userid;  //当前用户
            model.ROLE_CreatedOn = DateTime.Now;
            return DPBase.Add(model);
        }

        ///<summary>
        ///修改
        ///</summary>
        [HttpPost]
        public string Edit(JObject json)
        {
            SYS_SystemRole model = JsonToObject<SYS_SystemRole>(json);
            return DPBase.Update(model) ? "新增成功！" : "新增失败";
        }

        ///<summary>
        ///删除
        ///</summary>
        [HttpPost]
        public string Delete(JObject json)
        {
            var model = JsonToObject<SYS_SystemRole>(json);
            return DPBase.Delete<SYS_SystemRole>(model.ROLE_Id) ? "删除成功！" : "删除失败";
        }


        ///<summary>
        ///获取一级的菜单信息
        ///</summary>
        [HttpPost]
        public List<Array> GetMenuItem0()
        {
            try
            {
                List<Array> ret = new List<Array>();
                IQueryable<SYS_FunctionTree> query = DPBase.Get<SYS_FunctionTree>();
                Array _array0 = query.Where(c => c.FT_ParentId == 0).ToArray();
                Array _array1 = query.Where(c => c.FT_ParentId == 1).ToArray();
                ret.Add(_array0);
                ret.Add(_array1);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }

        ///<summary>
        ///获取二级的菜单信息
        ///</summary>
        [HttpGet]
        public Array GetMenuItem1(string FT_Id)
        {
            try
            {
                int _id = Convert.ToInt32(FT_Id);
                IQueryable<SYS_FunctionTree> query = DPBase.Get<SYS_FunctionTree>().Where(c => c.FT_ParentId == _id);
                Array _array = query.ToArray();
                return _array;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }


        /// <summary>
        ///保存现在已有的权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void SaveAlltheRights()
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转化的过程中发生了错误!");
            }

        }
    }
}
