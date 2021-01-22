using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Admin.Controllers
{
    public class TestController : ApiController
    {
        public async Task<IHttpActionResult> Test()
        {
            return Json<dynamic>(await Task.Run(() => new { code = 1, msg = "" }));
        }
        

    }
}
