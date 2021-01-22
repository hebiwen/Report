using Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Api.Admin
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public ApiController api;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            log4net.Config.XmlConfigurator.Configure(); // 启用Log4net
        }

        public static int PageSize
        {
            get
            {
                int pageSize = 10;
                return pageSize;
            }
        }

        //public IHttpActionResult ErrorResult(int? Code,string Message)
        //{
        //    if (Code == null) Code = (int)ResultCode.Successed;
        //    if (string.IsNullOrEmpty(Message)) Message = "Success";

        //    ResultData result = new ResultData() {
        //        code = Code.Value,
        //        msg = Message
        //    };
            
        //    return Json(result);
        //}

       
    }
}
