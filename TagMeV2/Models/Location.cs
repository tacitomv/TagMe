using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TagMeV2.Models
{
    public class Location
    {
        public double _x { get; set; }
        public double _y { get; set; }
    }
}