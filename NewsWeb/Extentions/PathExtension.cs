using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Extentions
{
    public static class PathExtension
    {

        #region uploader

        public static string UploadImage = "/images/upload/userImage/";
        public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/images/upload/userImage");

        #endregion

        #region متد دریافت اطلاعات مربوط به تصویر آواتار کاربر در پنل مدیریت

        public static string UserAvatarOrigin = "/Admin/images/users/userAvatar/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/images/users/userAvatar/origin/");

        public static string UserAvatarThumbnail = "/Admin/images/users/userAvatar/Thumbnail/";
        public static string UserAvatarThumbnailServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/images/users/userAvatar/Thumbnail/");

        #endregion

        #region متد دریافت تصویر پیش فرض رمبوط به کاربر در پنل مدیریت

        public static string UserAvatarDefaultOrigin = "/Admin/images/users/defaultAvatar/LogoCircle.png";

        #endregion
    }
}
