

namespace Finaviaapi.Http
{
    /// <summary>
    /// Represents a Http client that connects to finavias api for flight information.
    /// Gets api authentications from environment variables
    /// </summary>
    public class ApiConnector
    {
        // fields
        static readonly HttpClient clientObj = new();
        const string ID = "app_id";
        const string KEY = "app_key";

        // properties
        public Uri UriObj { get; }
        public List<string> ParametersFlightType { get; } = new()
        {"all", "arr", "dep"};
        public List<string> ParametersAirport { get; } = new()
        {"not_hel", "all", "HEL", "RVN"};
        /// <summary>
        /// Retrieves environment varialbe named this as app id.
        /// </summary>
        public string AppIdEnv { get; }
        /// <summary>
        /// Retrieves environment varialbe named this as app key.
        /// </summary>
        public string AppKeyEnv { get; }

        public ApiConnector(string uri, string appIdEnv, string appKeyEnv)
        {
            // Creates a uri object using the uri as parameter and assings clientObj BaseAddress as uirObj.
            // Finally sets up header info using app id and app key environment values.

            UriObj = new(uri);
            clientObj.BaseAddress = UriObj;
            AppIdEnv = appIdEnv;
            AppKeyEnv = appKeyEnv;

            SetUpHeaders();
        }

        // public methods

        /// <summary>
        /// Checks the connection to the api
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> TestConnectionAsync()
        {
            HttpResponseMessage response = await clientObj.GetAsync(clientObj.BaseAddress);

            return response;
        }

        /// <summary>
        /// Gets the arrival information.
        /// </summary>
        /// <param name="airport">Index of airport from ParametersAirport list</param>
        /// <returns>Content in string</returns>
        public async Task<string> GetArrivalStrAsync(int airport)
        {
            string content;

            var response = await clientObj.GetAsync(clientObj.BaseAddress + "/arr/" + ParametersAirport[airport]);
            content = await response.Content.ReadAsStringAsync();

            return content;
        }

        /// <summary>
        /// Sets up the headers for authentication
        /// </summary>
        // private methods
        private void SetUpHeaders()
        {
            clientObj.DefaultRequestHeaders.Accept.Clear();
            clientObj.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml")
            );
            clientObj.DefaultRequestHeaders.Add(ID, System.Environment.GetEnvironmentVariable(AppIdEnv));
            clientObj.DefaultRequestHeaders.Add(KEY, System.Environment.GetEnvironmentVariable(AppKeyEnv));
        }
    }
}
