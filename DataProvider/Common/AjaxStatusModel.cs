using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider
{
    public class AjaxStatusModel
    {
        /// <summary>
        /// 状态 成功200
        /// </summary>
        public EnumAjaxStatus status { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string msg { get; set; }

        public AjaxStatusModel()
        {
            this.status = EnumAjaxStatus.Error;
            this.data = null;
            this.msg = "操作失败，请稍后重试";
        }
        public AjaxStatusModel(EnumAjaxStatus status, string msg, object data = null)
        {
            this.status = status;
            this.data = data;
            this.msg = msg;
        }
    }

    public class ResultModel
    {
        /// <summary>
        /// 状态 成功200
        /// </summary>
        public EnumAjaxStatus Status { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Message { get; set; }

        public ResultModel()
        {
            this.Status = EnumAjaxStatus.Error;
            this.Message = "操作失败，请稍后重试";
        }
        public ResultModel(EnumAjaxStatus status, string msg)
        {
            this.Status = status;
            this.Message = msg;
        }
    }
    public class ResultModel<T>
    {
        /// <summary>
        /// 状态 成功200
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Message { get; set; }
        public T Model { get; set; }
        public ResultModel()
        {
            this.Status = -2;
            this.Message = "操作失败，请稍后重试";
        }
        public ResultModel(int status, string msg)
        {
            this.Status = status;
            this.Message = msg;
        }
    }
    /// <summary>
    /// ajaxz执行状态
    /// </summary>
    public enum EnumAjaxStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 失败
        /// </summary>
        Error = -2
    }
}
