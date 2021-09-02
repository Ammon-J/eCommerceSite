using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public CartController(ProductContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<IActionResult> Add(int id) // This is the is to add to the cart
        {
            Product p = await ProductDB.GetProductAsync(_context, id);

            // Create a cookie
            string data = JsonConvert.SerializeObject(p);

            // Set up the cookie options
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            _httpContext.HttpContext.Response.Cookies.Append("CartCookie", data, options);

            return RedirectToAction("Index", "Product"); ;
        }

        public IActionResult Summary()
        {
            // Display all the products in the shopping cart
            return View();
        }
    }
}
