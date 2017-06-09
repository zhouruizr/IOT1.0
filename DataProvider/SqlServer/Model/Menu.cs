using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSH.DataProvider.SqlServer.Model
{
    public class Menu
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 菜单状态
        /// </summary>
        public bool IsSuspended { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// 父级菜单id
        /// </summary>
        public int ParentId { get; set; }

        private IList<Menu> childs;

        /// <summary>
        /// 子级
        /// </summary>
        public IList<Menu> Childs
        {
            get
            {
                if (childs == null)
                    childs = new List<Menu>();
                return childs;
            }
            set { childs = value; }
        }


    }
}
