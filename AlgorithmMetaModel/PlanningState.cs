using GrammarMetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmMetaModel
{
    /// <summary>
    /// In a Planning State, it is defined which sequence of rule applications needs to be performed in the next step. 
    /// For every rule application, a set of part parameters can be set that the rule needs to be executed with. Undefined parameters are set to default values.
    /// </summary>
    public abstract class PlanningState : State
    {       
        public RuleCatalogue AvailableRules { get; set; }
        public CustomisationSettings ProjectParameters { get; set; }
        public List<RuleDefinition> RulesToBeExecuted { get; set; }
        public List<Dictionary<string, double>> PartParametersPerRule { get; set; }


        public PlanningState(RuleCatalogue availableRules, CustomisationSettings projectParameters)
        {
            AvailableRules = availableRules;
            ProjectParameters = projectParameters;
        }

        public override void Compute()
        {
            RulesToBeExecuted = DefineSequenceOfRuleApplications();
            PartParametersPerRule = DefinePartParameters();
            
            AssemblingState followingAssemblyState = (AssemblingState)NextState;
            followingAssemblyState.SetRuleApplicationStrategy(RulesToBeExecuted, PartParametersPerRule);
        }

        public abstract List<RuleDefinition> DefineSequenceOfRuleApplications();
        public abstract List<Dictionary<string, double>> DefinePartParameters();
    }
}
