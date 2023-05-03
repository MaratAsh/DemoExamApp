using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductArticleNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unit type identifier.
        /// </summary>
        /// <value>The unit type identifier.</value>
        public int UnitTypeId { get; set; }

        /// <summary>
        /// Gets or sets the product cost.
        /// </summary>
        /// <value>The product cost.</value>
        public decimal ProductCost { get; set; }

        /// <summary>
        /// Gets or sets the product maximum discount amount.
        /// </summary>
        /// <value>The product maximum discount amount.</value>
        public byte? ProductMaxDiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the product manufacturer identifier.
        /// </summary>
        /// <value>The product manufacturer identifier.</value>
        public int ProductManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the product supplier identifier.
        /// </summary>
        /// <value>The product supplier identifier.</value>
        public int ProductSupplierId { get; set; }

        /// <summary>
        /// Gets or sets the product category identifier.
        /// </summary>
        /// <value>The product category identifier.</value>
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the product discount amount.
        /// </summary>
        /// <value>The product discount amount.</value>
        public byte? ProductDiscountAmount { get; set; }

        [NotMapped]
        public SolidColorBrush ColorProductDiscountAmount => ProductDiscountAmount > 15 ? new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#7fff00")) : new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        public int ProductQuantityInStock { get; set; }
        public string ProductDescription { get; set; } = null!;
        public string? ProductPhoto { get; set; }
        [NotMapped]
        public string? ProductPhotoFromResources => "/Resources/" + ProductPhoto;


        /// <summary>
        /// Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        public virtual ProductCategory ProductCategory { get; set; } = null!;

    }
}
