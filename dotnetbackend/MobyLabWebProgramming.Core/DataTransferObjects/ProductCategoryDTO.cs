using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class ProductCategoryDTO
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
