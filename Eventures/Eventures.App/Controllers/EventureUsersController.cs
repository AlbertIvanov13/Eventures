using Eventures.App.Data;
using Eventures.App.Domain;
using Eventures.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.App.Controllers
{
    public class EventureUsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<EventuresUser> _userManager;

        public EventureUsersController(ApplicationDbContext _context, UserManager<EventuresUser> userManager)
        {
            this.context = _context;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users
                .Select(u => new UserListingViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,

                }).ToList();

            var adminIds = (await _userManager.GetUsersInRoleAsync("Administrator"))
                .Select(a => a.Id).ToList();

            foreach (var user in users)
            {
                user.IsAdmin = adminIds.Contains(user.Id);
            }

            var orderedUsers = users
                .OrderByDescending(u => u.UserName);

            return this.View(orderedUsers);
        }

        [HttpPost]

        public async Task<IActionResult> Promote(string userId)
        {
            if (userId == null)
            {
                return this.RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return this.RedirectToAction("Index");
            }

            await _userManager.AddToRoleAsync(user, "Administrator");

            return this.RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Demote(string userId)
        {
            if (userId == null)
            {
                return this.RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return this.RedirectToAction("Index");
            }

            await _userManager.RemoveFromRoleAsync(user, "Administrator");

            return this.RedirectToAction("Index");
        }
    }
}
