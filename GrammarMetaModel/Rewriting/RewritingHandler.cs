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
            Assembly processedAssembly = ExecuteRewriting(assemblyToBeProcessed, matches.GetMatch(false), rule.RhsModuleInterface, availableRules, rule);

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
        public static Assembly ExecuteRewriting(Assembly designGraph, ComponentInterface existingInterfaceToConnectTo, PartInterface newPartInterface, RuleCatalogue availableRules, RuleDefinition rule)
        {
            AbstractComponent newlyAddedComponent;
            ComponentInterface newComponentInterface;
            if (!rule.isHierarchical)
            {
                //create the component from the part with concrete geometry
                newlyAddedComponent = CreateComponentInGlobalOrigin(newPartInterface.ParentPart);
                //designGraph.AggregatedModules.Add(newlyAddedComponent);
                newlyAddedComponent.AddToAssembly(designGraph);

                newComponentInterface = newlyAddedComponent.ComponentInterfaces.Where(ci => ci.TemplateInterface == newPartInterface).First();
                
            }
            else
            {
                List<AbstractComponent> existingAggregation = new List<AbstractComponent>();
                AggregatedPart template = (AggregatedPart)rule.LhsModule;
                foreach(KeyValuePair<Part, int> pair in template.PartsDictionary)
                {
                    existingAggregation.AddRange(designGraph.AggregatedModules.Where(m => m.ComponentTemplate == pair.Key).Take(pair.Value).ToList());
                }

                List<AbstractComponent> newAggregation = new List<AbstractComponent>(existingAggregation);

                newlyAddedComponent = new AggregatedComponent(template, newAggregation);
                newlyAddedComponent.AddConnection(newAggregation[0].ComponentInterfaces.Where(r => r.TemplateInterface.Name == "Foundation").First());
                newlyAddedComponent.AddConnection(newAggregation[8].ComponentInterfaces.Where(r => r.TemplateInterface.Name.StartsWith("TopColumn")).First());

                newlyAddedComponent.AddToAssembly(designGraph);
                existingInterfaceToConnectTo = existingAggregation[8].ComponentInterfaces.Where(r => r.TemplateInterface.Name.StartsWith("TopColumn")).First();
                newComponentInterface = newlyAddedComponent.ComponentInterfaces[0];

            }
            newlyAddedComponent.TransformPlaneToPlane(existingInterfaceToConnectTo, newComponentInterface);

            // floating point errors may occur after transformation
            // loop through all interfaces of the new component and round the origin coordinated
            foreach (ComponentInterface componentInterface in newlyAddedComponent.ComponentInterfaces)
            {
                componentInterface.roundToTwoDecimals();
            }

            //Create the right topological links, also checking whether the other interfaces of the new component coincide with open interfaces in the assembly 
            existingInterfaceToConnectTo.OtherConnection = newComponentInterface;
            newComponentInterface.OtherConnection = existingInterfaceToConnectTo;


            //a beam may close two (column console) interfaces at the same time - so we should loop through the others to check for collisions
            bool checkConnectionsClosedAtTheSameTime = true;

            if (checkConnectionsClosedAtTheSameTime)
            {
                // loop through all open interfaces in the assembly
                foreach (ComponentInterface iface in designGraph.GetAllOpenInterfaces()) 
                {
                    // get rid of floating point errors
                    iface.roundToTwoDecimals();
                    // loop through all open interfaces of the newly added component
                    foreach(ComponentInterface jface in newlyAddedComponent.ComponentInterfaces.Where(ci => ci.OtherConnection.IsPlaceholder == true).ToList()) 
                    {
                        // check if both the interface in the new component and the interface in the existing assembly
                        // are part of an established rule
                        if (availableRules.Rules.Any(r => r.LhsModuleInterface.Name == iface.TemplateInterface.Name && r.RhsModuleInterface.Name == jface.TemplateInterface.Name))
                        {
                            // check if the connection planes are "equal"
                            // only the origin is compared to eliminate problems with plane orientation
                            // to be determined if this is the correct approach, maybe a full comparison of the planes is actually necessary for some cases
                            if (iface.ConnectionPlane.Origin == jface.ConnectionPlane.Origin)
                            {
                                jface.OtherConnection = iface;
                                iface.OtherConnection = jface;
                            }
                        }
                    }
                }
            }

            return designGraph;
        }
    }
}
