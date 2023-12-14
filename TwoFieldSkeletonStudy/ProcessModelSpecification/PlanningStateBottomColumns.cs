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
            int nrOfColumns = (int)Math.Pow(ProjectParameters.Parameters["NumberOfFields"] + 1, 2);
            //set four rules of column placement
            while (nrOfColumns > 0)
            {
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "ColumnOnFoundation"));
                nrOfColumns--;
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
                specifiedPartParams["RH_IN:SideLength"] = columnWidth;

                partParameters.Add(specifiedPartParams); //only one param per rule specified
                
            }
            return partParameters;
        }
    }
}
