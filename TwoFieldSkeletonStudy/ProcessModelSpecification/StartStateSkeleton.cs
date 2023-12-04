using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmMetaModel;
using GrammarMetaModel;
using Rhino.Geometry;

namespace TwoFieldSkeletonStudy
{
    /// <summary>
    /// places four foundations translated and rotated in the space.
    /// </summary>
    public class StartStateSkeleton : StartState
    {
        public StartStateSkeleton(Rectangle3d auxiliaryGeometry, Part startPart) : base(auxiliaryGeometry, startPart) { }

        public override List<Component> PlaceStartSymbol()
        {
            //get edges of rectangular plot
            Rectangle3d auxiliaryGeometry = (Rectangle3d)AuxiliaryGeometry;
            List<Vector3d> targetsForFoundations = new List<Vector3d>(); 
            List<double> rotationAnglesForFoundations = new List<double>();
            //in order to ensure columns are assembled differently at every corner
            for (int i=0; i<4; i++)
            {
                targetsForFoundations.Add(new Vector3d(auxiliaryGeometry.Corner(i))); //points are ccw ordered, departing from minx-miny 
                rotationAnglesForFoundations.Add(i*0.5*Math.PI); //a positive angle is ccw, i want to rotate the foundations cw 
            }

            //place the four foundations going clockwise, turning them cw around the z-axis 
            int j = 0;
            char rotationAxis = 'z';
            List<Component> startSymbol = new List<Component>();
            foreach (Vector3d target in targetsForFoundations)
            {
                Component foundation = RewritingHandler.CreateComponentInGlobalOrigin(this.StartPart);
                foundation.RotateAroundGlobalOrigin(rotationAnglesForFoundations.ElementAt(j), rotationAxis); //first rotate around origin, then translate (order is important!)
                foundation.Translate(target);
                startSymbol.Add(foundation);
                j++;
            }

            return startSymbol;
        }
    }

}
