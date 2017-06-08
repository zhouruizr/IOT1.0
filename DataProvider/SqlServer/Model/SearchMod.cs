using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.SqlServer
{
   public class SearchMod<T>
    {

        #region 传入的参数
        /// <summary>
        /// 默认当前页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int rp { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string sortorder { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string sortname { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public System.Linq.Expressions.Expression<Func<T, bool>> where { get; set; }
        public IQueryable<dynamic> query { get; set; }
        #endregion

        #region 后台返回的参数
        /// <summary>
        /// 记录总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 当前返回数据
        /// </summary>
        public dynamic rows { get; set; }

       /// <summary>
       /// 匿名对象
       /// </summary>
        public dynamic ret { get; set; }
        #endregion

    }
}
