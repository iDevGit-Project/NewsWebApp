using NewsWeb.Models.DatabaseContext;
using NewsWeb.PublicClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Models
{
    public class TBL_News
    {
        //اخبار
        [Key]
        public int NewsId { get; set; }

        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(3000, MinimumLength = 5, ErrorMessage = PublicConst.LengthMessage)]
        [RegularExpression(@"^[^\\/*\\)\(]+$", ErrorMessage = PublicConst.DangrouseMessageForBadCharachter)]
        public string Title { get; set; }


        [Display(Name = "متن")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        public string Content { get; set; }


        [Display(Name = "چکیده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = PublicConst.EnterMessage)]
        [StringLength(3000, MinimumLength = 5, ErrorMessage = PublicConst.LengthMessage)]
        [RegularExpression(@"^[^\\/*\\)\(]+$", ErrorMessage = PublicConst.DangrouseMessageForBadCharachter)]
        public string Abstract { get; set; }

        [Display(Name = "بازدیدها")]
        public int VisitCount { get; set; }

        [Display(Name = "تاریخ")]
        public string NewsDate { get; set; }

        [Display(Name = "زمان")]
        public string NewsTime { get; set; }

        [Display(Name = "تصویر")]
        public string IndexImage { get; set; }

        public string UserID { get; set; }

        public int CategoryID { get; set; }


        [ForeignKey("UserID")]
        public virtual ApplicationUsers Users { get; set; }

        //[ForeignKey(nameof(CategoryID))]  OR  
        [ForeignKey(nameof(CategoryID))]
        public virtual TBL_Category tblCategory { get; set; }
    }
}
