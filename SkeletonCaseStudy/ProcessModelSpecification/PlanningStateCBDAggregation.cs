using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmMetaModel;
using GrammarMetaModel;

namespace SkeletonCaseStudy
{

    /// <summary>
    /// Example of how to specify the abstract planning state and fundamentally specify which rules to apply with a specified parameter
    /// The complexity will certainly rise for more complex algorithmic design problems
    /// </summary>
    public class PlanningStateCBDAggregation : PlanningState
    {
        public PlanningStateCBDAggregation(RuleCatalogue availableRules, CustomisationSettings projectParameters) : base(availableRules, projectParameters) { }

        public override List<RuleDefinition> DefineSequenceOfRuleApplications()
        {
            List<RuleDefinition> rulesToBeExecuted = new List<RuleDefinition>();

            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "cbd"));
            return rulesToBeExecuted;
        }

        public override List<Dictionary<string, double>> DefinePartParameters()
        {
            List<Dictionary<string, double>> partParameters = new List<Dictionary<string, double>>();
            return partParameters;
        }

    }
}