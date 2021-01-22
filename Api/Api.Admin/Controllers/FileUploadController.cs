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
        [HttpPost]
        public Task<IEnumerable<UploadFile>> FileUploader()
        {
            string path = HttpContext.Current.Server.MapPath("./UploadFolder");
            var rootURL = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, string.Empty);
            if (Request.Content.IsMimeMultipartContent())
            {
                var provider = new CustomMultipartFormDataStreamProvider(path);
                var task = Request.Content.ReadAsMultipartAsync(provider).ContinueWith<IEnumerable<UploadFile>>(item =>
                {
                    if (item.IsFaulted || item.IsCanceled)
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }

                    var fileInfo = provider.FileData.Select(file =>
                    {
                        var iFile = new FileInfo(file.LocalFileName);
                        return new UploadFile { FileName = iFile.Name, FilePath = rootURL + "/UploadFolder/" + iFile.Name };
                    });
                    return fileInfo;
                });
                return task;
            }
            else {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "文件格式错误."));
            }
        }
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            var fileName = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : string.Empty;
            return fileName.Replace("\"", string.Empty);
        }
    }

    public class UploadFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

}
