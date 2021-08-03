using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    /// <summary>
    /// A salable product
    /// </summary>
    public class Product
    {
        [Key] // Makes PK in the database
        public int ProductId { get; set; }

        /// <summary>
        /// The consumer facing name of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The retail price as US Currency
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Category falls under. Ex. Electronics
        /// </summary>
        public string Category { get; set; }
    }
}
