using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarMetaModel
{
    public abstract class AbstractComponent
    {
        public AbstractPart ComponentTemplate { get; set; }
        public List<ComponentInterface> ComponentInterfaces { get; set; }

        public AbstractComponent(AbstractPart relatedPart)
        {
            ComponentTemplate = relatedPart;
        }

        public void AddConnection(ComponentInterface componentinterface)
        {
            ComponentInterfaces.Add(componentinterface);
        }

        public abstract void TransformPlaneToPlane(ComponentInterface existingInterface, ComponentInterface newInterface);

        public abstract void Translate(Vector3d translation);

        public abstract void RotateAroundGlobalOrigin(double angleInRadians, char axis);
    }
}
