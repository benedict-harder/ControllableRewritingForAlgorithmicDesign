using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarMetaModel
{
    public class AggregatedPart: AbstractPart
    {
        public List<Part> Parts;
        public List<PartInterface> Connections;

        public AggregatedPart(string name, List<Part> parts, List<PartInterface> partInterface) : base (name)
        {
            Parts = parts;
            Connections = partInterface;
        }
    }
}
