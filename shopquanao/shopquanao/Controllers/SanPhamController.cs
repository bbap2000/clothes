using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using shopquanao.Models;

namespace shopquanao.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult ListSanPham()
        {
            HomeModel objHomeModel = new HomeModel();

            objHomeModel.ListCategory = data.DanhMucs.ToList();
            objHomeModel.ListProduct = data.SanPhams.ToList();

            return View(objHomeModel);
        }

        public ActionResult TatCaSanPham(int? page)
        {
            if (page == null) page = 1;
            var all_sach = (from s in data.SanPhams select s).OrderBy(m => m.masp);
            int pageSize = 5;
            int pageNum = page ?? 1;
            return View(all_sach.ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            MyDataDataContext data = new MyDataDataContext();
            List<SanPham> sanpham = data.SanPhams.ToList();
            var main = from sp in sanpham
                       where (sp.masp == id)
                       select new ViewModel
                       {
                           sanpham = sp
                       };

            /*List<DanhMuc> danhmuc = data.DanhMucs.ToList();
            var sub = (from sp in sanpham join d in danhmuc
                      on sp.idDanhmuc equals d.idDanhmuc
                       where sp.idDanhmuc == dm
                       select new ViewModel
                       {
                           sanpham = sp,
                           danhmuc = d
                       }).Take(5);
            */
            Random rnd = new Random();
            var _randomizedList = (from sp in sanpham
                                   orderby rnd.Next()
                                   select new ViewModel
                                   {
                                       sanpham = sp
                                   }).Take(5);

            List<KhachHang> khachhang = data.KhachHangs.ToList();
            List<DanhGia> danhgia = data.DanhGias.ToList();
            var ViewDanhGia = from dg in danhgia
                              join kh in khachhang
                              on dg.id_kh equals kh.makh
                              where (dg.id_sp == id && dg.id_kh == kh.makh)
                              select new ViewModel
                              {
                                  danhgia = dg,
                                  khachhang = kh
                              };


            ViewBag.Main = main;
            //ViewBag.Sub = sub;
            ViewBag.SelectedPostt = _randomizedList;
            ViewBag.ViewDanhGia = ViewDanhGia;
            SanPham product = data.SanPhams.FirstOrDefault(n => n.masp == id);
            Session["IdSp"] = product.masp;


            return View();
        }

        [HttpPost]
        public ActionResult SendDanhGia(DanhGia review, double rating, string content)
        {
            string username = Session["Username"].ToString();
            review.Ngaycapnhap = DateTime.Now;
            review.Content = content;
            review.id_kh = data.KhachHangs.Single(
                 a => a.tendangnhap.Equals(username)).makh;
            review.Rating = rating;
            review.id_sp = (int)Session["IdSp"];
            data.DanhGias.InsertOnSubmit(review);
            data.SubmitChanges();
            return RedirectToAction("Details", "SanPham", new { id = review.id_sp });
        }

        public ActionResult productDanhmuc(int id)
        {
            var listpr = data.SanPhams.Where(n => n.idDanhmuc == id).ToList();
            return View(listpr);
        }


    }
}
