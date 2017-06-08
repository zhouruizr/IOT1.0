using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CCSH.DataProvider;
using IDataProvider;

namespace DataProvider
{
    public class ExcuteModelImplement<T>:IExcuteModel<T> where T : class
    {
        static string json = string.Empty;
        T t = (T)(JsonConvert.DeserializeObject(json, typeof(T)));

        T IExcuteModel<T>.GetModel(string id)
        {
             T _returnT = DPBase.Get<T>(id);
             return _returnT;
        }

        string IExcuteModel<T>.AddModel(T t)
        {
            DPBase.Add(t);
            return "新增成功！";
        }

        string IExcuteModel<T>.UpdateModel(T t)
        {
            DPBase.Update(t);
            return "保存成功！";
        }

        bool IExcuteModel<T>.DelModel(T t)
        {
           bool flag =  DPBase.Delete(t);
           return flag;
        }
    }
}
