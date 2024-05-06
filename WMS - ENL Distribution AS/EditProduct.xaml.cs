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
    public partial class EditProduct : Window
    {
        private DAL dal;
        public event Action<EditProduct> NextProduct;
        public event Action<EditProduct> PrevProduct;
        public event Action<Product> ProductUpdated;
        public EditProduct()
        {
            InitializeComponent();
            dal = new DAL();
        }

        public Product Products { get; set; }

        public void ShowProduct(Product prod)
        {
            try
            {
                Products = prod;

                if (Products != null)
                {
                    TbxProdId.Text = Products.productId.ToString() ?? string.Empty;
                    TbxName.Text = Products.productName ?? string.Empty;
                    TbxDescription.Text = Products.productDescription ?? string.Empty;
                    TbxPrice.Text = Products.productPrice.ToString() ?? string.Empty;
                    TbxAmount.Text = Products.productAmount.ToString() ?? string.Empty;
                    TbxPlacementId.Text = Products.productPlacementId ?? string.Empty;
                }
                else
                {

                    TbxProdId.Text = string.Empty;
                    TbxName.Text = string.Empty;
                    TbxDescription.Text = string.Empty;
                    TbxPrice.Text = string.Empty;
                    TbxAmount.Text = string.Empty;
                    TbxPlacementId.Text = string.Empty;
                }

                Show();
            }
            catch (Exception ex)
            {
                // Log or display the exception details for debugging
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NextProduct.Invoke(this);
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            PrevProduct.Invoke(this);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Products.productName = TbxName.Text;
                Products.productDescription = TbxDescription.Text;
                Products.productPrice = decimal.Parse(TbxPrice.Text);
                Products.productAmount = int.Parse(TbxAmount.Text);
                Products.productPlacementId = TbxPlacementId.Text;

                dal.UpdateProductInDatabase(Products);
                ProductUpdated?.Invoke(Products);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, "Are you sure you want to delete this product?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                dal.DeleteProductFromDatabase(Products);
                Close();
            }
        }
    }
}
