using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DataProvider.Paging
{
    [DataContract(Name="")]
    public class PagedList<T> : List<T>, IPagedList
    {
        /// <summary>
        /// 数据分页,适用于所有数据已查询
        /// </summary>
        /// <param name="items">所有数据</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            CurrentPageIndex = pageIndex;
            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Add(items[i]);
            }
        }

        /// <summary>
        /// 数据分页,适用于需要计算总记录数
        /// </summary>
        /// <param name="items">当前页数据</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalItemCount">总记录数</param>
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(items);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalItemCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }
        /// <summary>
        /// 数据开始索引
        /// </summary>
        public int StartRecordIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }
        /// <summary>
        /// 数据结束索引
        /// </summary>
        public int EndRecordIndex { get { return TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount; } }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage { get { return (CurrentPageIndex > 1); } }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage { get { return (CurrentPageIndex < TotalPageCount); } }
    }
}