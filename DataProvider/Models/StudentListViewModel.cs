using DataProvider.Entities;
using DataProvider.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace DataProvider.Models
{
    public class StudentListViewModel
    {

        /// <summary>
        /// 页面查询模型
        /// </summary>
        public StudentListSearchModel search = new StudentListSearchModel();
        /// <summary>
        /// 页面的列表数据tudents
        /// </summary>
        public PagedList<vw_Students> Studentlist { get; set; }

        /// <summary>
        /// 按钮下拉框，演示用
        /// </summary>
        public List<SelectListItem> SourceIL { get; set; } 

    }

    /// <summary>
    /// 按钮查询模型对象
    /// </summary>
    public class StudentListSearchModel : CommonPageEntity
    {
        /// <summary>
        ///姓名
        /// </summary>
        public string Name { set; get; }

        
        /// <summary>
        /// 创建时间开始
        /// </summary>
        public DateTime? CreateTime_start
        { set; get; }
         

       
        /// <summary>
        /// 创建时间结束
        /// </summary>
        public DateTime? CreateTime_end
        { set; get; }


        /// <summary>
        /// 下拉框按钮的选中值
        /// </summary>
        public int DicItemID { set; get; }


    }

}



