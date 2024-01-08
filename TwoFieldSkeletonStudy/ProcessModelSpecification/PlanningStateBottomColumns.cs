using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmMetaModel;
using GrammarMetaModel;

namespace TwoFieldSkeletonStudy
{

    /// <summary>
    /// Example of how to specify the abstract planning state and fundamentally specify which rules to apply with a specified parameter
    /// The complexity will certainly rise for more complex algorithmic design problems
    /// </summary>
    public class PlanningStateBottomColumns : PlanningState
    {
        public PlanningStateBottomColumns(RuleCatalogue availableRules, CustomisationSettings projectParameters) : base(availableRules, projectParameters) { }

        public override List<RuleDefinition> DefineSequenceOfRuleApplications()
        {
            List<RuleDefinition> rulesToBeExecuted = new List<RuleDefinition>();
            int nrOfFields = (int)ProjectParameters.GetParameterValue("NumberOfFields");
            int nrOfColumns = (int)Math.Pow(nrOfFields + 1, 2);
            int nrOfOuterColumns = nrOfFields * 4;
            int k = 0;
            int j = 0;
            for (int i = 0; i <nrOfColumns; i++)
            {
                if (i >= nrOfOuterColumns)
                {
                    rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation3"));
                }
                else if (i == 0 || k-nrOfFields == j)
                {
                    rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation1"));
                    j = k;
                }
                else
                {
                    rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation2"));
                }
                k++;
            }


            //note: you(@Bene) may think about a concept to abbreviate such repetitive pattern definitions
            return rulesToBeExecuted;
        }

        public override List<Dictionary<string, double>> DefinePartParameters()
        {
            List<Dictionary<string, double>> partParameters = new List<Dictionary<string, double>>();
            foreach (RuleDefinition rule in this.RulesToBeExecuted)
            {
                Dictionary<string, double> specifiedPartParams = new Dictionary<string, double>();
                double storeyHeight = this.ProjectParameters.GetParameterValue("StoreyHeight");
                double columnWidth = ProjectParameters.GetParameterValue("ColumnWidth");
                specifiedPartParams["RH_IN:Height"] = storeyHeight; //set column height
                partParameters.Add(specifiedPartParams); //only one param per rule specified
            }
            return partParameters;
        }
    }
}
