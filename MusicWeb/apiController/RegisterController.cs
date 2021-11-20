using MusicWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicWeb.apiController
{
    public class RegisterController : ApiController
    {
        DBMusicDataContext db = new DBMusicDataContext();
        // GET: api/Register
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Register/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Register
        public IHttpActionResult Post(User user)
        {
            db.Users.InsertOnSubmit(user);
            db.SubmitChanges();
            return Ok(user);
        }

        // PUT: api/Register/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Register/5
        public void Delete(int id)
        {
        }
    }
}
