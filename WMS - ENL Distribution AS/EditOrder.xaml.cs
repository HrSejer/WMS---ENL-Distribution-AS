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
using System.Windows.Shapes;
using WMS___ENL_Distribution_AS.Class;

namespace WMS___ENL_Distribution_AS
{
    /// <summary>
    /// Interaction logic for EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        private DAL dal;
        public event Action<EditOrder> NextOrder;
        public event Action<EditOrder> PrevOrder;
        public event Action<Order> OrderUpdated;
        public EditOrder()
        {
            InitializeComponent();
            dal = new DAL();
            PopulateComboBoxEmp();
            PopulateComboBoxProd();
        }

        public Order Orders { get; set; }

        public void ShowOrder(Order Ord)
        {
            try
            {
                Orders = Ord;

                if (Orders != null)
                {
                    TbxOrderId.Text = Orders.orderId.ToString() ?? string.Empty;
                    CbxProductName.Text = Orders.productName ?? string.Empty;
                    CbxOrderStatus.Text = Orders.orderStatus ?? string.Empty;
                    TbxOrderAmount.Text = Orders.orderAmount.ToString() ?? string.Empty;
                    CbxEmployeeName.Text = Orders.employeeName ?? string.Empty;
                }
                else
                {

                    TbxOrderId.Text = string.Empty;
                    CbxProductName.Text = string.Empty;
                    CbxOrderStatus.Text = string.Empty;
                    TbxOrderAmount.Text = string.Empty;
                    CbxEmployeeName.Text = string.Empty;
                }

                Show();
            }
            catch (Exception ex)
            {
                // Log or display the exception details for debugging
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PopulateComboBoxEmp()
        {
                DataTable dataTable = dal.ExecuteQuery("SELECT * FROM Employees");
                
                List<string> employeeNames = dataTable.AsEnumerable()
                    .Select(row => $"{row.Field<string>("firstName")} {row.Field<string>("lastName")}")
                    .ToList();

                CbxEmployeeName.ItemsSource = employeeNames;
        }

        private void PopulateComboBoxProd()
        {
            DataTable dataTable = dal.ExecuteQuery("SELECT * FROM Products");

            List<string> productNames = dataTable.AsEnumerable()
                .Select(row => $"{row.Field<string>("productName")}")
                .ToList();

            CbxProductName.ItemsSource = productNames;
        }
        private void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, "Are you sure you want to delete this order?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                dal.DeleteOrderFromDatabase(Orders);
                Close();
            }
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Orders.productName = CbxProductName.Text;
                Orders.orderStatus = CbxOrderStatus.Text;
                Orders.orderAmount = int.Parse(TbxOrderAmount.Text);
                Orders.employeeName = CbxEmployeeName.Text;

                dal.UpdateOrderInDatabase(Orders);
                OrderUpdated?.Invoke(Orders);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NextOrder.Invoke(this);
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            PrevOrder.Invoke(this);
        }
    }
}
