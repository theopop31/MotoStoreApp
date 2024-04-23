using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class OrderAddDTO
    {
        public string Status { get; set; } = default!;
        public DateTime OrderDate { get; set; }
    }
}
