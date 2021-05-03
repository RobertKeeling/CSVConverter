using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using CSVConverter;
using System.IO;

namespace UnitTestCSVConverter
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly string filePath = "C:\\GitHub\\CSVConverter\\FILESKNOWNTOBECORRECT\\";
        private static readonly JSONConverter jSONConverter = new JSONConverter();
        [TestMethod]
        public void TestMethod1()
        {
            JArray jArray = jSONConverter.ConvertCSVToJSON(filePath + "CORRECT.csv", true);
            string csv = jSONConverter.ConvertJSONToCSV(jArray);
            JArray jArray0 = jSONConverter.ConvertCSVToJSON(csv, false);
            string csv0 = jSONConverter.ConvertJSONToCSV(filePath + "CORRECT.json", true);
            string xml = jSONConverter.ConvertCSVToXML(filePath + "CORRECT.csv", true);
            string xml0 = jSONConverter.ConvertCSVToXML(csv, false);
            JArray jArray1 = jSONConverter.ConvertXMLToJSON(xml, false);
            JArray jArray2 = jSONConverter.ConvertXMLToJSON(filePath + "CORRECT.xml", true);
            string xml1 = jSONConverter.ConvertJSONToXML(jArray, filePath + "CORRECT.xml");
            string xml2 = jSONConverter.ConvertJSONToXML(filePath + "CORRECT.json", true);
            string csv1 = jSONConverter.ConvertXMLToCSV(xml, false, filePath + "CORRECT.csv");
            string csv2 = jSONConverter.ConvertXMLToCSV(filePath + "CORRECT.xml", true);

            string correctCSV = File.ReadAllText(filePath + "CORRECT.csv");
            string correctXML = File.ReadAllText(filePath + "CORRECT.xml");
            string correctJSON = File.ReadAllText(filePath + "CORRECT.json");
            Assert.AreEqual(correctCSV, csv);
            Assert.AreEqual(correctCSV, csv0);
            Assert.AreEqual(correctCSV, csv1);
            Assert.AreEqual(correctCSV, csv2);
            Assert.AreEqual(correctXML, xml);
            Assert.AreEqual(correctXML, xml0);
            Assert.AreEqual(correctXML, xml1);
            Assert.AreEqual(correctXML, xml2);
            Assert.AreEqual(correctJSON, jArray.ToString());
            Assert.AreEqual(correctJSON, jArray0.ToString());
            Assert.AreEqual(correctJSON, jArray1.ToString());
            Assert.AreEqual(correctJSON, jArray2.ToString());
        }
    }
}
