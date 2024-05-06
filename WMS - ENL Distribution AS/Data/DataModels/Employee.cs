using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace WMS___ENL_Distribution_AS.Class
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public DateTime createdDate { get; set; }
        public string employmentStatus { get; set; }

        public int employeeId { get; set; }
        public string phoneNum { get; set; }
        public int completedOrders { get; set; }

        
        public Employee()
        {
            this.firstName = string.Empty;
            this.lastName = string.Empty;
            this.jobTitle = string.Empty;
            this.mail = string.Empty;
            this.employeeId = 0;
            this.phoneNum = string.Empty;
            this.completedOrders = 0;
            this.employmentStatus = string.Empty;
            this.createdDate = DateTime.MinValue;
        }

        
        public Employee(string firstName, string lastName, string jobTitle, string mail, int employeeId, string phoneNum, int completedOrders, string employmentStatus, DateTime createdDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.jobTitle = jobTitle;
            this.mail = mail;
            this.employeeId = employeeId;
            this.phoneNum = phoneNum;
            this.completedOrders = completedOrders;
            this.employmentStatus = employmentStatus;
            this.createdDate = createdDate;
        }
    }
}
