using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MomWorld.Controllers
{
    public class StatusController : ApiController
    {

        // GET: api/Status
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Status/{{username}}
        public string Get(string username)
        {
         //   var statuses = identityDb.S .FirstOrDefault(u => u.UserName.Equals(id));
            return "value";
        }

        // POST: api/Status
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Status/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Status/5
        public void Delete(int id)
        {
        }
    }
}
