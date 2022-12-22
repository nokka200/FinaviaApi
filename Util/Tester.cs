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
            
            WriteToFile.Write("Current", ".xml", re);
        }

        /*Serializer Tests*/

        /// <summary>
        /// Test deseralizing flightdata from a xml file, just a single flight data
        /// </summary>
        static public void SerializeFlightSingle()
        {
            Flights flightObj = SerializeFlightData("TestXml");
            Console.WriteLine(flightObj.arr.flight[0].hApt);

        }

        
        /// <summary>
        /// Test printing all info from flight array, gets the current data from the api and displays it in the console
        /// </summary>
        static public void SerializePrintAllInfo()
        {
            Flights flightObj = SerializeFlightData("Current");

            if(flightObj != null && flightObj.arr != null && flightObj.arr.flight != null)
            {
                // NOTICE overriding null protection !
                foreach (var item in flightObj.arr.flight)
                {
                    DataPrinter(item);
                }
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
        /// <summary>
        /// Deserializes .xml file to flight object
        /// </summary>
        /// <param name="fileName">name of the file without the suffix</param>
        /// <returns></returns>
        private static Flights SerializeFlightData(string fileName)
        {
            string currentPath = Directory.GetCurrentDirectory();
            Flights flightObj;

            object tempObj = MyObjectSerializer.ReadObject(currentPath + "/" + fileName + ".xml", typeof(Flights));

            flightObj = (Flights)tempObj;
            return flightObj;
        }

        /// <summary>
        /// Prints flight data in a nice format
        /// </summary>
        /// <param name="item"></param>
        private static void DataPrinter(flight item)
        {
            DateTime estArrival;
            DateTime arrivalTime;
            ConsoleColor foreground = Console.ForegroundColor;

            DateTime.TryParse(item.sdt, out arrivalTime);
            DateTime.TryParse(item.estD, out estArrival); 

            // Methods that check stuff
            estArrival = CheckEstimate(item, estArrival, arrivalTime);

            Console.WriteLine($"Lennon numero:\t {item.fltnr}, {item.cflight1}" +
                $", {item.cflight2}, {item.cflight3}, {item.cflight4}, {item.cflight5}" +
                $", {item.cflight6}");
            Console.WriteLine($"Saapumisaika:\t {arrivalTime}");
            Console.WriteLine($"Tila:\t\t {item.prtF}");

            // change color based on estimate time and arrival time
            if (estArrival < arrivalTime)
                Console.ForegroundColor = ConsoleColor.Green;
            else if(estArrival > arrivalTime)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Arvioitu:\t {estArrival}");
            Console.ForegroundColor = foreground;

            Console.WriteLine("--------------------");
        }

        /// <summary>
        /// Checks if estimate is empyt, if empyt uses same time as arrival
        /// </summary>
        /// <param name="item"></param>
        /// <param name="estArrival"></param>
        /// <param name="arrivalTime"></param>
        /// <returns></returns>
        private static DateTime CheckEstimate(flight item, DateTime estArrival, DateTime arrivalTime)
        {
            if (item.estD == string.Empty)
            {
                estArrival = arrivalTime;
            }

            return estArrival;
        }
    }
}
