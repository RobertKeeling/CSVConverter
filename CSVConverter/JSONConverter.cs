using Newtonsoft.Json.Linq;
using System.IO;

namespace CSVConverter
{
    public class JSONConverter
    {
        private readonly IFormatConverter cSVConverter = new CSVConverter();
        private readonly IFormatConverter xMLConverter = new XMLConverter();
        public JArray ConvertCSVToJSON(string input, bool fromFile = true, string fileSavePath = null)
        {
            return cSVConverter.ConvertToJArray(input, fromFile, fileSavePath);
        }
        public string ConvertJSONToCSV(string input, bool fromFile = true, string fileSavePath = null)
        {
            JArray jArray;
            if (fromFile) jArray = JArray.Parse(File.ReadAllText(input));
            else jArray = JArray.Parse(input);
            return cSVConverter.ConvertFromJArray(jArray, fileSavePath);
        }
        public string ConvertJSONToCSV(JArray input, string fileSavePath = null)
        {
            return cSVConverter.ConvertFromJArray(input, fileSavePath);
        }
        public JArray ConvertXMLToJSON(string input, bool fromFile = true, string fileSavePath = null)
        {
            return xMLConverter.ConvertToJArray(input, fromFile, fileSavePath);
        }
        public string ConvertJSONToXML(string input, bool fromFile = true, string fileSavePath = null)
        {
            JArray jArray;
            if(fromFile) jArray = JArray.Parse(File.ReadAllText(input));
            else jArray = JArray.Parse(input);
            return xMLConverter.ConvertFromJArray(jArray, fileSavePath);
        }
        public string ConvertJSONToXML(JArray jArray, string fileSavePath = null)
        {
            return xMLConverter.ConvertFromJArray(jArray, fileSavePath);
        }
        public string ConvertCSVToXML(string input, bool fromFile = true, string fileSavePath = null)
        {
            JArray jArray = cSVConverter.ConvertToJArray(input, fromFile);
            return xMLConverter.ConvertFromJArray(jArray, fileSavePath);
        }
        public string ConvertXMLToCSV(string input, bool fromFile = true, string fileSavePath = null)
        {
            JArray jArray = xMLConverter.ConvertToJArray(input, fromFile);
            return cSVConverter.ConvertFromJArray(jArray, fileSavePath);
        }
    }
}
