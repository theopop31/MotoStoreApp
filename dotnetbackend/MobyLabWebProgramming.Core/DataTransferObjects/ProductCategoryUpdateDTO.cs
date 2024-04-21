using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class ProductCategoryUpdateDTO
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public List<Guid> ProductIds { get; set; } = new List<Guid>();  // List of product IDs to be associated with the category
    }
}
