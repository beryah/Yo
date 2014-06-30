using System.Linq;
using System.Xml.Linq;

namespace ZipToAdministrativeAreatInfoConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            XDocument xd = XDocument.Load("Zip32_10303.xml");

            var data = xd.Root.Elements("Zip32").Select(x =>
                new
                {
                    Zip = x.Element("Zip5").Value.Substring(0, 3),
                    City = x.Element("City").Value,
                    Area = x.Element("Area").Value,
                    Road = x.Element("Road").Value,
                }).Distinct().OrderBy(x => x.Zip).ToList(); ;
        }
    }
}