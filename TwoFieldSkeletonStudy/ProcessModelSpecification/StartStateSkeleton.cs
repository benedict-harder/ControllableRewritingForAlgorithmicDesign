using System;
using System.CodeDom;
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

        /// <summary>
        /// this recursive method calculated the coordinated and rotation angles for the foundations. It is applicable for any symmetric configaration
        /// Basically, we always start with the outer targets in ccw direction, and the recursively invoke the method again, but with a smaller rectangle which is located inside the old/bigger one
        /// </summary>
        /// <param name="rect"></param> The plot
        /// <param name="nrOfFields"></param> the number of fields you want per side
        /// <param name="zcoord"></param> the z height (static)
        /// <returns></returns> a list of tuples, which contain vectors and angles
        public List<Tuple<Vector3d, double>> recursiveTargets(Rectangle3d rect, int nrOfFields, double zcoord)
        {
            // declare the return list
            List<Tuple<Vector3d, double>> targetsForFoundations = new List<Tuple<Vector3d, double>>();

            // this is our exit condition
            if (nrOfFields == 0)
            {
                // if it is excatly 0, we need to add a tagret right in the middle
                Vector3d vec = new Vector3d(rect.X.Mid, rect.Y.Mid, zcoord);
                double angle = 0.0;
                targetsForFoundations.Add(Tuple.Create(vec, angle));
                return targetsForFoundations;
            }
            else if (nrOfFields < 0)
            {
                // otherwise just return an empty list
                return new List<Tuple<Vector3d, double>>();
            }
            // retrieve the section lengths, depending on the nr of fields specified
            double xSection = GetSection(nrOfFields, 'x', rect);
            double ySection = GetSection(nrOfFields, 'y', rect);

            // init counting variables
            int k = 0;
            int l = 0;
            int p = 0;
            int r = 0;
            // retrieve the rectangle origin
            double xcoord = rect.X.T0;
            double ycoord = rect.Y.T0;
            bool rec = false;
            while (!rec)
            {
                // here we check for certain conditions which define the target coordinated and the rotation angle
                // in summary we count the number of targets that we have placed and then calculate the coords and angles
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
                    // if none of the above conditions are met, we invoke the method again
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

        /// <summary>
        /// This method divides a rectangle into equidistant sections
        /// </summary>
        /// <param name="nrOfFields"></param>
        /// <param name="axis"></param>
        /// <param name="geom"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
