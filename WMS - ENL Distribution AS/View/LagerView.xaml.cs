using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMS___ENL_Distribution_AS.Class;

namespace WMS___ENL_Distribution_AS.View
{

    public partial class LagerView : UserControl
    {
        private readonly DAL dal;
        private List<Product> products = new List<Product>();
        private int currentIndex = -1;
        private int productCount;
        private List<Product> filteredProducts = new List<Product>();
        public LagerView()
        {
            InitializeComponent();
            dal = new DAL();
            LoadDataIntoCollection();
            LoadDataIntoGrid();
            InitializeTextBox();
        }
        private void InitializeTextBox()
        {
            TbSearchBox_LostFocus(TbSearchBox, null);
        }
        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TbSearchBox.Text == "Search")
            {
                TbSearchBox.Text = "";
                TbSearchBox.Foreground = Brushes.Black;
            }
        }

        private void TbSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search";
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row != null)
            {
                currentIndex = DgProdukt.SelectedIndex;

                if (currentIndex >= 0 && currentIndex < filteredProducts.Count)
                {
                    var prod = filteredProducts[currentIndex];
                    var editProd = new EditProduct();
                    Window ownerWindow = Window.GetWindow(this);
                    editProd.Owner = ownerWindow;
                    editProd.ShowProduct(prod);
                    editProd.PrevProduct += EditProduct_PrevProduct;
                    editProd.NextProduct += EditProduct_NextProduct;

                    UpdateProductCountLabel();
                }
            }
        }

        private void EditProduct_NextProduct(EditProduct editProd)
        {
            currentIndex = (currentIndex + 1) % products.Count;
            var prod = products[currentIndex];
            editProd.ShowProduct(prod);
        }

        private void EditProduct_PrevProduct(EditProduct editProd)
        {
            currentIndex = (currentIndex - 1 + products.Count) % products.Count;
            var prod = products[currentIndex];
            editProd.ShowProduct(prod);
        }

        private void LoadDataIntoGrid()
        {
            try
            {
                string selectQuery = "SELECT * FROM Products";
                var dataTable = dal.ExecuteQuery(selectQuery);
                DgProdukt.ItemsSource = dataTable.DefaultView;

                productCount = dataTable.Rows.Count;
                UpdateProductCountLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataIntoCollection()
        {
            products = dal.ExecuteQuery("SELECT * FROM Products")
                .AsEnumerable()
                .Select(row => new Product(new Location())
                {
                    productId = row.Field<int>("productId"),
                    productName = row.Field<string>("productName")!,
                    productDescription = row.Field<string>("productDescription")!,
                    productPrice = row.Field<decimal>("productPrice")!,
                    productAmount = row.Field<int>("productAmount")!,
                    productPlacementId = row.Field<string>("productPlacementId")!

                })
                .ToList();

            filteredProducts = new List<Product>(products);
        }

        private void BtnProdukt_Click(object sender, RoutedEventArgs e)
        {
            LoadDataIntoCollection();
            LoadDataIntoGrid();
        }

        private void BtnAddProdukt_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProduct();
            addProductWindow.ShowDialog();


            if (addProductWindow.DialogResult.HasValue && addProductWindow.DialogResult.Value)
            {
                var newProduct = addProductWindow.GetNewProduct();

                products.Add(newProduct);
                dal.AddProductToDatabase(newProduct);
                UpdateProductCountLabel();

                currentIndex = products.IndexOf(newProduct);
            }
        }

        private void UpdateProductCountLabel()
        {
            LblProdukt.Content = $"Products: {productCount}";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = TbSearchBox.Text;

            List<Product> filteredProducts = products.Where(prod =>
                prod.productName.Contains(searchTerm) ||
                prod.productId.ToString().ToLower().Contains(searchTerm)
                ).ToList();

            DgProdukt.ItemsSource = filteredProducts;

            currentIndex = -1;
        }
    }
}
