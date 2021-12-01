using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicWeb.Models;
using PagedList;
using PagedList.Mvc;

namespace MusicWeb.Controllers
{
    public class AdminController : Controller
    {
        DBMusicDataContext db = new DBMusicDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            //Creact login view
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //Gan gia tri
            var email = collection["email"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi1"] = "Phải nhập email";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập matkhau";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.Email == email && n.MatKhau == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Email hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult DoiMK()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoiMK(FormCollection collection)
        {
            var matkhaucu = collection["MatKhauCu"];
            var matkhaumoi = collection["MatKhauMoi"];
            var nhaplaiMK = collection["NhaplaiMK"];
            if (String.IsNullOrEmpty(matkhaucu))
            {
                ViewData["Loi1"] = "Hãy nhập mật khẩu cũ";
            }
            else if (String.IsNullOrEmpty(matkhaumoi))
            {
                ViewData["Loi2"] = "Hãy nhập mật khẩu mới";
            }
            else if (String.IsNullOrEmpty(nhaplaiMK))
            {
                ViewData["Loi3"] = "Hãy nhập lại mật khẩu mới";
            }
            else
            {
                //gan gi tri cho doi tuong dc tao moi
                Admin ad = (Admin)Session["Taikhoanadmin"];
                Admin admin = db.Admins.SingleOrDefault(n => n.MatKhau == matkhaucu);
                if (matkhaucu != ad.MatKhau)
                {
                    ViewBag.Thongbao = "Mật khẩu cũ không chính xác";
                }
                else
                {

                    admin.MatKhau = matkhaumoi;
                    UpdateModel(admin);
                    db.SubmitChanges();
                    Session["Taikhoanadmin"] = null;
                    return RedirectToAction("Login");
                }
            }
            return this.DoiMK();


        }
        public ActionResult Nhac(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.Nhacs.ToList());
            return View(db.Nhacs.ToList().OrderBy(n => n.MaBaiNhac).ToPagedList(pageNumber, pageSize));
        }
        //QL Bài Hát
        //Them moi bai hat
        [HttpGet]
        public ActionResult Themmoibaihat()
        {
            ViewBag.MaNgheSi = new SelectList(db.NgheSis.ToList().OrderBy(n => n.TenNgheSi), "MaNgheSi", "TenNgheSi");
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaSangTac = new SelectList(db.SangTacs.ToList().OrderBy(n => n.TenNguoiST), "MaSangTac", "TenNguoiST");
            ViewBag.MaAlbum = new SelectList(db.Albums.ToList().OrderBy(n => n.TenAlbum), "MaAlbum", "TenAlbum");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoibaihat(Nhac nhac, HttpPostedFileBase fileAnh, HttpPostedFileBase fileNhac)
        {
            ViewBag.MaNgheSi = new SelectList(db.NgheSis.ToList().OrderBy(n => n.TenNgheSi), "MaNgheSi", "TenNgheSi");
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaSangTac = new SelectList(db.SangTacs.ToList().OrderBy(n => n.TenNguoiST), "MaSangTac", "TenNguoiST");
            ViewBag.MaAlbum = new SelectList(db.Albums.ToList().OrderBy(n => n.TenAlbum), "MaAlbum", "TenAlbum");

            if (fileAnh == null || fileNhac == null)
            {
                ViewBag.Thongbao = "Thieu File";
                return this.View();
            }
            //Them csdl
            else
            {
                if (ModelState.IsValid)
                {
                    var fileNameAnh = Path.GetFileName(fileAnh.FileName);
                    var pathAnh = Path.Combine(Server.MapPath("~/img"), fileNameAnh);
                    var fileNameNhac = Path.GetFileName(fileNhac.FileName);
                    var pathNhac = Path.Combine(Server.MapPath("~/music"), fileNameNhac);

                    if (System.IO.File.Exists(pathAnh))
                    {
                        ViewBag.Thongbao = "Da ton tai file nhac";
                    }
                    else
                    {
                        fileAnh.SaveAs(pathAnh);
                        fileNhac.SaveAs(pathNhac);
                    }
                    nhac.FileAnh = fileNameAnh;
                    nhac.FileNhac = fileNameNhac;
                    //Luu vao csdl
                    db.Nhacs.InsertOnSubmit(nhac);
                    db.SubmitChanges();
                }
                return RedirectToAction("Nhac");
            }
        }
        //Xoa bai hat
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            //Lay bai hat theo ma
            Nhac nhac = db.Nhacs.SingleOrDefault(n => n.MaBaiNhac == id);
            ViewBag.MaBaiNhac = nhac.MaBaiNhac;
            if (nhac == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhac);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong can xoa theo ma
            Nhac nhac = db.Nhacs.SingleOrDefault(n => n.MaBaiNhac == id);
            ViewBag.MaBaiNhac = nhac.MaBaiNhac;
            if (nhac == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Nhacs.DeleteOnSubmit(nhac);
            db.SubmitChanges();
            return RedirectToAction("Nhac");
        }
        //sửa thông tin
        [HttpGet]
        public ActionResult Sua(int id)
        {
            //Lay sản phẩm theo mã
            Nhac nhac = db.Nhacs.SingleOrDefault(n => n.MaBaiNhac == id);
            ViewBag.MaBaiNhac = nhac.MaBaiNhac;
            ViewBag.TenBaiHat = nhac.TenNhac;
            if (nhac == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaNgheSi = new SelectList(db.NgheSis.ToList().OrderBy(n => n.TenNgheSi), "MaNgheSi", "TenNgheSi", nhac.MaBaiNhac);
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", nhac.MaBaiNhac);
            ViewBag.MaSangTac = new SelectList(db.SangTacs.ToList().OrderBy(n => n.TenNguoiST), "MaSangTac", "TenNguoiST", nhac.MaBaiNhac);
            ViewBag.MaAlbum = new SelectList(db.Albums.ToList().OrderBy(n => n.TenAlbum), "MaAlbum", "TenAlbum", nhac.MaBaiNhac);
            return View(nhac);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(Nhac nhac, HttpPostedFileBase fileAnh, HttpPostedFileBase fileNhac)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaNgheSi = new SelectList(db.NgheSis.ToList().OrderBy(n => n.TenNgheSi), "MaNgheSi", "TenNgheSi");
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaSangTac = new SelectList(db.SangTacs.ToList().OrderBy(n => n.TenNguoiST), "MaSangTac", "TenNguoiST");
            ViewBag.MaAlbum = new SelectList(db.Albums.ToList().OrderBy(n => n.TenAlbum), "MaAlbum", "TenAlbum");
            Nhac nhac2 = db.Nhacs.Single(n => n.MaBaiNhac == nhac.MaBaiNhac);
            //Kiem tra duong dan file
            if (fileAnh == null && fileNhac == null)
            {
                var fileNameAnh = Path.GetFileName(fileAnh.FileName);
                var fileNameNhac = Path.GetFileName(fileNhac.FileName);
                DateTime now = DateTime.Now;
                string date_str = now.ToString("yyyyMMdd_HHmmss");
                var path = Path.Combine(Server.MapPath("~/img"), fileNameAnh);
                var path1 = Path.Combine(Server.MapPath("~/music"), fileNameNhac);
                if (System.IO.File.Exists(path) && System.IO.File.Exists(path1))
                {
                    ViewBag.Thongbao = "File đã tồn tại";
                }
                else
                {
                    fileAnh.SaveAs(path);
                    fileNhac.SaveAs(path1);
                }
                nhac.FileAnh = fileNameAnh;
                nhac2.FileAnh = nhac.FileAnh;
                nhac.FileNhac = fileNameNhac;
                nhac2.FileNhac = nhac.FileNhac;
            }
            //Luu vao CSDL
            nhac2.TenNhac = nhac.TenNhac;
            if (nhac.NgayCN != null)
            {
                nhac2.NgayCN = nhac.NgayCN;
            }
            nhac2.MaLoai = nhac.MaLoai;
            nhac2.MaNgheSi = nhac.MaNgheSi;
            nhac2.MaSangTac = nhac.MaSangTac;
            nhac2.MaAlbum = nhac.MaAlbum;
            db.SubmitChanges();
            return RedirectToAction("Nhac");
        }
        //QL thể loại nhạc
        //DS the loại nhạc
        public ActionResult TheLoaiNhac(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.Nhacs.ToList());
            return View(db.TheLoais.ToList().OrderBy(n => n.MaLoai).ToPagedList(pageNumber, pageSize));
        }
        //Thêm thể loại nhạc
        [HttpGet]

        public ActionResult ThemLoaiNhac()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ThemLoaiNhac(TheLoai theloainhac)
        {
            db.TheLoais.InsertOnSubmit(theloainhac);
            db.SubmitChanges();
            return RedirectToAction("TheLoaiNhac");
        }
        //Xóa
        [HttpGet]
        public ActionResult XoaTheLoaiNhac(int id)
        {
            TheLoai loainhac = db.TheLoais.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.Maloai = loainhac.MaLoai;
            if (loainhac == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loainhac);
        }
        [HttpPost, ActionName("XoaTheLoaiNhac")]
        public ActionResult XacNhanXoa(int id)
        {
            TheLoai loainhac = db.TheLoais.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.Maloai = loainhac.MaLoai;
            if (loainhac == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.TheLoais.DeleteOnSubmit(loainhac);
            db.SubmitChanges();
            return RedirectToAction("TheLoaiNhac");
        }
        //QL Nghệ Sĩ
        //DS Nghệ Sĩ
        public ActionResult DSNgheSi(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.NgheSis.ToList().OrderBy(n => n.MaNgheSi).ToPagedList(pageNumber, pageSize));
        }
        //Thêm nghệ sĩ
        [HttpGet]

        public ActionResult ThemNgheSi()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ThemNgheSi(NgheSi nghesi)
        {
            db.NgheSis.InsertOnSubmit(nghesi);
            db.SubmitChanges();
            return RedirectToAction("DSNgheSi");
        }
        //Xóa
        [HttpGet]
        public ActionResult XoaNgheSi(int id)
        {
            NgheSi nghesi = db.NgheSis.SingleOrDefault(n => n.MaNgheSi == id);
            ViewBag.MaNgheSi = nghesi.MaNgheSi;
            if (nghesi == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nghesi);
        }
        [HttpPost, ActionName("XoaNgheSi")]
        public ActionResult XacNhanXoaNgheSi(int id)
        {
            NgheSi nghesi = db.NgheSis.SingleOrDefault(n => n.MaNgheSi == id);
            ViewBag.MaNgheSi = nghesi.MaNgheSi;
            if (nghesi == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NgheSis.DeleteOnSubmit(nghesi);
            db.SubmitChanges();
            return RedirectToAction("DSNgheSi");
        }
        //QL Sáng Tác
        //DS Nghệ Sĩ Sáng Tác
        public ActionResult DSSangTac(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SangTacs.ToList().OrderBy(n => n.MaSangTac).ToPagedList(pageNumber, pageSize));
        }
        //Thêm sáng tác
        [HttpGet]

        public ActionResult ThemSangTac()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ThemSangTac(SangTac sangtac)
        {
            db.SangTacs.InsertOnSubmit(sangtac);
            db.SubmitChanges();
            return RedirectToAction("DSSangTac");
        }
        //Xóa
        [HttpGet]
        public ActionResult XoaSangTac(int id)
        {
            SangTac sangtac = db.SangTacs.SingleOrDefault(n => n.MaSangTac == id);
            ViewBag.MaSangTac = sangtac.MaSangTac;
            if (sangtac == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sangtac);
        }
        [HttpPost, ActionName("XoaSangTac")]
        public ActionResult XacNhanXoaSangTac(int id)
        {
            SangTac sangtac = db.SangTacs.SingleOrDefault(n => n.MaSangTac == id);
            ViewBag.MaSangTac = sangtac.MaSangTac;
            if (sangtac == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SangTacs.DeleteOnSubmit(sangtac);
            db.SubmitChanges();
            return RedirectToAction("DSSangTac");
        }
        //QL Album
        //DS Album
        public ActionResult DSAlbum(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Albums.ToList().OrderBy(n => n.MaAlbum).ToPagedList(pageNumber, pageSize));
        }
        //Thêm sáng tác
        [HttpGet]

        public ActionResult ThemAlbum()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ThemAlbum(Album album)
        {
            db.Albums.InsertOnSubmit(album);
            db.SubmitChanges();
            return RedirectToAction("DSAlbum");
        }
        //Xóa
        [HttpGet]
        public ActionResult XoaAlbum(int id)
        {
            Album album = db.Albums.SingleOrDefault(n => n.MaAlbum == id);
            ViewBag.MaAlbum = album.MaAlbum;
            if (album == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(album);
        }
        [HttpPost, ActionName("XoaAlbum")]
        public ActionResult XacNhanXoaAlbum(int id)
        {
            Album album = db.Albums.SingleOrDefault(n => n.MaAlbum == id);
            ViewBag.MaAlbum = album.MaAlbum;
            if (album == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Albums.DeleteOnSubmit(album);
            db.SubmitChanges();
            return RedirectToAction("DSAlbum");
        }
        //QL User
        //DS User
        public ActionResult DSUser(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Users.ToList().OrderBy(n => n.MaUser).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult XoaUser(int id)
        {
            User user = db.Users.SingleOrDefault(n => n.MaUser == id);
            ViewBag.MaUser = user.MaUser;
            if (user == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(user);
        }
        [HttpPost, ActionName("XoaUser")]
        public ActionResult XacNhanXoaUser(int id)
        {
            Album album = db.Albums.SingleOrDefault(n => n.MaAlbum == id);
            ViewBag.MaAlbum = album.MaAlbum;
            if (album == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Albums.DeleteOnSubmit(album);
            db.SubmitChanges();
            return RedirectToAction("DSAlbum");
        }

    }
}