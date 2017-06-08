using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LinqKit;
using CCSH.DataProvider.SqlServer;
using DataProvider.SqlServer;
using DataProvider;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects.DataClasses;
namespace CCSH.DataProvider
{
    public abstract class DPBase
    {
        private static NERPEntities _db ;//定义上下文
        public static NERPEntities db
        {
            get
            {
                if (_db == null)
                {
                    _db = new NERPEntities();
                }
                return _db;
            }
        }

        /// <summary>
        /// 通用查询,必须先设置好查询类
        /// </summary>
        /// <param name="SearchMod">查询类</param>
        /// <returns></returns>
        public static List<dynamic> DPGetQueryLst<T>(SearchMod<T> smod, out SearchMod<T> res) where T : class
        {
            int skipCount = ((smod.page - 1) * smod.rp);
            IQueryable<dynamic> query = smod.query;
            Type t = typeof(T);
            //List<PropertyInfo> tempPro = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();
            smod.total = query.Count();
            List<dynamic> ret = query.Skip(skipCount).Take(smod.rp).ToList();
            res = smod;
            return ret;
        }

        /// <summary>
        /// 通用新增方法，返回影响行数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Mod"></param>
        /// <returns></returns>
        public static int Add<T>(T Mod) where T : class
        {

                DbEntityEntry entry = db.Entry<T>(Mod);

                Type t = typeof(T);
                List<PropertyInfo> tempPro = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();
                //Dictionary<string, PropertyInfo> propertyDic = new Dictionary<string, PropertyInfo>();

                foreach(PropertyInfo p in tempPro)
                {
                    if (p.Name.Substring(p.Name.IndexOf("_")) == "_CreatedOn")//系统时间
                    {
                        entry.Property(p.Name).CurrentValue = DateTime.Now;
                        continue;
                    }
                };
                db.Set<T>().Add(Mod);
                return db.SaveChanges();
         
        }

        /// <summary>
        /// 修改对象通用方法，返回true false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Mod"></param>
        /// <returns></returns>
        public static bool Update<T>(T Mod) where T : class
        {
            bool ret = false;
            if (db.Entry<T>(Mod).State == System.Data.Entity.EntityState.Modified)
            {
                ret = db.SaveChanges() > 0 ? true : false;
            }
            else if(db.Entry<T>(Mod).State == System.Data.Entity.EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(Mod);
                    db.Entry<T>(Mod).State = System.Data.Entity.EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    Type t = typeof(T);
                    List<PropertyInfo> tempPro = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();
                    foreach (PropertyInfo p in tempPro)
                    {
                        var attr = p.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), true).FirstOrDefault()
                                            as EdmScalarPropertyAttribute;
                        if (p.PropertyType.IsPrimitive)//主键
                        {
                            object key = db.Entry<T>(Mod).Property(p.Name).CurrentValue;
                            T old = db.Set<T>().Find(key);
                            db.Entry(old).CurrentValues.SetValues(Mod);
                            break;
                        }
                    }
                }
                ret = db.SaveChanges() > 0 ? true : false;
            }

            return ret;
        }

        /// <summary>
        /// 获取对象通用方法，返回一个实例，必须通过主键获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Mod"></param>
        /// <returns></returns>
        public static T Get<T>(params object[] keyValues) where T : class
        {

            //db.Set<T>().
            T res = null;
            //DbEntityEntry entry = db.Entry<T>(Mod);
            //Type t = typeof(T);
            //List<PropertyInfo> tempPro = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();
            //foreach (PropertyInfo p in tempPro)
            //{
            //    var attr = p.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), true).FirstOrDefault()
            //                        as EdmScalarPropertyAttribute;
            //    if (p.PropertyType.IsPrimitive)//主键
            //    {
            //        ParameterExpression left = Expression.Parameter(typeof(T), "c");//c=>
            //        Expression selector = Expression.Property(left, typeof(T).GetProperty(p.Name));//c.主键
            //        Expression body = Expression.Constant(Convert.ToInt32(id));//主键的值
            //        Expression hb = Expression.Equal(selector, body);
            //        Expression right = Expression.Call
            //           (
            //              Expression.Property(left, typeof(T).GetProperty(p.Name)),  //c.主键
            //              typeof(bool).GetMethod("Contains", new Type[] { typeof(string) }),// 反射使用.Contains()方法                         
            //              Expression.Constant(Convert.ToInt32(id))           // .Contains(optionName)
            //           );

            //        Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(hb, left);
            //        string aa = p.Name;
            res =  db.Set<T>().Find(keyValues);
            //    }
            //}
            return res;
            //db.Entry(Mod).State = System.Data.EntityState.Modified;
            //return db.SaveChanges() > 0 ? true : false;
        }

        /// <summary>
        /// 查找单个
        /// </summary>
        /// <param name="seleWhere">查询条件</param>
        /// <returns></returns>
        public static T GetSingle<T>(Expression<Func<T, bool>> seleWhere) where T : class
        {

                return db.Set<T>().AsExpandable().FirstOrDefault(seleWhere);
     
        }


        /// <summary>
        /// 物理物理删除，尽量不要用
        /// </summary>
        /// <param name="model"></param>
        public static bool Delete<T>(T model) where T : class
        {
            db.Set<T>().Remove(model);
            return db.SaveChanges() > 0 ? true : false;
        }

        public static bool Delete<T>(params object[] keyValues) where T : class
        {
            try
            {
                bool ret = false;

                T model = db.Set<T>().Find(keyValues);
                if (model != null)
                {
                    db.Set<T>().Remove(model);//查询和删除同时操作会出现并发错误，需注意。
                    // db.Entry<T>(model).State = System.Data.Entity.EntityState.Deleted;
                    ret = db.SaveChanges() > 0 ? true : false;
                }
                return ret;
            }
            catch (Exception ex)
            {
                _db = null;//重置db
                throw ex.InnerException;
                
            }
            
        }




        #region 原始sql操作
        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        public void ExecuteSql(string sql, params object[] paras)
        {
 
                db.Database.ExecuteSqlCommand(sql, paras);
    
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(string sql, params object[] paras) where T : class
        {

                return db.Database.SqlQuery<T>(sql, paras).ToList();
       
        }

        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, params object[] paras) where T : class
        {

                return db.Database.SqlQuery<T>(sql, paras).FirstOrDefault();

        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="lsSql"></param>
        /// <param name="lsParas"></param>
        public void ExecuteTransaction(List<String> lsSql, List<Object[]> lsParas)
        {
 
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < lsSql.Count; i++)
                        {
                            if (lsParas != null && lsParas.Count > 0)
                            {
                                db.Database.ExecuteSqlCommand(lsSql[i], lsParas[i]);
                            }
                        }
                        foreach (String item in lsSql)
                        {
                            db.Database.ExecuteSqlCommand(item);
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }

        }
        #endregion

        /// <summary>
        /// zr 去除字符串末尾的0
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string RemoveZero(string param)
        {
            if (param.Split('.').Length == 2)
            {
                string result = param.Split('.')[0];
                string strChar = param.Split('.')[1];
                strChar.ToArray();
                for (int i = strChar.Length - 1; i >= 0; i--)
                {
                    if (strChar[i] == 0 || strChar[i] == '0')
                    {
                        strChar = strChar.Remove(i);
                    }
                    else
                    {
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(strChar.ToString()))
                {
                    return result + "." + strChar.ToString();
                }
                else
                {
                    return result;
                }
            }
            return param;
        }


        /// <summary>
        /// 查询返回所有的值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IQueryable<T> Get<T>() where T : class
        {
            IQueryable<T> list = db.Set<T>();
            return list;
        }


    }
}
