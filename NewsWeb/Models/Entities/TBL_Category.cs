using NewsWeb.PublicClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Models
{
    public class TBL_Category
    {
        //دسته بندی
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "نام دسته بندی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(150, MinimumLength = 4, ErrorMessage = PublicConst.LengthMessage)]
        [RegularExpression(@"^[^\\/*\\)\(]+$", ErrorMessage = PublicConst.DangrouseMessageForBadCharachter)]
        public string Title { get; set; }


        [Display(Name = "توضیحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(800, MinimumLength = 5, ErrorMessage = PublicConst.LengthMessage)]
        //[RegularExpression(@"[0-9A-Za-z_\s\-\(\)\.]+", ErrorMessage = PublicConst.DangrouseMessageForBadCharachter)]
        [RegularExpression(@"^[^\\*\\)\(]+$", ErrorMessage = PublicConst.DangrouseMessageForBadCharachter)]
        public string Description { get; set; }
    }
}
