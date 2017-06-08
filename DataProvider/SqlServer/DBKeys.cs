namespace DataProvider.SqlServer
{
    public sealed class DBKeys
    {
        #region 常用数据库
        /// <summary>
        /// 上海主数据库(存放供应商,商品,品牌,分类等信息)
        /// </summary>
        public static readonly string PRX = "PrxConnectionString";

        #endregion

        #region 非常用数据库

        #endregion



        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="connectionName">connectionStrings 节点名</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ToString();
        }

    }
}