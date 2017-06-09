using Dapper;
using DataProvider.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataProvider
{
    public static class Common
    {

        //使用方法如下（针对ID，和Name进行Distinct）：
        //var query = people.DistinctBy(p => new { p.Id, p.Name });
        //若仅仅针对ID进行distinct：
        //var query = people.DistinctBy(p => p.Id);

        /// <summary>
        /// 扩展DistinctBy
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

    public class CommonEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
    }

    public class CommonPageEntity
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每页显示几条记录
        /// </summary>
        public int PageSize { get; set; }
    }

    public class CommonPage<T>
    {
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="allcount">总数</param>
        /// <param name="tablename">表名,支持多表联查</param>
        /// <param name="fields">字段名</param>
        /// <param name="where">where条件 不需要加where</param>
        /// <param name="orderby">*排序条件不能为空，不需要加order by</param>
        /// <param name="getallcount">获取记录总数,true是，false否</param>
        /// <param name="pageindex">当前页,从1开始,默认1</param>
        /// <param name="pagesize">每页显示多少条数据，默认15</param>
        /// <param name="commandTimeout">执行命令超时时间,默认30秒</param>
        /// <returns></returns>
        public static List<T> GetPageList(out int allcount, string tablename, string fields = "*", string where = "", string orderby = "",
            bool getallcount = true, int pageindex = 1, int pagesize = 15, string connect = "", int commandTimeout = 30)
        {
            if (pageindex <= 0)
                pageindex = 1;
            if (pagesize <= 0)
                pagesize = 15;

            var parameters = new DynamicParameters();
            parameters.Add("@Table", tablename);
            parameters.Add("@Fields", fields);
            parameters.Add("@Where", where);
            parameters.Add("@OrderBy", orderby);
            parameters.Add("@CurrentPage", pageindex);
            parameters.Add("@PageSize", pagesize);
            parameters.Add("@GetAllCount", getallcount);
            parameters.Add("@AllCount", 0, DbType.Int32, ParameterDirection.Output);

            MsSqlMapperHepler.commandTimeout = commandTimeout;
            var ret = MsSqlMapperHepler.StoredProcWithParams<T>("Proc_DataPagination", parameters, string.IsNullOrWhiteSpace(connect) ? DBKeys.PRX : connect);
            allcount = parameters.Get<int>("@AllCount");
            return ret;
        }
    }

}
