using System.Xml.Serialization;

namespace Finaviaapi.Flight
{
    [XmlRoot("flights", Namespace = "http://www.finavia.fi/FlightsService.xsd", IsNullable = false)]
    public class Flights
    {

    }

    public class FlightInfo
    {
        public string Hapt { get; set; }

    }
}