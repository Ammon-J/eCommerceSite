using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            // Get all products form the db
            List<Product> products = _context.Products.ToList();

            // Send the list of products to the view to be displayed0
            return View(products);
        }
    }
}
