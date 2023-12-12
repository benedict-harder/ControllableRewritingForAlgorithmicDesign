using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarMetaModel
{
    public class AggregatedPart: AbstractPart
    {
        //public List<Part> Parts;
        public Dictionary<Part, int> PartsDictionary = new Dictionary<Part, int>();
        public List<PartInterface> Connections;

        public AggregatedPart(string name, Dictionary<Part, int> partDict, List<PartInterface> partInterface) : base (name)
        {
            PartsDictionary = partDict;
            Connections = partInterface;
        }
    }
}
