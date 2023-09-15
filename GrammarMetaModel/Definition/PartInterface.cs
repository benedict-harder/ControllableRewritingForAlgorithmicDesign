using Rhino.Geometry;

namespace GrammarMetaModel
{
    /// <summary>
    /// Objects following this class description contain a description of a modular interface associated to a part. In reality, this is associated to a dry concrete/steel interface (e.g. column-deck).
    /// The component/componentInterface objects are the concrete, adapted and positioned instances of the generic module object
    /// </summary>
    public class PartInterface
    {
        public string Name;
        public Part ParentPart;

        public PartInterface(string name, Part parentPart)
        {
            Name = name;
            ParentPart = parentPart;
        }

    }
}
