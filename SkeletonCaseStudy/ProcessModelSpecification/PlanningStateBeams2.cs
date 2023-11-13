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
    public class PlanningStateBeams2 : PlanningState
    {
        public PlanningStateBeams2(RuleCatalogue availableRules, CustomisationSettings projectParameters) : base(availableRules, projectParameters) { }

        public override List<RuleDefinition> DefineSequenceOfRuleApplications()
        {
            List<RuleDefinition> rulesToBeExecuted = new List<RuleDefinition>();

            //set four rules of column placement
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole2"));
            //note: you(@Bene) may think about a concept to abbreviate such repetitive pattern definitions
            return rulesToBeExecuted;
        }

        public override List<Dictionary<string, double>> DefinePartParameters()
        {
            List<Dictionary<string, double>> partParameters = new List<Dictionary<string, double>>();
            foreach (RuleDefinition rule in this.RulesToBeExecuted)
            {
                Dictionary<string, double> specifiedPartParams = new Dictionary<string, double>();
                double length = ProjectParameters.GetParameterValue("BeamLength");
                specifiedPartParams["RH_IN:Length"] = length;
                partParameters.Add(specifiedPartParams); //only one param per rule specified
            }
            return partParameters;
        }

    }
}