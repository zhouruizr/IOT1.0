using CCSH.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.SqlServer
{
    public class DPTest : DPBase
    {
        /// <summary>
        /// 获取按钮组列表，列表必须在末尾加Lst
        /// </summary>
        /// <returns></returns>
        public static List<dynamic> DPGetButtonGroupLst(SYS_ButtonGroup BtnG, int rp, int page, out int allRows, out int allPages)
        {
            using (NERPEntities contex = new NERPEntities())
            {
                int skipCount = ((page - 1) * rp);
                IQueryable<SYS_ButtonGroup> query = contex.SYS_ButtonGroup;
                if (!string.IsNullOrEmpty(BtnG.BG_Name))
                {
                    query = query.Where(c => c.BG_Name == BtnG.BG_Name);
                }
                if (!string.IsNullOrEmpty(BtnG.BG_Name_En))
                {
                    query = query.Where(c => c.BG_Name_En == BtnG.BG_Name_En);
                }
                //表中有无法转换的字段，所以用动态类型进行转换
                List<dynamic> list = query.Select(c => c.BG_Name ).ToList<dynamic>();
                allRows = list.Count();
                allPages = allRows / rp + allRows % rp == 0 ? 0 : 1;
                return page == 0 ? list : list.Skip(skipCount).Take(rp).ToList();
            }
        }


        /// <summary>
        /// 获取按钮组
        /// </summary>
        /// <returns></returns>
        public static SYS_ButtonGroup DPGetBtnInformation(SYS_ButtonGroup bGroup)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    SYS_ButtonGroup btnGroup = context.SYS_ButtonGroup.Where(c => c.BG_Name_En == bGroup.BG_Name_En).FirstOrDefault();
                    return btnGroup;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }

        }


        /// <summary>
        /// 验证按钮属性
        /// </summary>
        /// <returns></returns>
        public static string DPValBtnAttribute(SYS_ButtonGroup bGroup)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    string flag = string.Empty;
                    IQueryable<SYS_ButtonGroup> _valbGroup = context.SYS_ButtonGroup.Where(c => c.BG_Name == bGroup.BG_Name);
                    if (_valbGroup.Count() != 0)
                    {
                        flag = "按钮组名称重复请从新输入！";
                    }
                    else
                    {
                        _valbGroup = context.SYS_ButtonGroup.Where(c => c.BG_Name_En == bGroup.BG_Name_En);
                        if (_valbGroup.Count() != 0)
                        {
                            flag = "按钮组编号重复请从新输入！";
                        }
                        else
                        {
                            _valbGroup = context.SYS_ButtonGroup.Where(c => c.BG_OrderIndex == bGroup.BG_OrderIndex);
                            if (_valbGroup.Count() != 0)
                            {
                                flag = "按钮组序号重复请从新输入！";
                            }
                        }
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }



        /// <summary>
        /// 保存按钮组
        /// </summary>
        /// <returns></returns>
        public static void DPSaveButtonModel(SYS_ButtonGroup bGroup)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    bGroup.BG_IsSuspended = false;
                    bGroup.BG_CreatedOn = DateTime.Now;
                    bGroup.BG_CreatedBy = "SYS";
                    context.SYS_ButtonGroup.Add(bGroup);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="BtnG"></param>
        /// <param name="rp"></param>
        /// <param name="page"></param>
        /// <param name="allRows"></param>
        /// <param name="allPages"></param>
        /// <returns></returns>
        public static List<dynamic> DPGetQueryLst<T>(SearchMod<T> aa)
        {
            using (NERPEntities contex = new NERPEntities())
            {
                int skipCount = ((aa.page - 1) * aa.rp);
                string afg = typeof(T).ToString();
                IQueryable<T> query = null;

                ParameterExpression left = Expression.Parameter(typeof(T), "c");//c=>
                ConstantExpression _constExp = Expression.Constant("aaa", typeof(string));//一个常量
                MethodCallExpression _methodCallexp = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), _constExp);
                Expression<Action> consoleLambdaExp = Expression.Lambda<Action>(_methodCallexp);

                //表中有无法转换的字段，所以用动态类型进行转换
                List<dynamic> list = query.Select(c => new { BG_Name = "123", BG_Name_En = "456", BG_OrderIndex = "678", BG_Desc = "789" }).ToList<dynamic>();
                aa.total = list.Count();
                aa.total = aa.total / aa.rp + aa.total % aa.rp == 0 ? 0 : 1;
                return aa.page == 0 ? list : list.Skip(skipCount).Take(aa.rp).ToList();
            }
        }

    }
}
