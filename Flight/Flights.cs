using System.Xml.Serialization;

namespace Finaviaapi.Flight
{
    [XmlRoot("flights", Namespace = "http://www.finavia.fi/FlightsService.xsd", IsNullable = false)]
    public class Flights
    {
        public arr arr;
    }

    public class arr
    {
        public header header;
        [XmlArray("body")]
        public flight[] flight;
    }

    public class header
    {
        public string timestamp;
        public string from;
        public string description;
    }

    public class flight
    {
        [XmlElement(IsNullable = false)]
        public string h_apt;
        public string fltnr;
        public string sdt;
        public string sdate;
        public string acreg;
        public string actype;
        public string mfltnr;
        public string cflight_1;
        public string cflight_2;
        public string cflight_3;
        public string cflight_4;
        public string cflight_5;
        public string cflight_6;
        public string route_1;
        public string route_2;
        public string route_3;
        public string route_4;
        public string route_n_1;
        public string route_n_2;
        public string route_n_3;
        public string route_n_4;
        public string park;
        public string park_prv;
        public string gate;
        public string gate_prv;
        public string prm;
        public string prt;
        public string prt_f;
        public string prt_s;
        public string est_d;
        public string pest_d;
        public string act_d;
        public string ablk_d;
        public string bltarea;
        public string callsign;
    }

}