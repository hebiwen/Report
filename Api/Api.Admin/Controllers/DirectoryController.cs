using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Api.Model;
using Api.Common;
using Newtonsoft.Json;

namespace Api.Admin.Controllers
{
    public class DirectoryController : BaseController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetPageDirectory(int pageIndex)
        {
            ResultData result = new ResultData();
            try
            {
                var query = (from c in db.Directory.AsEnumerable()
                             where !string.IsNullOrEmpty(c.Title)
                             orderby c.PublishDate descending
                             select new
                             {
                                 id = c.id,
                                 title = c.Title,
                                 hyfl = c.hyfl,
                                 publishDate = c.PublishDate,
                                 sendLastDate = c.SendLastDate
                             });

                var dItem = await Task.Run(() => query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList());
                result = new ResultData
                {
                    code = (int)ResultCode.Successed,
                    pageIndex = pageIndex,
                    total = query.Count(),
                    data = JsonConvert.SerializeObject(dItem)
                };
            }
            catch (Exception ex)
            {
                result = new ResultData() { code = (int)ResultCode.Faild, msg = ApiEnum.TransferResultCode(ResultCode.Faild) };
            }
            return Json<ResultData>(result);
        }

    }
}
