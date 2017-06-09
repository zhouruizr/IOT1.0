using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Entities
{
   public class SYSButton
    {

        /// <summary>
        /// BTN_Id
        /// </summary>				
        public int BTN_Id { get; set; }
        /// <summary>
        /// BTN_Name
        /// </summary>				
        public string BTN_Name { get; set; }
        /// <summary>
        /// BTN_Name_En
        /// </summary>				
        public string BTN_Name_En { get; set; }
        /// <summary>
        /// BTN_Desc
        /// </summary>				
        public string BTN_Desc { get; set; }
        /// <summary>
        /// BTN_OrderIndex
        /// </summary>				
        public int BTN_OrderIndex { get; set; }
        /// <summary>
        /// BTN_IsSuspended
        /// </summary>				
        public bool BTN_IsSuspended { get; set; }
        /// <summary>
        /// BTN_Mark
        /// </summary>				
        public string BTN_Mark { get; set; }
        /// <summary>
        /// BTN_CreatedBy
        /// </summary>				
        public string BTN_CreatedBy { get; set; }
        /// <summary>
        /// BTN_CreatedOn
        /// </summary>				
        public DateTime? BTN_CreatedOn { get; set; }
        /// <summary>
        /// BTN_LastUpdBy
        /// </summary>				
        public string BTN_LastUpdBy { get; set; }
        /// <summary>
        /// BTN_LastUpdOn
        /// </summary>				
        public DateTime? BTN_LastUpdOn { get; set; }
    }
   /// <summary>
   /// Deploy：实体对象映射关系
   /// </summary>
   [Serializable]
   public sealed class SYSButtonORMMapper : ClassMapper<SYSButton>
   {
       public SYSButtonORMMapper()
       {
           base.Table("SYS_Button");

           //Map(f => f.socketouts).Ignore();//设置忽略
           Map(f => f.BTN_Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
           AutoMap();
       }
   }
}
