using System.Collections.Generic;
using System.Linq;
using GrammarMetaModel;
using AlgorithmMetaModel;
using Rhino.Geometry;
using Rhino.FileIO;
using System;

namespace SkeletonCaseStudy
{
    class Program
    {
        /// <summary>
        /// Script illustrating (only) the fundamental building blocks of the algorithmic design framework (to be further expanded)
        /// To scale up the complexity of the method bit by bit, i would recommend to maybe proceed as follows:
        ///     Assemble entire field(single-storey) with beam and deck with varying x-y-z-extensions
        ///     Vary also number and height of storeys
        ///     Introduce possibility to vary number of fields ("intermediary column" module necessary...)
        ///     Generate two different fields with different number of storeys
        ///     ...
        /// </summary>
        static void Main(string[] args)
        {
            // For elegancy, the rule definition could be outsourced to a separate grasshopper definition file (maybe with some custom grasshopper components and previews etc. (compare WASP).
            // For now, i did it like this
            #region RuleDefinition

            // Part Definitions
            Part foundation = new Part("foundation", "foundation.ghx");
            Part column = new Part("column", "column.ghx");

            Part beam = new Part("beam", "beam.ghx");
            Part deck = new Part("deck", "deck.ghx");

            // Rule Definitions
            RuleDefinition columnOnFoundation = new RuleDefinition(
                "ColumnOnFoundation",
                foundation,
                foundation.Connections.First(),
                column.Connections.Where(c => c.Name == "Foundation").First(),
                column);

            //changed the rule to simply mention "beam" and "column" - but this cannot handle e.g. two interfaces to beams for one column at the moment.. 
            RuleDefinition beamOnColumnConsole1 = new RuleDefinition(
                "BeamOnColumnConsole1",
                column,
                column.Connections.Where(c => c.Name.StartsWith("Beam_1")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole2 = new RuleDefinition(
                "BeamOnColumnConsole2",
                column,
                column.Connections.Where(c => c.Name.StartsWith("Beam_2")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition deckOnColumn1 = new RuleDefinition(
                "DeckOnColumn",
                column,
                column.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                deck);

            RuleDefinition deckOnColumn2 = new RuleDefinition(
                "DeckOnColumn",
                column,
                column.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                deck);

            RuleDefinition deckOnColumn3 = new RuleDefinition(
                "DeckOnColumn",
                column,
                column.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_3")).First(),
                deck);

            RuleDefinition deckOnColumn4 = new RuleDefinition(
                "DeckOnColumn",
                column,
                column.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_4")).First(),
                deck);

            RuleDefinition columnOnDeck1 = new RuleDefinition(
                "ColumnOnDeck1",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_1")).First(),
                column.Connections.Where(c => c.Name == "Foundation").First(),
                column);

            RuleDefinition columnOnDeck2 = new RuleDefinition(
                "ColumnOnDeck2",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_2")).First(),
                column.Connections.Where(c => c.Name == "Foundation").First(),
                column);

            RuleDefinition columnOnDeck3 = new RuleDefinition(
                "ColumnOnDeck3",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_3")).First(),
                column.Connections.Where(c => c.Name == "Foundation").First(),
                column);

            RuleDefinition columnOnDeck4 = new RuleDefinition(
                "ColumnOnDeck4",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_4")).First(),
                column.Connections.Where(c => c.Name == "Foundation").First(),
                column);


            RuleCatalogue rules = new RuleCatalogue(
                "OneField", new List<RuleDefinition> { columnOnFoundation, beamOnColumnConsole1, beamOnColumnConsole2, deckOnColumn1, deckOnColumn2, deckOnColumn3, deckOnColumn4, columnOnDeck1, columnOnDeck2, columnOnDeck3, columnOnDeck4},
                 new List<Part> { foundation, column, beam, deck });
            #endregion RuleDefinition

            // The setup of the entire process model should happen prior to any rule execution i think. 
            // For now, the process model is only a linear one-after-the-other chain (of standardised blocks of type start/planning/assembly state).
            // Further types of states and non-linear things (like parallel/ if-else tracks) can be imagined)
            #region ProcessModelSetup
            // start symbol setup 
            CustomisationSettings projectParameters = new CustomisationSettings();
            projectParameters.Parameters["NumberOfFields"] = 1.0;
            projectParameters.Parameters["NumberOfStoreys"] = 1.0;
            projectParameters.Parameters["StoreyHeight"] = 3.5;
            projectParameters.Parameters["FieldLengthX"] = 10.0;
            projectParameters.Parameters["FieldLengthY"] = 14.0;
            projectParameters.Parameters["ColumnWidth"] = 0.3;
            Rectangle3d dummyPlot = new Rectangle3d(Plane.WorldXY, new Point3d(0.0, 0.0, -3.0), new Point3d(projectParameters.Parameters["FieldLengthX"], projectParameters.Parameters["FieldLengthY"], -3.0));

            //// process model
            ProcessModel skeletonProcessModel = new ProcessModel(rules);
            skeletonProcessModel.AddStartState(new StartStateSkeleton(dummyPlot, foundation));
            
            PlanningStateBottomColumns planningBottomColumnAssembly = new PlanningStateBottomColumns(rules, projectParameters);
            AssemblingState assemblingBottomColumns = new AssemblingState();
            skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningBottomColumnAssembly, assemblingBottomColumns);

            PlanningStateBeams planningBeams = new PlanningStateBeams(rules, projectParameters);
            AssemblingState assemmblingBeams = new AssemblingState();
            skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningBeams, assemmblingBeams);

            PlanningStateDeck planningDeck = new PlanningStateDeck(rules, projectParameters);
            AssemblingState assemblingDeck = new AssemblingState();
            skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningDeck, assemblingDeck);

            PlanningStateColumns planningColumns = new PlanningStateColumns(rules, projectParameters);
            AssemblingState assemblingColumns = new AssemblingState();
            skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningColumns, assemblingColumns);

            #endregion ProcessModelSetup

            bool endStateNotReached = true;
            //int i = 0;
            while (endStateNotReached)
            {
                endStateNotReached = skeletonProcessModel.MakeStep();
                //skeletonProcessModel.DesignGraph.SerializeToThreeDm(i); //For planning states, this produces a 3dm file without any geometric change happening --> to be fixed
                //i++;
            }

            skeletonProcessModel.DesignGraph.SerializeToThreeDm(0);

        }
    }
}


