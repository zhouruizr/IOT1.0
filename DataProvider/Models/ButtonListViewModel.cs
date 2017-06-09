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
   public class ButtonListViewModel
    {
       /// <summary>
       /// 页面查询模型
       /// </summary>
       public ButtonListSearchModel search = new ButtonListSearchModel();
       /// <summary>
       /// 页面的列表数据
       /// </summary>
       public PagedList<SYSButton> buttonlist { get; set; }
       /// <summary>
       /// 按钮下拉框，演示用
       /// </summary>
       public List<SelectListItem> buttonIL { get; set; } 
    }
    /// <summary>
    /// 按钮查询模型对象
    /// </summary>
   public class ButtonListSearchModel : CommonPageEntity
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string BTN_Name{set;get;}
        /// <summary>
        /// 按钮英文名称
        /// </summary>
        public string BTN_Name_En { set; get; }
       /// <summary>
       /// 下拉框按钮的选中值
       /// </summary>
        public int DicItemID { set; get; }
    }
}
