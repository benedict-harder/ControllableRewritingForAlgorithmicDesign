using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarMetaModel
{
    /// <summary>
    /// This class encapsulates the rule matching process.
    /// It may additionally host methods to order the matches in different ways (for example along xyz-axis) and to chose a specific/random match for rule application
    /// This may be already necessary when needing to order the beam assembly in a skeleton field that is wider than high... 
    /// </summary>
    public class RuleMatchingResult
    {
        List<ComponentInterface> Matches = new List<ComponentInterface>();

        /// <summary>
        /// The constructor computes possible matches of the rule. Missing yet is a geometrical collision checking (see WASP), the method is topology/semantics only so far
        /// </summary>
        public RuleMatchingResult(Assembly designGraph, RuleDefinition rule)
        {
            //Get all open interfaces
            List<ComponentInterface> potentialInterfacesToAttachTo = designGraph.GetAllOpenInterfaces();

            //filter to those interfaces that match the lhs of the rule
            List<ComponentInterface> validInterfacesToAttachTo = new List<ComponentInterface>();

            foreach (ComponentInterface potentialInterface in potentialInterfacesToAttachTo)
            {
                if (rule.LhsModule == potentialInterface.ParentComponent.ComponentTemplate && rule.LhsModuleInterface == potentialInterface.TemplateInterface)
                {
                    validInterfacesToAttachTo.Add(potentialInterface);
                }
            }
            Matches = validInterfacesToAttachTo;
        }


        /// <summary>
        /// order matches by the x,y or z axis value of the matching component interfaces (maybe add functionality to also order in two axis (first x, then y) 
        /// </summary>
        /// <param name="axis"> 'x', 'y' and 'z'</param>
        public void OrderMatches(char axis)
        {
            throw new NotImplementedException();
        }

        public ComponentInterface GetMatch(bool randomMatch)
        {
            ComponentInterface match;
            if (randomMatch)
            {
                Random generator = new Random();
                int randomIndex = generator.Next(0, Matches.Count - 1);
                match = Matches.ElementAt(randomIndex);
            }
            else match = Matches.First();
            return match;
        }

    }
}
