﻿using AKCore.DataModel;
using AKCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKCore.Components
{
    public class MemberListViewComponent : ViewComponent
    {
        private readonly UserManager<AkUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MemberListViewComponent(
            UserManager<AkUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = (await _userManager.GetUsersInRoleAsync("Medlem")).Where(x => x.Instrument != null);
               
            var model = new MemberListModel
            {
                Users = users.OrderBy(x => x.FirstName).GroupBy(x => x.Instrument).ToList()
            };

            return View(model);
        }
    }
}