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
    public class PlanningStateBasementColumns : PlanningState
    {
        public PlanningStateBasementColumns(RuleCatalogue availableRules, CustomisationSettings projectParameters) : base(availableRules, projectParameters) { }

        public override List<RuleDefinition> DefineSequenceOfRuleApplications()
        {
            List<RuleDefinition> rulesToBeExecuted = new List<RuleDefinition>();

            //set four rules of column placement
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation"));
            //note: you(@Bene) may think about a concept to abbreviate such repetitive pattern definitions
            return rulesToBeExecuted;

                
        }

        public override List<Dictionary<string, double>> DefinePartParameters()
        {
            List<Dictionary<string, double>> partParameters = new List<Dictionary<string, double>>();
            if (this.RulesToBeExecuted.Count == 4)
            {
                foreach (RuleDefinition rule in this.RulesToBeExecuted)
                {
                    Dictionary<string, double> specifiedPartParams = new Dictionary<string, double>();
                    double storeyHeight = this.ProjectParameters.GetParameterValue("StoreyHeight");
                    specifiedPartParams["RH_IN:Height"] = storeyHeight; //set column height
                    partParameters.Add(specifiedPartParams); //only one param per rule specified
                }
            }
            return partParameters;
        }

    }
}
