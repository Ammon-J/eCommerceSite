using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductContext _context;

        public UserController(ProductContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if(ModelState.IsValid)
            {
                // Check if username/email is in use
                bool isEmailTaken = await (from account in _context.UserAccounts
                                             where account.Email == reg.Email
                                             select account).AnyAsync();

                // If email or username is taken add custom error and send back to ciew
                if(isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "That email is already in use");
                    return View(reg);
                }

                // Do the same as above but with username
                // Check if username/email is in use
                bool isUsernameTaken = await (from account in _context.UserAccounts
                                           where account.Username == reg.Username
                                           select account).AnyAsync();

                // If email or username is taken add custom error and send back to view
                if (isUsernameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Username), "That username is already in use");
                    return View(reg);
                }

                // Map data to user account instances
                UserAccount acc = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    Username = reg.Username
                };

                // Add the user acount to the db
                await _context.UserAccounts.AddAsync(acc);
                await _context.SaveChangesAsync();

                LogUserIn(acc.UserId);

                // Redirect to homepage if everything is valid
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }

        public IActionResult Login()
        {
            // Chack if user is already logged in
            if(HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // Gets the mathing account from the db
            UserAccount acc =
                await (from u in _context.UserAccounts
                       where (u.Username == model.UsernameOrEmail
                           || u.Email == model.UsernameOrEmail)
                           && u.Password == model.Password
                       select u).SingleOrDefaultAsync();

            if (acc == null)
            {
                // Custom error message
                ModelState.AddModelError(string.Empty, "Account was not found with the given Username and Password");

                // No account found
                return View(model);
            }

            LogUserIn(acc.UserId);

            return RedirectToAction("Index", "Home");
        }

        private void LogUserIn(int accountId)
        {
            // Login 
            // Create a session for the user
            HttpContext.Session.SetInt32("UserId", accountId);
        }

        public IActionResult Logout()
        {
            // Remove all current session data
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
