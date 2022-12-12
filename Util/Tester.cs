using Finaviaapi.Http;

namespace Finaviaapi.Util
{
    public static class Tester
    {
        // constants
        const string BASE_URI = "https://api.finavia.fi/flights/public/v0/flights/";

        // classes
        static ApiConnector apiObj = new(BASE_URI);

        /*ApiConnector Tests*/

        static public void ApiConnectorTestProperties()
        {
            string value;

            value = apiObj.UriObj.ToString();
            Console.WriteLine($"Value: {value}");
        }

        static public async Task ApiConnectorTestConnection()
        {
            var re = await apiObj.TestConnection();
            Console.WriteLine(re);
        }

    }
}
