using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarMetaModel;

namespace AlgorithmMetaModel
{

    /// <summary>
    /// Class that shall encapsulate the algorithmic design process. 
    /// The model shall consist out of semi-standardised blocks (start, planning, assembly etc.) whose concatenation and execution is coordinated by objects of this class
    /// </summary>
    public class ProcessModel
    {
        // Elements constituting the control process model
        public Assembly DesignGraph { get; set; }
        public List<State> States { get; set; } = new List<State>();
        public State ActiveState { get; set; }
        public RuleCatalogue RuleCatalogue { get; set; }
        

        public ProcessModel(RuleCatalogue ruleCatalogue)
        {
            RuleCatalogue = ruleCatalogue;
            DesignGraph = new Assembly();
           
        }

        public void AddStartState(StartState startState)
        {
            States.Add(startState);
            ActiveState = startState;
            startState.DesignGraph = DesignGraph;
            startState.NextState = null;
        }

        public void AddRelatedPlanningAndAssemblingState(PlanningState planningState, AssemblingState assemblingState)
        {
            States.Last().NextState = planningState; // assuming a linear one-after-the-other state sequuence for now, parallel
            States.Add(planningState);
            planningState.DesignGraph = DesignGraph;
            planningState.NextState = assemblingState;
            States.Add(planningState);
            assemblingState.DesignGraph = DesignGraph;
            assemblingState.NextState = null;
        }


        public void ExecuteModel()
        {
            bool endStateNotReached = true;
            while (endStateNotReached)
            {
                endStateNotReached = MakeStep();
            }
        }

        public bool MakeStep()
        {
            if (ActiveState != null)
            {
                ActiveState.Compute();
                ActiveState = ActiveState.NextState;
                return true;
            }

            else return false;
        }


    }
}

