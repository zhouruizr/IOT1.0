using CCSH.DataProvider;
using DataProvider;
using DataProvider.SqlServer;
using IOT1._0.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace IOT1._0.Controllers
{
    public class ButtonGroupController : BaseController
    {
        /// <summary>
        /// 查询按钮组
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        [HttpPost]
        public Flexigride GetButtonGroup(JObject json)
        {
            SearchMod<SYS_ButtonGroup> searchModel = new SearchMod<SYS_ButtonGroup>();
            searchModel.page = Convert.ToInt32(json["page"].ToString());
            searchModel.rp = Convert.ToInt32(json["rp"].ToString());

            SYS_ButtonGroup bGroup = (SYS_ButtonGroup)(JsonConvert.DeserializeObject(json.ToString(), typeof(SYS_ButtonGroup)));
            IQueryable<SYS_ButtonGroup> query = DPBase.db.SYS_ButtonGroup;
            query = string.IsNullOrEmpty(searchModel.sortorder) ? query.OrderByDescending(c => searchModel.sortname) : query.OrderBy(c => searchModel.sortname);
            if (!string.IsNullOrEmpty(bGroup.BG_Name))
            {
                query = query.Where(c => c.BG_Name.Contains(bGroup.BG_Name));
            }
            if (!string.IsNullOrEmpty(bGroup.BG_Name_En))
            {
                query = query.Where(c => c.BG_Name_En.Contains(bGroup.BG_Name_En));
            }
            //aa.query = query.Select(c => new { BG_Name = c.BG_Name, BG_Name_En = c.BG_Name_En, BG_OrderIndex = c.BG_OrderIndex, BG_Desc = c.BG_Desc,BG_Id = c.BG_Id });
            searchModel.query = query.OrderBy(c => c.BG_CreatedOn);
            Flexigride grid = new Flexigride();
            grid.rows = DPBase.DPGetQueryLst(searchModel, out searchModel);
            grid.page = searchModel.page;
            grid.total = searchModel.total;
            return grid;
        }

        /////<summary>
        /////获取单个实体
        /////</summary>
        [HttpPost]
        public JObject GetBtnInformation(JObject json)
        {
            //SYS_ButtonGroup bGroup = (SYS_ButtonGroup)(JsonConvert.DeserializeObject(json.ToString(), typeof(SYS_ButtonGroup)));
            int BG_Id = Convert.ToInt32(json["BG_Id"].ToString());
            SYS_ButtonGroup _btnGroup = DPBase.Get<SYS_ButtonGroup>(BG_Id);
            JObject ret = new JObject();
            ret.Add("group", JObject.FromObject(_btnGroup));
            return ret;
        }

        ///<summary>
        ///验证button属性方法
        ///</summary>
        [HttpPost]
        public string ValBtnAttribute(JObject json)
        {
            if (string.IsNullOrEmpty(json["BG_OrderIndex"].ToString()))
            {
                json["BG_OrderIndex"] = 0;
            }
            SYS_ButtonGroup bGroup = (SYS_ButtonGroup)(JsonConvert.DeserializeObject(json.ToString(), typeof(SYS_ButtonGroup)));
            return DPButton.DPValBtnGroupAttribute(bGroup);
        }


        ///<summary>
        ///新增
        ///</summary>
        ///<param name="userName">用户名</param>
        [HttpPost]
        public string AddNewButtonGroup(JObject json)
        {
            json["BG_OrderIndex"] = 0;
            SYS_ButtonGroup bGroup = (SYS_ButtonGroup)(JsonConvert.DeserializeObject(json.ToString(), typeof(SYS_ButtonGroup)));
            bGroup.BG_CreatedOn = DateTime.Now;
            bGroup.BG_IsSuspended = false;
            DPBase.Add(bGroup);
            return "保存成功！";
        }


        ///<summary>
        ///删除
        ///</summary>
        ///<param name="userName">用户名</param>
        [HttpPost]
        public string DelBtnInformation(JObject json)
        {
            int BG_Id = Convert.ToInt32(json["BG_Id"].ToString());
            DPBase.Delete<SYS_ButtonGroup>(BG_Id);
            return "删除成功！";
        }

        ///<summary>
        ///修改
        ///</summary>
        [HttpPost]
        public string EditButtonGroup(JObject json)
        {
            SYS_ButtonGroup bGroup = (SYS_ButtonGroup)(JsonConvert.DeserializeObject(json.ToString(), typeof(SYS_ButtonGroup)));
            DPBase.Update(bGroup);
            return "保存成功！";
        }

        /// <summary>
        ///获得所有按钮组信息,供给combox使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetComboxJson()
        {
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_ButtonGroup.OrderBy(c => c.BG_OrderIndex).Select(c => new { name = c.BG_Name, code = c.BG_Id, OrderIndex = 0 }).ToList();
                return JsonConvert.SerializeObject(d);
            }
        }


        public static Expression<Func<T, bool>> GetConditionExpression<T>(string[] options, string fieldName)
        {
            ParameterExpression left = Expression.Parameter(typeof(T), "c");//c=>
            Expression expression = Expression.Constant(false);
            foreach (var optionName in options)
            {
                Expression right = Expression.Call
                       (
                          Expression.Property(left, typeof(T).GetProperty(fieldName)),  //c.DataSourceName
                          typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),// 反射使用.Contains()方法                         
                          Expression.Constant(optionName)           // .Contains(optionName)
                       );
                expression = Expression.Or(right, expression);//c.DataSourceName.contain("") || c.DataSourceName.contain("") 
            }
            Expression<Func<T, bool>> finalExpression
                = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[] { left });
            return finalExpression;
        }
    }
}