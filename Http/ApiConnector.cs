

namespace Finaviaapi.Http
{
    
    public class ApiConnector
    {
        // fields
        static readonly HttpClient clientObj = new();

        // properties
        public Uri UriObj { get;}
        public List<string> ParametersFlightType {get;} = new()
        {"all", "arr", "dep"};
        public List<string> ParametersAirport { get;} = new()
        {"not_hel", "all", "HEL", "RVN"};

        public ApiConnector(string uri)
        {
            UriObj = new(uri);
            clientObj.BaseAddress = UriObj;
        }

        public async Task<HttpResponseMessage> TestConnection()
        {
            HttpResponseMessage response = await clientObj.GetAsync(clientObj.BaseAddress);
            
            return response;
        }

    }
}
