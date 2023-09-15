using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace GrammarMetaModel
{
    /// <summary>
    /// This is a concrete interface, hosted by a concrete part (a component) and instantiated according to the partInterface attached to the part
    /// a component interface object can be translated, rotated (what may be useful during the start symbol generation) or generically transformed (what happens if the component is transformed)
    /// </summary>
    public class ComponentInterface
    {
        public PartInterface TemplateInterface { get; set; }
        public Component ParentComponent { get; set; }
        public ComponentInterface OtherConnection { get; set; }
        public Plane ConnectionPlane { get; set; }
        public bool IsPlaceholder { get; set; }


        public ComponentInterface(PartInterface templateInterface, Component parentComponent, Plane connectionPlane)
        {
            TemplateInterface = templateInterface;
            ParentComponent = parentComponent;
            OtherConnection = new ComponentInterface(); //placeholder until another component gets attached
            ConnectionPlane = connectionPlane;
            IsPlaceholder = false;
        }

        //constructor for placeholder until another component gets attached
        public ComponentInterface()
        {
            TemplateInterface = null;
            ParentComponent = null;
            OtherConnection = null;
            ConnectionPlane = Plane.WorldXY;
            IsPlaceholder = true;
        }


        public void TranslateInterface(Vector3d translation)
        {
            Plane tmpPlane = ConnectionPlane;
            tmpPlane.Translate(translation);
            ConnectionPlane = tmpPlane;
            // i dont know exactly why the temporary storage/assignment is necessary, but it worked only like that

        }

        public void RotateInterface(double angleInRadians, Vector3d rotationAxis, Point3d origin)
        {
            Plane tmpPlane = ConnectionPlane;
            tmpPlane.Rotate(angleInRadians, rotationAxis, origin);
            ConnectionPlane = tmpPlane;
        }


        public void TransformInterface(Transform transformation)
        {
            Plane tmpPlane = ConnectionPlane;
            ConnectionPlane.Transform(transformation);
            ConnectionPlane = tmpPlane;
        }

        /// <summary>
        /// During the rewriting process, the transformation for the newly added part is computed by the logic Transform.PlaneToPlane(newComponent.ConnectionPlane(flipped by yaxis) and existingComponent.ConnectionPlane)
        /// With this logic, the rewriting process means that both interfaces are laid over each other with the same x-axis and reversed z-axis. Try out the WASP grasshopper plugin to learn the logic.
        /// </summary>
        /// <returns>the connection plane of the interface flipped by its y-axis</returns>
        public Plane GetPlaneFlippedAtYAxis()
        {
            Vector3d yAxisCopy = ConnectionPlane.YAxis;
            yAxisCopy.Reverse();
            Plane flippedYPlane = new Plane(ConnectionPlane.Origin, ConnectionPlane.XAxis, yAxisCopy);
            return flippedYPlane;
        }

    }
}
