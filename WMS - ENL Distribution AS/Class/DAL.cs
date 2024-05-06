using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WMS___ENL_Distribution_AS.Class;

namespace WMS___ENL_Distribution_AS.Class
{
    public class DAL
    {
        private readonly string connectionString;

        public DAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["EmployeeDatabaseConnection"].ConnectionString;
        }

        public DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        

        public void ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

       
        public void AddEmployeeToDatabase(Employee employee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Employees (firstName, lastName, jobTitle, mail, phoneNum, completedOrders, employmentStatus) " +
                                         "VALUES (@FirstName, @LastName, @JobTitle, @Mail, @PhoneNum, @CompletedOrders, @EmploymentStatus)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        
                        command.Parameters.AddWithValue("@FirstName", employee.firstName);
                        command.Parameters.AddWithValue("@LastName", employee.lastName);
                        command.Parameters.AddWithValue("@JobTitle", employee.jobTitle);
                        command.Parameters.AddWithValue("@Mail", employee.mail);
                        command.Parameters.AddWithValue("@PhoneNum", employee.phoneNum);
                        command.Parameters.AddWithValue("@CompletedOrders", employee.completedOrders);
                        command.Parameters.AddWithValue("@EmploymentStatus", employee.employmentStatus);

                       
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding employee to the database: {ex.Message}");
               
            }
        }

        public void DeleteEmployeeFromDatabase(Employee employee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string deleteQuery = "DELETE FROM Employees WHERE employeeId = @EmployeeId";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        
                        command.Parameters.AddWithValue("@EmployeeId", employee.employeeId);

                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting employee from the database: {ex.Message}");
            }
        }

        public void UpdateEmployeeInDatabase(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE Employees SET firstName = @FirstName, lastName = @LastName, phoneNum = @PhoneNum, mail = @Mail, employmentStatus = @EmploymentStatus, jobTitle = @JobTitle, completedOrders = @CompletedOrders WHERE employeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.employeeId);
                    command.Parameters.AddWithValue("@FirstName", employee.firstName);
                    command.Parameters.AddWithValue("@LastName", employee.lastName);
                    command.Parameters.AddWithValue("@PhoneNum", employee.phoneNum);
                    command.Parameters.AddWithValue("@Mail", employee.mail);
                    command.Parameters.AddWithValue("@EmploymentStatus", employee.employmentStatus);
                    command.Parameters.AddWithValue("@JobTitle", employee.jobTitle);
                    command.Parameters.AddWithValue("@CompletedOrders", employee.completedOrders);


                    command.ExecuteNonQuery();
                   //MessageBox.Show(Application.Current.MainWindow, "Now showing the most up to date version of the employee list");
                }
                
            }
        }

        public bool IsEmployeeExist(string employeeName)
        {
            bool employeeExists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Employees WHERE CONCAT(firstName, ' ', lastName) = @EmployeeName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeName", employeeName);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            employeeExists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking employee existence: {ex.Message}");
            }

            return employeeExists;
        }


        //PRODUKT SEKTION//

        public void AddProductToDatabase(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Products (productName, productDescription, productPrice, productAmount, productPlacementId) " +
                                         "VALUES (@ProductName, @ProductDescription, @ProductPrice, @ProductAmount, @ProductPlacementId)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {

                        command.Parameters.AddWithValue("@ProductName", product.productName);
                        command.Parameters.AddWithValue("@ProductDescription", product.productDescription);
                        command.Parameters.AddWithValue("@ProductPrice", product.productPrice);
                        command.Parameters.AddWithValue("@ProductAmount", product.productAmount);
                        command.Parameters.AddWithValue("@ProductPlacementId", product.productPlacementId);
                       
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding employee to the database: {ex.Message}");

            }
        }

        public void DeleteProductFromDatabase(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string deleteQuery = "DELETE FROM Products WHERE productId = @ProductId";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {

                        command.Parameters.AddWithValue("@ProductId", product.productId);


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting employee from the database: {ex.Message}");
            }
        }

        public void UpdateProductInDatabase(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE Products SET productName = @ProductName, productDescription = @ProductDescription, productPrice = @ProductPrice, productAmount = @ProductAmount, productPlacementId = @ProductPlacementId WHERE productId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", product.productId);
                    command.Parameters.AddWithValue("@ProductName", product.productName);
                    command.Parameters.AddWithValue("@ProductDescription", product.productDescription);
                    command.Parameters.AddWithValue("@ProductPrice", product.productPrice);
                    command.Parameters.AddWithValue("@ProductAmount", product.productAmount);
                    command.Parameters.AddWithValue("@ProductPlacementId", product.productPlacementId);

                    command.ExecuteNonQuery();
                    MessageBox.Show(Application.Current.MainWindow, "Successfully Updated");
                }


            }
        }

        public bool IsProductExist(string productName)
        {
            bool productExists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Products WHERE productName = @ProductName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            productExists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking product existence: {ex.Message}");
            }

            return productExists;
        }

        //ORDER SEKTION//

        public void AddOrderToDatabase(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertOrderQuery = "INSERT INTO Orders (productName, orderStatus, orderAmount, employeeName) " +
                                              "VALUES (@ProductName, @OrderStatus, @OrderAmount, @EmployeeName)";

                    using (SqlCommand command = new SqlCommand(insertOrderQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", order.productName);
                        command.Parameters.AddWithValue("@OrderStatus", order.orderStatus);
                        command.Parameters.AddWithValue("@OrderAmount", order.orderAmount);
                        command.Parameters.AddWithValue("@EmployeeName", order.employeeName);

                        command.ExecuteNonQuery();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order to the database: {ex.Message}");
            }
        }


        public void DeleteOrderFromDatabase(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string deleteQuery = "DELETE FROM Orders WHERE orderId = @OrderId";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {

                        command.Parameters.AddWithValue("@OrderId", order.orderId);


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting employee from the database: {ex.Message}");
            }
        }

        public void UpdateOrderInDatabase(Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE Orders SET productName = @ProductName, employeeName = @EmployeeName, orderStatus = @OrderStatus, orderAmount = @OrderAmount WHERE orderId = @OrderId", connection))
                {
                    command.Parameters.AddWithValue("@OrderId", order.orderId);
                    command.Parameters.AddWithValue("@ProductName", order.productName);
                    command.Parameters.AddWithValue("@EmployeeName", order.employeeName);
                    command.Parameters.AddWithValue("@OrderStatus", order.orderStatus);
                    command.Parameters.AddWithValue("@OrderAmount", order.orderAmount);
                    
                    command.ExecuteNonQuery();
                    MessageBox.Show(Application.Current.MainWindow, "Successfully Updated");
                }


            }
        }

        public List<Order> LoadOrdersFromDatabase()
        {
            List<Order> orders = new List<Order>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Orders";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Order order = new Order
                                {
                                    orderId = Convert.ToInt32(reader["orderId"]),
                                    productName = Convert.ToString(reader["productName"])!,
                                    orderAmount = Convert.ToInt32(reader["orderAmount"])!,
                                    orderStatus = Convert.ToString(reader["orderStatus"])!,
                                    employeeName = Convert.ToString(reader["employeeName"])!
                                };
                                orders.Add(order);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading orders from the database: {ex.Message}");
            }

            return orders;
        }

    }
}
