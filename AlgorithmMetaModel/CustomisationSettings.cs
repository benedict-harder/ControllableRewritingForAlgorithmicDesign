using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarMetaModel;

namespace AlgorithmMetaModel
{
    /// <summary>
    /// Class to store and pass parameters which can not be related to a single part and need to be converted into part parameters in a PlanAssemblyState
    /// </summary>
    public class CustomisationSettings
    {
        public Dictionary<string, double> Parameters { get; private set; } = new Dictionary<string, double>();// maybe change double to dynamic in future to host also boolean/string/int parameters... 

        public void AddParameter(string name, double value)
        {
            Parameters.Add(name, value);
        }

        public double GetParameterValue(string key)
        {
            double value = -1.0;
            Parameters.TryGetValue(key, out value);
            return value;

        }
    }
}
