using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CCSH.DataProvider.SqlServer.Account
{
    public partial class EFunctionTree
    {
        [DataMember]
        public bool CurrentFuncTreeHasRights { get; set; }

        [DataMember]
        public List<EFunctionTree> EFunctionTreeLst { get; set; }

        private bool _isNode = false;
        [DataMember]
        public bool IsNode
        {
            get
            {
                return (this.EFunctionTreeLst == null || this.EFunctionTreeLst.Count == 0);
            }
            private set { _isNode = value; }
        }
    }
}
