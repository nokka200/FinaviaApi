using Finaviaapi.Http;
using Finaviaapi.Files;
using Finaviaapi.Serializer;
using Finaviaapi.Flight;
using System.Xml;

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

        /*Serializer Tests*/

        /// <summary>
        /// Test deseralizing flightdata from a xml file, just a single flight data
        /// </summary>
        static public void SerializeFlightSingle()
        {
            Flights flightObj = SerializeFlightData();
            Console.WriteLine(flightObj.arr.flight[0].hApt);

        }

        
        /// <summary>
        /// Test printing all info from flight array
        /// </summary>
        static public void SerializePrintAllInfo()
        {
            Flights flightObj = SerializeFlightData();

            // NOTICE overriding null protection !
            foreach(var item in flightObj!.arr!.flight!)
            {
                
            }
        }

        /// <summary>
        /// Test converting the .xml file to xml object
        /// </summary>
        static public void XmlDocument()
        {
            string currentPath = Directory.GetCurrentDirectory();
            string re;
            XmlDocument docObj = new();

            re = File.ReadAllText(currentPath + "/TestXml.xml");
            docObj.LoadXml(re);

        }

        // Private methods
        private static Flights SerializeFlightData()
        {
            string currentPath = Directory.GetCurrentDirectory();
            Flights flightObj;

            object tempObj = MyObjectSerializer.ReadObject(currentPath + "/TestXml.xml", typeof(Flights));

            flightObj = (Flights)tempObj;
            return flightObj;
        }
    }
}
