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
    public class ReportController : ApiController
    {
        public DbEntities db = new DbEntities();
        public int pageSize = 20; // 调用global中的全局变量

        /// <summary>
        /// 获取报告分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPageReport(int pageIndex, string title, string category)
        {
            ResultData result = new ResultData();
            try
            {
                List<CodeValueItem> lstStatus = CodeValueItem.lstStatus;
                List<CodeValueItem> lstBgfl = CodeValueItem.lstBgfl;

                var query = (from c in db.Report.AsEnumerable()
                             join d in lstStatus on c.Status.Value equals d.Code
                             join e in lstBgfl on c.bgfl equals e.Code
                             where c.Status.HasValue && !string.IsNullOrEmpty(c.Title)
                             orderby c.PublishDate descending
                             select new 
                             {
                                 id = c.id,
                                 title = c.Title,
                                 bgfl = c.bgfl,
                                 bgflName = e.Value,
                                 status = c.Status,
                                 statusName = d.Value,
                                 publishDate = c.PublishDate
                             }).AsEnumerable();
                var dItems = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();    

                result = new ResultData()
                {
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

        /// <summary>
        /// 获取报告实体
        /// </summary>
        /// <param name="id">实体类型参数需要使用[FormUri]</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetReport(int id)
        {
            ResultData result = new ResultData();
            try
            {
                var report = await Task.Run(() => db.Report.FirstOrDefault(c => c.id == id));
                if (report == null) return ErrorResult(ResultCode.Faild, "报告不存在.");
                var iResult = new { data = report, statusName = ApiEnum.TransferReportStatus(report.Status) };

                result = new ResultData() { code = (int)ResultCode.Successed, data = JsonConvert.SerializeObject(iResult) };
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ex.Message };
                LogClient.WriteLog(ex.Message, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 审核报告
        /// </summary>
        /// <param name="id">报告ID</param>
        /// <param name="status">审核状态</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AuditReport()
        {
            ResultData result = new ResultData();
            try
            {
                int id = Convert.ToInt32(HttpContext.Current.Request["id"]);
                int status = Convert.ToInt32(HttpContext.Current.Request["status"]);
                var report = db.Report.FirstOrDefault(en => en.id == id);
                if (report == null) return ErrorResult(ResultCode.Faild, "报告不存在.");
                report.Status = status;
                if (status == 1 || status == 2)
                {
                    report.VerifyFirstDate = DateTime.Now;
                }
                if(status == 3 || status == 4)  {
                    report.VerifyLastDate = DateTime.Now;
                }
                
                db.Entry(report).State = System.Data.Entity.EntityState.Modified;
                int res = db.SaveChanges();
                if (res > 0) {
                    var iResult = new { data = report, statusName = ApiEnum.TransferReportStatus(report.Status) };
                    result = new ResultData {
                        code = (int)ResultCode.Successed,
                        msg = ApiEnum.TransferResultCode((int)ResultCode.Successed),
                        data = JsonConvert.SerializeObject(iResult)
                    };
                }
            }
            catch (Exception ex)
            {
                LogClient.WriteLog(ex.Message, ex);
                result = new ResultData() { code = (int)ResultCode.Faild, msg = ApiEnum.TransferResultCode(ResultCode.Faild) };
            }
            return Json(result);
        }

        /// <summary>
        /// 添加/修改报告
        /// </summary>
        /// <param name="obj">HttpPost多个基础型参数使用dynmic</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveReport()
        {
            ResultData result = new ResultData();
            List<string> message = new List<string>();
            try
            {
                HttpContext context = HttpContext.Current;  //var title = HttpContext.Current.Request["title"];
                if (!TextValidation(context, out message))
                {
                    result = new ResultData { code = (int)ResultCode.Faild, msg = String.Join(",", message) };
                    return Json(result);
                }

                if (!string.IsNullOrEmpty(context.Request["id"]))
                {
                    EditReport(context);     // edit
                }
                else
                {
                    CreateReport(context);   // add
                }
                int res = db.SaveChanges();
                if (res > 0)
                {
                    result = new ResultData() { code = (int)ResultCode.Successed, msg = ApiEnum.TransferResultCode(ResultCode.Successed) };
                }
            }
            catch (Exception ex)
            {
                LogClient.WriteLog(ex.Message, ex);
                result = new ResultData() { code = (int)ResultCode.Faild, msg = ApiEnum.TransferResultCode(ResultCode.Faild) };
            }
            return Json(result);
        }

        public void EditReport(HttpContext context)
        {
            string title = context.Request["title"];
            string id = context.Request["id"];
            var report = db.resource_info.FirstOrDefault(c => string.Compare(c.rec_id.ToString(), id) == 0);
            if (report != null)
            {
                report.x_title = title;

                db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            }
            LogClient.WriteLog("EditReport");
        }

        public void CreateReport(HttpContext context)
        {
            string id = context.Request["id"];
            string title = context.Request["title"];

            resource_info report = new resource_info();
            report.x_title = title;

            db.resource_info.Add(report);
        }

        /// <summary>
        /// 删除报告/批量删除报告
        /// </summary>
        /// <param name="id">HttpPost少许参数使用[FormBody]</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RemoveReport(string ids)
        {
            ResultData result = new ResultData();
            try
            {
                if (string.IsNullOrEmpty(ids)) return ErrorResult(ResultCode.Faild, "参数值为空，请刷新后重试.");
                var arrIds = ids.Split(',').Where(en => !string.IsNullOrEmpty(en));
                var reports = db.resource_info.Where(en => arrIds.Contains(en.rec_id.ToString()));
                if (reports.Count()==0) return ErrorResult(ResultCode.Faild, "报告不存在，请刷新后重试.");

                db.resource_info.RemoveRange(reports);
                int res = db.SaveChanges();
                if (res > 0) {
                    result = new ResultData() { code = (int)ResultCode.Successed, msg = ApiEnum.TransferResultCode((int)ResultCode.Successed) };
                }
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ApiEnum.TransferResultCode(ResultCode.Faild) };
                LogClient.WriteLog(ex.Message, ex);
            }
            return Json<ResultData>(result);
        }


        public bool TextValidation(HttpContext context, out List<string> messageList)
        {
            bool result = true;
            bool checkResult = true;
            messageList = new List<string>();
            List<string> errMessage = new List<string>();

            string title = context.Request["title"];

            checkResult = Helper.TextValueCheck(title, 1, 32, eTitle(title), "cannot empty", "is too long", "is illeagl", string.Empty, "is existed", DataValidateTypes.SafeCharsOnly, out errMessage);
            if (!checkResult)
            {
                messageList.AddRange(errMessage);
                result = checkResult;
            }

            return result;
        }

        public bool eTitle(string title)
        {
            bool isExisted = false;
            if (string.IsNullOrEmpty(title)) return isExisted;
            return isExisted;
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



        #region The Old Version Code
        [HttpGet]
        public ResultData Test()
        {
            ResultData result = new ResultData();

            var dItems = db.resource_info.Take(10).ToList();
            var dResult = new { pageIndex = 1, data = dItems };

            result.code = (int)ResultCode.Successed;
            result.data = dResult.ToJson();

            return result;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetReport1(Guid id)
        {
            ResultData result = new ResultData();
            try
            {
                var report = await Task.Run(() => db.resource_info.FirstOrDefault(c => c.rec_id == id));
                if (report == null) return ErrorResult(ResultCode.Faild, "Report not found.");

                result = new ResultData() { code = (int)ResultCode.Successed, data = JsonConvert.SerializeObject(report) };
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ex.Message };
                LogClient.WriteLog(ex.Message, ex);
            }
            return Json(result);
        }

        [HttpGet]
        public IHttpActionResult GetPageReport1(int pageIndex, string title, string category)
        {
            ResultData result = new ResultData();
            try
            {
                db.Database.Log = (s) => System.Diagnostics.Debug.WriteLine(s);  // 监控执行timeline

                var query = (from c in db.V_Report
                             orderby c.PublishDate descending
                             select new
                             {
                                 ID = c.id,
                                 Title = c.Title,
                                 CategoryId = c.CategoryId,
                                 CategoryName = c.CategoryName,
                                 Status = c.Status,
                                 StatusStr = c.StatusStr,
                                 CreateBy = c.CreateBy,
                                 PublishDate = c.PublishDate
                             });

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(en => en.Title.Contains(title.Trim()));
                }
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(en => en.CategoryName.Contains(category.Trim()));
                }

                var dItems = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();     //分页方法1

                //var dItems = new PagedList<vReport>(query, pageIndex, pageSize);              //分页方法2
                //var dItems = query.ToPagedList<object>(pageIndex, pageSize);

                result = new ResultData()
                {
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
        #endregion

    }

    public class vReport
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public Guid Category { get; set; }
        public Guid Status { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime PublishDate { get; set; }
    }


}
