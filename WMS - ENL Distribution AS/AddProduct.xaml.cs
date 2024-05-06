using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WMS___ENL_Distribution_AS.Class;

namespace WMS___ENL_Distribution_AS
{
  
    public partial class AddProduct : Window
    {
        private DAL dal;
        public Product Products { get; set; }
        public event Action<Product> ProductAdded;
        public AddProduct()
        {
            InitializeComponent();
            dal = new DAL();
        }

        public Product GetNewProduct()
        {
            decimal productPriceValue = decimal.Parse(TbxPrice.Text);
            int productAmountValue = int.Parse(TbxAmount.Text);
            
            return new Product()
            {
                productName = TbxName.Text,
                productDescription = TbxDescription.Text,
                productPrice = productPriceValue,
                productAmount = productAmountValue,
                productPlacementId = TbxPlacementId.Text

            };
        }

        private void BtnAddProd_Click(object sender, RoutedEventArgs e)
        {
            Products = GetNewProduct();
            if (Products != null)
            {
                dal.AddProductToDatabase(Products);
                ProductAdded?.Invoke(Products);
            }

            MessageBox.Show("Created a new product");
            Close();
        }
    }
}
