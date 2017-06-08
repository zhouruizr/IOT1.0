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
    public class ButtonController : BaseController
    {
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        [HttpPost]
        public Flexigride GetJson(JObject json)
        {
            SearchMod<SYS_Button> searchModel = new SearchMod<SYS_Button>();
            searchModel.page = Convert.ToInt32(json["page"].ToString());//当前页
            searchModel.rp = Convert.ToInt32(json["rp"].ToString());//页面大小
            searchModel.sortorder = json["sortorder"].ToString();//排序字段
            searchModel.sortname = json["sortname"].ToString();//排序方式

            SYS_Button model = JsonToObject<SYS_Button>(json);
            IQueryable<SYS_Button> query = DPBase.db.SYS_Button;
            query = string.IsNullOrEmpty(searchModel.sortorder) ? query.OrderByDescending(c => searchModel.sortorder) : query.OrderBy(c => searchModel.sortorder);
            if (!string.IsNullOrEmpty(model.BTN_Name))
            {
                query = query.Where(c => c.BTN_Name.Contains(model.BTN_Name));
            }
            if (!string.IsNullOrEmpty(model.BTN_Name_En))
            {
                query = query.Where(c => c.BTN_Name_En.Contains(model.BTN_Name_En));
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
        public SYS_Button Get(JObject json)
        {
            var model = JsonToObject<SYS_Button>(json);
            SYS_Button _model = DPBase.Get<SYS_Button>(model.BTN_Id);
            return _model;
        }

        ///<summary>
        ///验证button属性方法
        ///</summary>
        [HttpPost]
        public string ValBtnAttribute(JObject json)
        {
            if (string.IsNullOrEmpty(json["BTN_OrderIndex"].ToString()))
            {
                json["BTN_OrderIndex"] = 0;
            }
            SYS_Button model = JsonToObject<SYS_Button>(json);
            return DPButton.DPValBtnAttribute(model);
        }

        ///<summary>
        ///新增
        ///</summary>
        ///<param name="userName">用户名</param>
        [HttpPost]
        public int Add(JObject json)
        {
            SYS_Button model = JsonToObject<SYS_Button>(json);
            model.BTN_IsSuspended = false;   //默认为false
            model.BTN_Mark = model.BTN_Name_En;         
            model.BTN_CreatedBy = UserSession.username;  //当前用户
            model.BTN_CreatedOn = DateTime.Now;
            return DPBase.Add(model);
        }

        ///<summary>
        ///修改
        ///</summary>
        [HttpPost]
        public string Edit(JObject json)
        {
            SYS_Button model = JsonToObject<SYS_Button>(json);
            return DPBase.Update(model) ? "新增成功！" : "新增失败";
        }

        ///<summary>
        ///删除
        ///</summary>
        [HttpPost]
        public string Delete(JObject json)
        {
            var model = JsonToObject<SYS_Button>(json);
            return DPBase.Delete<SYS_Button>(model.BTN_Id) ? "删除成功！" : "删除失败"; 
        }
        
        /// <summary>
        ///获得所有按钮信息--供combox使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetComboxJson()
        {
            using (NERPEntities db = new NERPEntities())
            {
                var d = db.SYS_Button.OrderBy(c => c.BTN_OrderIndex).Select(c => new { name = c.BTN_Name, code = c.BTN_Id }).ToList();
                return JsonConvert.SerializeObject(d);
            }
        }

        /// <summary>
        ///获得所有按钮信息--供表格使用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Array> GetBtnForTB(string roleId)
        {
            using (NERPEntities db = new NERPEntities())
            {
                //菜单ID
                int MenuID = Convert.ToInt32(roleId);

                //获取全部按钮列表
                var arrayBtn = db.SYS_Button.OrderBy(c => c.BTN_OrderIndex).Select(c => new { BTN_Name = c.BTN_Name, BTN_Id = c.BTN_Id,
                  BTN_OrderIndex = c.BTN_OrderIndex  }).OrderBy(c=>c.BTN_Id).ToArray();
               
                //按钮组界面结构

                List<dynamic> list = db.vw_FunctionItem
                    .GroupBy(c => new { c.BG_Name, c.FI_ButtonGroupId, c.FI_ButtonGroupOrderIndex })
                    .Select(c => new
                    {
                        BG_Name = c.Key.BG_Name,
                        FI_ButtonGroupId = c.Key.FI_ButtonGroupId,
                        FI_ButtonGroupOrderIndex = c.Key.FI_ButtonGroupOrderIndex,
                    }).ToList<dynamic>();//按钮组
                IQueryable<vw_FunctionItem> querylistGroup = db.vw_FunctionItem.Where(c => c.FI_TreeId == MenuID).OrderBy(c => c.FI_ButtonId);
                List<GroupAndBtn> Groups = new List<GroupAndBtn>();
                foreach (var item in list)
                {
                    GroupAndBtn group = new GroupAndBtn();
                    group.FI_TreeId = MenuID;
                    group.BG_Name = item.BG_Name;
                    group.FI_ButtonGroupId = item.FI_ButtonGroupId;
                    List<vw_FunctionItem> listGroup = querylistGroup.Where(c => c.FI_ButtonGroupId == group.FI_ButtonGroupId).ToList();
                    group.SYS_Button = listGroup;
                    Groups.Add(group);
                }
                Array GpAndBtn = Groups.ToArray();
                List<Array> arryList = new List<Array>();
                arryList.Add(arrayBtn);
                arryList.Add(GpAndBtn);
                return arryList;
            }
        }


        /// <summary>
        ///保存现在已有的权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public static void SaveAlltheRights()
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
