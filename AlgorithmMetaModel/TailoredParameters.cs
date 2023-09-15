using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarMetaModel;

namespace AlgorithmMetaModel
{
    /// <summary>
    /// This class helps to formalize and encapsulate background information to compute specific parameters for a rewriting process
    /// </summary>
    public class TailoredParameters
    {
        public List<RuleDefinition> SequenceOfRuleApplications;
        public Dictionary<Part, Tuple<string, string>> PartParameters; //some (locally determinable) parameters are captured in rules, some (globally computed) parameters need to be added

        public TailoredParameters(List<RuleDefinition> sequenceOfRuleApplications, Dictionary<Part, Tuple<string, string>> partParameters)
        {
            SequenceOfRuleApplications = sequenceOfRuleApplications;
            PartParameters = partParameters;
        }
    }
}
