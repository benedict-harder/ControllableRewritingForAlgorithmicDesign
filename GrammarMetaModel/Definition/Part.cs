using System;
using System.Collections.Generic;
using Rhino.Geometry;
using GH_IO;
using System.Linq;
using Grasshopper.Kernel.Special;

namespace GrammarMetaModel
{
    /// <summary>
    /// Objects following this class description contain a generic/adaptable description of a module.
    /// The component/componentInterface objects are the concrete, adapted and positioned instances of the generic module description, i.e. the "part"
    /// I borrowed the part-component concept from siemens nx (see p.72 in this guide https://docs.plm.automation.siemens.com/data_services/resources/nx/10/nx_api/en_US/graphics/fileLibrary/nx/snap/SNAP_Getting_Started_V10.pdf) 
    /// In WASP, simple copies of the "part-objects" are created every time a component is positioned - i find the part-component distinction clearer.
    /// </summary>
    public class Part
    {
        public string Name = "";
        public List<PartInterface> Connections = new List<PartInterface>();

        // props for parametric geometry generation via Resthopper interface
        public string GhDefinitionFullPath = "";
        public Dictionary<string, double> Parameters = new Dictionary<string, double>(); 
    

        public Part(string name, string ghDefinitionFileName)
        {
            Name = name;
            GhDefinitionFullPath = @"C:\Users\ga24nix\source\repos\ControlledBridgeGrammar\SkeletonCaseStudy\GhDefinitionFiles\"+ghDefinitionFileName;
            Parameters = this.LoadInputParams();
            List<string> interfaceNames = this.LoadInterfaces();
            foreach (string interfaceName in interfaceNames) 
            {
                Connections.Add(new PartInterface(interfaceName, this));
            };
           
        }

        private Dictionary<string, double> LoadInputParams()
        {
            Dictionary<string, double> inputParams = new Dictionary<string, double>();

            //read file via archive object
            GH_IO.Serialization.GH_Archive Archive = new GH_IO.Serialization.GH_Archive();
            Archive.ReadFromFile(GhDefinitionFullPath);
            var root = Archive.GetRootNode;
            var def = root.FindChunk("Definition");
            var objs = def.FindChunk("DefinitionObjects");

            // get inputs
            if (objs != null)
            {
                int count = objs.GetInt32("ObjectCount");

                for (int i = 0; i < count; i++)
                {
                    var obj = objs.FindChunk("Object", i);
                    var container = obj.FindChunk("Container");
                    var nickname = container.GetString("NickName");
                    var name = container.GetString("Name");

                    // i didnt find out how to get the values of panels hosted by a grasshopper group - so i just decided to take the values of the embedded elements (sliders or numbers) directly that also must be named RH_IN
                    if (name != "Group" && nickname.StartsWith("RH_IN"))
                    {
                        double defaultValue = 0.0;
                        if (name == "Number Slider")
                        {
                            var sliderData = container.FindChunk("Slider");
                            defaultValue = (double)sliderData.FindItem("Value").InternalData;
                        }
                        else
                        {
                            throw new NotImplementedException("only number sliders implemented for input fields so far");
                        }
                        inputParams[nickname] =  defaultValue;
                                
                    }
                }
            }

            return inputParams;
        }

        private List<string> LoadInterfaces()
        {
            List<string> rawConnections = new List<string>();

            GH_IO.Serialization.GH_Archive Archive = new GH_IO.Serialization.GH_Archive();
            Archive.ReadFromFile(GhDefinitionFullPath);
            var root = Archive.GetRootNode;
            var def = root.FindChunk("Definition");
            var objs = def.FindChunk("DefinitionObjects");

            // get inputs
            if (objs != null)
            {
                int count = objs.GetInt32("ObjectCount");

                for (int i = 0; i < count; i++)
                {
                    var obj = objs.FindChunk("Object", i);
                    var container = obj.FindChunk("Container");

                    var name = container.GetString("Name");
                    var nickname = container.GetString("NickName");
                    if (name == "Group" && nickname.StartsWith("RH_OUT:ComponentInterface"))
                    {
                        var interfaceName = nickname.Replace("RH_OUT:ComponentInterface_", "");
                        rawConnections.Add(interfaceName);
                    }
                }
            }

            return rawConnections;


        }


    }
}
