using CCSH.DataProvider;
using DataProvider;
using DataProvider.SqlServer;
using IOT1._0.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace IOT1._0.Controllers
{
    public class DataDicController : BaseController
    {
        #region 数据字典类别
        /// <summary>
        /// 获得所有数据字典分类
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        [HttpPost]
        public string GetCategoryJson()
        {
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_DicCategory.OrderBy(c => c.Sortnum).ToList();
                return JsonConvert.SerializeObject(d);
            }
        }
        [HttpPost]
        public string CategoryAdd(JObject json)
        {
            var model = JsonToObject<SYS_DicCategory>(json);
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_DicCategory.Select(c => c.DicCategory_Code).ToList().LastOrDefault();//查询id最大值 
                model.DicCategory_Code = (int.Parse(d) + 1).ToString();//新的id为最大值id++
                db.SYS_DicCategory.Add(model);
                return (db.SaveChanges() > 0) ? "新增成功！" : "新增失败";
            }
        }
        [HttpPost]
        public string CategoryUpdate(JObject json)
        {
            var model = JsonToObject<SYS_DicCategory>(json);
            using (NERPEntities db = new NERPEntities())
            {
                db.SYS_DicCategory.Attach(model);
                db.Entry(model).State = EntityState.Modified;
                return (db.SaveChanges() > 0) ? "修改成功！" : "修改失败";
            }
        }
        [HttpPost]
        public string CategoryDelete(JObject json)
        {
            var model = JsonToObject<SYS_DicCategory>(json);
            using (NERPEntities db = new NERPEntities())
            {
                var count = db.SYS_Dics.Count(c => c.CategoryId.ToString() == model.DicCategory_Code);
                if (count > 0)
                {
                    return "删除失败，因为该分类下有数据字典，请先删除数据字典！";
                }
                var d = db.SYS_DicCategory.Where(c => c.DicCategory_Code == model.DicCategory_Code).FirstOrDefault();
                db.SYS_DicCategory.Remove(d);

                return (db.SaveChanges() > 0) ? "删除成功！" : "删除失败";
            }
        }
        [HttpPost]
        public SYS_DicCategory CategoryGet(JObject json)
        {
            var model = JsonToObject<SYS_DicCategory>(json);
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_DicCategory.Where(c => c.DicCategory_Code == model.DicCategory_Code).ToList().FirstOrDefault();
                return d;
            }
        }

        ///<summary>
        ///验证属性方法
        ///</summary>
        [HttpPost]
        public string CategoryValAttribute(JObject json)
        {
            var model = JsonToObject<SYS_DicCategory>(json);
            return DPButton.DPValDicCategoryAttribute(model);
        }
        #endregion

        #region 数据字典
        /// <summary>
        /// 数据字典查询
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Flexigride GetJson(JObject json)
        {
            SearchMod<SYS_Dics> searchModel = new SearchMod<SYS_Dics>();
            searchModel.page = Convert.ToInt32(json["page"].ToString());//当前页
            searchModel.rp = Convert.ToInt32(json["rp"].ToString());//页面大小
            searchModel.sortorder = json["sortorder"].ToString();//排序字段
            searchModel.sortname = json["sortname"].ToString();//排序方式


            var model = JsonToObject<SYS_Dics>(json);
            IQueryable<SYS_Dics> query = DPBase.db.SYS_Dics;
            //query = string.IsNullOrEmpty(searchModel.sortorder) ? query.OrderByDescending(c => searchModel.sortorder) : query.OrderBy(c => searchModel.sortorder);
            query = query.OrderBy(c => c.Sortnum);
            if (model.CategoryId != 0)
            {
                query = query.Where(c => c.CategoryId == model.CategoryId);
            }

            searchModel.query = query;
            Flexigride grid = new Flexigride();
            grid.rows = DPBase.DPGetQueryLst(searchModel, out searchModel);
            grid.page = searchModel.page;
            grid.total = searchModel.total;
            return grid;
        }
        [HttpPost]
        public SYS_Dics Get(JObject json)
        {
            var model = JsonToObject<SYS_Dics>(json);
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_Dics.Where(c => c.Dics_Code == model.Dics_Code).ToList().FirstOrDefault();
                return d;
            }
        }
        [HttpPost]
        public string Add(JObject json)
        {
            var model = JsonToObject<SYS_Dics>(json);
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_Dics.Select(c => c.Dics_Code).ToList().LastOrDefault();//查询id最大值 
                model.Dics_Code = (int.Parse(d) + 1).ToString();//新的id为最大值id++
                db.SYS_Dics.Add(model);
                return (db.SaveChanges() > 0) ? "新增成功！" : "新增失败";
            }
        }
        [HttpPost]
        public string Update(JObject json)
        {
            var model = JsonToObject<SYS_Dics>(json);
            using (NERPEntities db = new NERPEntities())
            {
                db.SYS_Dics.Attach(model);
                db.Entry(model).State = EntityState.Modified;
                return (db.SaveChanges() > 0) ? "修改成功！" : "修改失败";
            }
        }
        [HttpPost]
        public string Delete(JObject json)
        {
            var model = JsonToObject<SYS_Dics>(json);
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_Dics.Where(c => c.Dics_Code == model.Dics_Code).FirstOrDefault();
                db.SYS_Dics.Remove(d);

                return (db.SaveChanges() > 0) ? "删除成功！" : "删除失败";
            }
        }

        ///<summary>
        ///验证属性方法
        ///</summary>
        [HttpPost]
        public string ValAttribute(JObject json)
        {
            var model = JsonToObject<SYS_Dics>(json);
            return DPButton.DPValDicAttribute(model);
        }
        #endregion

    }
}
