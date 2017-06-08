using DataProvider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace IOT1._0.Controllers
{
    public abstract class BaseController : ApiController
    {
        /// <summary>
        /// json字符串转换为对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(JObject json) where T : class
        {
            T model = (T)(JsonConvert.DeserializeObject(json.ToString(), typeof(T)));
            return model;
        }
    }
}
