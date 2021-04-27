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
    public class ReportController : BaseController
    {
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
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]))
                    pageSize = int.Parse(HttpContext.Current.Request["pageSize"]);

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
                                 publishDate = c.PublishDate.Value.ToString("yyyy-MM-dd")
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

        /// <summary>
        /// 获取所有报告
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllReport(string category, string level, int pageIndex, int pageSize = 20)
        {
            ResultData result = new ResultData();
            try
            {
                List<CodeValueItem> lstBgfl = CodeValueItem.lstBgfl;
                // 终审通过的报告列表
                var query = (from c in db.Report.AsEnumerable()
                             join e in lstBgfl on c.bgfl equals e.Code
                             where c.Status.HasValue && c.Status == (int)ReportStatus.LastAuditSuccess && !string.IsNullOrEmpty(c.Title)
                             orderby c.PublishDate descending
                             select new
                             {
                                 id = c.id,
                                 title = c.Title,
                                 bgfl = c.bgfl,
                                 bgflName = e.Value,
                                 level = c.Level,
                                 publishDate = c.PublishDate.Value.ToString("yyyy-MM-dd")
                             }).AsEnumerable();
                if (!string.IsNullOrEmpty(level))
                {
                    query = query.Where(el => el.level.ToString() == level);
                }
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(el => el.bgfl.ToString() == category);
                }
                var dItems = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                result = new ResultData()
                {
                    code = (int)ResultCode.Successed,
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

                result = new ResultData { code = (int)ResultCode.Successed, data = JsonConvert.SerializeObject(iResult) };
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
                if (status == 3 || status == 4)
                {
                    report.VerifyLastDate = DateTime.Now;
                }

                db.Entry(report).State = System.Data.Entity.EntityState.Modified;
                int res = db.SaveChanges();
                if (res > 0)
                {
                    var iResult = new { data = report, statusName = ApiEnum.TransferReportStatus(report.Status) };
                    result = new ResultData
                    {
                        code = (int)ResultCode.Successed,
                        msg = ApiEnum.TransferResultCode(ResultCode.Successed),
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
            var report = db.Report.FirstOrDefault(c => string.Compare(c.id.ToString(), context.Request["id"]) == 0);
            if (report != null)
            {
                report.Title = context.Request["title"];
                report.SubTitle = context.Request["subTitle"];
                report.KeyWords = context.Request["keyWords"];
                report.Abstract = context.Request["abstract"];
                report.Directory = context.Request["directory"];
                report.Content = context.Request["content"];
                report.FileName = context.Request["fileName"];
                report.FilePath = context.Request["filePath"];
                report.FilePage = ConvertInt32(context.Request["filePage"]);
                report.bgfl = ConvertInt32(context.Request["bgfl"]);
                report.zdgzfl = context.Request["zdgzfl"];
                report.ztfl = context.Request["ztfl"];
                report.hyfl = context.Request["hyfl"];
                report.Level = ConvertInt32(context.Request["level"]);
                report.Author = context.Request["author"];
                report.Company = context.Request["company"];
                report.Source = context.Request["source"];
                report.Language = context.Request["language"];
                report.Country = context.Request["country"];
                report.Area = context.Request["area"];
                report.IsMain = ConvertBoolen(context.Request["isMain"]);
                report.PublishDate = ConvertDateTime(context.Request["publishDate"]);

                db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            }
            LogClient.WriteLog("EditReport");
        }

        public void CreateReport(HttpContext context)
        {
            RP_Report report = new RP_Report()
            {
                Title = context.Request["title"],
                SubTitle = context.Request["subTitle"],
                bgfl = ConvertInt32(context.Request["bgfl"]),
                ztfl = context.Request["ztfl"],
                KeyWords = context.Request["keyword"],
                Abstract = context.Request["abstract"],
                Directory = context.Request["directory"],
                Content = context.Request["content"],
                Status = (int)ReportStatus.NotAudit,
                FileName = context.Request["fileName"],
                FilePath = context.Request["filePath"],
                FilePage = ConvertInt32(context.Request["filePage"]),
                zdgzfl = context.Request["zdgz"],
                hyfl = context.Request["hyfl"],
                Level = ConvertInt32(context.Request["level"]),
                Author = context.Request["author"],
                Company = context.Request["company"],
                Source = context.Request["source"],
                Language = context.Request["language"],
                Country = context.Request["country"],
                Area = context.Request["area"],
                IsMain = ConvertBoolen(context.Request["zdbg"]),
                PublishDate = ConvertDateTime(context.Request["publishDate"]),
                CreateBy = "Session",
                CreateDate = DateTime.Now,
            };

            db.Report.Add(report);
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
                if (reports.Count() == 0) return ErrorResult(ResultCode.Faild, "报告不存在，请刷新后重试.");

                db.resource_info.RemoveRange(reports);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    result = new ResultData() { code = (int)ResultCode.Successed, msg = ApiEnum.TransferResultCode(ResultCode.Successed) };
                }
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ApiEnum.TransferResultCode(ResultCode.Faild) };
                LogClient.WriteLog(ex.Message, ex);
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

            checkResult = Helper.TextValueCheck(title, 1, 32, eTitle(title), "标题不能为空.", "标题最大长度是32.", "标题无效.", string.Empty, "标题已存在.", DataValidateTypes.SafeCharsOnly, out errMessage);
            if (!checkResult)
            {
                messageList.AddRange(errMessage);
                result = checkResult;
            }
            checkResult = Helper.TextValueCheck(bgfl, 1, 32, "报告分类不能为空.", "报告分类最大长度为32.", "报告分类无效.", string.Empty, DataValidateTypes.Integer, out errMessage);

            return result;
        }

        public bool eTitle(string title)
        {
            bool isExisted = false;
            if (string.IsNullOrEmpty(title)) return isExisted;
            return isExisted;
        }

        /// <summary>
        /// 获取分类JSON数据
        /// </summary>
        /// <param name="type">分类类型</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCategory(int type)
        {
            ResultData result = new ResultData();
            try
            {
                var lstCategory = db.Category.Where(en => en.type == type && en.parentId == null).OrderBy(en => en.sort.Trim()).ToList();
                if (lstCategory.Count == 0) return ErrorResult(ResultCode.Successed, "暂无数据.");

                List<CategoryTree> lstCategoryTree = new List<CategoryTree>();
                lstCategory.ForEach(item =>
                {
                    var child = new CategoryTree();
                    child.id = item.id;
                    child.title = item.title;
                    child.parentId = item.parentId;
                    child.parentName = item.parentName;
                    child.sort = item.sort;
                    child.select = string.Empty;
                    var childCategory = GetChildCategory(child, item.id);
                    if (childCategory == null)
                    {
                        lstCategoryTree.Add(child);
                    }
                    else
                    {
                        lstCategoryTree.Add(childCategory);
                    }
                });
                result.code = (int)ResultCode.Successed;
                result.data = JsonConvert.SerializeObject(lstCategoryTree);
            }
            catch (Exception ex)
            {
                result = new ResultData { code = (int)ResultCode.Faild, msg = ex.Message };
            }
            return Json(result);
        }

        public CategoryTree GetChildCategory(CategoryTree category, int? parentId)
        {
            var childCategory = db.Category.Where(en => en.parentId == parentId).OrderBy(en => en.sort).ToList();
            if (childCategory.Count == 0) return null;
            List<CategoryTree> lstCategory = new List<CategoryTree>();
            childCategory.ForEach(child =>
            {
                var nCategory = new CategoryTree()
                {
                    id = child.id,
                    title = child.title,
                    parentId = child.parentId,
                    parentName = child.parentName,
                    sort = child.sort,
                    select = string.Empty
                };
                GetChildCategory(nCategory, child.id);
                lstCategory.Add(nCategory);
            });
            if (lstCategory != null) category.children = lstCategory;
            return category;
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

    /// <summary>
    /// 分类树
    /// </summary>
    public class CategoryTree
    {
        public int id { get; set; }
        public string title { get; set; }
        public int? parentId { get; set; }
        public string parentName { get; set; }
        public string sort { get; set; }
        public string select { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<CategoryTree> children { get; set; }
    }
}
