using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using DapperExtensions;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Configuration;

namespace DataProvider.SqlServer
{
    public class DBRepository
    {
        public DBRepository(string Constr)
        {
            DB = CreateDBSession(Constr);
        }


        private DBSession CreateDBSession(string Constr)
        {
            IDbConnection connection = null;
            string strConn = DBKeys.GetConnectionString(Constr);
            connection = new System.Data.SqlClient.SqlConnection(strConn);

            return new DBSession(connection);
        }
        private DBSession DB;

        /// <summary>
        ///     根据Id获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T GetById<T>(dynamic primaryId) where T : class
        {
            return DB.Connection.Get<T>(primaryId as object,DB.Transaction);
        }

        /// <summary>
        ///     根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class
        {
            string tblName = string.Format("dbo.{0}", typeof(T).Name);
            string idsin = string.Join(",", ids.ToArray());
            string sql = "SELECT * FROM @table WHERE Id in (@ids)";
            IEnumerable<T> dataList = DB.Connection.Query<T>(sql, new { table = tblName, ids = idsin });
            return dataList;
        }


        /// <summary>
        ///     获取全部数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            return DB.Connection.GetList<T>();
        }

        /// <summary>
        ///     统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public int Count<T>(object predicate, bool buffered = false) where T : class
        {
            return DB.Connection.Count<T>(predicate);
        }

        /// <summary>
        ///     查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null,
            bool buffered = false) where T : class
        {
            return DB.Connection.GetList<T>(predicate, sort, null, null, buffered);
        }

        public IEnumerable<T> Query<T>(string sql, object parameters = null) where T : class
        {
            return DB.Connection.Query<T>(sql, parameters, DB.Transaction);
        }

        public IEnumerable<dynamic> Query(string sql, object parameters = null)
        {
            return DB.Connection.Query(sql, parameters, DB.Transaction);
        }

        /// <summary>
        ///     插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public dynamic Insert<T>(T entity) where T : class
        {
            dynamic result = DB.Connection.Insert(entity, DB.Transaction);
            return result;
        }

        /// <summary>
        ///     更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool Update<T>(T entity) where T : class
        {
            bool isOk = DB.Connection.Update(entity, DB.Transaction);
            return isOk;
        }

        /// <summary>
        ///     删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool Delete<T>(dynamic primaryId) where T : class
        {
            var entity = GetById<T>(primaryId);
            var obj = entity as T;
            bool isOk = DB.Connection.Delete(obj, DB.Transaction);
            return isOk;
        }

        /// <summary>
        ///     批量插入功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="transaction"></param>
        public bool InsertBatch<T>(IEnumerable<T> entityList) where T : class
        {
            bool isOk = false;
            foreach (T item in entityList)
            {
                Insert(item);
            }
            isOk = true;
            return isOk;
        }

        /// <summary>
        ///     批量更新（）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool UpdateBatch<T>(IEnumerable<T> entityList) where T : class
        {
            bool isOk = false;
            foreach (T item in entityList)
            {
                Update(item);
            }
            isOk = true;
            return isOk;
        }

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool DeleteBatch<T>(IEnumerable<dynamic> ids) where T : class
        {
            bool isOk = false;
            foreach (dynamic id in ids)
            {
                Delete<T>(id);
            }
            isOk = true;
            return isOk;
        }


        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, dynamic param = null)
        {
            return DB.Connection.Execute(sql, param as object, DB.Transaction);
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <returns>当前事务</returns>
        public IDbTransaction BeginTransaction()
        {
            return DB.Begin();
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            DB.Commit();
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            DB.Rollback();
        }
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            DB.Dispose();
        }
    }

    /// <summary>
    /// 数据库连接事务的Session对象
    /// </summary>
    public class DBSession
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;


        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public IDbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// 数据库事务对象
        /// </summary>
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        public DBSession(IDbConnection Connection)
        {
            _connection = Connection;
        }

        /// <summary>
        /// 开启会话
        /// </summary>
        /// <param name="isolation"></param>
        /// <returns></returns>
        public IDbTransaction Begin(IsolationLevel isolation = IsolationLevel.ReadUncommitted)
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction(isolation);
            return _transaction;
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction = null;
                }
                _connection.Close();
                _connection = null;
            }
            GC.SuppressFinalize(this);
        }
    }

}