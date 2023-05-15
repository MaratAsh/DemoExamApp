using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace WpfApp.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductArticleNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int UnitTypeId { get; set; }

    public decimal ProductCost { get; set; }
    [NotMapped]
    public string ProductCostL
    {
        get
        {
            return Convert.ToInt32(ProductCost).ToString();
        }
        set
        {
            decimal cost = Convert.ToDecimal(value);
            decimal coins = ProductCost * 10000 % 10000;
            ProductCost = cost + coins / 10000;
        }
    }
    [NotMapped]
    public string ProductCostR
    {
        get
        {
            var coins = Convert.ToInt32(ProductCost * 10000 % 10000);
            var coinsString = coins.ToString();
            while (coinsString.Length < 4)
                coinsString = "0" + coinsString;
            return coinsString;
        }
        set
        {
            decimal coins = Convert.ToDecimal(value);
            decimal cost = Convert.ToInt32(ProductCost);
            ProductCost = cost + coins / 10000;
        }
    }


    public byte? ProductMaxDiscountAmount { get; set; }

    public int ProductManufacturerId { get; set; }

    public int ProductSupplierId { get; set; }

    public int ProductCategoryId { get; set; }

    public byte? ProductDiscountAmount { get; set; }

    public int ProductQuantityInStock { get; set; }

    public string ProductDescription { get; set; } = null!;

    public string? ProductPhoto { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ProductCategory ProductCategory { get; set; } = null!;

    public virtual ProductManufacturer ProductManufacturer { get; set; } = null!;

    public virtual ProductSupplier ProductSupplier { get; set; } = null!;

    public virtual UnitType UnitType { get; set; } = null!;

    [NotMapped]
    public SolidColorBrush ColorProductDiscountAmount => ProductDiscountAmount > 15 ? new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#7fff00")) : new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
    [NotMapped]
    public string? ProductPhotoFromResources => "/Resources/" + ProductPhoto;
}
