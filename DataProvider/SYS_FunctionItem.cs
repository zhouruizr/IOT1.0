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
    
    public partial class SYS_FunctionItem
    {
        public SYS_FunctionItem()
        {
            this.SYS_RoleFunction = new HashSet<SYS_RoleFunction>();
        }
    
        public int FI_Id { get; set; }
        public int FI_TreeId { get; set; }
        public int FI_ButtonGroupId { get; set; }
        public int FI_ButtonId { get; set; }
        public Nullable<int> FI_ButtonGroupOrderIndex { get; set; }
        public Nullable<int> FI_ButtonOrderIndex { get; set; }
        public string FI_Desc { get; set; }
        public Nullable<bool> FI_IsSuspended { get; set; }
        public string FI_Mark { get; set; }
        public string FI_CreatedBy { get; set; }
        public System.DateTime FI_CreatedOn { get; set; }
        public string FI_LastUpdBy { get; set; }
        public Nullable<System.DateTime> FI_LastUpdOn { get; set; }
        public byte[] FI_RowVersion { get; set; }
    
        public virtual SYS_Button SYS_Button { get; set; }
        public virtual SYS_FunctionTree SYS_FunctionTree { get; set; }
        public virtual ICollection<SYS_RoleFunction> SYS_RoleFunction { get; set; }
    }
}
