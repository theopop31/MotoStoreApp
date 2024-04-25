using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public record ProductUpdateDTO(Guid id, 
        string? Name = default, 
        string? Description = default, 
        decimal? Price = default,
        int? Stock = default,
        string? ProducerName = default
        /*List<string>? Categories = default*/) { }
}
