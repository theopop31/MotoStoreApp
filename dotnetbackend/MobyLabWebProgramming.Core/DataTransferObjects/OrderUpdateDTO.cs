using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class OrderUpdateDTO
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public List<OrderDetailUpdateDTO> Details { get; set; } = new List<OrderDetailUpdateDTO>();
    }
}
