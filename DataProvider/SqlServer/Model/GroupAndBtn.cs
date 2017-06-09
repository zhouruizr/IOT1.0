using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.SqlServer
{
    public class GroupAndBtn
    {
        /// <summary>
        /// 所属的菜单ID
        /// </summary>
        public int FI_TreeId { get; set; }


        /// <summary>
        /// 按钮组的ID
        /// </summary>
        public int FI_ButtonGroupId { get; set; }

        /// <summary>
        /// 按钮组的名称
        /// </summary>
        public string BG_Name { get; set; }


        /// <summary>
        /// 子类按钮字段的信息
        /// </summary>
        private List<vw_FunctionItem> sys_Buttons;


        /// <summary>
        /// 子类按钮的属性信息
        /// </summary>
        public List<vw_FunctionItem> SYS_Button
        {
            get 
            {
                if (sys_Buttons == null)
                {
                    return new List<vw_FunctionItem>();
                }
                else
                {
                    return sys_Buttons;
                }
            }
            set 
            {
                sys_Buttons = value ;
            }
        }
    }
}
