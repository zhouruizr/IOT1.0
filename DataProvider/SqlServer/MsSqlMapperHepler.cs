using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Dapper;
using DapperExtensions;
using System.Data.Common;

namespace DataProvider.SqlServer
{
    public static class MsSqlMapperHepler
    {
        /// <summary>
        /// 默认为30秒
        /// </summary>
        public static int? commandTimeout { get; set; }

        /// <summary>
        /// 获取数据库链接实例
        /// </summary>
        /// <param name="name">appSetting节点名</param>
        /// <returns></returns>
        public static SqlConnection GetOpenConnection(string connectionName)
        {
            try
            {
                string connString = DBKeys.GetConnectionString(connectionName);
                if (string.IsNullOrWhiteSpace(connString))
                    throw new Exception("The database connection config error.");

                var connection = new SqlConnection(connString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Can not be connected to the data server.", ex);
            }
        }

        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName) where T : class, new()
        {
            using (SqlConnection cnn = GetOpenConnection(connectionName))
            {
                int records = 0;

                foreach (T entity in entities)
                {
                    records += cnn.Execute(sql, entity, commandTimeout: commandTimeout);
                }
                return records;
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        public static DynamicParameters GetParametersFromObject(object obj, string[] propertyNamesToIgnore)
        {
            if (propertyNamesToIgnore == null) propertyNamesToIgnore = new string[] { String.Empty };
            DynamicParameters p = new DynamicParameters();
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in properties)
            {
                if (!propertyNamesToIgnore.Contains(prop.Name))
                    p.Add("@" + prop.Name, prop.GetValue(obj, null));
            }
            return p;
        }

        public static void SetIdentity<T>(IDbConnection connection, Action<T> setId)
        {
            dynamic identity = connection.Query("SELECT @@IDENTITY AS Id", commandTimeout: commandTimeout).Single();
            T newId = (T)identity.Id;
            setId(newId);
        }

        public static object GetPropertyValue(object target, string propertyName)
        {
            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object theValue = null;
            foreach (PropertyInfo prop in properties)
            {
                if (string.Compare(prop.Name, propertyName, true) == 0)
                {
                    theValue = prop.GetValue(target, null);
                }
            }
            return theValue;
        }

        public static void SetPropertyValue(object p, string propName, object value)
        {
            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
                return;
            if (!info.CanWrite)
                return;
            info.SetValue(p, value, null);
        }

        public static List<T> StoredProcWithParams<T>(string procname, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout).ToList();
            }
        }

        public static List<dynamic> StoredProcWithParamsDynamic(string procname, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query(procname, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout).ToList();
            }
        }

        public static U StoredProcInsertWithID<T, U>(string procName, DynamicParameters parms, string connectionName)
        {
            using (SqlConnection connection = MsSqlMapperHepler.GetOpenConnection(connectionName))
            {
                var x = connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
                return parms.Get<U>("@ID");
            }
        }

        public static T StoredProcExecuteGet<T>(string procName, DynamicParameters parms, string outParamName, string connectionName)
        {
            using (SqlConnection connection = MsSqlMapperHepler.GetOpenConnection(connectionName))
            {
                var x = connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
                return parms.Get<T>("@" + outParamName);
            }
        }

        public static T SqlExecuteGet<T>(string procName, DynamicParameters parms, string outParamName, string connectionName)
        {
            using (SqlConnection connection = MsSqlMapperHepler.GetOpenConnection(connectionName))
            {
                var x = connection.Execute(procName, (object)parms, commandType: CommandType.Text, commandTimeout: commandTimeout);
                return parms.Get<T>("@" + outParamName);
            }
        }

        public static List<T> SqlWithParams<T>(string sql, dynamic parms, string connectionnName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionnName))
            {
                return connection.Query<T>(sql, (object)parms, commandTimeout: commandTimeout).ToList();
            }
        }

        public static int InsertUpdateOrDeleteSql(string sql, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(sql, (object)parms, commandTimeout: commandTimeout);
            }
        }

        public static int InsertUpdateOrDeleteStoredProc(string procName, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
        }

        public static T SqlWithParamsSingle<T>(string sql, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(sql, (object)parms, commandTimeout: commandTimeout).FirstOrDefault();
            }
        }

        public static System.Dynamic.DynamicObject DynamicProcWithParamsSingle<T>(string sql, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout).FirstOrDefault();
            }
        }

        public static IEnumerable<dynamic> DynamicProcWithParams<T>(string sql, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
        }

        public static T StoredProcWithParamsSingle<T>(string procname, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout).SingleOrDefault();
            }
        }

        public static int SqlExecuteNonQuery(string procname, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(procname, (object)parms, commandType: CommandType.Text, commandTimeout: commandTimeout);
            }
        }

        public static int StoredProcExecuteNonQuery(string procname, dynamic parms, string connectionName)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(procname, (object)parms, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
        }

        public static int ExecuteSqlTransaction(string sql, dynamic parms,IDbTransaction trans)
        {       
            return trans.Connection.Execute(sql, (object)parms, transaction: trans, commandTimeout: commandTimeout);
        }

        public static int ExecuteSqlTransaction(string sql, SqlParameter[] parms, string connectionName)
        {
            int ret = 0;
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("PackageTransaction");
                command.Connection = connection;
                command.Transaction = transaction;
                if (commandTimeout.HasValue)
                    command.CommandTimeout = commandTimeout.Value;
                try
                {
                    command.CommandText = sql;
                    if (null != parms && parms.Length > 0)
                        command.Parameters.AddRange(parms);
                    ret = command.ExecuteNonQuery();
                    transaction.Commit();

                    if (connection != null)
                        connection.Close();
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        if (connection != null)
                            connection.Close();
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 新增数据，如果T只有一个主键，且新增的主键为数字，则会将新增的主键赋值给entity相应的字段，如果为多主键，或主键不是数字和Guid,则主键需输入 
        /// 如果要进行匿名类型新增，因为匿名类型无法赋值，需调用ISqlAdapter的Insert，由数据库新增的主键需自己写方法查询 
        /// 实体名称和属性名称必须和数据库一致 zr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>返回主键，如果是符合主键则返回key object</returns>
        public static dynamic Insert<T>(T model, string connectionName) where T : class, new()
        {
            using (SqlConnection cnn = GetOpenConnection(connectionName))
            {
                dynamic ret = 0;

                ret = cnn.Insert(model);
                return ret;
            }
        }

        /// <summary>
        /// 更新数据，如果entity为T,则全字段更新，如果为匿名类型，则修改包含的字段，但匿名类型必须包含主键对应的字段 实体名称和属性名称必须和数据库一致 zr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update<T>(T model, string connectionName) where T : class
        {
            using (SqlConnection cnn = GetOpenConnection(connectionName))
            {
                bool records = false;

                records = cnn.Update(model);
                return records;
            }
        }

        /// <summary>
        /// 更新数据，如果entity为T,则全字段更新，如果为匿名类型，则修改包含的字段，但匿名类型必须包含主键对应的字段 实体名称和属性名称必须和数据库一致 zr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update<T>(T model, IDbTransaction transaction = null) where T : class
        {
            using (DbConnection cnn = (DbConnection)transaction.Connection)
            {
                bool records = false;

                records = cnn.Update(model, transaction);
                return records;
            }
        }
        /// <summary>
        ///     批量更新 zr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool UpdateBatch<T>(IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
            bool isOk = false;
            foreach (T item in entityList)
            {
                MsSqlMapperHepler.Update(item,transaction);
            }
            isOk = true;
            return isOk;
        }


        /// <summary>
        /// 根据主键获取一个对象 zr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T GetOne<T>(int id, string connectionName) where T : class, new()
        {
            using (SqlConnection cnn = GetOpenConnection(connectionName))
            {
                T record = null;

                record = cnn.Get<T>(id);
                return record;
            }
        }

        /// <summary>
        /// 根据主键获取一个对象 zr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T GetOne<T>(string id, string connectionName) where T : class, new()
        {
            using (SqlConnection cnn = GetOpenConnection(connectionName))
            {
                T record = null;

                record = cnn.Get<T>(id);
                return record;
            }
        }

        /// <summary>
        ///     分页 zr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public static IEnumerable<T> GetPageList<T>(int currentPage, int pageSize, out long allRowsCount, string connectionName,
            object predicate = null, IList<ISort> sort = null,bool buffered = false) where T : class
        {
            using (SqlConnection cnn = GetOpenConnection(connectionName))
            {

                if (sort == null)
                {
                    sort = new List<ISort>();
                   // sort.Add(new Sort { PropertyName = "id", Ascending = false });
                }
                IEnumerable<T> entityList = cnn.GetPage<T>(predicate, sort, currentPage - 1, pageSize, null, null,
                    buffered).ToList();
                allRowsCount = cnn.Count<T>(predicate);
                return entityList;
            }
        }

    }
}