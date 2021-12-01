using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MusicWeb.Models;
using System.Data.Entity;

namespace MusicWeb.apiController
{
    public class PlayListController : ApiController
    {
        DBMusicDataContext db = new DBMusicDataContext();
        // GET: api/PlayList
        public IEnumerable<PlayList> Get()
        {
            List<PlayList> ps = new List<PlayList>();
            foreach (var item in db.PlayLists)
            {
                ps.Add(new PlayList() { MaPlayList = item.MaPlayList, TenPlayList = item.TenPlayList, MaUser = item.MaUser, ThoiGianTao = item.ThoiGianTao });
            }
            return ps;
        }

        // GET: api/PlayList/5
        public IEnumerable<PlaylistModel> Get(int id)
        {
            var pl = db.PlayLists.Where(n => n.MaUser == id);
            List<PlaylistModel> pls = new List<PlaylistModel>();
            foreach (var t in pl)
            {
                pls.Add(new PlaylistModel() { MaPlayList = t.MaPlayList, TenPlayList = t.TenPlayList, MaUser = (int)t.MaUser });
            }
            return  pls;
        }

        //tạo một play list mới
        // POST: api/PlayList
        public IHttpActionResult Post(PlayList playList)
        {
            if (playList.TenPlayList == null)
            {
                return Ok(0);
            }
            else
            {
                playList.ThoiGianTao = DateTime.Now;
                db.PlayLists.InsertOnSubmit(playList);
                db.SubmitChanges();
                return Ok();
            }
        }

        // PUT: api/PlayList/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PlayList/5
        public void Delete(int id)
        {
        }
    }
}
