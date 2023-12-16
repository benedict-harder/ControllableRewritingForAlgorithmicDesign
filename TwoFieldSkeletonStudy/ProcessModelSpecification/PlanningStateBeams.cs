using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmMetaModel;
using GrammarMetaModel;
using Rhino;

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

            int i = 12;
            while (i > 0) 
            {
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole1"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole2"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole3"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole4"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole5"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole6"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole7"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole8"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole9"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole10"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole11"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole12"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole13"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole14"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole15"));
                rulesToBeExecuted.Add(AvailableRules.Rules.First(r => r.Name == "BeamOnColumnConsole16"));
                i--;
            }
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
                //double length = i % 2 == 0 ? ProjectParameters.GetParameterValue("FieldLengthX") - width : ProjectParameters.GetParameterValue("FieldLengthY") - width;
                double length = 4.7;
                specifiedPartParams["RH_IN:Length"] = length;
                specifiedPartParams["RH_IN:Width"] = width;
                partParameters.Add(specifiedPartParams); //only one param per rule specified
                i++;
            }
            return partParameters;
        }

    }
}