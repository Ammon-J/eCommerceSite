using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Add(int id) // This is the is to add to the cart
        {
            return View();
        }

        public IActionResult Summary()
        {
            // Display all the products in the shopping cart
            return View();
        }
    }
}
