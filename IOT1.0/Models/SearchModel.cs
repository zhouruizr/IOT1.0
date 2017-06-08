using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOT1._0.Models
{
    public class SearchModel
    {
        public int allRows { get; set; }
        public int pageIndex { get; set;}
        public int allPages { get; set; }
        /// <summary>
        /// 泛型对象
        /// </summary>
        public dynamic T_OBJ { get; set; }
    }
}