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
    /// Interaction logic for EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        private DAL dal;
        public event Action<EditEmployee> NextEmployee;
        public event Action<EditEmployee> PrevEmployee;
        public event Action<Employee> EmployeeUpdated;
        public EditEmployee()
        {
            InitializeComponent();
            dal = new DAL();
        }

        public Employee Employees { get; set; }

        public void ShowEmployee(Employee emp)
        {
            try
            {
                Employees = emp;

                if (Employees != null)
                {
                    TbxEmpId.Text = Employees.employeeId.ToString() ?? string.Empty;
                    TbxFirstName.Text = Employees.firstName ?? string.Empty;
                    TbxLastName.Text = Employees.lastName ?? string.Empty;
                    TbxEmail.Text = Employees.mail ?? string.Empty;
                    TbxPhone.Text = Employees.phoneNum ?? string.Empty;
                    TbxEmpStatus.Text = Employees.employmentStatus ?? string.Empty;
                    CbxJobTitle.Text = Employees.jobTitle ?? string.Empty;
                    TbxCompletedOrders.Text = Employees.completedOrders.ToString() ?? string.Empty;
                }
                else
                {

                    TbxEmpId.Text = string.Empty;
                    TbxFirstName.Text = string.Empty;
                    TbxLastName.Text = string.Empty;
                    TbxEmail.Text = string.Empty;
                    TbxPhone.Text = string.Empty;
                    TbxEmpStatus.Text = string.Empty;
                    CbxJobTitle.Text = string.Empty;
                    TbxCompletedOrders.Text = string.Empty;
                }

                Show();
            }
            catch (Exception ex)
            {
                // Log or display the exception details for debugging
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            PrevEmployee.Invoke(this);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NextEmployee.Invoke(this);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, "Are you sure you want to delete this employee?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                dal.DeleteEmployeeFromDatabase(Employees); 
                Close();
            }
        }

        private void BtnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Employees.firstName = TbxFirstName.Text;
                Employees.lastName = TbxLastName.Text;
                Employees.mail = TbxEmail.Text;
                Employees.phoneNum = TbxPhone.Text;
                Employees.employmentStatus = TbxEmpStatus.Text;
                Employees.jobTitle = CbxJobTitle.Text;
                Employees.completedOrders = int.Parse(TbxCompletedOrders.Text);

                dal.UpdateEmployeeInDatabase(Employees);
                EmployeeUpdated?.Invoke(Employees);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
