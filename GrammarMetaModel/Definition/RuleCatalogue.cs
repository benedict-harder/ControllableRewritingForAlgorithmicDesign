using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GrammarMetaModel
{
    /// <summary>
    /// Class for a set of rules, managed by RuleCatalogueManager
    /// </summary>
    public class RuleCatalogue
    {
        public string CatalogueName { get; set; } = "";
        public List<RuleDefinition> Rules { get; set; }
        public List<AbstractPart> RegisteredModules { get; set; }

        public RuleCatalogue(string catalogueName, List<RuleDefinition> rules, List<AbstractPart> registeredModules)
        {
            CatalogueName = catalogueName;
            Rules = rules;
            RegisteredModules = registeredModules;
        }
    }
}
