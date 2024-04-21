using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class ProducerDTO
    {
        public Guid Id { get; set; }
        public string ProducerName { get; set; }
        public string ContactInfo { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
