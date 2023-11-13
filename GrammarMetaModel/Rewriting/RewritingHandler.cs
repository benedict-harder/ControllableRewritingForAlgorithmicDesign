using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Rhino.Geometry;
using Rhino.FileIO;

namespace GrammarMetaModel
{
    /// <summary>
    /// Static class with a set of functions helping to perform matching and the geometric/topologic-semantic execution of a chosen rewriting rule.
    /// </summary>
    public static class RewritingHandler
    {

        /// <summary>
        ///  Applies a rule and returns the transformed assembly. If randomMatchIndex is set true, a random rule gets applied, otherwise the first one.
        ///  For better control, the method should be extended to be able to specify a certain component or componentInterface (or other criteria like minZ etc.) to restrict matches
        /// </summary>
        public static Assembly ApplyRule(Assembly assemblyToBeProcessed, RuleDefinition rule, RuleCatalogue availableRules)
        {
            RuleMatchingResult matches = new RuleMatchingResult(assemblyToBeProcessed, rule);
            Assembly processedAssembly = ExecuteRewriting(assemblyToBeProcessed, matches.GetMatch(false), rule.RhsModuleInterface, availableRules);

            return assemblyToBeProcessed;
        }


        /// <summary>
        /// Generates a component from a part and places it in the global origin (to be further transformed later)
        /// </summary>
        public static Component CreateComponentInGlobalOrigin(Part part)
        {
            Tuple<Mesh, List<Plane>> ComponentAndInterfaceGeos = ResthopperBridge.EvaluateDefinition(part.GhDefinitionFullPath, part.Parameters);
            Component component = new Component(part, ComponentAndInterfaceGeos.Item1);
            int i = 0;
            foreach (Plane interfaceGeo in ComponentAndInterfaceGeos.Item2)
            {
                ComponentInterface componentInterface = new ComponentInterface(part.Connections.ElementAt(i), component, interfaceGeo);
                component.AddConnection(componentInterface);
                i++;
            }

            return component;
        }


        /// <summary>
        ///  Creates the new component, transforms it in place and processes the design graph
        /// </summary>
        public static Assembly ExecuteRewriting(Assembly designGraph, ComponentInterface existingInterfaceToConnectTo, PartInterface newPartInterface, RuleCatalogue availableRules)
        {
            //create the component from the part with concrete geometry
            Component newlyAddedComponent = CreateComponentInGlobalOrigin(newPartInterface.ParentPart);
            designGraph.AggregatedModules.Add(newlyAddedComponent);

            ComponentInterface newComponentInterface = newlyAddedComponent.ComponentInterfaces.Where(ci => ci.TemplateInterface == newPartInterface).First();
            newlyAddedComponent.TransformPlaneToPlane(existingInterfaceToConnectTo, newComponentInterface);

            foreach (ComponentInterface componentInterface in newlyAddedComponent.ComponentInterfaces)
            {
                componentInterface.roundToTwoDecimals();
            }

            //Create the right topological links, also checking whether the other interfaces of the new component coincide with open interfaces in the assembly 
            existingInterfaceToConnectTo.OtherConnection = newComponentInterface;
            newComponentInterface.OtherConnection = existingInterfaceToConnectTo;


            //a beam may close two (column console) interfaces at the same time - so we should loop through the others to check for collisions
            bool checkConnectionsClosedAtTheSameTime = true;
            
            //if (newlyAddedComponent.ComponentTemplate.Name == "beam")
            //{
            //    checkConnectionsClosedAtTheSameTime = true;
            //}

            if (checkConnectionsClosedAtTheSameTime)
            {
                foreach (ComponentInterface iface in designGraph.GetAllOpenInterfaces()) 
                {
                    iface.roundToTwoDecimals();
                    foreach(ComponentInterface jface in newlyAddedComponent.ComponentInterfaces.Where(ci => ci.OtherConnection.IsPlaceholder == true).ToList()) 
                    {
                        if (availableRules.Rules.Any(r => r.LhsModuleInterface.Name == iface.TemplateInterface.Name && r.RhsModuleInterface.Name == jface.TemplateInterface.Name))
                        {
                            if (iface.ConnectionPlane.Origin == jface.ConnectionPlane.Origin)
                            {
                                jface.OtherConnection = iface;
                                iface.OtherConnection = jface;
                            }
                        }
                        //foreach (RuleDefinition rule in availableRules.Rules)
                        //{
                        //    if (rule.LhsModuleInterface.Name == iface.TemplateInterface.Name && rule.RhsModuleInterface.Name == jface.TemplateInterface.Name)
                        //    {
                        //        if (iface.ConnectionPlane.Origin == jface.ConnectionPlane.Origin)
                        //        {
                        //            jface.OtherConnection = iface;
                        //            iface.OtherConnection = jface;
                        //        }
                        //    }
                        //}
                    }
                }
            }

            return designGraph;
        }

    }
}
