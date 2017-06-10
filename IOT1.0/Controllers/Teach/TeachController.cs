using DataProvider;
using DataProvider.Data;
using DataProvider.Entities;
using DataProvider.Models;
using IOT1._0.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT1._0.Controllers.Teach
{
    public class TeachController : Controller
    {



        //
        // GET: /ButtonList/
        /// <summary>
        /// 按钮查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult Student(StudentListSearchModel search)
        {

            StudentListViewModel model = new StudentListViewModel();//页面模型
            model.search = search;//页面的搜索模型
            model.search.PageSize = 15;//每页显示
            model.search.CurrentPage = Convert.ToInt32(Request["pageindex"]) <= 0 ? 1 : Convert.ToInt32(Request["pageindex"]);//当前页

            //按钮下拉项
            List<CommonEntity> SourceIL = CommonData.GetDictionaryList(2);//1是字典类型值,仅供测试参考
            model.SourceIL = CommonData.Instance.GetBropDownListData(SourceIL);

            model.Studentlist = StudentData.GetButtonList(search);//填充页面模型数据
             return View(model);//返回页面模型


        }
        /// <summary>
        /// 根据按钮ID获取按钮详细信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult GetStudentByID(string id)
        {
            AjaxStatusModel ajax = new AjaxStatusModel();//功能操作类的返回类型都是AjaxStatusModel，数据放到AjaxStatusModel.data中，前台获取json后加载
            ajax.status = EnumAjaxStatus.Error;//默认失败
            ajax.msg = "获取失败！";//前台获取，用于显示提示信息
            Students btn = StudentData.GetStudentsByID(id);//业务层获取底层方法，返回数据
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
        public JsonResult SaveStudent()
        {
            AjaxStatusModel ajax = new AjaxStatusModel();//功能操作类的返回类型都是AjaxStatusModel，数据放到AjaxStatusModel.data中，前台获取json后加载
            ajax.status = EnumAjaxStatus.Error;//默认失败
            ajax.msg = "保存失败！";//前台获取，用于显示提示信息
            var data = Request["data"];//获取前台传递的数据，主要序列化
            if (string.IsNullOrEmpty(data))
            {
                return Json(ajax);
            }
            Students Stu = (Students)(JsonConvert.DeserializeObject(data.ToString(), typeof(Students)));

            //判断手机号码是否唯一

            int BindPhone_count = StudentData.BindPhone_update(Stu.ID, Stu.BindPhone);
            if (BindPhone_count > 0)
            {
                ajax.msg = "手机号码已存在，不能重复使用！";
                return Json(ajax);
            }

             
          


            if (StudentData.UpdateStudent(Stu))//注意时间类型，而且需要在前台把所有的值
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
        public JsonResult AddStudent()
        {
            AjaxStatusModel ajax = new AjaxStatusModel();//功能操作类的返回类型都是AjaxStatusModel，数据放到AjaxStatusModel.data中，前台获取json后加载
            ajax.status = EnumAjaxStatus.Error;//默认失败
            ajax.msg = "新增失败！";//前台获取，用于显示提示信息
            var data = Request["data"];//获取前台传递的数据，主要序列化
            if (string.IsNullOrEmpty(data))
            {
                return Json(ajax);
            }
            Students Stu = (Students)(JsonConvert.DeserializeObject(data.ToString(), typeof(Students)));

            //判断手机号码是否唯一


            int BindPhone_count = StudentData.BindPhone_insert(Stu.BindPhone);
            if (BindPhone_count > 0)
            {
                ajax.msg = "手机号码已存在，不能重复使用！";
                return Json(ajax);
            }
               
            Stu.CreateTime = DateTime.Now;
            Stu.CreatorId = UserSession.userid;

            Students query = StudentData.GetStudentsList();//获取创建时间最新的一条数据
            string MAX_ID;
            if (query !=null)
            {
                  MAX_ID = query.ID;
            }
            else
            {
                MAX_ID=null;
            }
           
            var year = Stu.Birthday.ToString().Substring(2, 2);

            if (!string.IsNullOrEmpty(MAX_ID))
            {
                Stu.ID = year + (Convert.ToInt32(MAX_ID.Substring(2)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                Stu.ID = year + "0001";
            }

              
            if (StudentData.AddStudent(Stu)!="")//注意时间类型，而且需要在前台把所有的值
            {
                ajax.msg = "新增成功！";
                ajax.status = EnumAjaxStatus.Success;
            }
            return Json(ajax);
        }

 


 

        public ActionResult Curriculum()
        {
            return View();
        }

        public ActionResult Shift()
        {
            return View();
        }
        public ActionResult Teacher()
        {
            return View();
        }
         
    }
}
 
  
 

