using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public record ProducerUpdateDTO(Guid Id, string? ProducerName = default, string? ContactInfo = default);
}
