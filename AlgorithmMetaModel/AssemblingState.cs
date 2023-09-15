using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarMetaModel;

namespace AlgorithmMetaModel
{
    /// <summary>
    /// In an Assembling State, rules are applied sequentially according to what was set in the precedent planningState.
    /// </summary>
    public class AssemblingState : State
    {
        public List<RuleDefinition> RulesToBeExecuted { get; private set; }
        public List<Dictionary<string, double>> PartParametersForRuleApplication { get; private set; }
        public AssemblingState(){}

        public void SetRuleApplicationStrategy(List<RuleDefinition> rulesToBeExecuted, List<Dictionary<string, double>> partParametersPerRule)
        {
            RulesToBeExecuted = rulesToBeExecuted;
            PartParametersForRuleApplication = partParametersPerRule;
        }


        public override void Compute()
        {
            int i = 0;
            foreach (RuleDefinition rule in RulesToBeExecuted)
            {

                //adjusting the part parameters for the part
                Dictionary<string, double> ParameterSet = PartParametersForRuleApplication.ElementAt(i);

                if (ParameterSet.Count>0)
                {
                    Part correspondingPart = rule.RhsModule;
                    foreach (KeyValuePair<string, double> keyVal in ParameterSet)
                    {
                        //set parameter
                        correspondingPart.Parameters[keyVal.Key] = keyVal.Value;
                    }
                }
                
                //executing the rule
                DesignGraph = RewritingHandler.ApplyRule(DesignGraph, rule);
                i++;
            }

            
        }


    }
}
