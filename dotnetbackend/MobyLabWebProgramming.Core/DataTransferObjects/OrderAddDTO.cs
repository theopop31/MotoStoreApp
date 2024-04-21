using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class OrderAddDTO
    {
        public Guid UserId { get; set; }
        public List<OrderDetailAddDTO> Details { get; set; } = new List<OrderDetailAddDTO>();
    }
}
