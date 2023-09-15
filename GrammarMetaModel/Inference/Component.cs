using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace GrammarMetaModel
{
    /// <summary>
    /// Objects of this type populate the assembly.  
    /// Every component/componentInterface object is a "part occurence" of a part/partInterface object, mostly set according to a RuleDefinition
    /// </summary>
    public class Component
    {
        public Part ComponentTemplate;
        public List<ComponentInterface> ComponentInterfaces = new List<ComponentInterface>();
        public Mesh MeshGeometry;

        public Component(Part relatedPart, Mesh meshGeometry)
        {
            ComponentTemplate = relatedPart;
            MeshGeometry = meshGeometry;
        }


        public void AddConnection(ComponentInterface componentInterface)
        {
            ComponentInterfaces.Add(componentInterface);
        }

        public void TransformPlaneToPlane(ComponentInterface existingInterface, ComponentInterface newInterface)
        {
            //compute transformation like it is done in WASP (please study the application logic with help of the wasp grasshopper plugin)
            Transform transformationMatrix = Transform.PlaneToPlane(newInterface.GetPlaneFlippedAtYAxis(),  existingInterface.ConnectionPlane);
            MeshGeometry.Transform(transformationMatrix);
            foreach(ComponentInterface componentInterface in ComponentInterfaces)
            {
                componentInterface.TransformInterface(transformationMatrix);
            }

        }

        public void Translate(Vector3d translation)
        {
            //transform geos
            bool valid = MeshGeometry.Translate(translation);
            foreach (ComponentInterface componentInterface in ComponentInterfaces)
            {
                componentInterface.TranslateInterface(translation);
            }
           

        }

        public void RotateAroundGlobalOrigin( double angleInRadians, char axis)
        {
            //define parameters
            Vector3d rotationAxis = new Vector3d();
            Point3d origin = new Point3d(0.0, 0.0, 0.0);
            if (axis == 'x') { rotationAxis = new Vector3d(1.0, 0.0, 0.0); }
            else if (axis == 'y') { rotationAxis = new Vector3d(0.0, 1.0, 0.0); }
            else if (axis == 'z') { rotationAxis = new Vector3d(0.0, 0.0, 1.0); }
            else { throw new ArgumentOutOfRangeException("axis", "insert x,y, or z as an axis"); }

            //transform geos
            MeshGeometry.Rotate(angleInRadians, rotationAxis, origin);
            foreach (ComponentInterface componentInterface in ComponentInterfaces)
            {
                componentInterface.RotateInterface(angleInRadians, rotationAxis, origin);
            }

        }

    }
}
