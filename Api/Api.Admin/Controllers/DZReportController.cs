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
using PagedList;
using Newtonsoft.Json.Linq;

namespace Api.Admin.Controllers
{
    public class DZReportController : BaseController
    {
        // GET: DZReport
        [HttpGet]
        public IHttpActionResult GetPageDZReport(int pageIndex,string title)
        {
            ResultData result = new ResultData();
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]))
                    pageSize = int.Parse(HttpContext.Current.Request["pageSize"]);

                var query = (from c in db.DZReport.AsEnumerable()
                             where !string.IsNullOrEmpty(c.Title)
                             orderby c.Seq
                             select new
                             {
                                 id = c.id,
                                 title = c.Title,
                                 category = c.Category,
                                 cName = ApiEnum.TransferDZCategory(c.Category),
                                 price = c.Price,
                                 previewPage = c.PreviewPage,
                                 hyfl = c.hyfl,
                                 filePath = c.FilePath
                             });
                var dItems = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                result = new ResultData()
                {
                    code = (int)ResultCode.Successed,
                    pageIndex = pageIndex,
                    total = query.Count(),
                    data = JsonConvert.SerializeObject(dItems)
                };
            }
            catch (Exception ex)
            {
                result = new ResultData() { code = (int)ResultCode.Faild, msg = ex.Message };
                LogClient.WriteLog(ex.Message, ex);
            }
            return Json(result);
        }

        [HttpGet]
        public IHttpActionResult GetDZReport(int id)
        {
            ResultData result = new ResultData();
            try
            {
                var report = db.DZReport.FirstOrDefault(el => el.id == id);
                if (report == null) throw new Exception("报告不存在,请刷新");
                result = new ResultData { code = (int)ResultCode.Successed, data = JsonConvert.SerializeObject(report) };
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ex.Message };
            }
            return Json(result);
        }

        [HttpPost]
        public IHttpActionResult RemoveReport(string ids)
        {
            ResultData result = new ResultData();
            try
            {
                if (string.IsNullOrEmpty(ids)) throw new Exception("参数不能为空，请刷新");
                var arrIds = ids.Split(',').Where(el => !string.IsNullOrEmpty(el));
                var dzReport = db.DZReport.Where(el => arrIds.Contains(el.id.ToString()));
                db.DZReport.RemoveRange(dzReport);
                db.SaveChanges();

                result = new ResultData { code = (int)ResultCode.Successed, msg = "操作成功" };
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ex.Message };
            }
            return Json<ResultData>(result);
        }

        [HttpPost]
        public IHttpActionResult SaveDZReport()
        {
            ResultData result = new ResultData();
            try
            {
                List<string> message = new List<string>();
                HttpContext context = HttpContext.Current;
                if (!TextValidation(context, out message))
                {
                    result = new ResultData { code = (int)ResultCode.Faild, msg = String.Join(",", message) };
                    return Json(result);
                }
                if (string.IsNullOrEmpty(context.Request["id"])){   // create
                    CreateReport(context);
                }
                else { // edit
                    EditReport(context);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ex.Message };
            }
            return Json<ResultData>(result);
        }

        /// <summary>
        /// 表单验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="messageList"></param>
        /// <returns></returns>
        public bool TextValidation(HttpContext context, out List<string> messageList)
        {
            bool result = true;
            bool checkResult = true;
            messageList = new List<string>();
            List<string> errMessage = new List<string>();

            string title = context.Request["title"].Trim();
            string bgfl = context.Request["bgfl"].Trim();
            string hyfl = context.Request["hyfl"].Trim();
            string keyword = context.Request["keyword"].Trim();

            checkResult = Helper.TextValueCheck(title, 1, 32,true, "标题不能为空.", "标题最大长度是32.", "标题无效.", string.Empty, "标题已存在.", DataValidateTypes.SafeCharsOnly, out errMessage);
            if (!checkResult)
            {
                messageList.AddRange(errMessage);
                result = checkResult;
            }
            checkResult = Helper.TextValueCheck(bgfl, 1, 32, "报告分类不能为空.", "报告分类最大长度为32.", "报告分类无效.", string.Empty, DataValidateTypes.Integer, out errMessage);

            return result;
        }

        public void CreateReport(HttpContext context)
        {
            RP_DZReport dzReport = new RP_DZReport() {
                Title = context.Request["title"].Trim(),
                Category = ConvertInt32(context.Request["category"]).Value,
                hyfl = context.Request["hyfl"],
                Price = ConvertDecimal(context.Request["price"]),
                FileName = context.Request["fileName"],
                FilePath = context.Request["filePath"],
                FilePage = ConvertInt32(context.Request["filePage"]),
                PreviewPage = ConvertInt32(context.Request["previewPage"]),
                Thumb = context.Request["thumb"],
                Description = context.Request["description"],
                Seq = 1,
                Source = context.Request["sourece"],
                Remark = context.Request["remark"],
                CreateBy = "Session",
                CreateDate = DateTime.Now
            };
            db.DZReport.Add(dzReport);
        }

        public void EditReport(HttpContext context)
        {
            RP_DZReport dzReport = db.DZReport.FirstOrDefault(el => el.id.ToString() == context.Request["id"]);
            if (dzReport != null) {
                dzReport.Title = context.Request["title"];
                dzReport.Category = ConvertInt32(context.Request["category"]).Value;
                dzReport.hyfl = context.Request["hyfl"];
                dzReport.Price = ConvertDecimal(context.Request["price"]);
                dzReport.FileName = context.Request["fileName"];
                dzReport.FilePath = context.Request["filePath"];
                dzReport.FilePage = ConvertInt32(context.Request["filePage"]);
                dzReport.PreviewPage = ConvertInt32(context.Request["previewPage"]);
                dzReport.Thumb = context.Request["thumb"];
                dzReport.Description = context.Request["description"];
                dzReport.Seq = 1;
                dzReport.Source = context.Request["source"];
                dzReport.Remark = context.Request["remark"];
            }
            db.Entry(dzReport).State = System.Data.Entity.EntityState.Modified;
        }

    }
}