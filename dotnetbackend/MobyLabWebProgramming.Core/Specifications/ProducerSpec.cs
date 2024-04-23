using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class ProducerSpec : BaseSpec<ProducerSpec, Producer>
    {
        public ProducerSpec(Guid id) : base(id)
        {
            Query.Where(p => p.Id == id);
        }
        public ProducerSpec(string ProducerName)
        {
            Query.Where(e => e.ProducerName == ProducerName);
        }
    }
}
