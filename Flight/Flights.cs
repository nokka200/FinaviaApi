using System.Xml.Serialization;

namespace Finaviaapi.Flight
{
    [XmlRoot("flights", Namespace = "http://www.finavia.fi/FlightsService.xsd", IsNullable = false)]
    public class Flights
    {
        [XmlElement("arr")]
        public Arr? arr;
    }

    public class Arr
    {
        [XmlElement("header")]
        public Header? header;

        [XmlArray("body")]
        public flight[]? flight;
    }
    
    public class Header
    {
        public string? timestamp;
        public string? from;
        public string? description;
    }

    // BUG naming to Flight not working 
    public class flight
    {
        [XmlElement(IsNullable = false, ElementName = "h_apt")]
        public string? hApt;
        [XmlElement(IsNullable = false)]
        public string? fltnr;
        [XmlElement(IsNullable = false)]
        public string? sdt;
        [XmlElement(IsNullable = false)]
        public string? sdate;
        [XmlElement(IsNullable = false)]
        public string? acreg;
        [XmlElement(IsNullable = false)]
        public string? actype;
        [XmlElement(IsNullable = false)]
        public string? mfltnr;
        [XmlElement(IsNullable = false, ElementName = "cflight_1")]
        public string? cflight1;
        [XmlElement(IsNullable = false, ElementName = "cflight_2")]
        public string? cflight2;
        [XmlElement(IsNullable = false, ElementName = "cflight_3")]
        public string? cflight3;
        [XmlElement(IsNullable = false, ElementName = "cflight_4")]
        public string? cflight4;
        [XmlElement(IsNullable = false, ElementName = "cflight_5")]
        public string? cflight5;
        [XmlElement(IsNullable = false, ElementName = "cflight_6")]
        public string? cflight6;
        [XmlElement(IsNullable = false, ElementName = "route_1")]
        public string? route1;
        [XmlElement(IsNullable = false, ElementName = "route_2")]
        public string? route2;
        [XmlElement(IsNullable = false, ElementName = "route_3")]
        public string? route3;
        [XmlElement(IsNullable = false, ElementName = "route_4")]
        public string? route4;
        [XmlElement(IsNullable = false, ElementName = "route_n_1")]
        public string? routeN1;
        [XmlElement(IsNullable = false, ElementName = "route_n_2")]
        public string? routeN2;
        [XmlElement(IsNullable = false, ElementName = "route_n_3")]
        public string? routeN3;
        [XmlElement(IsNullable = false, ElementName = "route_n_4")]
        public string? routeN4;
        [XmlElement(IsNullable = false)]
        public string? park;
        [XmlElement(IsNullable = false, ElementName = "park_prv")]
        public string? parkPrv;
        [XmlElement(IsNullable = false)]
        public string? gate;
        [XmlElement(IsNullable = false, ElementName = "gate_prv")]
        public string? gatePrv;
        [XmlElement(IsNullable = false)]
        public string? prm;
        [XmlElement(IsNullable = false)]
        public string? prt;
        [XmlElement(IsNullable = false, ElementName = "prt_f")]
        public string? prtF;
        [XmlElement(IsNullable = false, ElementName = "prt_s")]
        public string? prtS;
        [XmlElement(IsNullable = false, ElementName = "est_d")]
        public string? estD;
        [XmlElement(IsNullable = false, ElementName = "pest_d")]
        public string? pestD;
        [XmlElement(IsNullable = false, ElementName = "act_d")]
        public string? actD;
        [XmlElement(IsNullable = false, ElementName = "ablk_d")]
        public string? ablkD;
        [XmlElement(IsNullable = false)]
        public string? bltarea;
        [XmlElement(IsNullable = false)]
        public string? callsign;
    }

}