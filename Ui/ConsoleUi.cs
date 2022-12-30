using System;
using Finaviaapi.Flight;
using Finaviaapi.Serializer;

namespace Finaviaapi.Ui
{
	public class ConsoleUi
	{
        // fields
		Flights? flightObj;

        // properties
        public string FileName { get;}
        public int HourDifference { get; set; }

        // constructor
        public ConsoleUi(string fileName)
		{
            FileName = fileName;
            HourDifference = 2;
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

        private static DateTime CheckEstimate(flight item, DateTime estArrival, DateTime arrivalTime)
        {
            if (item.estD == string.Empty)
            {
                estArrival = arrivalTime;
            }

            return estArrival;
        }

        // public methods

        /// <summary>
        /// Updates the flight data
        /// </summary>
        public void UpdateData()
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
                // NOTICE overriding null protection !
                foreach (var item in flightObj.arr.flight)
                {
                    DateTime.TryParse(item.sdt, out arrival);
                    if (arrival.Date == DateTime.Now.Date && arrival.Hour < (DateTime.Now.Hour + HourDifference))
                        DataPrinter(item);
                }
            }
        }
    }
}

