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
    public class MenuController : BaseController
    {
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        [HttpPost]
        public Flexigride GetJson(JObject json)
        {
            SearchMod<SYS_FunctionTree> searchModel = new SearchMod<SYS_FunctionTree>();
            searchModel.page = Convert.ToInt32(json["page"].ToString());//当前页
            searchModel.rp = Convert.ToInt32(json["rp"].ToString());//页面大小
            searchModel.sortorder = json["sortorder"].ToString();//排序字段
            searchModel.sortname = json["sortname"].ToString();//排序方式

            var model = JsonToObject<SYS_FunctionTree>(json);
            IQueryable<SYS_FunctionTree> query = DPBase.db.SYS_FunctionTree;
            query = string.IsNullOrEmpty(searchModel.sortorder) ? query.OrderByDescending(c => searchModel.sortorder) : query.OrderBy(c => searchModel.sortorder);
            if (!string.IsNullOrEmpty(model.FT_Name))
            {
                query = query.Where(c => c.FT_Name.Contains(model.FT_Name));
            }
         
            searchModel.query = query;
            Flexigride grid = new Flexigride();
            grid.rows = DPBase.DPGetQueryLst(searchModel, out searchModel);
            grid.page = searchModel.page;
            grid.total = searchModel.total;
            return grid;
        }

        ///<summary>
        ///获取单个实体的信息
        ///</summary>
        [HttpPost]
        public SYS_FunctionTree Get(JObject json)
        {
            SetDefaultValue(json);
            var model = JsonToObject<SYS_FunctionTree>(json);
            var _model = DPBase.Get<SYS_FunctionTree>(model.FT_Id);
            return _model;
        }

        ///<summary>
        ///验证属性方法
        ///</summary>
        [HttpPost]
        public string ValAttribute(JObject json)
        {

            SetDefaultValue(json);
            var model =(SYS_FunctionTree)(JsonConvert.DeserializeObject(json.ToString(), typeof(SYS_FunctionTree)));
            return DPButton.DPValMenuAttribute(model);
        }
        

        ///<summary>
        ///新增
        ///</summary>
        ///<param name="userName">用户名</param>
        [HttpPost]
        public string Add(JObject json)
        {
            SetDefaultValue(json);
            var model = JsonToObject<SYS_FunctionTree>(json);
            //model.FT_IsSuspended = false;   //默认为false
            model.FT_CreatedBy = UserSession.username;  //当前用户
            model.FT_CreatedOn = DateTime.Now;
            using (NERPEntities db = new NERPEntities())
            {
                db.SYS_FunctionTree.Add(model);
                return (db.SaveChanges()>0)? "新增成功！" : "新增失败";
            }
        }

        ///<summary>
        ///修改
        ///</summary>
        [HttpPost]
        public string Edit(JObject json)
        {
            SetDefaultValue(json);
            SYS_FunctionTree model = JsonToObject<SYS_FunctionTree>(json);
            return DPBase.Update(model) ? "修改成功！" : "修改失败";
        }

        ///<summary>
        ///删除
        ///</summary>
        [HttpPost]
        public string Delete(JObject json)
        {
            SetDefaultValue(json);
            var model = JsonToObject<SYS_FunctionTree>(json);
            return DPBase.Delete<SYS_FunctionTree>(model.FT_Id) ? "删除成功！" : "删除失败";
        }

        /// <summary>
        /// 设置模型默认值
        /// </summary>
        /// <param name="json"></param>
        private static void SetDefaultValue(JObject json)
        {
            if (string.IsNullOrEmpty(json["FT_OrderIndex"].ToString()))
            {
                json["FT_OrderIndex"] = 0;
            }
            if (string.IsNullOrEmpty(json["FT_ParentId"].ToString()))
            {
                json["FT_ParentId"] = null;
            }
        }

       
    }
}

