using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMS___ENL_Distribution_AS.Class;

namespace WMS___ENL_Distribution_AS.View
{
    /// <summary>
    /// Interaction logic for OrdrerView.xaml
    /// </summary>
    public partial class OrdrerView : UserControl
    {
        private readonly DAL dal;
        private List<Order> orders = new List<Order>();
        private int currentIndex = -1;
        private int orderCount;
        private List<Order> filteredOrders = new List<Order>();
        public OrdrerView()
        {
            InitializeComponent();
            dal = new DAL();
            LoadDataIntoGrid();
            LoadDataIntoCollection();
            InitializeTextBox();
            LoadOrders();
            DgOrders.AutoGeneratingColumn += DgOrders_AutoGeneratingColumn!;
        }

        private void DgOrders_AutoGeneratingColumn (object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "IsEmployeeMissing")
            {
                e.Cancel = true;
            }

            if(e.PropertyName == "IsProductMissing")
            {
                e.Cancel = true;
            }
        }
        private void InitializeTextBox()
        {
            TbSearchBox_LostFocus(TbSearchBox, null!);
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
        private void LoadDataIntoGrid()
        {
            try
            {
                string selectQuery = "SELECT * FROM Orders";
                var dataTable = dal.ExecuteQuery(selectQuery);
                DgOrders.ItemsSource = dataTable.DefaultView;

                orderCount = dataTable.Rows.Count;
                UpdateOrderCountLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataIntoCollection()
        {
            orders = dal.ExecuteQuery("SELECT * FROM orders")
                .AsEnumerable()
                .Select(row => new Order
                {
                    orderId = row.Field<int>("orderId"),
                    productName = row.Field<string>("productName")!,
                    employeeName = row.Field<string>("employeeName")!,
                    orderAmount = row.Field<int>("orderAmount")!,
                    orderStatus = row.Field<string>("orderStatus")!
                    
                })
                .ToList();

            filteredOrders = new List<Order>(orders);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row != null)
            {
                currentIndex = DgOrders.SelectedIndex;

                if (currentIndex >= 0 && currentIndex < filteredOrders.Count)
                {
                    var ord = filteredOrders[currentIndex];
                    var editOrd = new EditOrder();
                    Window ownerWindow = Window.GetWindow(this);
                    editOrd.Owner = ownerWindow;
                    editOrd.ShowOrder(ord);
                    editOrd.PrevOrder += EditOrder_PrevOrder;
                    editOrd.NextOrder += EditOrder_NextOrder;

                    UpdateOrderCountLabel();
                }
            }
        }
        private void EditOrder_PrevOrder(EditOrder editOrd)
        {
            currentIndex = (currentIndex + 1) % orders.Count;
            var ord = orders[currentIndex];
            editOrd.ShowOrder(ord);
        }

        private void EditOrder_NextOrder(EditOrder editOrd)
        {
            currentIndex = (currentIndex - 1 + orders.Count) % orders.Count;
            var ord = orders[currentIndex];
            editOrd.ShowOrder(ord);
        }
        private void BtnAddOrders_Click(object sender, RoutedEventArgs e)
        {
            var addOrderWindow = new AddOrder();
            addOrderWindow.ShowDialog();


            if (addOrderWindow.DialogResult.HasValue && addOrderWindow.DialogResult.Value)
            {
                var newOrder = addOrderWindow.GetNewOrder();

                orders.Add(newOrder);
                dal.AddOrderToDatabase(newOrder);
                UpdateOrderCountLabel();

                currentIndex = orders.IndexOf(newOrder);
            }
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadDataIntoCollection();
            LoadDataIntoGrid();
        }

        private void UpdateOrderCountLabel()
        {
            LblOrder.Content = $"Orders: {orderCount}";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = TbSearchBox.Text;

            List<Order> filteredOrders = orders.Where(ord =>
                ord.productName.Contains(searchTerm) ||
                ord.employeeName.Contains(searchTerm) ||
                ord.orderId.ToString().ToLower().Contains(searchTerm)
                ).ToList();

            DgOrders.ItemsSource = filteredOrders;

            currentIndex = -1;
        }

        public void LoadOrders()
        {
            List<Order> orders = dal.LoadOrdersFromDatabase();
            List<Order> invalidOrders = new List<Order>();

            foreach(var order in orders)
            {
               order.IsEmployeeMissing = !dal.IsEmployeeExist(order.employeeName);
               order.IsProductMissing = !dal.IsProductExist(order.productName);
            }

            DgOrders.ItemsSource = orders;

            foreach (Order order in orders)
            {
                if(!dal.IsEmployeeExist(order.employeeName))
                {
                    invalidOrders.Add(order);
                }

                if(!dal.IsProductExist(order.productName))
                {
                    invalidOrders.Add(order);
                }

                if(invalidOrders.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("These orders are associated with a non-existent employee(s) and or product(s)");

                    foreach (Order invalidOrder in invalidOrders)
                    {
                        errorMessage.AppendLine($"Order ID: {invalidOrder.orderId}, Product: {invalidOrder.productName}, Employee: {invalidOrder.employeeName}");
                    }

                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        MessageBox.Show(errorMessage.ToString(), "Error - Invalid Orders", MessageBoxButton.OK, MessageBoxImage.Error);
                    }));
                }
            }
        }
    }
}
