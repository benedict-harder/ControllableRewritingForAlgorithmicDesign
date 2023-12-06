using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarMetaModel
{
    public abstract class AbstractPart
    {
        public string Name { get; set; }

        public AbstractPart(string name)
        {
            Name = name;
        }
    }
}
