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
using WMS___ENL_Distribution_AS.View;
namespace WMS___ENL_Distribution_AS
{
    /// <summary>
    /// Interaction logic for addEmployee.xaml
    /// </summary>
    public partial class addEmployee : Window
    {
        private DAL dal;
        public Employee Employees { get; set; }
        public event Action<Employee> EmployeeAdded;
        public addEmployee()
        {
            InitializeComponent();
            dal = new DAL();
        }

        public Employee GetNewEmployee()
        {
            int completedOrdersValue = int.Parse(TbxCompletedOrders.Text);

                return new Employee
                {
                    firstName = TbxFirstName.Text,
                    lastName = TbxLastName.Text,
                    mail = TbxEmail.Text,
                    phoneNum = TbxPhone.Text,
                    completedOrders = completedOrdersValue,
                    employmentStatus = CbxEmploymentStatus.Text,
                    jobTitle = CbxJobTitle.Text,
                };
        }



        private void BtnAddEmp_Click(object sender, RoutedEventArgs e)
        {
            Employees = GetNewEmployee();
            if (Employees != null)
            {
                dal.AddEmployeeToDatabase(Employees);
                EmployeeAdded?.Invoke(Employees);
            }

            MessageBox.Show("Created a new Employee");
            Close();
            
        }
        
    }
}
