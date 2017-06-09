using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;

namespace CCSH.DataProvider.SqlServer.Account
{
    public partial class EButtonGroup
    {
        [DataMember]
        public virtual ICollection<EButton> ButtonLst { get; set; }
    }
}
