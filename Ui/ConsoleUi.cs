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
        readonly ApiConnector apiObj;
        const string BASE_URI = "https://api.finavia.fi/flights/public/v0/flights/";
        const string APP_ID = "FINAVIA_APP_ID";
        const string APP_KEY = "FINAVIA_APP_KEY";
        int refreshCount;
        readonly TimeOnly startTime;
        string FileName { get; }

        // PROPERTIES
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
            startTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);
        }

        // PRIVATE METHODS
        private static void DataPrinter(flight item)
        {
            // First compares estimated arrival time with arrival time, then prints a the information in a 
            // formated form.
            ConsoleColor foreground = Console.ForegroundColor;

            bool arrivalRe = DateTime.TryParse(item.sdt, out DateTime arrivalTime);
            bool estamateRe = DateTime.TryParse(item.estD, out DateTime estArrival);

            // Check if the arrival time and estimate time are available
            if (!arrivalRe || !estamateRe)
            {
                Console.WriteLine("Aika ei saatavilla");
                return;
            }

            estArrival = CheckEstimate(item, estArrival, arrivalTime);
            Console.WriteLine($"Lennon numero:\t {item.fltnr}, {item.cflight1}" +
                $", {item.cflight2}, {item.cflight3}, {item.cflight4}, {item.cflight5}" +
                $", {item.cflight6}");
            Console.WriteLine($"Lähtöpaikka:\t {item.routeN1}");
            Console.WriteLine($"Saapumisaika:\t {arrivalTime}");
            ChangeColorState(item, foreground);

            // change color based on estimate time and arrival time and writes it
            ChangeColorArrival(estArrival, arrivalTime, foreground);
            Console.WriteLine("--------------------");
        }

        private static double GetTimeDifference(DateTime estArrival, DateTime arrivalTime)
        {
            // Gets the time difference between estimated arrival time and arrival time
            TimeSpan timeDifference = estArrival - arrivalTime;
            return Math.Abs(Math.Round(timeDifference.TotalMinutes));
        }
        private static void ChangeColorState(flight item, ConsoleColor foreground)
        {
            if (item.prtF == null)
                return;

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
            double timeDifference = GetTimeDifference(estArrival, arrivalTime);
            Console.WriteLine($"Erotus:\t\t {timeDifference}");
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
            if (item == null || item.arr == null || item.arr.flight == null)
            {
                Console.WriteLine("Flight data null");
                return;
            }

            var flightItem = item.arr.flight[0];
            Console.WriteLine(DateTime.Now);
            Console.WriteLine($"Lentoasema: {flightItem.hApt}");
            Console.WriteLine($"Päivitys aika/sec: {RefreshInterval / 1000}");
            Console.WriteLine($"Kierros: {refreshCount}");
            Console.WriteLine($"Aloitusaika: {startTime}");
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
        /// <param name="hourLimit">How many hours from now</param>
        private void PrintAllInfo(int hourLimit)
        {
            flightObj = SerializeFlightData(FileName);

            if (flightObj != null && flightObj.arr != null && flightObj.arr.flight != null)
            {
                PrintMetaData(flightObj);
                foreach (var item in flightObj.arr.flight)
                {
                    bool arrivalRe = DateTime.TryParse(item.sdt, out DateTime arrival);
                    bool estArrivalRe = DateTime.TryParse(item.estD, out DateTime predictedEstamatedTime);
                    if (arrivalRe && estArrivalRe)
                    {
                        // Prints only IF
                        // 1. Arrival date is today. Makes sure that only shows todays flights NOTE, when date changes
                        // 2. Arrival hour is less than time now + hourLimit added to this. This makes we only show a limited amount of flights
                        // 3. Estimated arrival hour is more than time now hours - 1 hour, this shows later fligths that are still in the air
                        if (arrival.Date == DateTime.Now.Date && arrival.Hour < DateTime.Now.AddHours(hourLimit).Hour && predictedEstamatedTime.Hour > (DateTime.Now.Hour - 1))
                            DataPrinter(item);
                    }
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
        /// <param name="airport">Airport to track</param>
        /// <param name="hourLimit">How long get flights</param>
        /// <returns></returns>
        public async Task PrintAndUpdateAsync(int airport, int hourLimit)
        {
            // Clears console and then enters an eternal loop.
            // First gets the current data from the finavia api and writes it to Current.xml
            // Updates flightObj with the new data and prints it. 
            // sleeps the tread for X amount of time and finally clears the screen.
            Console.Clear();
            while (true)
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

