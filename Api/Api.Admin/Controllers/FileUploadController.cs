using Api.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.Admin.Controllers
{
    public class FileUploadController : ApiController
    {
        /// <summary>
        /// 上传PDF文件
        /// </summary>
        /// <returns>返回文件名称，相对路径，文件页数</returns>
        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            try
            {
                string uploadFolder = HttpContext.Current.Server.MapPath("/UploadFolder");
                HttpFileCollection fileCollection = HttpContext.Current.Request.Files;

                if (fileCollection.Count <= 0) throw new Exception("文件上传失败..");

                string uploadDirectory = Path.Combine(uploadFolder, DateTime.Now.ToString("yyyyMMdd"));
                if (!System.IO.Directory.Exists(uploadDirectory)) System.IO.Directory.CreateDirectory(uploadDirectory);

                HttpPostedFile postFile = fileCollection[0];
                string filePath = Path.Combine(uploadDirectory, postFile.FileName);
                while (!File.Exists(filePath))
                {
                    postFile.SaveAs(filePath);
                }
                string webPath = "/UploadFolder/" + DateTime.Now.ToString("yyyyMMdd") + "/" + postFile.FileName;
                return Json<dynamic>(new { status = (int)ResultCode.Successed, fileName = postFile.FileName, filePath = webPath,filePage = 10 });
            }
            catch (Exception ex)
            {
                return Json<dynamic>(new { status = (int)ResultCode.Faild, message = ex.Message });
            }
        }
    }


}
