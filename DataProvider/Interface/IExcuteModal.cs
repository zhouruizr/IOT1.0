using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IDataProvider
{
    public interface IExcuteModel<T> where T: class
    {
        //取值
        T GetModel(string id);
        //新增
        string AddModel(T t);
        //修改
        string UpdateModel(T t);
        //删除
        bool DelModel(T t);
    }
}
