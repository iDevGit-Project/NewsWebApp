using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsWeb.Models.DatabaseContext;
using NewsWeb.Models.UnitOfWork;
using NewsWeb.Models.ViewModels;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UserController : Controller
    {
        #region متدها و کلاس های سازنده اصلی کنترلر کاربران
        //متدهای مربوط به پایگاه داده، اطلاع رسانی به هنگام درج، ویرایش و حذف اطلاعات و استفاده از کلاس مدیریت کاربران 
        private readonly IUnitOfWork _iuw;
        private readonly IToastNotification _toastNotification;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly UserManager<ApplicationUsers> _userManager;

        public UserController(IUnitOfWork iuw, IToastNotification toastNotification,
            IHostingEnvironment HostEnvironment,
            UserManager<ApplicationUsers> userManager)
        {
            _iuw = iuw;
            _toastNotification = toastNotification;
            _HostEnvironment = HostEnvironment;
            _userManager = userManager;
        }
        #endregion

        #region متد نمایش لیست کاربران
        public IActionResult UserList(int page = 1)
        {
            int MovePage = (page - 1) * 5;
            int totalCount = _iuw.userManagerUW.Get().Count();
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
            return View(_iuw.userManagerUW.Get().Skip(MovePage).Take(5));
        }
        #endregion

        #region متد بارگزاری فایل عکس در سرور و ثبت آدرس در پایگاه داده
        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> files)
        {
            var upload = Path.Combine(_HostEnvironment.WebRootPath, "Admin\\images\\upload\\userImage\\normalImage\\");
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
            NewsWeb.Extentions.ImageResizer img = new NewsWeb.Extentions.ImageResizer();
            img.Resize(upload + filename, _HostEnvironment.WebRootPath + "\\Admin\\images\\upload\\userImage\\thumbnailimage\\" + filename);
            _toastNotification.AddInfoToastMessage("تصویر کاربر با موفقیت بارگزاری شد.", new NotyOptions()
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

        #region متد اضافه کردن کاربرجدید توسط کد های غیرهمزمانی
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel model, string imagename)
        {
            if (ModelState.IsValid)
            {
                if (imagename == null)
                {
                    model.UserImage = "defaultAvatar.jpg";
                }
                else
                {
                    model.UserImage = imagename;
                }

                ApplicationUsers user = new ApplicationUsers
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName,
                    Email = model.Email,
                    GenderSex = model.GenderSex,
                    BirthDayDate = model.BirthDayDate,
                    UserImagePath = model.UserImage
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("ثبت کاربر جدید با موفقیت انجام شد.", new NotyOptions()
                    {
                        ProgressBar = true,
                        Timeout = 1000,
                        Layout = "topCenter",
                        Theme = "metroui"
                    });
                    return RedirectToAction("UserList", "User");
                }
            }
            return View(model);
        }
        #endregion

        #region متد ویرایش اطلاعات کاربر
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel model = new EditUserViewModel();
            ApplicationUsers user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;
                model.PhoneNumber = user.PhoneNumber;
                model.BirthDayDate = user.BirthDayDate;
                model.GenderSex = user.GenderSex;
                model.UserImage = user.UserImagePath;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model, string id,
            string imagename, string chkinput)
        {
            if (ModelState.IsValid)
            {
                //Update
                ApplicationUsers user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.GenderSex = model.GenderSex;
                    user.BirthDayDate = model.BirthDayDate;
                    if (imagename != null) user.UserImagePath = imagename;

                    if (chkinput == "on")
                    {
                        user.PasswordHash = "AQAAAAEAACcQAAAAEHFGDrRKLP/TN2zQVGLgXoAK29f5pfM2OsWAMULM79x5JXChFRp10oqR1QBVIw3EIA==";
                    }

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {

                        _toastNotification.AddInfoToastMessage("اطلاعات حساب کاربری شما با موفقیت شد.", new NotyOptions()
                        {
                            ProgressBar = true,
                            Timeout = 1000,
                            Layout = "topCenter",
                            Theme = "metroui"
                        });
                        return RedirectToAction("UserList", "User");
                    }

                }
            }
            return View(model);
        }
        #endregion
    }
}
