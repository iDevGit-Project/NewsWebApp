using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsWeb.Extentions;
using NewsWeb.Models;
using NewsWeb.Models.DatabaseContext;
using NewsWeb.Models.UnitOfWork;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class NewsController : Controller
    {
        #region متدهای کلاس سازنده کنترلر اخبار
        private readonly IUnitOfWork _iuw;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IToastNotification _toastNotification;
        public NewsController(IUnitOfWork iuw, IToastNotification toastNotification,
            IWebHostEnvironment HostEnvironment,
            UserManager<ApplicationUsers> userManager)
        {
            _iuw = iuw;
            _toastNotification = toastNotification;
            _webHostEnvironment = HostEnvironment;
            _userManager = userManager;
        }
        #endregion

        #region متد های مربوط به نمایش لیست اخبار و صفحه بندی
        public IActionResult ListNews(int page = 1)
        {
            int MovePage = (page - 1) * 5;
            int totalCount = _iuw.NewsRepositoryUW.Get().Count();
            ViewBag.PageID = page;

            //ViewBag.RecordCountNews = _iuw.NewsRepositoryUW.Get().ToList().Select(s => s.NewsId).Count();

            double counterRemain = totalCount % 5;

            if (counterRemain == 0)
            {
                ViewBag.PageCount = totalCount / 5;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 5) + 1;
            }
            return View(_iuw.NewsRepositoryUW.Get(null, null, "tblCategory").Skip(MovePage).Take(5));

            // جهت نمایش شرط هایی که در کد های جنریک برای نمایش آیتم های مربوطه نیاز داریم
            //var model = _iuw.NewsRepositoryUW.Get(null, null, "tblCategory");
            //return View(model);
        }
        #endregion

        #region متد بارگزاری فایل عکس در سرور و ثبت آدرس در پایگاه داده
        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> files)
        {
            var upload = Path.Combine(_webHostEnvironment.WebRootPath, "Admin\\images\\upload\\newsImage\\normalImage\\");
            var filename = "";
            foreach (var file in files)
            {
                filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                using (var fileStreamWebHost = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                {
                    await file.CopyToAsync(fileStreamWebHost);
                }
            }
            /////////تغییر سایز عکس و ذخیره
            Extentions.ImageResizer img = new Extentions.ImageResizer();
            img.Resize(upload + filename, _webHostEnvironment.WebRootPath + "\\Admin\\images\\upload\\newsImage\\thumbnailimage\\" + filename);

            _toastNotification.AddInfoToastMessage("تصویر خبر با موفقیت بارگزاری شد.", new NotyOptions()
            {
                ProgressBar = true,
                Timeout = 1000,
                Layout = "topCenter",
                Theme = "metroui"
            });
            return Json(
                new
                {
                    status = "success",
                    imagename = filename
                }
                );
        }
        #endregion

        #region متد ثبت اخبار
        [HttpGet]
        public IActionResult AddNews()
        {
            ViewBag.ItemCategoryList = _iuw.CategoryRepositoryUW.Get();
            return View();
        }

        [HttpPost]
        public IActionResult AddNews(TBL_News model, string IndexImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (IndexImage == null)
                    {
                        model.IndexImage = "defaultNews.jpg";
                    }
                    else
                    {
                        model.IndexImage = IndexImage;
                    }

                    TBL_News Addnews = new TBL_News
                    {
                        Title = model.Title,
                        Abstract = model.Abstract,
                        Content = model.Content,
                        NewsDate = model.NewsDate,
                        NewsTime = model.NewsTime,
                        CategoryID = model.CategoryID,
                        UserID = model.UserID,
                        IndexImage = model.IndexImage
                    };
                    _iuw.NewsRepositoryUW.Create(Addnews);
                    _toastNotification.AddSuccessToastMessage("ثبت خبر با موفقیت انجام شد.", new NotyOptions()
                    {

                        ProgressBar = true,
                        Timeout = 1000,
                        Layout = "topCenter",
                        Theme = "metroui"
                    });
                    _iuw.Save();
                    return RedirectToAction("ListNews", "News");
                }
                catch
                {
                    throw;
                }
            }
            ViewBag.ItemCategoryList = _iuw.CategoryRepositoryUW.Get();
            return View(model);
        }
        #endregion

        #region متد مربوط به ویرایش اخبار
        [HttpGet]
        public IActionResult EditNews(int id)
        {
            var model = _iuw.NewsRepositoryUW.GetById(id);
            if (model != null)
            {
                //نمایش اطلاعات دسته بندی در فرم اطلاعات ویرایش
                ViewBag.ItemCategoryList = _iuw.CategoryRepositoryUW.Get();
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult EditNews(TBL_News model, string newIndexImage, string currentImageName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (newIndexImage != null)
                    {
                        model.IndexImage = newIndexImage;
                    }
                    else
                    {
                        model.IndexImage = currentImageName;
                    }
                    _iuw.NewsRepositoryUW.Update(model);
                    _toastNotification.AddInfoToastMessage("ویرایش خبر با موفقیت انجام شد.", new NotyOptions()
                    {

                        ProgressBar = true,
                        Timeout = 1000,
                        Layout = "topCenter",
                        Theme = "metroui"
                    });
                    _iuw.Save();
                    return RedirectToAction("ListNews", "News");
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return View(model);
            }
        }
        #endregion

        #region متد مربوط به حذف اخبار
        [HttpGet]
        public IActionResult DeleteNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TBL_News tblnews = _iuw.NewsRepositoryUW.GetById(id);
            if (tblnews == null)
            {
                return NotFound();
            }
            return PartialView(tblnews);
        }

        [HttpPost, ActionName("DeleteNews")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItemListNews(int id)
        {
            _iuw.NewsRepositoryUW.DeleteById(id);
            _toastNotification.AddSuccessToastMessage("خبر مورد نظر حذف شد.", new NotyOptions()
            {

                ProgressBar = true,
                Timeout = 1000,
                Layout = "topCenter",
                Theme = "metroui"
            });
            _iuw.NewsRepositoryUW.Save();

            return RedirectToAction("ListNews", "News");
        }
        #endregion

    }
}
