using Microsoft.AspNetCore.Mvc;
using NewsWeb.Models;
using NewsWeb.Models.UnitOfWork;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        #region متد های کلاس سازنده کنترلر دسته بندی
        private readonly IUnitOfWork _iuw;
        private readonly IToastNotification _toastNotification;
        public CategoryController(IUnitOfWork iuw, IToastNotification toastNotification)
        {
            _iuw = iuw;
            _toastNotification = toastNotification;
        }
        #endregion

        #region متد مربوط به نمایش دسته بندی ها و صفحه بندی
        public IActionResult ListCategories(int page = 1)
        {
            int MovePage = (page - 1) * 5;
            int totalCount = _iuw.CategoryRepositoryUW.Get().Count();

            ViewBag.PageID = page;

            double counterRemain = totalCount % 5;

            if (counterRemain == 0)
            {
                ViewBag.PageCount = totalCount / 5;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 5) + 1;
            }
            return View(_iuw.CategoryRepositoryUW.Get().Skip(MovePage).Take(5));
        }
        #endregion

        #region متد مربوط به ثبت دسته بندی
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddCategory(TBL_Category model)
        {
            if (ModelState.IsValid)
            {
                _iuw.CategoryRepositoryUW.Create(model);
                _toastNotification.AddSuccessToastMessage("ثبت دسته بندی با موفقیت انجام شد.", new NotyOptions()
                {
                    ProgressBar = true,
                    Timeout = 1000,
                    Layout = "topCenter",
                    Theme = "metroui"
                });
                _iuw.Save();
                return RedirectToAction("AddCategory", "Category");
            }

            return View(model);
        }
        #endregion

        #region متد مربوط به ویرایش دسته بندی اطلاعات
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TBL_Category ct = _iuw.CategoryRepositoryUW.GetById(id);
            if (ct == null)
            {
                return NotFound();
            }
            return View(ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(int categoryid, TBL_Category model)
        {
            if (categoryid != model.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _iuw.CategoryRepositoryUW.Update(model);
                    _iuw.CategoryRepositoryUW.Save();
                }
                catch
                {
                    throw;
                }
            }
            _toastNotification.AddInfoToastMessage("دسته بندی مورد نظر با موفقیت ویرایش شد.", new NotyOptions()
            {

                ProgressBar = true,
                Timeout = 1000,
                Layout = "topCenter",
                Theme = "metroui"
            });
            return RedirectToAction("ListCategories", "Category");
        }
        #endregion

        #region متد مربوط به حذف اطلاعات
        [HttpGet]
        public IActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TBL_Category tblct = _iuw.CategoryRepositoryUW.GetById(id);
            if (tblct == null)
            {
                return NotFound();
            }
            return PartialView(tblct);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItemListCategory(int id)
        {
            _iuw.CategoryRepositoryUW.DeleteById(id);
            _iuw.CategoryRepositoryUW.Save();
            _toastNotification.AddInfoToastMessage("دسته بندی مورد نظر با موفقیت حذف شد.", new NotyOptions()
            {

                ProgressBar = true,
                Timeout = 1000,
                Layout = "topCenter",
                Theme = "metroui"
            });

            return RedirectToAction("AddCategory", "Category");
        }
        #endregion
    }
}
