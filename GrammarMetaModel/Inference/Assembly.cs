using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Rhino.FileIO;
using Rhino.Geometry;
using Rhino.DocObjects;

namespace GrammarMetaModel
{
    /// <summary>
    ///  Assembly aggregating components by applying rules. Eventually here or in the rules another level of hierarchy may be added (grouping a few modules and their assembly) (see WASP) 
    /// </summary>
    public class Assembly
    {
        public List<Component> AggregatedModules { get; set; } = new List<Component>();

        public Assembly()
        {       
        }

        //loops through all components to find the open interfaces
        public List<ComponentInterface> GetAllOpenInterfaces()
        {
            List<ComponentInterface> interfaces = new List<ComponentInterface>();
            foreach(Component component in AggregatedModules)
            {
                interfaces.AddRange(component.ComponentInterfaces.Where(ci => ci.OtherConnection.IsPlaceholder==true).ToList());
            }
            return interfaces;
        }


        //generates a 3dm file with meshes, planes, text for plane orientation and lines between 
        public bool SerializeToThreeDm(int fileIndex)
        {
            File3dm outputFile = new File3dm();
            File3dmObjectTable objects = outputFile.Objects;
            List<Component> plottedComponents = new List<Component>();

            foreach (Component component in AggregatedModules)
            {
                //add mesh
                objects.AddMesh(component.MeshGeometry);
                Point3d rhinoGeoMidPoint = Rhino.Compute.AreaMassPropertiesCompute.Compute(component.MeshGeometry).Centroid;
                ObjectAttributes attributes = new ObjectAttributes();

                //add midpoint of mesh in red to visualise assembly graph
                attributes.ObjectColor = Color.Red;
                attributes.ColorSource = ObjectColorSource.ColorFromObject;
                if (!plottedComponents.Contains(component)) objects.AddPoint(rhinoGeoMidPoint, attributes);

                foreach (ComponentInterface componentInterface in component.ComponentInterfaces)
                {
                    //add planes as plane surfaces (normal planes are not rhino, but only GH-objects. Thus, add casted planar surfaces and a text stating the orientation of the plane.
                    PlaneSurface planeAsSurface = new PlaneSurface(componentInterface.ConnectionPlane, new Interval(-0.2, 0.2), new Interval(-0.2, 0.2));
                    objects.AddSurface(planeAsSurface);
                    objects.AddTextDot(
                        string.Format("plane with x-axis {0} and y-axis {1}", componentInterface.ConnectionPlane.XAxis.ToString(), componentInterface.ConnectionPlane.YAxis.ToString()),
                        componentInterface.ConnectionPlane.Origin);

                    //add red line between midpoints of connected parts to visualise assembly graph
                    if (!componentInterface.OtherConnection.IsPlaceholder && !(plottedComponents.Contains(componentInterface.OtherConnection.ParentComponent)))
                    {
                        Point3d connectedComponentCenter = Rhino.Compute.AreaMassPropertiesCompute.Compute(componentInterface.OtherConnection.ParentComponent.MeshGeometry).Centroid;
                        objects.AddLine(new Line(rhinoGeoMidPoint, connectedComponentCenter), attributes);
                    }
                }
                plottedComponents.Add(component);
            }
            string path = string.Format(@"C:\Users\Benedict\Documents\repos\ControllableRewritingForAlgorithmicDesign\SkeletonCaseStudy\ThreeDmModelsGenerated\testModel{0}.3dm", fileIndex);
            outputFile.Write(path, 0);
            return true;
        }
    
    }

}
