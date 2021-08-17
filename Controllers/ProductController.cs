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
        /// Displays a view that lists a page of products
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? id)
        {
            int pageNum = id ?? 1;
            const int pageSize = 3;

            ViewData["CurrentPage"] = pageNum;

            // Get the number of products form the db
            int numProducts = await ProductDB.GetTotalProductsAync(_context);
            // Devide the number of products by the desired amount of products to see on one page
            int totalPages = (int)Math.Ceiling((double)numProducts / pageSize);

            ViewData["MaxPage"] = totalPages;

            // Get the list of products
            List<Product> products = await ProductDB.GetProductsAsync(_context, pageSize, pageNum);

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
                await ProductDB.AddProductAsync(_context, p);
                // Holy cow that was easy! I can do this all day

                // TempData will not get deleted when you redirect to another page.
                // Use TempData to display a success message.
                TempData["Message"] = $"{p.Title} was added seccessfully";

                // Redirect back to the catalog
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get product with corrisponding id
            Product p =
                await (from prod in _context.Products
                 where prod.ProductId == id
                 select prod).SingleAsync();
            // pass product to view
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            if(ModelState.IsValid)
            {
                _context.Entry(p).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                ViewData["Updated"] = "Product updated successfully";
            }

            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product p =
                await (from prod in _context.Products
                       where prod.ProductId == id
                       select prod).SingleAsync();

            return View(p);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int ProductId)
        {
            Product p =
                await (from prod in _context.Products
                       where prod.ProductId == ProductId
                       select prod).SingleAsync();

            _context.Entry(p).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            TempData["Deleted"] = $"{p.Title} was deleted";

            return RedirectToAction("Index");
        }
    }
}
