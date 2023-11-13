using GrammarMetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmMetaModel
{
    /// <summary>
    /// In the start state, the so-called start symbol (the initial components) for the following algorithmic design process is set. 
    ///  Auxiliary geometry and parameters can be processed and the first components with open interfaces are set in space.
    /// </summary>
    public abstract class StartState : State
    {
        public dynamic AuxiliaryGeometry { get; set; }
        public Part StartPart { get; set; }

        public StartState(dynamic auxiliaryGeometry, Part startPart)
        {
            this.AuxiliaryGeometry = auxiliaryGeometry;
            this.StartPart = startPart;
    }

        public override void Compute(RuleCatalogue availableRules)
        {
            List<Component> startSymbol = PlaceStartSymbol();
            this.DesignGraph.AggregatedModules = startSymbol;
        }

        public abstract List<Component> PlaceStartSymbol();

    }
}
