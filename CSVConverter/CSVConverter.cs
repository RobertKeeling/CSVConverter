using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVConverter
{
    class CSVConverter : IFormatConverter
    {
        private string BuildHeaderRow(JObject jObject, string prefix = "")
        {
            string result = "";
            IList<string> keys = jObject.Properties().Select(p => p.Name).ToList();
            foreach (string key in keys)
            {
                if (jObject[key].Type == JTokenType.Object)
                {
                    result += BuildHeaderRow((JObject)jObject[key], prefix + key + "_");
                }
                else
                {
                    result += prefix + key + ",";
                }
            }
            return result.Substring(0, result.Length - 1) + "\n";
        }
        private string BuildDataRow(JObject jObject, bool first = true)
        {
            string result = "";
            IList<string> keys = jObject.Properties().Select(p => p.Name).ToList();
            foreach (string key in keys)
            {
                if (jObject[key].Type == JTokenType.Object) result += BuildDataRow((JObject)jObject[key], false);
                else result += jObject[key].ToString() + ",";
            }
            return first ? result.Substring(0, result.Length - 1) : result;
        }
        private string BuildDataRows(JArray jArray)
        {
            string result = "";
            for (var i = 0; i < jArray.Count(); i++)
            {
                result += BuildDataRow((JObject)jArray[i]) + "\n";
            }
            return result.Substring(0, result.Length - 1);
        }
        public string ConvertFromJArray(JArray jArray, string fileSavePath = null)
        {
            string result = BuildHeaderRow((JObject)jArray[0]);
            result += BuildDataRows(jArray);
            if (fileSavePath != null) File.WriteAllText(fileSavePath, result);
            return result;
        }
        private void CreateJObjectTemplate0(JObject jObject, string[] split)
        {
            if (split.Length > 1)
            {
                if (jObject[split[0]] == null) jObject[split[0]] = new JObject();
                CreateJObjectTemplate0((JObject)jObject[split[0]], split.Skip(1).ToArray());
            }
            else
            {
                jObject[split[0]] = null;
            }
        }
        private JObject CreateJObjectTemplate(string line, ref Dictionary<int, string[]> columnKeys)
        {
            JObject jObjectTemplate = new JObject();
            string[] split = line.Split(',');
            for (var i = 0; i < split.Length; i++)
            {
                string fieldName = split[i];
                if (!fieldName.Contains('_'))
                {
                    columnKeys[i] = new string[1] { fieldName };
                    jObjectTemplate[fieldName] = null;
                }
                else
                {
                    string[] fieldNameSplit = fieldName.Split('_');
                    columnKeys[i] = fieldNameSplit;
                    CreateJObjectTemplate0(jObjectTemplate, fieldNameSplit);
                }
            }
            return jObjectTemplate;
        }
        private void PopulateJarrayfromTemplate(JObject jObjectTemplate, JArray jArray, string dataLine, Dictionary<int, string[]> columnKeys)
        {
            JObject templateClone = (JObject)jObjectTemplate.DeepClone();
            string[] split = dataLine.Split(',');
            for (var i = 0; i < split.Length; i++)
            {
                string value = split[i];
                JObject currentObject = templateClone;
                for (var j = 0; j < columnKeys[i].Length; j++)
                {
                    string key = columnKeys[i][j];
                    var ad = currentObject[key];
                    if (!currentObject[key].HasValues) currentObject[key] = value;
                    else currentObject = (JObject)currentObject[key];
                }
            }
            jArray.Add(templateClone);
        }
        public JArray ConvertToJArray(string filepath, bool fromFile = true, string fileSavePath = null)
        {
            JArray result = new JArray();
            if (fromFile)
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    Dictionary<int, string[]> columnKeys = new Dictionary<int, string[]>();
                    JObject jObjectTemplate = new JObject();
                    if (!reader.EndOfStream) jObjectTemplate = CreateJObjectTemplate(reader.ReadLine(), ref columnKeys);
                    while (!reader.EndOfStream) PopulateJarrayfromTemplate(jObjectTemplate, result, reader.ReadLine(), columnKeys);
                }
            }
            else
            {
                using (StringReader reader = new StringReader(filepath))
                {
                    Dictionary<int, string[]> columnKeys = new Dictionary<int, string[]>();
                    JObject jObjectTemplate = new JObject();
                    if (!(reader.Peek() == -1)) jObjectTemplate = CreateJObjectTemplate(reader.ReadLine(), ref columnKeys);
                    while (!(reader.Peek() == -1)) PopulateJarrayfromTemplate(jObjectTemplate, result, reader.ReadLine(), columnKeys);
                }
            }
            if (fileSavePath != null) File.WriteAllText(fileSavePath, result.ToString());
            return result;
        }
    }
}
