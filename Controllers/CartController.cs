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

        // Add the selected item to the cart
        public async Task<IActionResult> Add(int id, string prevUrl) // This is the is to add to the cart
        {
            Product p = await ProductDB.GetProductAsync(_context, id);

            CookieHelper.addProductToCart(p, _httpContext);

            TempData["Message"] = p.Title + " added successfully!";

            return Redirect(prevUrl);
        }

        public IActionResult Summary()
        {
            return View(CookieHelper.GetCartProducts(_httpContext));
        }
    }
}
