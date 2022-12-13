using Finaviaapi.Util;

namespace Finaviaapi
{
    class Program
    {
        static async Task Main(string[] args)
        {
           // Tester.ApiConnectorTestProperties();
           //await Tester.ApiConnectorTestConnection();

           await Tester.ApiConnectorTestArrivaAirport(3);
        }
    }
}