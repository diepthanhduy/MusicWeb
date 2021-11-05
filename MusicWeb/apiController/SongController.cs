using MusicWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicWeb.apiController
{
    public class SongController : ApiController
    {
        DBMusicDataContext db = new DBMusicDataContext();

        // GET: api/Song
        public IEnumerable<Song> Get()
        {
            List<Song> listSong = new List<Song>();
            foreach (var item in db.Nhacs)
            {
                listSong.Add(new Song() { IDBaiHat = item.MaBaiNhac, TenBaiHat = item.TenNhac, FileAnh = item.FileAnh,
                                          TenNgheSi = item.NgheSi.TenNgheSi});
            }
            return listSong;
        }

        // GET: api/Song/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Song
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Song/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Song/5
        public void Delete(int id)
        {
        }
    }
}
