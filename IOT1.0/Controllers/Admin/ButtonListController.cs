using DataProvider;
using DataProvider.Data;
using DataProvider.Entities;
using DataProvider.Models;
using IOT1._0.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT1._0.Controllers.Admin
{
    public partial class AdminController : Controller
    {
        //
        // GET: /ButtonList/
        /// <summary>
        /// 按钮查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult ButtonList(ButtonListSearchModel search)
        {
            ButtonListViewModel model = new ButtonListViewModel();//页面模型
            model.search = search;//页面的搜索模型
            model.search.PageSize = 15;//每页显示
            model.search.CurrentPage = Convert.ToInt32(Request["pageindex"]) <= 0 ? 1 : Convert.ToInt32(Request["pageindex"]);//当前页
            //按钮下拉项
            List<CommonEntity> ButtonIL = CommonData.GetDictionaryList(1);//1是字典类型值,仅供测试参考
            model.buttonIL = CommonData.Instance.GetBropDownListData(ButtonIL);

            model.buttonlist = ButtonData.GetButtonList(search);//填充页面模型数据
            return View(model);//返回页面模型

        }
        /// <summary>
        /// 根据按钮ID获取按钮详细信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult GetButtonByID(int id)
        {
            AjaxStatusModel ajax = new AjaxStatusModel();//功能操作类的返回类型都是AjaxStatusModel，数据放到AjaxStatusModel.data中，前台获取json后加载
            ajax.status = EnumAjaxStatus.Error;//默认失败
            ajax.msg = "获取失败！";//前台获取，用于显示提示信息
            SYSButton btn = ButtonData.GetButtonByID(id);//业务层获取底层方法，返回数据
            if (btn != null)
            {
                ajax.data = btn;//放入数据
                ajax.msg = "获取成功！";
            }
            return Json(ajax);

        }
        /// <summary>
        /// 保存编辑按钮
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public JsonResult SaveButton()
        {
            AjaxStatusModel ajax = new AjaxStatusModel();//功能操作类的返回类型都是AjaxStatusModel，数据放到AjaxStatusModel.data中，前台获取json后加载
            ajax.status = EnumAjaxStatus.Error;//默认失败
            ajax.msg = "保存失败！";//前台获取，用于显示提示信息
            var data = Request["data"];//获取前台传递的数据，主要序列化
            if (string.IsNullOrEmpty(data))
            {
                return Json(ajax);
            }
            SYSButton btn = (SYSButton)(JsonConvert.DeserializeObject(data.ToString(), typeof(SYSButton)));

            if (ButtonData.UpdateButton(btn))//注意时间类型，而且需要在前台把所有的值
            {
                ajax.msg = "保存成功！";
                ajax.status = EnumAjaxStatus.Success;
            }
            return Json(ajax);
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <returns></returns>
        public JsonResult AddButton()
        {
            AjaxStatusModel ajax = new AjaxStatusModel();//功能操作类的返回类型都是AjaxStatusModel，数据放到AjaxStatusModel.data中，前台获取json后加载
            ajax.status = EnumAjaxStatus.Error;//默认失败
            ajax.msg = "新增失败！";//前台获取，用于显示提示信息
            var data = Request["data"];//获取前台传递的数据，主要序列化
            if (string.IsNullOrEmpty(data))
            {
                return Json(ajax);
            }
            SYSButton btn = (SYSButton)(JsonConvert.DeserializeObject(data.ToString(), typeof(SYSButton)));
            btn.BTN_CreatedOn = DateTime.Now;
            btn.BTN_CreatedBy = UserSession.userid;
            if (ButtonData.AddButton(btn)>0)//注意时间类型，而且需要在前台把所有的值
            {
                ajax.msg = "新增成功！";
                ajax.status = EnumAjaxStatus.Success;
            }
            return Json(ajax);
        }
    }
}
