using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Entities
{
    public class vw_Students
    {

        /// <summary>
        /// 学号
        /// </summary>				
        public string ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>				
        public string Name { get; set; }
        /// <summary>
        /// 绑定手机手机号码
        /// </summary>				
        public string BindPhone { get; set; }
        /// <summary>
        ///性别
        /// </summary>				
        public string Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>				
        public DateTime? Birthday { get; set; }
        /// <summary>
        ///父亲姓名
        /// </summary>				
        public string FatherName { get; set; }
        /// <summary>
        /// 父亲联系方式
        /// </summary>				
        public string FatherContract { get; set; }
        /// <summary>
        /// 母亲姓名
        /// </summary>				
        public string MotherName { get; set; }
        /// <summary>
        /// 母亲联系方式
        /// </summary>				
        public string MotherContract { get; set; }
        /// <summary>
        /// 地址
        /// </summary>				
        public string Address { get; set; }
        /// <summary>
        /// 微信
        /// </summary>				
        public string Wechart { get; set; }
        /// <summary>
        /// 来源渠道
        /// </summary>				
        public string SourceID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>				
        public string StateID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>				
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>				
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>				
        public string CreatorId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>				
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>				
        public string UpdatorId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>				
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>				
        public string DeletorId{ get; set; }
        /// <summary>
        /// BindAccount
        /// </summary>				
        public string BindAccount { get; set; }
    }
   /// <summary>
   /// Deploy：实体对象映射关系
   /// </summary>
   [Serializable]
    public sealed class StudentsORMMapper : ClassMapper<vw_Students>
   {
       public StudentsORMMapper()
       {
           base.Table("vw_Students");

           //Map(f => f.socketouts).Ignore();//设置忽略
           Map(f => f.ID).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
           AutoMap();
       }
   }
}
