using System;
using Finaviaapi.Flight;
using Finaviaapi.Serializer;
using Finaviaapi.Http;
using Finaviaapi.Files;

namespace Finaviaapi.Ui
{
	public class ConsoleUi
	{
        // fields
		Flights? flightObj;
        ApiConnector apiObj;
        const string BASE_URI = "https://api.finavia.fi/flights/public/v0/flights/";
        const string APP_ID = "FINAVIA_APP_ID";
        const string APP_KEY = "FINAVIA_APP_KEY";

        // properties
        public string FileName { get;}
        public int HourDifference { get; set; }

        // constructor
        public ConsoleUi(string fileName)
		{
            FileName = fileName;
            HourDifference = 2;
            apiObj = new(BASE_URI, APP_ID, APP_KEY);
        }

        // private methods
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
            else if (estArrival > arrivalTime)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Arvioitu:\t {estArrival}");
            Console.ForegroundColor = foreground;

            Console.WriteLine("--------------------");
        }

        private Flights SerializeFlightData(string fileName)
        {
            string currentPath = Directory.GetCurrentDirectory();

            object tempObj = MyObjectSerializer.ReadObject(currentPath + "/" + fileName + ".xml", typeof(Flights));

            flightObj = (Flights)tempObj;
            return flightObj;
        }

        /// <summary>
        /// 
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

        /// <summary>
        /// Prints flight/airport metadata 
        /// </summary>
        /// <param name="item"></param>
        private void PrintMetaData(Flights item)
        {
            // null check
            if(item == null || item.arr == null || item.arr.flight == null)
            {
                Console.WriteLine("Flight data null");
                return;
            }
                
            var flightItem = item.arr.flight[0];
            Console.WriteLine($"Lentoasema: {flightItem.hApt}");
            Console.WriteLine("--------------------");

        }

        // public methods

        public async Task PrintAndUpdateAsync(int refreshInterval, int airport, int waitTime)
        {
            // TODO kopio Program.cs ajo tähän metodiksi
            Console.Clear();
            while(true)
            {
                var re = await apiObj.GetArrivalStrAsync(airport);
                
                WriteToFile.Write(FileName, ".xml", re);

                Console.WriteLine(DateTime.Now);

                UpdateDataLocal();
                PrintAllInfoDate();

                Thread.Sleep(waitTime);
                Console.Clear();
            }
        }

        /// <summary>
        /// Updates the flight data
        /// </summary>
        public void UpdateDataLocal()
        {
            flightObj = SerializeFlightData(FileName);
        }

        /// <summary>
        /// Prints all the info in a nice format, hour now + HourDifference property
        /// </summary>
        public void PrintAllInfoDate()
        {
            flightObj = SerializeFlightData(FileName);
            DateTime arrival;

            if (flightObj != null && flightObj.arr != null && flightObj.arr.flight != null)
            {
                PrintMetaData(flightObj);
                foreach (var item in flightObj.arr.flight)
                {
                    DateTime.TryParse(item.sdt, out arrival);
                    if (arrival.Date == DateTime.Now.Date && arrival.Hour < (DateTime.Now.Hour + HourDifference))
                        DataPrinter(item);
                }
            }
            else
            {
                Console.WriteLine("No flight found");
            }
        }
    }
}

            