using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        CustomisationSettings customisationSettings;
        public StartStateSkeleton(Rectangle3d auxiliaryGeometry, Part startPart, CustomisationSettings projectParams) : base(auxiliaryGeometry, startPart) 
        {
            customisationSettings = projectParams;
        }

        public override List<Component> PlaceStartSymbol()
        {
            //get edges of rectangular plot
            Rectangle3d auxiliaryGeometry = (Rectangle3d)AuxiliaryGeometry;
            List<Vector3d> targetsForFoundations = new List<Vector3d>(); 
            List<double> rotationAnglesForFoundations = new List<double>();
            int nrOfFields = (int)customisationSettings.Parameters["NumberOfFields"];
            double zcoord = customisationSettings.Parameters["NumberOfBasementStoreys"] * customisationSettings.Parameters["StoreyHeight"] * (-1);

            targetsForFoundations.AddRange(recursiveTargets(auxiliaryGeometry, nrOfFields, zcoord));
            
            //place the four foundations going clockwise, turning them cw around the z-axis 
            int j = 0;
            char rotationAxis = 'z';
            List<Component> startSymbol = new List<Component>();
            foreach (Vector3d target in targetsForFoundations)
            {
                Component foundation = RewritingHandler.CreateComponentInGlobalOrigin(this.StartPart);
                //foundation.RotateAroundGlobalOrigin(rotationAnglesForFoundations.ElementAt(j), rotationAxis); //first rotate around origin, then translate (order is important!)
                foundation.Translate(target);
                startSymbol.Add(foundation);
                j++;
            }
            return startSymbol;
        }

        public List<Vector3d> recursiveTargets(Rectangle3d rect, int nrOfFields, double zcoord)
        {
            List<Vector3d> targetsForFoundations = new List<Vector3d>();
            if (nrOfFields <= 0)
            {
                targetsForFoundations.Add(new Vector3d(rect.X.Mid, rect.Y.Mid, zcoord));
                return targetsForFoundations;
            }
            double xSection = GetSection(nrOfFields, 'x', rect);
            double ySection = GetSection(nrOfFields, 'y', rect);

            int k = 0;
            int l = 0;
            double xcoord = rect.X.T0;
            double ycoord = rect.Y.T0;
            bool rec = false;
            while (!rec)
            {
                if (k <= nrOfFields - 1 && l == 0)
                {
                    targetsForFoundations.Add(new Vector3d(xcoord, ycoord, zcoord));
                    xcoord += xSection;
                    k++;
                }
                else if (k > nrOfFields - 1 && l <= nrOfFields - 1)
                {
                    targetsForFoundations.Add(new Vector3d(xcoord, ycoord, zcoord));
                    ycoord += ySection;
                    l++;
                }
                else if (k <= 2 * nrOfFields - 1 && l > nrOfFields - 1)
                {
                    targetsForFoundations.Add(new Vector3d(xcoord, ycoord, zcoord));
                    xcoord -= xSection;
                    k++;
                }
                else if (k > 2 * nrOfFields - 1 && l <= 2 * nrOfFields - 1)
                {
                    targetsForFoundations.Add(new Vector3d(xcoord, ycoord, zcoord));
                    ycoord -= ySection;
                    l++;
                }
                else
                {
                    double singleXSection = GetSection(nrOfFields, 'x', rect);
                    double singleYSection = GetSection(nrOfFields, 'y', rect);
                    Rectangle3d newRect = new Rectangle3d(Plane.WorldXY, new Point3d(rect.X.T0 + singleXSection, rect.Y.T0 + singleYSection, zcoord), new Point3d(rect.X.T1 - singleXSection, rect.Y.T1 - singleYSection, zcoord));
                    targetsForFoundations.AddRange(recursiveTargets(newRect, nrOfFields - 2, zcoord));
                    rec = true;
                }
            }
            return targetsForFoundations;
        }

        public double GetSection(int nrOfFields, char axis, Rectangle3d geom)
        {
            if (axis == 'x')
            {
                return (geom.X.T1 - geom.X.T0) / nrOfFields;
            }
            else if (axis == 'y')
            {
                return (geom.Y.T1 - geom.Y.T0) / nrOfFields;
            }
            else
            {
                throw new ArgumentOutOfRangeException("axis", "insert x or y as an axis");
            }
        }
    }

}
