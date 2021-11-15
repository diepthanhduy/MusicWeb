using MusicWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicWeb.apiController
{
    public class RandSongController : ApiController
    {
        DBMusicDataContext db = new DBMusicDataContext();
        // GET: api/RandSong
        public IEnumerable<Song> Get()
        {
            var qry = from row in db.Nhacs
                      select row;

            int count = qry.Count();
            int index = new Random().Next(count);

            var nhac = qry.Skip(index).FirstOrDefault();
            List<Song> song = new List<Song>();
            song.Add(new Song()
            {
                IDBaiHat = nhac.MaBaiNhac,
                TenBaiHat = nhac.TenNhac
            ,
                FileAnh = nhac.FileAnh,
                FileNhac = nhac.FileNhac,
                TenNgheSi = nhac.NgheSi.TenNgheSi
            });
            return (IEnumerable<Song>)song;
        }

        // GET: api/RandSong/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/RandSong
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/RandSong/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RandSong/5
        public void Delete(int id)
        {
        }
    }
}
