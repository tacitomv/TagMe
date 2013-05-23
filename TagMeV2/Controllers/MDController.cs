using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using TagMeV2.Models;
using MongoDB.Driver.Linq;

namespace TagMeV2.Controllers
{
    public class MDController : ApiController
    {
        private MongoDatabase _database;

        public MDController()
        {
			//"mongodb://127.0.0.1"
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
			var server = client.GetServer();
			_database = server.GetDatabase("TagMeDB");
        }

        // GET api/MD/getDistances
        [ActionName("MyLocation")]
		public HttpResponseMessage PostLocation(Guid id, MobileDeviceLocation value)
        {
			if (id == null)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foram enviados dados.");
			}

			var mobileDevices = _database.GetCollection<MobileDevice>("MobileDevices");
            MobileDevice device = mobileDevices.FindOneById(id);

			var accessPoints = _database.GetCollection<AccessPoint>("AccessPoints");

			//Para cada AP que o dispositivo enxerga
            foreach(AccessPointSignal aps in value.APs)
            {
				//Se AP não existir, adiciona
				if (aps.AP._id.Equals(Guid.Empty))
				{
					accessPoints.Insert(aps.AP);
				}
                //Calcula distancia
                aps.Distance = Math.Pow(10, ((-32.44 -20 * Math.Log10(aps.AP.Frequency) - aps.Signal)/20));
            }

            //Para cada AP que o dispositivo enxerga
            List<AccessPointSignal> lista = value.APs.ToList();
            List<AccessPointIntersection> intersecs = new List<AccessPointIntersection>();
            var next = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                next += 1;
                if(next == lista.Count){
                    next = 0;
                }
                var inter = new AccessPointIntersection(lista[i], lista[next]);
                if (inter.hasIntersection())
                {
                    //Se existe intersecção
                    intersecs.Add(inter);
                }
            }

            //Se temos pelo menos 3 intersecções
            if (intersecs.Count >= 3)
            {
                double deltaX = 0, deltaY = 0;
                foreach (AccessPointIntersection intersec in intersecs)
                {
                    deltaX += intersec.Center._x;
                    deltaY += intersec.Center._y;
                }

                device.LastSeen = new Location()
                {
                    _x = deltaX / intersecs.Count,
                    _y = intersecs.Count,
                };

                mobileDevices.Save(device);
            }

			return Request.CreateResponse(HttpStatusCode.Created, device);
        }

		// POST api/MD/register
		// registra um novo dispositivo
		[ActionName("Register")]
		public HttpResponseMessage PostRegister(MobileDevice value)
		{
			if (value == null)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foram enviados dados.");
			}

			if (value._id.Equals(Guid.Empty))
			{
				MongoCollection<MobileDevice> collection = _database.GetCollection<MobileDevice>("MobileDevices");
				MobileDevice md = collection.AsQueryable().FirstOrDefault(m => m.MACAddress == value.MACAddress);

				if (md == null)
				{
					collection.Insert(value);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.OK, md);
				}
			}

            return Request.CreateResponse(HttpStatusCode.Created, value);
		}

		// GET api/MD/register
		// registra um novo dispositivo
		[ActionName("Register")]
		public HttpResponseMessage GetRegister([FromUri] MobileDevice value)
		{
			if (value == null)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foram enviados dados.");
			}

			if (value._id.Equals(Guid.Empty))
			{
				MongoCollection<MobileDevice> collection = _database.GetCollection<MobileDevice>("MobileDevices");
				MobileDevice md = collection.AsQueryable().FirstOrDefault(m => m.MACAddress == value.MACAddress);

				if (md == null)
				{
					collection.Insert(value);
				}
				else if (value.Equals(md))
				{
					collection.Save(md);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.OK, md);
				}
			}

			return Request.CreateResponse(HttpStatusCode.Created, value);
		}
    }
}