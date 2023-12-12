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
        public AbstractPart ComponentTemplate;
        public List<ComponentInterface> ComponentInterfaces = new List<ComponentInterface>();

        public AbstractComponent() { }
        public AbstractComponent(AbstractPart relatedPart)
        {
            ComponentTemplate = relatedPart;
        }

        public void AddConnection(ComponentInterface componentinterface)
        {
            ComponentInterfaces.Add(componentinterface);
        }

        public abstract void AddToAssembly(Assembly assembly);
        public abstract void TransformPlaneToPlane(ComponentInterface existingInterface, ComponentInterface newInterface);

        public abstract void Translate(Vector3d translation);

        public abstract void RotateAroundGlobalOrigin(double angleInRadians, char axis);
    }
}
