using Api.Common;
using Api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Api.Admin.Controllers
{
    public class BaseController : ApiController
    {
        public int pageSize = 30;  //设置全局变量

        public DbEntities db = new DbEntities();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        protected HttpResponseMessage GetHttpResponseMessage(object result)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "application/json")
            };
        }

        protected HttpResponseMessage GetHttpResponseMessage(object result,ref string msg)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ResultData()), System.Text.Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// 返回自定义Json结果
        /// </summary>
        /// <param name="Code">状态码 (默认:0)</param>
        /// <param name="Message">消息</param>
        /// <returns></returns>
        public IHttpActionResult ErrorResult(ResultCode Code = ResultCode.Successed, string Message = "")
        {
            ResultData result = new ResultData()
            {
                code = (int)Code,
                msg = Message
            };
            return Json(result);
        }

        /// <summary>
        /// 字符串转换成数字
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public static int? ConvertInt32(string value)
        {
            int? result = null;
            if (!string.IsNullOrEmpty(value.Trim()) && value != "null" && value != "undefined")
            {
                result = Convert.ToInt32(value);
            }
            return result;
        }
        /// <summary>
        /// 字符串转换成小数
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public static decimal? ConvertDecimal(string value)
        {
            decimal? result = null;
            if (!string.IsNullOrEmpty(value.Trim()) && value != "null" && value != "undefined")
            {
                result = Convert.ToDecimal(value);
            }
            return result;
        }
        /// <summary>
        /// 字符串转换成日期
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public static DateTime? ConvertDateTime(string value)
        {
            DateTime result = DateTime.Now;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// 字符串转换成布尔值
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public static bool ConvertBoolen(string value)
        {
            bool result = true;
            if (Boolean.TryParse(value, out result))
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// 小数格式化成字符串
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="format">保留小数位</param>
        /// <returns></returns>
        public static string FormatDecimal(decimal? value, string format)
        {
            if (value.HasValue)
            {
                return Convert.ToDecimal(value).ToString(format);
            }
            return null;
        }
    }
}
