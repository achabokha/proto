using CoreTechs.Common.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCountries
{
    class Program
    {
        static void Main(string[] args)
        {
            string csv = File.ReadAllText("countries.csv");
            TextReader reader = new StringReader(csv);

            var list = reader.ReadCsvWithHeader();

            foreach (var item in list)
            {
                Console.WriteLine(item["name"], item["alpha-3"]);
            }

        }
    }
}
