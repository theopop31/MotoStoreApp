using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class ProductAddDTO
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
        public Guid? ProducerId { get; set; }
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
    }
}
