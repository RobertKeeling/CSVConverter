using Newtonsoft.Json.Linq;
using System;

namespace CSVConverter
{
    class Program
    {
        private static readonly JSONConverter jSONConverter = new JSONConverter();
        static void Main(string[] args)
        {
            bool finished = false;
            while (!finished)
            {
                if (args.Length < 2 || args.Length > 2)
                {
                    Console.WriteLine("Please enter the input path of the file");
                    string inputPath = Console.ReadLine();
                    Console.WriteLine("Now enter the output path");
                    args = new string[2] { inputPath, Console.ReadLine() };
                }
                if (args[0].ToUpper().EndsWith(".CSV"))
                {
                    if (args[1].ToUpper().EndsWith(".XML")) jSONConverter.ConvertCSVToXML(args[0], true, args[1]);
                    else jSONConverter.ConvertCSVToJSON(args[0], true, args[1]);
                }
                else if (args[0].ToUpper().EndsWith(".JSON"))
                {
                    if (args[1].ToUpper().EndsWith(".XML")) jSONConverter.ConvertJSONToXML(args[0], true, args[1]);
                    else jSONConverter.ConvertJSONToCSV(args[0], true, args[1]);
                }
                else if (args[0].ToUpper().EndsWith(".XML"))
                {
                    if (args[1].ToUpper().EndsWith(".JSON")) jSONConverter.ConvertXMLToJSON(args[0], true, args[1]);
                    else jSONConverter.ConvertXMLToCSV(args[0], true, args[1]);
                }
                Console.WriteLine();
                Console.WriteLine("The File: " + args[0]);
                Console.WriteLine("Has Been Succefully Converted To: " + args[1]);
                Console.WriteLine("Type y to convert another file or any other key to escape.");
                if (Console.ReadLine().ToUpper() != "Y") finished = true;
                args = new string[0] { };
            }
        }
    }
}
