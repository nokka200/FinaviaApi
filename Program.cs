using Finaviaapi.Util;

namespace Finaviaapi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Tester.ApiConnectorTestProperties();
            //await Tester.ApiConnectorTestConnection();

            //var re = Tester.ApiConnectorTestArrivaAirport(3);
            //await re;

            //var re = Tester.WriteToFileTestWritingToFile(3);
            //await re;

            //Tester.SerializeFlight();

            Tester.XmlDocument();
        }
    }
}