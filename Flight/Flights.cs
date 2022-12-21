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
        [XmlElement(IsNullable = false, ElementName = "cflight1")]
        public string? cflight_1;
        [XmlElement(IsNullable = false, ElementName = "cflight2")]
        public string? cflight_2;
        [XmlElement(IsNullable = false, ElementName = "cflight3")]
        public string? cflight_3;
        [XmlElement(IsNullable = false, ElementName = "cflight4")]
        public string? cflight_4;
        [XmlElement(IsNullable = false, ElementName = "cflight5")]
        public string? cflight_5;
        [XmlElement(IsNullable = false, ElementName = "cflight6")]
        public string? cflight_6;
        [XmlElement(IsNullable = false, ElementName = "route1")]
        public string? route_1;
        [XmlElement(IsNullable = false, ElementName = "route2")]
        public string? route_2;
        [XmlElement(IsNullable = false, ElementName = "route3")]
        public string? route_3;
        [XmlElement(IsNullable = false, ElementName = "route4")]
        public string? route_4;
        [XmlElement(IsNullable = false, ElementName = "routeN1")]
        public string? route_n_1;
        [XmlElement(IsNullable = false, ElementName = "routeN2")]
        public string? route_n_2;
        [XmlElement(IsNullable = false, ElementName = "routeN3")]
        public string? route_n_3;
        [XmlElement(IsNullable = false, ElementName = "routeN4")]
        public string? route_n_4;
        [XmlElement(IsNullable = false)]
        public string? park;
        [XmlElement(IsNullable = false, ElementName = "parkPrv")]
        public string? park_prv;
        [XmlElement(IsNullable = false)]
        public string? gate;
        [XmlElement(IsNullable = false, ElementName = "gatePrv")]
        public string? gate_prv;
        [XmlElement(IsNullable = false)]
        public string? prm;
        [XmlElement(IsNullable = false)]
        public string? prt;
        [XmlElement(IsNullable = false, ElementName = "prtF")]
        public string? prt_f;
        [XmlElement(IsNullable = false, ElementName = "prtS")]
        public string? prt_s;
        [XmlElement(IsNullable = false, ElementName = "prtD")]
        public string? est_d;
        [XmlElement(IsNullable = false, ElementName = "pestD")]
        public string? pest_d;
        [XmlElement(IsNullable = false, ElementName = "actD")]
        public string? act_d;
        [XmlElement(IsNullable = false, ElementName = "ablkD")]
        public string? ablk_d;
        [XmlElement(IsNullable = false)]
        public string? bltarea;
        [XmlElement(IsNullable = false)]
        public string? callsign;
    }

}