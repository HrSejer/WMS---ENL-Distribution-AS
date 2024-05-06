using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WMS___ENL_Distribution_AS.View
{
    /// <summary>
    /// Interaction logic for MedarbejderView.xaml
    /// </summary>
    public partial class MedarbejderView : UserControl
    {
        private DAL dal;
        private List<Employee> employees = new List<Employee>();
        private int currentIndex = -1;
        private int employeeCount;
        private List<Employee> filteredEmployees = new List<Employee>();
        private DataTable dtEmployees;
        private DataTable dtOrders;

        public MedarbejderView()
        {
            InitializeComponent();
            dal = new DAL();
            LoadDataIntoCollection();
            dtEmployees = new DataTable("Employees");
            dtOrders = new DataTable("Orders");
            InitializeTextBox();
            AutoUpdateToDatabase();
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

        private void UpdateEmployeeCountLabel()
        {
            LblEmployee.Content = $"Employees: {employeeCount}";
        }

        private void LoadDataIntoCollection()
        {
            if (dtEmployees == null)
            {
                dtEmployees = new DataTable("Employees");
            }

            if (dtOrders == null)
            {
                dtOrders = new DataTable("Orders");
            }


            dtEmployees.Clear();
            dtOrders.Clear();


            dtEmployees = dal.ExecuteQuery("SELECT * FROM Employees");


            dtOrders = dal.ExecuteQuery("SELECT * FROM Orders");


            foreach (DataRow employeeRow in dtEmployees.Rows)
            {
                string fullName = $"{employeeRow["firstName"]} {employeeRow["lastName"]}";


                int completedOrdersCount = dtOrders.AsEnumerable()
                    .Count(order => order.Field<string>("employeeName") == fullName && order.Field<string>("orderStatus") == "Finished");


                employeeRow["completedOrders"] = completedOrdersCount;
            }


            employees = dtEmployees.AsEnumerable()
                .Select(row => new Employee
                {
                    employeeId = row.Field<int>("employeeId"),
                    firstName = row.Field<string>("firstName")!,
                    lastName = row.Field<string>("lastName")!,
                    mail = row.Field<string>("mail")!,
                    phoneNum = row.Field<string>("phoneNum")!,
                    employmentStatus = row.Field<string>("employmentStatus")!,
                    jobTitle = row.Field<string>("jobTitle")!,
                    completedOrders = row.Field<int>("completedOrders")!
                })
                .ToList();


            DgEmployee.ItemsSource = dtEmployees.DefaultView;

            employeeCount = employees.Count;
            UpdateEmployeeCountLabel();


            filteredEmployees = new List<Employee>(employees);
        }


        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = TbSearchBox.Text;

            List<Employee> filteredEmployees = employees.Where(emp =>
                emp.firstName.Contains(searchTerm) ||
                emp.lastName.Contains(searchTerm) ||
                emp.jobTitle.Contains(searchTerm) ||
                emp.employeeId.ToString().ToLower().Contains(searchTerm)
                ).ToList();

            DgEmployee.ItemsSource = filteredEmployees;

            currentIndex = -1;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row != null)
            {
                currentIndex = DgEmployee.SelectedIndex;

                if (currentIndex >= 0 && currentIndex < filteredEmployees.Count)
                {
                    var emp = filteredEmployees[currentIndex];
                    var editEmp = new EditEmployee();
                    Window ownerWindow = Window.GetWindow(this);
                    editEmp.Owner = ownerWindow;
                    editEmp.ShowEmployee(emp);
                    editEmp.PrevEmployee += EditEmp_PrevEmployee;
                    editEmp.NextEmployee += EditEmp_NextEmployee;

                    UpdateEmployeeCountLabel();
                }
            }
        }



        private void EditEmp_NextEmployee(EditEmployee editEmp)
        {
            currentIndex = (currentIndex + 1) % employees.Count;
            var emp = employees[currentIndex];
            editEmp.ShowEmployee(emp);
        }

        private void EditEmp_PrevEmployee(EditEmployee editEmp)
        {
            currentIndex = (currentIndex - 1 + employees.Count) % employees.Count;
            var emp = employees[currentIndex];
            editEmp.ShowEmployee(emp);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new addEmployee();
            addEmployeeWindow.ShowDialog();

            
            if (addEmployeeWindow.DialogResult.HasValue && addEmployeeWindow.DialogResult.Value)
            {
                var newEmployee = addEmployeeWindow.GetNewEmployee();

                employees.Add(newEmployee);
                dal.AddEmployeeToDatabase(newEmployee);
                UpdateEmployeeCountLabel();

                currentIndex = employees.IndexOf(newEmployee);
            }
        }

        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            LoadDataIntoCollection(); 
        }

        private void AutoUpdateToDatabase()
        {
            try
            {
               foreach (DataRowView row in DgEmployee.ItemsSource)
               {
                    Employee employee = new Employee
                    {
                        employeeId = Convert.ToInt32(row["employeeId"])!,
                        firstName = Convert.ToString(row["firstName"])!,
                        lastName = Convert.ToString(row["lastName"])!,
                        jobTitle = Convert.ToString(row["jobTitle"])!,
                        mail = Convert.ToString(row["mail"])!,
                        employmentStatus = Convert.ToString(row["employmentStatus"])!,
                        phoneNum = Convert.ToString(row["phoneNum"])!,
                        completedOrders = Convert.ToInt32(row["completedOrders"])!
                    };

                    dal.UpdateEmployeeInDatabase(employee);

               }
            }
            catch (Exception ex)
            {
              MessageBox.Show(Application.Current.MainWindow, $"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
