using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOT1._0.Models
{
    public class Flexigride
    {   //总行数
        public int total { get; set; }
        //总页数
        public int page { get; set; }
        //当前返回数据
        public dynamic rows { get; set; }
        ////当前页
        //public int pageIndex { get; set; }
        ////每页数据行
        //public int pageSize { get; set; }
    }

    public class FlexigridParam
    {
        public int page { get; set; }
        public int rp { get; set; }
        public string sortname { get; set; }
        public string sortorder { get; set; }
        public dynamic query { get; set; }
        public string qtype { get; set; }
    }
}