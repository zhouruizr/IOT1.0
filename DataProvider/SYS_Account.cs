//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataProvider
{
    using System;
    using System.Collections.Generic;
    
    public partial class SYS_Account
    {
        public SYS_Account()
        {
            this.SYS_AccountRole = new HashSet<SYS_AccountRole>();
        }
    
        public int ACC_Id { get; set; }
        public string ACC_Account { get; set; }
        public string ACC_Password { get; set; }
        public byte[] ACC_BinaryPassword { get; set; }
        public string ACC_Email { get; set; }
        public string ACC_MobilePhone { get; set; }
        public string ACC_ActiveStatus { get; set; }
        public string ACC_ActiveCode { get; set; }
        public Nullable<System.DateTime> ACC_ActiveDateTime { get; set; }
        public Nullable<System.DateTime> ACC_LastLoginTime { get; set; }
        public Nullable<System.DateTime> ACC_LastUpdatePwdTime { get; set; }
        public Nullable<int> ACC_UserKey1 { get; set; }
        public string ACC_UserKey2 { get; set; }
        public bool ACC_IsSuspended { get; set; }
        public string ACC_Mark { get; set; }
        public string ACC_CreatedBy { get; set; }
        public System.DateTime ACC_CreatedOn { get; set; }
        public string ACC_LastUpdBy { get; set; }
        public Nullable<System.DateTime> ACC_LastUpdOn { get; set; }
        public byte[] ACC_RowVersion { get; set; }
        public string USRP_UserCode { get; set; }
    
        public virtual ICollection<SYS_AccountRole> SYS_AccountRole { get; set; }
    }
}
