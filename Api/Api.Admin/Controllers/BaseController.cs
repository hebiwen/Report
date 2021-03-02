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
        public const int pageSize = 30;  //设置全局变量

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
    }
}
