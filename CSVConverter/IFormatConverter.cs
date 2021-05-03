using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVConverter
{
    interface IFormatConverter
    {
        string ConvertFromJArray(JArray jArray, string fileSavePath = null);
        JArray ConvertToJArray(string input, bool fromFile = true, string fileSavePath = null);
    }
}
