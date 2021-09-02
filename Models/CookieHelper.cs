using eCommerceSite.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public static class CookieHelper
    {

        // Cookie key for accessing the cookie
        const string cartCookie = "CartCookie";

        /// <summary>
        /// Returns the current list of products
        /// </summary>
        /// <param name="http"></param>
        /// <returns>An empty list if cart is empty</returns>
        public static List<Product> GetCartProducts(IHttpContextAccessor http)
        {

            // Get existing cart items
            string existingItems = http.HttpContext.Request.Cookies[cartCookie];

            List<Product> cartProducts = new List<Product>();
            if (existingItems != null)
            {
                // Get all the products in the cookie and return it in a list
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(existingItems);
            }

            return cartProducts;
        }

        // Add product to cart
        public static void addProductToCart(Product p, IHttpContextAccessor http)
        {
            List<Product> cartProducts = GetCartProducts(http);
            cartProducts.Add(p);

            string data = JsonConvert.SerializeObject(cartProducts);

            // Set up the cookie options
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            http.HttpContext.Response.Cookies.Append(cartCookie, data, options);
        }

        // How many items are in the cart?
        public static int GetNumberOfCartItems(IHttpContextAccessor http)
        {
            List<Product> cartProducts = GetCartProducts(http);
            return cartProducts.Count;
        }
    }
}
