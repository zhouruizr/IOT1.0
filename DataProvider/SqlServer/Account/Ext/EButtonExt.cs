using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CCSH.DataProvider.SqlServer.Account
{
    public partial class EButton
    {
        [DataMember]
        public bool CurrentBtnHasRights { get; set; }
    }
}
