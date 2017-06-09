using System.Collections.Generic;

namespace DataProvider.Paging
{
    public interface IPagedList
    {
        /// <summary>
        /// 当前页
        /// </summary>
        int CurrentPageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        int TotalItemCount { get; set; }
    }
}