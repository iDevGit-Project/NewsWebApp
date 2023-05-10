using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Areas.AdminPanel.Views.ViewComponents
{

    #region Admin_BoxNoti_News
    public class Admin_BoxNoti_News : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Admin_BoxNoti_News");
        }
    }
    #endregion
}
