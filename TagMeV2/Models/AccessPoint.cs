using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagMeV2.Models
{
    public class AccessPoint : Entity
    {
        public string Name { get; set; }
        public string BSSID { get; set; }
        public int Frequency { get; set; }
        public string Capabilities { get; set; }
        public Location Location { get; set; }
    }
}