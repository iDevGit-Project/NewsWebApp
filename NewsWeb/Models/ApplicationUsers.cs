using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Models.DatabaseContext
{
    public class ApplicationUsers : IdentityUser
    {
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "جنسیت")]
        public byte GenderSex { get; set; }

        [Display(Name = "موبایل")]
        public override string PhoneNumber { get; set; }

        [Display(Name = "تصویر کاربر")]
        public string UserImagePath { get; set; }

        [Display(Name = "تاریخ تولد")]
        public string BirthDayDate { get; set; }
    }
}