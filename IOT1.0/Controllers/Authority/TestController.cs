//using CCSH.DataProvider;
//using DataProvider;
//using DataProvider.SqlServer;
//using IOT1._0.Models;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web.Http;


//namespace IOT1._0.Controllers
//{
//    public class TestController : BaseController
//    {
//        /// <summary>
//        /// 查询
//        /// </summary>
//        [HttpPost]
//        public Flexigride GetButtonGroup(JObject json)
//        {
//            SearchMod<SYS_Button> searchModel = new SearchMod<SYS_Button>();
//            searchModel.page = Convert.ToInt32(json["page"].ToString());//当前页
//            searchModel.rp = Convert.ToInt32(json["rp"].ToString());//页面大小
//            searchModel.sortorder = json["sortorder"].ToString();//排序字段
//            searchModel.sortname = json["sortname"].ToString();//排序方式
//            searchModel.sortname = "BG_Id";
//            SYS_Button model = (SYS_Button)(JsonConvert.DeserializeObject(json.ToString(), typeof(SYS_Button)));
//            IQueryable<SYS_Button> query = DPBase.db.SYS_ButtonGroup;
//            query = string.IsNullOrEmpty(searchModel.sortorder) ? query.OrderByDescending(c => searchModel.sortname) : query.OrderBy(c => searchModel.sortname);
//            if (!string.IsNullOrEmpty(model.BG_Name))
//            {
//                query = query.Where(c => c.BG_Name == model.BG_Name);
//            }
//            if (!string.IsNullOrEmpty(model.BG_Name_En))
//            {
//                query = query.Where(c => c.BG_Name_En == model.BG_Name_En);
//            }
//            //aa.query = query.Select(c => new { BG_Name = c.BG_Name, BG_Name_En = c.BG_Name_En, BG_OrderIndex = c.BG_OrderIndex, BG_Desc = c.BG_Desc,BG_Id = c.BG_Id });
//            searchModel.query = query;
//            Flexigride grid = new Flexigride();
//            grid.rows = DPBase.DPGetQueryLst(searchModel,out searchModel);
//            grid.page = searchModel.page;
//            grid.total = searchModel.total;
//            return grid;
//        }

//        ///<summary>
//        ///获取单个实体的信息
//        ///</summary>
//        [HttpPost]
//        public SYS_Button GetBtnInformation(JObject json)
//        {
//            SYS_Button model = JsonToObject<SYS_Button>(json);
//            SYS_Button _model = DPBase.Get<SYS_Button>(3);
//            return _model;
//        }

//        ///<summary>
//        ///新增
//        ///</summary>
//        ///<param name="userName">用户名</param>
//        [HttpPost]
//        public string AddNewButtonGroup(JObject json)
//        {
//            SYS_Button model = JsonToObject<SYS_Button>(json);
//            DPBase.Add(model);
//            return "保存成功！";
//        }

//        ///<summary>
//        ///修改
//        ///</summary>
//        [HttpPost]
//        public string EditButtonGroup(JObject json)
//        {
//            SYS_Button model = JsonToObject<SYS_Button>(json);
//            DPBase.Update(model);
//            return "保存成功！";
//        }

//        ///<summary>
//        ///删除
//        ///</summary>
//        [HttpPost]
//        public string DeleteButtonGroup(JObject json)
//        {
//            SYS_Button model = JsonToObject<SYS_Button>(json);
//            DPBase.Delete(model);
//            return "删除成功！";
//        }

//        ///<summary>
//        ///验证属性方法
//        ///</summary>
//        //[HttpPost]
//        //public string ValBtnAttribute(JObject json)
//        //{
            
//        //    SYS_ButtonGroup model = JsonToObject<SYS_ButtonGroup>(json);
//        //    Expression<Func<SYS_ButtonGroup, bool>> aa = (c => c.BG_Name == model.BG_Name);
//        //    if(DPBase.GetSingle<SYS_ButtonGroup>(aa) == )
//        //    if (_valbGroup.Count() != 0)
//        //    {
//        //        flag = "按钮组名称重复请从新输入！";
//        //    }
//        //    else
//        //    {
//        //        _valbGroup = context.SYS_ButtonGroup.Where(c => c.BG_Name_En == bGroup.BG_Name_En);
//        //        if (_valbGroup.Count() != 0)
//        //        {
//        //            flag = "按钮组编号重复请从新输入！";
//        //        }
//        //        else
//        //        {
//        //            _valbGroup = context.SYS_ButtonGroup.Where(c => c.BG_OrderIndex == bGroup.BG_OrderIndex);
//        //            if (_valbGroup.Count() != 0)
//        //            {
//        //                flag = "按钮组序号重复请从新输入！";
//        //            }
//        //        }
//        //    }
//        //    return flag;
//        //    return DPButton.DPValBtnAttribute(model);
//        //    //var aa = DPBase.db.SYS_ButtonGroup.Where(c => c.BG_Name == model.BG_Name);
//        //    SYS_ButtonGroup sdf = DPBase.GetSingle<SYS_ButtonGroup>(aa);
//        //}



//    }
//}
