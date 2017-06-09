using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.SqlServer
{
   public class test2<T> where T : class
    {
        /// <summary>
        /// 实体查询
        /// </summary>
        public List<dynamic> getList(System.Linq.Expressions.Expression<Func<T, bool>> where)
         {
             using (NERPEntities contex = new NERPEntities())
             {
                 var aa = new { BG_Name = "123", BG_Name_En = "456", BG_OrderIndex = "678", BG_Desc = "789" };
                 List<dynamic> list = contex.Set<T>().Where(where).Select(c => aa ).ToList<dynamic>();
                 return list;
             }
         }


        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="BtnG"></param>
        /// <param name="rp"></param>
        /// <param name="pageIndex"></param>
        /// <param name="allRows"></param>
        /// <param name="allPages"></param>
        /// <returns></returns>
        public static List<dynamic> DPGetQueryLst(SearchMod<T> aa, dynamic expression)
        {
            using (NERPEntities contex = new NERPEntities())
            {
                int skipCount = ((aa.page - 1) * aa.rp);
                var bb = aa.ret; //匿名对象，用于显示输出
                //List<dynamic> list = contex.Set<T>().Select(c => aa).ToList<dynamic>();
                IQueryable<dynamic> query = aa.query;
                var custom = new SYS_ButtonGroup { BG_Name = "BG_Name", BG_Name_En = "BG_Name_En" };
                //aa.where.Compile()(custom);
                //query.Where(aa.where);
                //query = aa.query;

               // var custom = expression.Compile()(expression);

               // var ww = expression.Compile()(custom);
                //表中有无法转换的字段，所以用动态类型进行转换
                List<dynamic> list = query.ToList<dynamic>();
                aa.total = list.Count();
                aa.total = aa.total / aa.rp + aa.total % aa.rp == 0 ? 0 : 1;
                List<dynamic> ret= aa.page == 0 ? list : list.Skip(skipCount).Take(aa.rp).ToList();
                return ret;
            }
        }
    }
}
