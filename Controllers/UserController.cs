using eCommerceSite.Data;
using eCommerceSite.Models;
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

                // Redirect to homepage if everything is valid
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid) 
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

            if(acc == null)
            {
                // Custom error message
                ModelState.AddModelError(string.Empty, "Account was not found with the given Username and Password");

                // No account found
                return View(model);
            }

            // Login 
            return RedirectToAction("Index", "Home");
        }
    }
}
