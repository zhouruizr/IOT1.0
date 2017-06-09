using Dapper;
using DataProvider.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace DataProvider.Data
{
    /// <summary>
    /// 数据库通用方法
    /// </summary>
   public class CommonData
    {
       public static readonly CommonData Instance = new CommonData();
        #region 获取字典列表
        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="dicTypeID"></param>
        /// <returns>用于下拉的绑定项目</returns>
        public static List<CommonEntity> GetDictionaryList(int DicTypeID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT DicItemID id,DicItemName name");
            sb.Append(" FROM DictionaryItem a WITH(NOLOCK)");
            sb.Append(" INNER JOIN DictionaryType b WITH(NOLOCK) ON a.DicTypeID = b.DicTypeID");
            sb.Append(" WHERE b.DicTypeID = @DicTypeID and a.recordState <> '2'");
            sb.Append(" ORDER BY Sort");
            var parameters = new DynamicParameters();
            parameters.Add("@DicTypeID", DicTypeID);
            return MsSqlMapperHepler.SqlWithParams<CommonEntity>(sb.ToString(), parameters, DBKeys.PRX);
        }
        #endregion

        public List<SelectListItem> GetBropDownListData(List<CommonEntity> list)
        {
            return GetBropDownListData(list, true);
        }

        public List<SelectListItem> GetBropDownListData(List<CommonEntity> list, bool needDefault, string selectedValueText = "")
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var model in list)
            {
                if ((!string.IsNullOrWhiteSpace(selectedValueText)) && ((model.id == selectedValueText) || (model.name == selectedValueText)))
                    items.Add(new SelectListItem { Value = model.id, Text = model.name, Selected = true });
                else
                    items.Add(new SelectListItem { Value = model.id, Text = model.name });
            }
            if (needDefault)
                items.Insert(0, new SelectListItem { Value = string.Empty, Text = "--请选择--" });
            return items;
        }
    }
}
