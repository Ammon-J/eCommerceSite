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
    public class ProductController : Controller
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all the products
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            // Get all products form the db
            List<Product> products =
                await (from p in _context.Products
                 select p).ToListAsync();

            // Send the list of products to the view to be displayed
            return View(products);
        }

        // Page for adding a new item
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // After all the inforamation has been entered and add it to the database
        [HttpPost]
        public async Task<IActionResult> Add(Product p)
        {
            if(ModelState.IsValid)
            {
                // Add the entered product info to the database
                _context.Products.Add(p);
                await _context.SaveChangesAsync();
                // Holy cow that was easy! I can do this all day

                // TempData will not get deleted when you redirect to another page.
                // Use TempData to display a success message.
                TempData["Message"] = $"{p.Title} was added seccessfully";

                // Redirect back to the catalog
                return RedirectToAction("Index");
            }

            return View();
        }

        //public async Task<IActionResult> 
    }
}
