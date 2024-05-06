using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS___ENL_Distribution_AS.Class
{
    public class Order
    {
        public int orderId { get; set; }
        public string productName { get; set; }
        public string employeeName { get; set; }
        public int orderAmount { get; set; }
        public string orderStatus { get; set; }
        public bool IsEmployeeMissing { get; set; }
        public bool IsProductMissing { get; set; }

        public Order()
        {
            this.orderId = 0;
            this.productName = string.Empty;
            this.employeeName = string.Empty;
            this.orderAmount = 0;
            this.orderStatus = string.Empty;
        }
    }
}
