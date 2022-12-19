using Finaviaapi.Http;
using Finaviaapi.Files;

namespace Finaviaapi.Util
{
    /// <summary>
    /// Represents a Tester for developing.
    /// </summary>
    public static class Tester
    {
        // constants
        const string BASE_URI = "https://api.finavia.fi/flights/public/v0/flights/";
        const string APP_ID = "FINAVIA_APP_ID";
        const string APP_KEY = "FINAVIA_APP_KEY";

        // classes
        static ApiConnector apiObj = new(BASE_URI, APP_ID, APP_KEY);

        /*ApiConnector Tests*/

        /// <summary>
        /// Console print and test ApiConnector class properties
        /// </summary>
        static public void ApiConnectorTestProperties()
        {
            string value;

            value = apiObj.UriObj.ToString();
            Console.WriteLine($"Value: {value}");
        }

        /// <summary>
        /// Test if connection is ok and what is the status code
        /// </summary>
        /// <returns></returns>
        static public async Task ApiConnectorTestConnection()
        {
            var re = await apiObj.TestConnectionAsync();
            Console.WriteLine(re);
        }

        /// <summary>
        /// Retrieves arrival data in string format from an airport
        /// </summary>
        /// <param name="airport">Index of airport from ApiConnector ParametersAirport property</param>
        /// <returns></returns>
        static public async Task ApiConnectorTestArrivaAirport(int airport)
        {
            var re = await apiObj.GetArrivalStrAsync(airport);

            Console.WriteLine(re);
        }

        /*Files Tests*/

        static public async Task WriteToFileTestWritingToFile(int airport)
        {
            var re = await apiObj.GetArrivalStrAsync(airport);
            Console.WriteLine(re);
            
            WriteToFile.Write("Current", ".xml", re);
        }

    }
}
