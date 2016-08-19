using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestRunnerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GetLocationFromAddress();
        }

        private static void GetLocationFromAddress()
        {
            var address = Console.ReadLine();
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var result = xdoc.Element("GeocodeResponse").Element("result");

            if (result != null)
            {
                var locationElement = result.Element("geometry").Element("location");
                var lat = locationElement.Element("lat").Value;
                var lng = locationElement.Element("lng").Value;

                Console.WriteLine($"lat: {lat}, long: {lng}");
            }
            else
            {
                Console.WriteLine("Could not find that city.");
            }
        }
    }
}
