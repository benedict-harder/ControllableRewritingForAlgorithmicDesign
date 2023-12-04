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
    public class PlanningStateBeams : PlanningState
    {
        public PlanningStateBeams(RuleCatalogue availableRules, CustomisationSettings projectParameters) : base(availableRules, projectParameters) { }

        public override List<RuleDefinition> DefineSequenceOfRuleApplications()
        {
            List<RuleDefinition> rulesToBeExecuted = new List<RuleDefinition>();

            //set four rules of column placement
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole1"));
            //note: you(@Bene) may think about a concept to abbreviate such repetitive pattern definitions
            return rulesToBeExecuted;
        }

        public override List<Dictionary<string, double>> DefinePartParameters()
        {
            List<Dictionary<string, double>> partParameters = new List<Dictionary<string, double>>();
            int i = 0;
            foreach (RuleDefinition rule in this.RulesToBeExecuted)
            {
                Dictionary<string, double> specifiedPartParams = new Dictionary<string, double>();
                double width = ProjectParameters.GetParameterValue("ColumnWidth");
                // alternate between fieldlengths used
                double length = i % 2 == 0 ? ProjectParameters.GetParameterValue("FieldLengthX") - width : ProjectParameters.GetParameterValue("FieldLengthY") - width;

                specifiedPartParams["RH_IN:Length"] = length;
                specifiedPartParams["RH_IN:Width"] = width;
                partParameters.Add(specifiedPartParams); //only one param per rule specified
                i++;
            }
            return partParameters;
        }

    }
}