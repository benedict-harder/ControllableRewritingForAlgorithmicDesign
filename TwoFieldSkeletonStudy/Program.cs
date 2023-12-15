using System.Collections.Generic;
using System.Linq;
using GrammarMetaModel;
using AlgorithmMetaModel;
using Rhino.Geometry;
using Rhino.FileIO;
using System;

namespace TwoFieldSkeletonStudy
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
            Part column2 = new Part("column", "column.ghx");
            Part column3 = new Part("column", "column_3cons.ghx");
            Part column4 = new Part("column", "column_4cons.ghx");

            Part beam = new Part("beam", "beam.ghx");
            Part deck = new Part("deck", "deck.ghx");

            // Rule Definitions
            RuleDefinition columnOnFoundation1 = new RuleDefinition(
                "ColumnOnFoundation1",
                foundation,
                foundation.Connections.First(),
                column2.Connections.Where(c => c.Name == "Foundation").First(),
                column2);

            RuleDefinition columnOnFoundation2 = new RuleDefinition(
                "ColumnOnFoundation2",
                foundation,
                foundation.Connections.First(),
                column3.Connections.Where(c => c.Name == "Foundation").First(),
                column3);

            RuleDefinition columnOnFoundation3 = new RuleDefinition(
                "ColumnOnFoundation3",
                foundation,
                foundation.Connections.First(),
                column4.Connections.Where(c => c.Name == "Foundation").First(),
                column4);

            //changed the rule to simply mention "beam" and "column" - but this cannot handle e.g. two interfaces to beams for one column at the moment.. 
            RuleDefinition beamOnColumnConsole1 = new RuleDefinition(
                "BeamOnColumnConsole1",
                column2,
                column2.Connections.Where(c => c.Name.StartsWith("Beam_1")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole2 = new RuleDefinition(
                "BeamOnColumnConsole2",
                column2,
                column2.Connections.Where(c => c.Name.StartsWith("Beam_2")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition beamOnColumnConsole3 = new RuleDefinition(
                "BeamOnColumnConsole3",
                column3,
                column3.Connections.Where(c => c.Name.StartsWith("Beam_1")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole4 = new RuleDefinition(
                "BeamOnColumnConsole4",
                column3,
                column3.Connections.Where(c => c.Name.StartsWith("Beam_1")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition beamOnColumnConsole5 = new RuleDefinition(
                "BeamOnColumnConsole5",
                column3,
                column3.Connections.Where(c => c.Name.StartsWith("Beam_2")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole6 = new RuleDefinition(
                "BeamOnColumnConsole6",
                column3,
                column3.Connections.Where(c => c.Name.StartsWith("Beam_2")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition beamOnColumnConsole7 = new RuleDefinition(
                "BeamOnColumnConsole7",
                column3,
                column3.Connections.Where(c => c.Name.StartsWith("Beam_3")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole8 = new RuleDefinition(
                "BeamOnColumnConsole8",
                column3,
                column3.Connections.Where(c => c.Name.StartsWith("Beam_3")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition beamOnColumnConsole9 = new RuleDefinition(
                "BeamOnColumnConsole9",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_1")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole10 = new RuleDefinition(
                "BeamOnColumnConsole10",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_1")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition beamOnColumnConsole11 = new RuleDefinition(
                "BeamOnColumnConsole11",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_2")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole12 = new RuleDefinition(
                "BeamOnColumnConsole12",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_2")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition beamOnColumnConsole13 = new RuleDefinition(
                "BeamOnColumnConsole13",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_3")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole14 = new RuleDefinition(
                "BeamOnColumnConsole14",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_3")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition beamOnColumnConsole15 = new RuleDefinition(
                "BeamOnColumnConsole15",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_4")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                beam);

            RuleDefinition beamOnColumnConsole16 = new RuleDefinition(
                "BeamOnColumnConsole16",
                column4,
                column4.Connections.Where(c => c.Name.StartsWith("Beam_4")).First(),
                beam.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                beam);

            RuleDefinition deckOnColumn1 = new RuleDefinition(
                "DeckOnColumn1",
                column2,
                column2.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_1")).First(),
                deck);

            RuleDefinition deckOnColumn2 = new RuleDefinition(
                "DeckOnColumn2",
                column2,
                column2.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_2")).First(),
                deck);

            RuleDefinition deckOnColumn3 = new RuleDefinition(
                "DeckOnColumn3",
                column2,
                column2.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_3")).First(),
                deck);

            RuleDefinition deckOnColumn4 = new RuleDefinition(
                "DeckOnColumn4",
                column2,
                column2.Connections.Where(c => c.Name == "Deck").First(),
                deck.Connections.Where(c => c.Name.StartsWith("Column_4")).First(),
                deck);

            RuleDefinition columnOnDeck1 = new RuleDefinition(
                "ColumnOnDeck1",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_1")).First(),
                column2.Connections.Where(c => c.Name == "Foundation").First(),
                column2);

            RuleDefinition columnOnDeck2 = new RuleDefinition(
                "ColumnOnDeck2",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_2")).First(),
                column2.Connections.Where(c => c.Name == "Foundation").First(),
                column2);

            RuleDefinition columnOnDeck3 = new RuleDefinition(
                "ColumnOnDeck3",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_3")).First(),
                column2.Connections.Where(c => c.Name == "Foundation").First(),
                column2);

            RuleDefinition columnOnDeck4 = new RuleDefinition(
                "ColumnOnDeck4",
                deck,
                deck.Connections.Where(c => c.Name.StartsWith("TopColumn_4")).First(),
                column2.Connections.Where(c => c.Name == "Foundation").First(),
                column2);


            RuleCatalogue rules = new RuleCatalogue(
                "OneField", new List<RuleDefinition> { 
                    columnOnFoundation1, 
                    columnOnFoundation2, 
                    columnOnFoundation3, 
                    beamOnColumnConsole1, 
                    beamOnColumnConsole2, 
                    beamOnColumnConsole3, 
                    beamOnColumnConsole4, 
                    beamOnColumnConsole5, 
                    beamOnColumnConsole6,
                    beamOnColumnConsole7,
                    beamOnColumnConsole8,
                    beamOnColumnConsole9,
                    beamOnColumnConsole10,
                    beamOnColumnConsole11,
                    beamOnColumnConsole12,
                    beamOnColumnConsole13,
                    beamOnColumnConsole14,
                    beamOnColumnConsole15,
                    beamOnColumnConsole16,
                    deckOnColumn1, 
                    deckOnColumn2, 
                    deckOnColumn3, 
                    deckOnColumn4, 
                    columnOnDeck1, 
                    columnOnDeck2, 
                    columnOnDeck3, 
                    columnOnDeck4 },
                 new List<Part> { foundation, column2, column3, column4, beam, deck });
            #endregion RuleDefinition

            // The setup of the entire process model should happen prior to any rule execution i think. 
            // For now, the process model is only a linear one-after-the-other chain (of standardised blocks of type start/planning/assembly state).
            // Further types of states and non-linear things (like parallel/ if-else tracks) can be imagined)
            #region ProcessModelSetup
            // start symbol setup 
            CustomisationSettings projectParameters = new CustomisationSettings();
            projectParameters.Parameters["NumberOfFields"] = 2.0;
            projectParameters.Parameters["NumberOfStoreys"] = 2.0;
            projectParameters.Parameters["NumberOfBasementStoreys"] = 1.0;
            projectParameters.Parameters["StoreyHeight"] = 3.5;
            projectParameters.Parameters["FieldLengthX"] = 10.0;
            projectParameters.Parameters["FieldLengthY"] = 10.0;
            projectParameters.Parameters["ColumnWidth"] = 0.3;

            double zcoord = projectParameters.Parameters["NumberOfBasementStoreys"] * projectParameters.Parameters["StoreyHeight"] * (-1);
            Rectangle3d dummyPlot = new Rectangle3d(Plane.WorldXY, new Point3d(0.0, 0.0, zcoord), new Point3d(projectParameters.Parameters["FieldLengthX"], projectParameters.Parameters["FieldLengthY"], zcoord));

            //// process model
            ProcessModel skeletonProcessModel = new ProcessModel(rules);
            skeletonProcessModel.AddStartState(new StartStateSkeleton(dummyPlot, foundation, projectParameters));

            PlanningStateBottomColumns planningBottomColumnAssembly = new PlanningStateBottomColumns(rules, projectParameters);
            AssemblingState assemblingBottomColumns = new AssemblingState();
            skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningBottomColumnAssembly, assemblingBottomColumns);

            //PlanningStateBeams planningBottomBeams = new PlanningStateBeams(rules, projectParameters);
            //AssemblingState assemmblingBottomBeams = new AssemblingState();
            //skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningBottomBeams, assemmblingBottomBeams);

            //PlanningStateDeck planningBottomDeck = new PlanningStateDeck(rules, projectParameters);
            //AssemblingState assemblingBottomDeck = new AssemblingState();
            //skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningBottomDeck, assemblingBottomDeck);

            //int nrOfStoreys = Convert.ToInt16(projectParameters.Parameters["NumberOfStoreys"] + projectParameters.Parameters["NumberOfBasementStoreys"]) - 1;
            //for (int i = 0; i < nrOfStoreys; i++)
            //{
            //    PlanningStateColumns planningColumns = new PlanningStateColumns(rules, projectParameters);
            //    AssemblingState assemblingColumns = new AssemblingState();
            //    skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningColumns, assemblingColumns);

            //    PlanningStateBeams planningBeams = new PlanningStateBeams(rules, projectParameters);
            //    AssemblingState assemmblingBeams = new AssemblingState();
            //    skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningBeams, assemmblingBeams);

            //    PlanningStateDeck planningDeck = new PlanningStateDeck(rules, projectParameters);
            //    AssemblingState assemblingDeck = new AssemblingState();
            //    skeletonProcessModel.AddRelatedPlanningAndAssemblingState(planningDeck, assemblingDeck);
            //}


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


