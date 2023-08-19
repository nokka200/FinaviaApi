using Finaviaapi.Util;
using Finaviaapi.Ui;

namespace Finaviaapi
{
    class Program
    {
        static async Task Main()
        {
            await RunConsole(2, 1);
        }

        /// <summary>
        /// Runs the console ui
        /// </summary>
        /// <param name="airport">Airport to watch</param>
        /// <param name="hourLimit">How many hours ahead watching</param>
        /// <returns></returns>
        private static async Task RunConsole(int airport, int hourLimit)
        {
            ConsoleUi consoleObj = new("Current")
            {
                RefreshInterval = 50000
            };
            await consoleObj.PrintAndUpdateAsync(airport, hourLimit);
        }
    }
}