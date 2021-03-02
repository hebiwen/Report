using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Api.Model;

namespace Api.Admin.Controllers
{
    public class ImportDataController : ApiController
    {
        public DbEntities db = new DbEntities();
        [HttpGet]
        public IHttpActionResult ResourceInfoToReport()
        {
            int count = db.resource_info.Where(en => en.old == 0).Count();
            try
            {
                while (count / 100 > 0)
                {
                    var iReport = db.resource_info.Where(en => en.old == 0).Take(100).ToList();
                    iReport.ForEach(item =>
                    {
                        RP_Report rpt = new RP_Report();
                        rpt.Title = CheckLength(item.x_title, 200);
                        rpt.SubTitle = CheckLength(item.x_subtitle, 200);
                        rpt.WebTitle = CheckLength(item.x_webtitle, 2000);
                        rpt.KeyWords = CheckLength(item.x_keywords, 400);
                        rpt.Abstract = CheckLength(item.x_remark, 4000);
                        rpt.Directory = CheckLength(item.x_directory, 4000);
                        rpt.Content = CheckLength(item.x_content, 4000);
                        rpt.Status = ConvertStatus(item.x_verifystate);
                        rpt.FileName = CheckLength(item.x_file, 100);
                        rpt.FilePage = item.x_page;
                        rpt.bgfl = ConvertBgfl(item.x_reportId);
                        rpt.ztfl = ConvertZtfl(item.x_zhongtuId);
                        rpt.zdgzfl = ConvertZdgz(item.x_watchId);
                        rpt.hyfl = ConvertHyfl(item.rec_id);
                        rpt.Level = item.x_level;
                        rpt.Author = CheckLength(item.x_author, 200);
                        rpt.Company = CheckLength(item.x_company, 200);
                        rpt.Source = CheckLength(item.x_infosource, 200);
                        rpt.Language = CheckLength(item.x_language, 50);
                        rpt.Country = CheckLength(item.x_country, 50);
                        rpt.Area = CheckLength(item.x_area, 50);
                        rpt.IsMain = item.is_main;
                        rpt.PublishDate = item.x_publishdate;
                        rpt.CreateBy = ConvertUser(item.x_manager);
                        rpt.CreateDate = item.create_date;
                        rpt.docmain_id = item.docmain_id;
                        rpt.rec_id = item.rec_id;

                        db.Report.Add(rpt);

                        item.old = 1;
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    });
                    db.SaveChanges();
                    count -= 100;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json<dynamic>(new { result = count,sucess = "successed" });
        }

        #region 导入Report表中的相关数据
        public string CheckLength(string value, int length)
        {
            string result = null;
            if (string.IsNullOrEmpty(value)) return result;
            if (value.Length > length)
            {
                result = "Over";
            }
            else {
                result = value;
            }
            return result;
        }

        public int? ConvertStatus(Guid status)
        {
            int? result = null;
            switch (status.ToString().ToUpper())
            {
                case "075C286F-F0A0-4755-A0DA-997E1A3D613E": result = 0; break;
                case "815E95BC-CD97-4FED-B72F-1BA8FDE4A2A4": result = 1; break;
                case "7849C6DC-FE9C-418D-B53F-52379D2A9E0E": result = 2; break;
                case "28206977-8AC5-4BA7-B061-C2E18CF366F8": result = 3; break;
                case "1B359E5A-CDEF-4914-8425-BCEE3CCA99BB": result = 4; break;
                default: result = null; break;
            }
            return result;
        }

        public int? ConvertBgfl(Guid bgfl)
        {
            int? result = null;
            switch (bgfl.ToString().ToUpper())
            {
                case "B6FF03FC-D90A-41D2-B6F5-467EA1EC9C69": result = 10; break;
                case "08A59B9F-4D74-4973-82FA-550EFD968EDB": result = 11; break;
                case "5F52C877-FDA6-4809-BD58-B868C1EF949B": result = 12; break;
                case "4E954640-960E-4335-9E20-7639CE5D21D2": result = 13; break;
                case "F4CF54B9-56DB-46A5-BBA6-44A1D778FC8C": result = 14; break;
                case "09C0FC4F-74A3-4C63-A7EA-1717B18A1ED3": result = 15; break;
                case "D4CE0372-9E37-4F59-9465-CF344B6E63C7": result = 16; break;
                case "928F8375-96BA-4B8A-94CF-325E77A2AF46": result = 17; break;
                case "80EFBF4F-D625-4EFD-9B2D-BE5374E57DA6": result = 18; break;
                case "FF691863-1344-4810-85B7-C6C937ED6F1E": result = 19; break;
                default:result = null;break;
            }
            return result;
        }

        public string ConvertZtfl(Guid ztfl)
        {
            string result = null;
            if (ztfl != Guid.Empty)
            {
                result = db.dict_zhongtu_category.FirstOrDefault(en => en.rec_id == ztfl).x_name;
            }
            return result;
        }

        public string ConvertZdgz(Guid zdgz)
        {
            string result = string.Empty;
            switch (zdgz.ToString().ToUpper())
            {
                case "9BB4CC1D-4C1A-402B-8184-27FB5DE1AB82": result = "每周报告"; break;
                case "09BC7E4A-1BBB-463B-8723-D2C8C07C12C8": result = "月度报告"; break;
                case "889B183C-EF3A-40FA-A40A-CFA5290ACF85": result = "季度报告"; break;
                case "3F65D62A-8F03-407B-83F4-6755144C269D": result = "年度报告"; break;
                default:result = null;break;
            }
            return result;
        }

        public string ConvertHyfl(Guid recId)
        {
            string result = null;
            if (recId == Guid.Empty) return null;
            var industry = (from c in db.rel_resource_industry
                            join d in db.dict_industry_category on c.industry_id equals d.rec_id into dic
                            from dIndustry in dic.DefaultIfEmpty()
                            where c.resource_id == recId
                            select new
                            {
                                ID = dIndustry.x_no,
                                Name = dIndustry.x_name
                            }).ToList();
            if (industry.Count > 0) {
                result = String.Join(",", industry.Select(en => en.ID));
            }

            return result;
        }

        public string ConvertUser(Guid managerId)
        {
            if (managerId == Guid.Empty) return null;
            var user = db.sys_user.FirstOrDefault(en => en.rec_id == managerId);
            if (user == null) return null;
            return user.x_name;
        }

        #endregion
    }
}
