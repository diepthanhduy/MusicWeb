using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using MusicWeb.Models;

namespace MusicWeb.apiController
{
    public class LoginController : ApiController
    {
        // GET: api/Login
        DBMusicDataContext db = new DBMusicDataContext();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Login/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Login
        public IHttpActionResult Post(LoginModel login)
        {

            try
            {
                if (login.Equals(null)){
                    return Ok(login);
                }

                var check = db.Users.Where(c => c.TaiKhoan == login.TaiKhoan && c.MatKhau == login.MatKhau).Single();
                //List<LoginModel> user = new List<LoginModel>();
                LoginModel user = new LoginModel();
                user.TenUser = check.TenUser;
                user.TaiKhoan = check.TaiKhoan;
                user.MaUser = check.MaUser;
                if (check.TaiKhoan.Length > 0)
                {
                    return Ok(user);
                }
                return Ok(user);
            }
            catch (Exception e) {
                return Ok(e);
            }
        }

        // PUT: api/Login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }
}
