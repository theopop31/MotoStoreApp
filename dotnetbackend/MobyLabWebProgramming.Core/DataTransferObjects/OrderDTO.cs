using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }  // Including user's name for display purposes
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();  // List of order details
    }
}
