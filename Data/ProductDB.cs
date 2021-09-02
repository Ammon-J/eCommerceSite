using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Data
{
    public static class ProductDB
    {
        /// <summary>
        /// Return the total count of products
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public async static Task<int> GetTotalProductsAync(ProductContext _context)
        {
            return await(from p in _context.Products
                  select p).CountAsync();
        }

        /// <summary>
        /// Get page worth of products
        /// </summary>
        /// <param name="_context">Databse to use</param>
        /// <param name="pageSize">Nukber of products per page</param>
        /// <param name="pageNum">Page of products to return</param>
        /// <returns></returns>
        public async static Task<List<Product>> GetProductsAsync(ProductContext _context, int pageSize, int pageNum)
        {
            return await (from p in _context.Products
                       orderby p.Title ascending
                       select p).Skip(pageSize * (pageNum - 1)) //Skip() must be before Take()
                 .Take(pageSize)
                 .ToListAsync();
        }

        /// <summary>
        /// Add the product to the db
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="p">Product the user created</param>
        /// <returns></returns>
        public static async Task<Product> AddProductAsync(ProductContext _context, Product p)
        {
            _context.Products.Add(p);
            await _context.SaveChangesAsync();

            return p;
        }

        public static async Task<Product> GetProductAsync(ProductContext context, int prodId)
        {
            Product p = await (from products in context.Products
                               where products.ProductId == prodId
                               select products).SingleAsync();
            return p;
        }
    }
}
