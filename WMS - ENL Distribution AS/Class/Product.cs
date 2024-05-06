using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace WMS___ENL_Distribution_AS.Class
{
    [Table("Products")]
    public class Product
    {
        [Key]

        public Location location;
        public int productId { get; set; }
        public int productAmount { get; set; }
        public string productPlacementId { get; set; }

        public string productName { get; set; }
        public string productDescription { get; set; }
        public decimal productPrice { get; set; }


        public Product()
        {
            this.productId = 0;
            this.productAmount = 0;
            this.productPlacementId = string.Empty;
            this.productName = string.Empty;
            this.productDescription = string.Empty;
            this.productPrice = 0;

            this.location = null;
        }

        public Product(Location productLocation) : this()
        {
            location = productLocation;
            productPlacementId = location?.Placement() ?? string.Empty;
        }
    }
}
