using NewsWeb.PublicClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Models.ViewModels
{
    public class AddUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "نام کاربری (حروف انگلیسی)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = PublicConst.LengthMessage)]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور (حروف انگلیسی)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = PublicConst.LengthMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور (حروف انگلیسی)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = PublicConst.LengthMessage)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار آن یکسان نیست")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string LastName { get; set; }

        [Display(Name = "جنسیت")]
        public byte GenderSex { get; set; }

        [Display(Name = "موبایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل 11 رقمی می باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "شماره موبایل نمی تواند شامل حرف باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string Email { get; set; }

        [Display(Name = "تصویر کاربر")]
        public string UserImage { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string BirthDayDate { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string LastName { get; set; }

        [Display(Name = "جنسیت")]
        public byte GenderSex { get; set; }

        [Display(Name = "موبایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل 11 رقمی می باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "شماره موبایل نمی تواند شامل حرف باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string Email { get; set; }

        [Display(Name = "تصویر کاربر")]
        public string UserImage { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string BirthDayDate { get; set; }

        public bool ResetPass { get; set; }
    }
}
