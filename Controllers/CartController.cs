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
            // Cookie key
            const string cartCookie = "CartCookie";

            Product p = await ProductDB.GetProductAsync(_context, id);

            // Get existing cart items
            string existingItems = _httpContext.HttpContext.Request.Cookies[cartCookie];

            List<Product> cartProducts = new List<Product>();
            if(existingItems != null)
            {
                // Get all the products in the cookie and return it in a list
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(existingItems);
            }

            // Add current products to existing cart
            cartProducts.Add(p);

            // Add products in a cookie
            string data = JsonConvert.SerializeObject(cartProducts);

            // Set up the cookie options
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            _httpContext.HttpContext.Response.Cookies.Append(cartCookie, data, options);

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Summary()
        {
            // Display all the products in the shopping cart
            string cookieData = _httpContext.HttpContext.Request.Cookies["CartCookie"];

            List<Product> cartProducts = JsonConvert.DeserializeObject<List<Product>>(cookieData);
            return View(cartProducts);
        }
    }
}
