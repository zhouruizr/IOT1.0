using DataProvider.Entities;
using DataProvider.Models;
using DataProvider.Paging;
using DataProvider.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Data
{
   public class ButtonData
    {
       /// <summary>
       /// 分页获取按钮列表
       /// </summary>
       /// <param name="search"></param>
       /// <returns></returns>
       public static PagedList<SYSButton> GetButtonList(ButtonListSearchModel search)
       {
           string table = string.Empty, fields = string.Empty, orderby = string.Empty, where = string.Empty;//定义结构
           fields = @"  * ";//输出字段
           table = @" SYS_Button ";//表或者视图
           orderby = "BTN_Id";//排序信息
           StringBuilder sb = new StringBuilder();//构建where条件
           sb.Append(" 1=1 ");
           if (!string.IsNullOrWhiteSpace(search.BTN_Name)) ;//按钮中文名称
           sb.AppendFormat(" and BTN_Name like '%{0}%' ", search.BTN_Name);
           if (!string.IsNullOrWhiteSpace(search.BTN_Name_En))//城市
               sb.AppendFormat(" and BTN_Name_En like '%{0}%' ", search.BTN_Name_En);
           where = sb.ToString();
           int allcount = 0;
           var list = CommonPage<SYSButton>.GetPageList(
   out allcount, table, fields: fields, where: where.Trim(),
   orderby: orderby, pageindex: search.CurrentPage, pagesize: search.PageSize, connect: DBKeys.PRX);
           return new PagedList<SYSButton>(list, search.CurrentPage, search.PageSize, allcount);
       }
       /// <summary>
       /// 获取按钮
       /// </summary>
       /// <param name="ID"></param>
       /// <returns></returns>
       public static SYSButton GetButtonByID(int ID)
       {
           return MsSqlMapperHepler.GetOne<SYSButton>(ID,DBKeys.PRX);
       }
       /// <summary>
       /// 新增,返回的是主键
       /// </summary>
       /// <param name="btn"></param>
       /// <returns></returns>
       public static int AddButton(SYSButton btn)
       {
           return MsSqlMapperHepler.Insert<SYSButton>(btn, DBKeys.PRX);
       }
       /// <summary>
       /// 保存
       /// </summary>
       /// <param name="btn"></param>
       /// <returns></returns>
       public static bool UpdateButton(SYSButton btn)
       {
           SYSButton btnto = ButtonData.GetButtonByID(btn.BTN_Id);//获取对象
           Cloner<SYSButton, SYSButton>.CopyTo(btn, btnto);//代码克隆，把前台或者的值也就是变更内容复制到目标对象，不做变更的数据不变
           return MsSqlMapperHepler.Update(btnto, DBKeys.PRX);
       }


    }
}
