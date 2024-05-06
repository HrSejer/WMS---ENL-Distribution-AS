using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private DAL dal;
        public Order Orders { get; set; }
        public event Action<Order> OrderAdded;
        public AddOrder()
        {
            InitializeComponent();
            dal = new DAL();
            PopulateComboBoxEmp();
            PopulateComboBoxProd();
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

        public Order GetNewOrder()
        {
            int orderAmountValue = int.Parse(TbxOrderAmount.Text);

            return new Order
            {
                productName = CbxProductName.Text,
                orderAmount = orderAmountValue,
                orderStatus = CbxOrderStatus.Text,
                employeeName = CbxEmployeeName.Text
            };
        }
        private void BtnAddOrd_Click(object sender, RoutedEventArgs e)
        {
            Orders = GetNewOrder();
            if (Orders != null)
            {
                dal.AddOrderToDatabase(Orders);
                OrderAdded?.Invoke(Orders);
            }

            MessageBox.Show("Created a new order");
            Close();
        }
    }
}
