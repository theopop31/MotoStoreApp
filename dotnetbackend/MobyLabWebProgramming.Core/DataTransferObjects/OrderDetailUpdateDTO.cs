using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class OrderDetailUpdateDTO
    {
        public Guid Id { get; set; }  // Reference to the specific OrderDetail ID
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }  // Allows updates to unit price if necessary
    }
}
