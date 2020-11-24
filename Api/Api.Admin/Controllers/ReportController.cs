using Api.Model;
using Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api.Admin.Controllers
{
    public class ReportController : ApiController
    {
        public DbEntities db = new DbEntities();
        public int pageSize = 20; // 调用global中的全局变量

        [HttpGet]
        public ResultData Test()
        {
            ResultData result = new ResultData();

            var dItems = db.resource_info.Take(10).ToList();
            var dResult = new { pageIndex = 1, data = dItems };

            result.code = (int)ResultCode.Successed;
            result.data = dResult.ToJson();
            result.msg.Add(string.Empty);

            return result;
        }

        /// <summary>
        /// 获取报告实体
        /// </summary>
        /// <param name="id">实体类型参数需要使用[FormUri]</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetReport(Guid id)
        {
            try
            {
                var report = await Task.Run(() => db.resource_info.FirstOrDefault(c => c.rec_id == id));
                if (report == null) throw new Exception("Report not found.");

                return Json<dynamic>(report);
            }
            catch (Exception ex)
            {
                return Json<dynamic>(new { code = HttpStatusCode.NotFound, message = ex.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult GetPageReport(int pageIndex)
        {
            try
            {
                var query = (from c in db.resource_info
                             orderby c.x_publishdate descending
                             select new
                             {
                                 ID = c.rec_id,
                                 Title = c.x_title,
                                 Category = c.x_reportId,
                                 Status = c.x_verifystate,
                                 CreateBy = c.x_manager,
                                 PublishDate = c.x_publishdate
                             });
                var dItems = query.Skip(pageSize * pageIndex).Take(pageSize).ToList();
                var dResult = new
                {
                    pageIndex = pageIndex,
                    total = query.Count(),
                    data = dItems
                };

                return Json<dynamic>(dResult);
            }
            catch (Exception ex)
            {
                return Json<dynamic>(new { code = HttpStatusCode.BadRequest, message = ex.Message });
            }
        }

        /// <summary>
        /// 添加/修改报告
        /// </summary>
        /// <param name="obj">HttpPost多个基础型参数使用dynmic</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveReport(dynamic obj)
        {
            ResultData result = new ResultData();
            try
            {
                string id = obj["id"];
                string title = obj["title"];
                string reportId = obj["reportId"];
                string file = obj["file"];

                List<string> message = new List<string>();
                resource_info report = null;

                if (!TextValidation(obj, out message))
                {
                    result = new ResultData { code = (int)ResultCode.Failed, msg = message };
                    return Json(result);
                }

                if (!string.IsNullOrEmpty(id))
                {
                    // edit
                    report = db.resource_info.FirstOrDefault(c => string.Compare(c.rec_id.ToString(), id) == 0);
                    if (report != null)
                    {
                        report.x_title = title;
                        report.x_reportId = Guid.Parse(reportId);

                        db.Entry(report).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                {
                    // add
                    report = new resource_info();
                    report.x_title = title;
                    report.x_reportId = Guid.Parse(reportId);

                    db.resource_info.Add(report);
                }

                int res = db.SaveChanges();
                if (res > 0)
                {
                    result = new ResultData() { code = (int)ResultCode.Successed };
                }
                else {
                    result = new ResultData() { code = (int)ResultCode.Failed };
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 删除报告
        /// </summary>
        /// <param name="id">HttpPost少许参数使用[FormBody]</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RemoveReport([FromBody]Guid id)
        {
            ResultData result = new ResultData();
            try
            {

            }
            catch (Exception ex)
            {
                result.code = (int)HttpStatusCode.BadRequest;
                result.msg.Add(ex.Message);
            }
            return Json<ResultData>(result);
        }


        public bool TextValidation(dynamic obj, out List<string> messageList)
        {
            bool result = true;
            bool checkResult = true;
            messageList = new List<string>();
            List<string> errMessage = new List<string>();

            string id = obj["id"];
            string title = obj["title"];
            string reportId = obj["reportId"];
            string file = obj["file"];

            checkResult = Helper.TextValueCheck(title, 1, 32, "cannot empty", "is too long", "is invalid","is existed", DataValidateTypes.SafeCharsOnly, out errMessage);
            if (!checkResult)
            {
                messageList.AddRange(errMessage);
                result = checkResult;
            }

            return result;
        }

        public void EditReport(dynamic obj)
        {
            string id = obj["id"];
            string title = obj["title"];
            string reportId = obj["reportId"];
            string file = obj["file"];
            var report = db.resource_info.FirstOrDefault(c => string.Compare(c.rec_id.ToString(), id) == 0);
            if (report != null)
            {
                report.x_title = title;
                report.x_reportId = Guid.Parse(reportId);

                db.Entry(report).State = System.Data.Entity.EntityState.Modified;

            }
            int res = db.SaveChanges();
            if (res > 0)
            {
                // add Audit trail
            }
        }

        public void CreateReport(dynamic obj)
        {
            string id = obj["id"];
            string title = obj["title"];
            string reportId = obj["reportId"];
            string file = obj["file"];

        }

    }
}
