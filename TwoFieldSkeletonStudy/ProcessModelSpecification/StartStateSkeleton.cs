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
            List<Tuple<Vector3d, double>> targetsForFoundations = new List<Tuple<Vector3d, double>>();
            int nrOfFields = (int)customisationSettings.Parameters["NumberOfFields"];
            double zcoord = customisationSettings.Parameters["NumberOfBasementStoreys"] * customisationSettings.Parameters["StoreyHeight"] * (-1);

            targetsForFoundations.AddRange(recursiveTargets(auxiliaryGeometry, nrOfFields, zcoord));
            
            //place the four foundations going clockwise, turning them cw around the z-axis 
            int j = 0;
            char rotationAxis = 'z';
            List<Component> startSymbol = new List<Component>();
            foreach (Tuple<Vector3d, double> target in targetsForFoundations)
            {
                Component foundation = RewritingHandler.CreateComponentInGlobalOrigin(this.StartPart);
                foundation.RotateAroundGlobalOrigin(target.Item2, rotationAxis); //first rotate around origin, then translate (order is important!)
                foundation.Translate(target.Item1);
                startSymbol.Add(foundation);
                j++;
            }
            return startSymbol;
        }

        public List<Tuple<Vector3d, double>> recursiveTargets(Rectangle3d rect, int nrOfFields, double zcoord)
        {
            List<Tuple<Vector3d, double>> targetsForFoundations = new List<Tuple<Vector3d, double>>();

            if (nrOfFields == 0)
            {
                Vector3d vec = new Vector3d(rect.X.Mid, rect.Y.Mid, zcoord);
                double angle = 0.0;
                targetsForFoundations.Add(Tuple.Create(vec, angle));
                return targetsForFoundations;
            }
            else if (nrOfFields < 0)
            {
                return new List<Tuple<Vector3d, double>>();
            }
            double xSection = GetSection(nrOfFields, 'x', rect);
            double ySection = GetSection(nrOfFields, 'y', rect);

            int k = 0;
            int l = 0;
            int p = 0;
            int r = 0;
            double xcoord = rect.X.T0;
            double ycoord = rect.Y.T0;
            bool rec = false;
            while (!rec)
            {
                
                if (k <= nrOfFields - 1 && l == 0)
                {
                    Vector3d vec = new Vector3d(xcoord, ycoord, zcoord);
                    double angle = r * 0.5 * Math.PI;
                    targetsForFoundations.Add(Tuple.Create(vec, angle));
                    xcoord += xSection;
                    k++;
                }
                else if (k > nrOfFields - 1 && l <= nrOfFields - 1)
                {
                    Vector3d vec = new Vector3d(xcoord, ycoord, zcoord);
                    double angle = r * 0.5 * Math.PI;
                    targetsForFoundations.Add(Tuple.Create(vec, angle));
                    ycoord += ySection;
                    l++;
                }
                else if (k <= 2 * nrOfFields - 1 && l > nrOfFields - 1)
                {
                    Vector3d vec = new Vector3d(xcoord, ycoord, zcoord);
                    double angle = r * 0.5 * Math.PI;
                    targetsForFoundations.Add(Tuple.Create(vec, angle));
                    xcoord -= xSection;
                    k++;
                }
                else if (k > 2 * nrOfFields - 1 && l <= 2 * nrOfFields - 1)
                {
                    Vector3d vec = new Vector3d(xcoord, ycoord, zcoord);
                    double angle = r * 0.5 * Math.PI;
                    targetsForFoundations.Add(Tuple.Create(vec, angle));
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
                if (p == nrOfFields - 1)
                {
                    p = 0;
                    r += 1;
                }
                else
                {
                    p += 1;
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
