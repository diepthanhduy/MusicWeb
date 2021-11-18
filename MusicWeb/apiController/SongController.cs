
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MusicWeb.Models;

namespace MusicWeb.apiController
{
    public class SongController : ApiController
    {
        DBMusicDataContext db = new DBMusicDataContext();

        // GET: api/Song (get all song)
        public IEnumerable<Song> Get()
        {
            List<Song> listSong = new List<Song>();
            foreach (var item in db.Nhacs)
            {
                listSong.Add(new Song() { IDBaiHat = item.MaBaiNhac, TenBaiHat = item.TenNhac, FileAnh = item.FileAnh,
                                          TenNgheSi = item.NgheSi.TenNgheSi, FileNhac = item.FileNhac});
            }
            return listSong;
        }

        // GET: api/Song/5 (get by id)
        public IEnumerable<Song> Get(int id)
        {
            var nhac = db.Nhacs.SingleOrDefault(x => x.MaBaiNhac == id);
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
