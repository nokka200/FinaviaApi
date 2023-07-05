using Finaviaapi.Util;
using Finaviaapi.Ui;

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

            //Tester.SerializeFlightSingle();

            //Tester.XmlDocument();

            /*
            Console.Clear();
            while(true)
            {
                var re = Tester.WriteToFileTestWritingToFile(3);
                //await re;
                re.Wait();
                
                Console.WriteLine(DateTime.Now);

                Tester.TestUi();

                Thread.Sleep(20000);
                Console.Clear();
            }*/

            //await Tester.TestUiPrintAndUpdateAsync();


            //Tester.DiscordUiCreateClass();
            //await Tester.TestDiscordUiEchoBot();

            await RunConsole(2, 1);
        }

        private static async Task RunConsole(int airport)
        {
            ConsoleUi consoleObj = new("Current");
            consoleObj.RefreshInterval = 50000;
            consoleObj.HourDifference = 24;
            await consoleObj.PrintAndUpdateAsync(airport);
        }

        private static async Task RunConsole(int airport, int hourLimit)
        {
            ConsoleUi consoleObj = new("Current");
            consoleObj.RefreshInterval = 50000;
            consoleObj.HourDifference = 24;
            await consoleObj.PrintAndUpdateAsync(airport, hourLimit);
        }
    }
}