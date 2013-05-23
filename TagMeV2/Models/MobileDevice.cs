using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagMeV2.Models
{
    public class MobileDevice : Entity
    {
        public string Name { get; set; }
		public string MACAddress { get; set; }
        public string Context { get; set; }
        public Location LastSeen { get; set; }
		public string Tags { get; set; }
    }
}