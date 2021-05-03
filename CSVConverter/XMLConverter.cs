using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace CSVConverter
{
    class XMLConverter : IFormatConverter
    {
        private JToken ConvertToJObject(string input)
        {
            JObject result = new JObject();
            while (!input.Trim().Equals(""))
            {
                int greaterIndex = input.IndexOf("<"), lessIndex = input.IndexOf(">");
                if (greaterIndex == -1 || lessIndex == -1) return input;
                string key = input.Substring(greaterIndex + 1, lessIndex - greaterIndex - 1);
                int len = input.IndexOf("</" + key + ">") - lessIndex - 1;
                var value = input.Substring(lessIndex + 1, len);
                if (value.IndexOf("<") == -1 || value.IndexOf(">") == -1) result[key] = value;
                else result[key] = ConvertToJObject(value);
                input = input.Substring(lessIndex + len + key.Length + 4);
            }
            return result;
        }
        public JArray ConvertToJArray(string input, bool fromFile, string fileSavePath = null)
        {
            if (fromFile) input = File.ReadAllText(input);
            JArray jArray = new JArray();
            input = input.Replace("\n", "");
            string[] split = input.Split(new string[] { "<ConvertedObject>" }, StringSplitOptions.None);
            foreach (string match in split)
            {
                if (match.Trim().Equals("")) continue;
                string XMLString = match.Replace("</ConvertedObject>", "");
                jArray.Add(ConvertToJObject(XMLString));
            }
            if (fileSavePath != null) File.WriteAllText(fileSavePath, jArray.ToString());
            return jArray;
        }
        private string ConvertFromJObject(JObject jObject, int depth = 4)
        {
            string result = "";
            foreach (string key in jObject.Properties().Select(p => p.Name).ToList())
            {
                string spaces = new string(' ', depth);
                if (jObject[key].Type == JTokenType.Object)
                {
                    result += spaces + "<" + key + ">\n" + ConvertFromJObject((JObject) jObject[key], depth+4) + spaces + "</" + key + ">\n";
                }
                else
                {
                    result += spaces + "<" + key + ">" + jObject[key] + "</" + key + ">\n";
                }
            }
            return result;
        }
        public string ConvertFromJArray(JArray jArray, string fileSavePath = null)
        {
            string result = "";
            foreach(JObject jObject in jArray)
            {
                result += "<ConvertedObject>\n" + ConvertFromJObject(jObject) + "</ConvertedObject>\n";
            }
            if(fileSavePath != null) File.WriteAllText(fileSavePath, result);
            return result;
        }
    }
}
