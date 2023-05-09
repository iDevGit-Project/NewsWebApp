using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWeb.Extentions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Areas.AdminPanel.Controllers
{
    public class UploaderController : Controller
    {
        [HttpPost]
        public IActionResult UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var notImageMessage = "لطفا یک تصویر انتخاب کنید";
                var notImage = JsonConvert.DeserializeObject("{'uploaded':0, 'error': {'message': \" " + notImageMessage + " \"}}");
                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            upload.AddImageToServer(fileName, PathExtension.UploadImageServer, null, null);
            return Json(new
            {
                uploaded = true,
                url = $"{PathExtension.UploadImage}{fileName}"
            });
        }
    }
}
