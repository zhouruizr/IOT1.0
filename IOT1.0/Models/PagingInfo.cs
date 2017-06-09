using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IOT1._0.Models
{
    public class PagingInfo<T>
    {
        public List<T> DataToRet { get; set; }

        public List<T> data { get; set; }

        public Object ArrData { get; set; }

        public int AllPages { get; set; }

        public int AllRows { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}