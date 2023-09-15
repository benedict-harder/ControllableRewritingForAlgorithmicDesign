using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Rhino.Compute;
using Newtonsoft.Json;
using Rhino.FileIO;
using Rhino.Geometry;

namespace GrammarMetaModel
{
    /// <summary>
    /// this class helps to sends grasshopper definition files with parameters (starting with RH_IN:...) to the rhino.compute server and parses meshes and planes (RH_OUT:...) in the response.
    /// </summary>
    public static class ResthopperBridge
    {
        public static Tuple<Mesh, List<Plane>> EvaluateDefinition(string definitionPath, Dictionary<string, double> partParameters)
        {

            //Send the request
            ComputeServer.WebAddress = "http://localhost:8081/"; //must be running locally, see https://developer.rhino3d.com/guides/compute/development/ 
            var trees = new List<GrasshopperDataTree>();
            foreach (KeyValuePair<string, double> partParameter in partParameters)
            {
                var param = new GrasshopperDataTree(partParameter.Key);
                var value = new GrasshopperObject(partParameter.Value);
                param.Add("0", new List<GrasshopperObject> { value });
                trees.Add(param);
            }
            var result = Rhino.Compute.GrasshopperCompute.EvaluateDefinition(definitionPath, trees);


            //Read results
            Mesh componentGeo = null;
            List<Plane> componentInterfaceGeos = new List<Plane>();
            foreach (var geo in result.ToList())
            {
                var geoSpecification = geo.InnerTree.First().Value[0];
                if (geoSpecification.Type.Contains("Mesh"))
                {
                    componentGeo = (Mesh)JsonConvert.DeserializeObject<Mesh>(geoSpecification.Data);
                }
                else if (geoSpecification.Type.Contains("Plane"))
                {
                    Plane componentInterfaceGeo = (Plane)JsonConvert.DeserializeObject<Plane>(geoSpecification.Data);
                    componentInterfaceGeos.Add(componentInterfaceGeo);
                }
            }
            return new Tuple<Mesh, List<Plane>>(componentGeo, componentInterfaceGeos);
        }
      

    }
}
