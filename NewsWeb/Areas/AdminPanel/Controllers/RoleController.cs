﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsWeb.Models;
using NewsWeb.Models.DatabaseContext;
using NewsWeb.Models.Services;
using NewsWeb.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRoles> _roleManager;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IAspNetUserRoleRepository _iAppNetUserRepo;

        public RoleController(RoleManager<ApplicationRoles> roleManager, UserManager<ApplicationUsers> userManager, IAspNetUserRoleRepository iAppNetUserRepo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _iAppNetUserRepo = iAppNetUserRepo;
        }

        #region نمایش لیست دسترسی ها
        public IActionResult ListAccessControl()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            nodes.Add(new TreeViewNode
            {
                id = "asd",
                parent = "0",
                text = "اجزای سیستم"
            });

            foreach (ApplicationRoles subnode in _roleManager.Roles.Where(r => r.RoleLevel != "0"))
            {
                nodes.Add(new TreeViewNode
                {
                    id = subnode.Id.ToString(),
                    parent = subnode.RoleLevel.ToString(),
                    text = subnode.Description
                });
            }
            ///////////////////////////////////////
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
        #endregion

        #region ساخت دسترسی جدید
        [HttpGet]
        public IActionResult AddAccessControl()
        {
            ViewBag.SystemPart = _roleManager.Roles.ToList();
            ViewBag.ViewTitle = "ایجاد اجزای جدید";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAccessControl(ApplicationRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRoles role = new ApplicationRoles();
                role.Name = model.Name;
                role.Description = model.Description;
                role.RoleLevel = model.RoleLevel;

                IdentityResult roleResult = await _roleManager.CreateAsync(role);
                if (roleResult.Succeeded)
                {
                    ViewBag.ViewTitle = "تعریف اجزای سیستم";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        #endregion

        #region دریافت لیست دسترسی ها
        //[HttpGet]
        //public async Task<IActionResult> AccessRight(string Id)
        //{
        //    List<TreeViewNode> nodes = new List<TreeViewNode>();

        //    nodes.Add(new TreeViewNode
        //    {
        //        id = "asd",
        //        parent = "#",
        //        text = "اجزای سیستم"
        //    });

        //    foreach (ApplicationRoles subnode in _roleManager.Roles.Where(r => r.RoleLevel != "0"))
        //    {
        //        nodes.Add(new TreeViewNode
        //        {
        //            id = subnode.Id.ToString(),
        //            parent = subnode.RoleLevel.ToString(),
        //            text = subnode.Description
        //        });
        //    }
        //    ///////////////////////////////////////
        //    ViewBag.Json = JsonConvert.SerializeObject(nodes);
        //    ApplicationUsers user = await _userManager.FindByIdAsync(Id);
        //    if (user != null)
        //    {
        //        ViewBag.userId = Id;
        //        string getRoleid = _iau.GetRoleId(Id);
        //        if (getRoleid.Length > 0)
        //        {
        //            ViewBag.roleList = getRoleid.Substring(0, getRoleid.Length - 1);
        //        }

        //        ViewBag.ViewTitle = "ثبت دسترسی برای " + user.FirstName + " " + user.LastName;
        //        return View();
        //    }

        //    return NotFound();
        //}

        //[HttpPost]
        //public async Task<IActionResult> AccessRight(string selectedItems, string userId)
        //{
        //    List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
        //    ApplicationUsers user = await _userManager.FindByIdAsync(userId);
        //    if (user != null)
        //    {
        //        //Delete All Roles For user
        //        var roles = await _userManager.GetRolesAsync(user);
        //        IdentityResult delRoleResult = await _userManager.RemoveFromRolesAsync(user, roles);
        //        if (delRoleResult.Succeeded)
        //        {
        //            for (int i = 0; i <= items.Count - 1; i++)
        //            {
        //                //insert Roles for user
        //                ApplicationRoles approle = await _roleManager.FindByIdAsync(items[i].id);
        //                if (approle != null)
        //                {
        //                    IdentityResult roleresult = await _userManager.AddToRoleAsync(user, approle.Name);
        //                    if (roleresult.Succeeded)
        //                    {
        //                        //return RedirectToAction("Index", "User");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index", "User");
        //}
        #endregion
    }
}