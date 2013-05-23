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
    public class APController : ApiController
    {
        private MongoDatabase _database;

        public APController()
        {
			//"mongodb://127.0.0.1"
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
			var server = client.GetServer();
			_database = server.GetDatabase("TagMeDB");
        }

		// POST api/AP/Register
		//registra um ponto de acesso e armazena em banco
		[ActionName("Register")]
		public HttpResponseMessage PostRegister(AccessPoint value)
		{
			if (value == null)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foram enviados dados.");
			}

			if (value._id.Equals(Guid.Empty))
			{
				MongoCollection<AccessPoint> collection = _database.GetCollection<AccessPoint>("AccessPoints");
				AccessPoint ap = collection.AsQueryable().FirstOrDefault(a => a.BSSID == value.BSSID);

				if (ap == null)
				{
					collection.Insert(value);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.OK, ap);
				}
			}

			return Request.CreateResponse(HttpStatusCode.Created, value);
		}
    }
}
