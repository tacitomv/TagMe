using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagMeV2.Models
{
	public class AccessPointSignal
	{
		public AccessPoint AP { get; set; }
		public double Signal { get; set; }
		public double Distance { get; set; }
	}

	public class MobileDeviceLocation
	{
		public Location Local { get; set; }
		public ICollection<AccessPointSignal> APs { get; set; }
	}
}