using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarMetaModel;

namespace AlgorithmMetaModel
{
    /// <summary>
    /// There are different types of states (so far start, planning, assembling) which can be concatenated to create a more complex design algorithm.
    /// They share only that they have a link to the design graph, to the following state and some computation happening (which needs to be specified by the programmer)
    /// </summary>
    public abstract class State
    {
        public Assembly DesignGraph { get; set; }
        public State NextState { get; set; }

        public abstract void Compute();
    }
}
