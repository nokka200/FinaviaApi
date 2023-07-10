using System;
using Finaviaapi.Flight;
using Finaviaapi.Serializer;
using Finaviaapi.Http;
using Finaviaapi.Files;

namespace Finaviaapi.Ui
{
    /// <summary>
    /// Represents a console unserinterface for tracking flights in Finavias airports
    /// </summary>
	public class ConsoleUi
	{
        // FIELDS
		Flights? flightObj;
        ApiConnector apiObj;
        const string BASE_URI = "https://api.finavia.fi/flights/public/v0/flights/";
        const string APP_ID = "FINAVIA_APP_ID";
        const string APP_KEY = "FINAVIA_APP_KEY";
        int refreshCount;

        // PROPERTIES
        public string FileName { get;}
        public int HourDifference { get; set; } = 2;
        /// <summary>
        /// Time between new call's to Finavpia API MIN time should be 20000 milliseconds
        /// </summary>
        public int RefreshInterval { get; set; } = 20000;

        // CONSTRUCTOR
        public ConsoleUi(string fileName)
		{
            // Also constructs an ApiConnector object with BASE_URI, APP_ID and APP_KEY
            FileName = fileName;
            apiObj = new(BASE_URI, APP_ID, APP_KEY);
        }

        // PRIVATE METHODS
        private static void DataPrinter(flight item)
        {
            // First compated estimated arrival time with arrival time, then prints a the information in a 
            // formated form.
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

            ChangeColorState(item, foreground);

            // change color based on estimate time and arrival time and writes it
            ChangeColorArrival(estArrival, arrivalTime, foreground);

            Console.WriteLine("--------------------");
        }

        private static void ChangeColorState(flight item, ConsoleColor foreground)
        {
            if (item.prtF.ToLower() == "laskeutunut")
                Console.ForegroundColor = ConsoleColor.Blue;
            if (item.prtF.ToLower() == "lähestyy")
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"Tila:\t\t {item.prtF}");
            Console.ForegroundColor = foreground;
        }

        private static void ChangeColorArrival(DateTime estArrival, DateTime arrivalTime, ConsoleColor foreground)
        {
            if (estArrival < arrivalTime)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (estArrival > arrivalTime)
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Arvioitu:\t {estArrival}");
            Console.ForegroundColor = foreground;
        }

        private Flights SerializeFlightData(string fileName)
        {
            // Gets the current working directory and serializes a Flight object from the information.
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
            Console.WriteLine(DateTime.Now);
            Console.WriteLine($"Lentoasema: {flightItem.hApt}");
            Console.WriteLine($"Päivitys aika/sec: {RefreshInterval / 1000}");
            Console.WriteLine($"Kierros: {refreshCount}");
            Console.WriteLine("--------------------");
        }

        private void UpdateDataLocal()
        {
            // Updates the Flight object with new data from Current.xml
            flightObj = SerializeFlightData(FileName);
        }

        /// <summary>
        /// Prints all the info in a nice format, hour now + HourDifference property
        /// </summary>
        private void PrintAllInfo()
        {
            flightObj = SerializeFlightData(FileName);

            if (flightObj != null && flightObj.arr != null && flightObj.arr.flight != null)
            {
                PrintMetaData(flightObj);
                foreach (var item in flightObj.arr.flight)
                {
                    DateTime.TryParse(item.sdt, out DateTime arrival);
                    if (arrival.Date == DateTime.Now.Date && arrival.Hour < (DateTime.Now.Hour + HourDifference))
                        DataPrinter(item);
                }
            }
            else
            {
                Console.WriteLine("No flight found");
            }
        }

        private void PrintAllInfo(int hourLimit)
        {
            flightObj = SerializeFlightData(FileName);

            if (flightObj != null && flightObj.arr != null && flightObj.arr.flight != null)
            {
                PrintMetaData(flightObj);
                foreach (var item in flightObj.arr.flight)
                {
                    DateTime.TryParse(item.sdt, out DateTime arrival);
                    if (arrival.Date == DateTime.Now.Date && arrival.Hour < DateTime.Now.AddHours(hourLimit).Hour && arrival.Hour > DateTime.Now.Hour - 1)
                        DataPrinter(item);
                }
            }
            else
            {
                Console.WriteLine("No flight found");
            }
        }

        // PUBLIC METHODS

        /// <summary>
        /// Makes an async call to Finavia api, creates a .xml file and updates Flights objects with the current data.
        /// </summary>
        /// <param name="airport">Airport from list</param>
        /// <returns></returns>
        public async Task PrintAndUpdateAsync(int airport)
        {
            // Clears console and then enters an eternal loop.
            // First gets the current data from the finavia api and writes it to Current.xml
            // Updates flightObj with the new data and prints it. 
            // sleeps the tread for X amount of time and finally clears the screen.
            Console.Clear();
            while(true)
            {
                var re = await apiObj.GetArrivalStrAsync(airport);
                refreshCount++;
                
                WriteToFile.Write(FileName, ".xml", re);

                UpdateDataLocal();
                PrintAllInfo();

                Thread.Sleep(RefreshInterval);
                Console.Clear();
            }
        }    
        public async Task PrintAndUpdateAsync(int airport, int hourLimit)
        {
            // Clears console and then enters an eternal loop.
            // First gets the current data from the finavia api and writes it to Current.xml
            // Updates flightObj with the new data and prints it. 
            // sleeps the tread for X amount of time and finally clears the screen.
            Console.Clear();
            while(true)
            {
                try
                {
                    var re = await apiObj.GetArrivalStrAsync(airport);
                    refreshCount++;
                    
                    WriteToFile.Write(FileName, ".xml", re);

                    UpdateDataLocal();
                    Console.WriteLine(new string('*', 20));
                    PrintAllInfo(hourLimit);
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Thread.Sleep(RefreshInterval);
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }     
    }
}

            