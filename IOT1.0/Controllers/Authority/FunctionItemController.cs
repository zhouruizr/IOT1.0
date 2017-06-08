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
    public class FunctionItemController : BaseController
    {

        #region 分配功能和功能组
        ///<summary>
        ///获取菜单按钮组信息
        ///</summary>
        [HttpPost]
        public string GetJson(JObject json)
        {
            var model = JsonToObject<vw_FunctionItem>(json);

            using (NERPEntities db = new NERPEntities())
            {
                var d = db.vw_FunctionItem.
                   OrderBy(c => c.FI_ButtonGroupOrderIndex).
                   Where(c => c.FI_TreeId == model.FI_TreeId).
                   Select(c => new { name = c.BG_Name, code = c.FI_ButtonGroupId, OrderIndex = c.FI_ButtonGroupOrderIndex, TreeId = c.FI_TreeId, FI_Id = c.FI_Id }).
                   DistinctBy(p => new { p.code }); //过滤重复字段
                return JsonConvert.SerializeObject(d);
            }
        }

        ///<summary>
        ///获取菜单按钮信息
        ///</summary>
        [HttpPost]
        public string GetButtonJson(JObject json)
        {
            var model = JsonToObject<vw_FunctionItem>(json);

            using (NERPEntities db = new NERPEntities())
            {
                var d = db.vw_FunctionItem.OrderBy(c => c.FI_Id).
                     OrderBy(c => c.FI_ButtonGroupOrderIndex).
                    Where(c => c.FI_TreeId == model.FI_TreeId && c.FI_ButtonGroupId == model.FI_ButtonGroupId).
                    Select(c => new { name = c.BTN_Name, code = c.FI_ButtonId, OrderIndex = c.FI_ButtonOrderIndex, TreeId = c.FI_TreeId, FI_Id = c.FI_Id }).ToList();
                return JsonConvert.SerializeObject(d);
            }
        }

        [HttpPost]
        public string Add(JObject json)
        {
            var model = JsonToObject<SYS_FunctionItem>(json);
            model.FI_CreatedOn = DateTime.Now;
            using (NERPEntities db = new NERPEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false;  
                db.SYS_FunctionItem.Add(model);
                if (db.SaveChanges() > 0)//成功1，失败-2
                {
                    var d = new { id = model.FI_Id, ordexindex = model.FI_ButtonOrderIndex, treeid =model.FI_TreeId, result = 1 };
                    return JsonConvert.SerializeObject(d);
                }
                else
                {
                    var d = new { result = -2 };
                    return JsonConvert.SerializeObject(d);
                }
            }
        }

        [HttpPost]
        public int Update(JObject json)
        {
            var model = JsonToObject<SYS_FunctionItem>(json);
            model.FI_CreatedOn = DateTime.Now;
            using (NERPEntities db = new NERPEntities())
            {
                db.SYS_FunctionItem.Attach(model);
                db.Entry(model).State = EntityState.Modified;
                return (db.SaveChanges() > 0) ? 1 : -2;  //成功1，失败-2
            }
        }

        [HttpPost]
        public int Delete(JObject json)
        {
            var model = JsonToObject<SYS_FunctionItem>(json);
           
            using (NERPEntities db = new NERPEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false;  
                var d = db.SYS_FunctionItem.Where(c => c.FI_TreeId == model.FI_TreeId && c.FI_ButtonGroupId == model.FI_ButtonGroupId && c.FI_ButtonId == model.FI_ButtonId).FirstOrDefault();
                db.SYS_FunctionItem.Remove(d);

                return (db.SaveChanges() > 0) ? 1 : -2;  //成功1，失败-2
            }
        }

        #endregion
        
    }
}
