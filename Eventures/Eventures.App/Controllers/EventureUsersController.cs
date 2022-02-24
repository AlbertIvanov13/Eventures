using Eventures.App.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.App.Controllers
{
    public class EventureUsersController : Controller
    {

        public async Task<IActionResult> Index()
        {
            var users = this.userManager.Users
                .Select(u => new UserListingViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,

                }).ToList();

            var adminIds = (await this.userManager.GetUser)
        }
    }
}
