﻿using System;
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
    public class PlanningStateColumns : PlanningState
    {
        public PlanningStateColumns(RuleCatalogue availableRules, CustomisationSettings projectParameters) : base(availableRules, projectParameters) { }

        public override List<RuleDefinition> DefineSequenceOfRuleApplications()
        {
            List<RuleDefinition> rulesToBeExecuted = new List<RuleDefinition>();

            //set four rules of column placement
            int nrOfFields = (int)ProjectParameters.GetParameterValue("NumberOfFields");
            int nrOfColumns = (int)Math.Pow(nrOfFields + 1, 2);

            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn1"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn1"));

            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn3"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn3"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn3"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn3"));

            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));
            rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn2"));

            //for (int i = 0; i < nrOfColumns; i++)
            //{
                
                
            //    rulesToBeExecuted.Add(AvailableRules.Rules.First(c => c.Name == "ColumnOnColumn3"));
            //}
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
                specifiedPartParams["RH_IN:Height"] = storeyHeight; //set column height
                partParameters.Add(specifiedPartParams); //only one param per rule specified
            }
            return partParameters;
        }
    }
}
