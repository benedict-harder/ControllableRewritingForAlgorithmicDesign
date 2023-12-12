using System;
using System.Collections.Generic;
using System.Linq;

namespace GrammarMetaModel
{
    /// <summary>
    /// class for the definition of a single rule. Only RHS 
    /// </summary>
    public class RuleDefinition
    {
        public string Name;
        public bool isHierarchical = false;
        public AbstractPart LhsModule;
        public PartInterface LhsModuleInterface;
        public PartInterface RhsModuleInterface;
        public AbstractPart RhsModule;

        public RuleDefinition(string name, AbstractPart lhsModule, PartInterface lhsModuleInterface, PartInterface rhsModuleInterface, AbstractPart rhsModule)
        {
            Name = name;
            isHierarchical = true;
            LhsModule = lhsModule;
            LhsModuleInterface = lhsModuleInterface;
            RhsModuleInterface = rhsModuleInterface;
            RhsModule = rhsModule;
        }
        public RuleDefinition(string name, Part lhsModule, PartInterface lhsModuleInterface, PartInterface rhsModuleInterface, Part rhsModule)
        {
            Name = name;
            LhsModule = lhsModule;
            LhsModuleInterface = lhsModuleInterface;
            RhsModuleInterface = rhsModuleInterface;
            RhsModule = rhsModule;
        }

        //public RuleDefinition(string name, Part lhsModule, PartInterface lhsModuleInterface, PartInterface rhsModuleInterface, Part rhsModule, List<string> paramValues)
        //{
        //    Name = name;
        //    isHierarchical = false;
        //    LhsModule = lhsModule;
        //    LhsModuleInterface = lhsModuleInterface;
        //    RhsModuleInterface = rhsModuleInterface;
        //    RhsModule = rhsModule;
        //}

        //public RuleDefinition(string name, Part lhsModule, List<PartInterface> lhsModuleInterfaces, PartInterface rhsModuleInterface, Part rhsModule)
        //{
        //    Name = name;
        //    LhsModule = lhsModule;
        //    LhsModuleInterfaceList = lhsModuleInterfaces;
        //    LhsModuleInterface = null;
        //    RhsModuleInterface = rhsModuleInterface;
        //    RhsModule = rhsModule;
        //}

    }
}
