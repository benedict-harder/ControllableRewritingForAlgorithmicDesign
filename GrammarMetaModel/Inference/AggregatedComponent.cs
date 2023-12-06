using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GrammarMetaModel
{
    internal class AggregatedComponent : AbstractComponent
    {
        public List<Component> Components;

        public AggregatedComponent(AbstractPart relatedPart, List<Component> components) : base(relatedPart)
        {
            Components = components;
        }

        public override void TransformPlaneToPlane(ComponentInterface existingInterface, ComponentInterface newInterface)
        {
            Transform transformationMatrix = Transform.PlaneToPlane(newInterface.GetPlaneFlippedAtYAxis(), existingInterface.ConnectionPlane);
            foreach (Component component in Components)
            {
                component.MeshGeometry.Transform(transformationMatrix);
                foreach(ComponentInterface componentInterface in ComponentInterfaces)
                {
                    componentInterface.TransformInterface(transformationMatrix);
                }
            }
        }

        public override void Translate(Vector3d translation)
        {
            foreach (Component component in Components)
            {
                component.MeshGeometry.Translate(translation);
                foreach (ComponentInterface componentInterface in ComponentInterfaces)
                {
                    componentInterface.TranslateInterface(translation);
                }
            }
        }

        public override void RotateAroundGlobalOrigin(double angleInRadians, char axis)
        {
            //define parameters
            Vector3d rotationAxis = new Vector3d();
            Point3d origin = new Point3d(0.0, 0.0, 0.0);
            if (axis == 'x') { rotationAxis = new Vector3d(1.0, 0.0, 0.0); }
            else if (axis == 'y') { rotationAxis = new Vector3d(0.0, 1.0, 0.0); }
            else if (axis == 'z') { rotationAxis = new Vector3d(0.0, 0.0, 1.0); }
            else { throw new ArgumentOutOfRangeException("axis", "insert x,y, or z as an axis"); }

            foreach (Component component in Components)
            {
                component.MeshGeometry.Rotate(angleInRadians, rotationAxis, origin);
                foreach (ComponentInterface componentInterface in ComponentInterfaces)
                {
                    componentInterface.RotateInterface(angleInRadians, rotationAxis, origin);
                }
            }
        }
    }
}
