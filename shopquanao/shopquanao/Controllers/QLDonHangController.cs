﻿using shopquanao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace shopquanao.Controllers
{
    public class QLDonHangController : BaseController
    {
        MyDataDataContext data = new MyDataDataContext();

        //---------Index-DanhMuc------------
        public ActionResult Index()
        {
            var all_danhmuc = from tt in data.DonHangs select tt;
            return View(all_danhmuc);

        }

        //---------Detail-------------
        public ActionResult DetailsCTDH(int id)
        {
            var results = (from t1 in data.ChiTietDonHangs
                          join t2 in data.DonHangs
                          on new { t1.madon} equals
                              new { t2.madon }
                          where t2.madon == id 
                          select t1).ToList();

            List<KhachHang> khachhang = data.KhachHangs.ToList();
            List<DonHang> donhang = data.DonHangs.ToList();

            List<ChiTietDonHang> ctdh = data.ChiTietDonHangs.ToList();
            List<SanPham> sanpham = data.SanPhams.ToList();

            var ViewKH = from kh in khachhang join dh in donhang
                         on kh.makh equals dh.makh
                         where dh.madon == id && kh.makh == dh.makh
                         select new ViewModel
                         {
                             khachhang = kh,
                             donhang = dh
                         };

            var ViewSP = from sp in sanpham join ct in ctdh
                         on sp.masp equals ct.masp
                         where ct.madon == id && sp.masp == ct.masp
                         select new ViewModel
                         {
                             sanpham = sp,
                             ctdh = ct
                         };

            ViewBag.ViewChiTietDH = ViewKH;
            ViewBag.ViewSP = ViewSP;

            return View(results);
        }

        //---------Edit-Don Hang------------
        public ActionResult Edit(int id)
        {
            var E_category = data.DonHangs.First(m => m.madon == id);
            return View(E_category);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var danhmuc = data.DonHangs.First(m => m.madon == id);
            var E_tendanhmuc = collection["giaohang"];
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            danhmuc.madon = id;
            if (string.IsNullOrEmpty(E_tendanhmuc))
            {
                ViewData["Error"] = "Du lieu khong duoc de trong!";
            }
            else
            {
                danhmuc.giaohang = E_tendanhmuc;
                danhmuc.ngaygiao = DateTime.Parse(ngaygiao);
                UpdateModel(danhmuc);
                
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        //---------Delete------------
        public ActionResult Delete(int id)
        {
            var D_danhmuc = data.DonHangs.FirstOrDefault(n => n.madon == id);
            return View(D_danhmuc);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {

            var D_danhmuc2 = data.ChiTietDonHangs.Where(m => m.madon == id).First();
            data.ChiTietDonHangs.DeleteOnSubmit(D_danhmuc2);
            data.SubmitChanges();

            var D_danhmuc = data.DonHangs.Where(m => m.madon == id).First();
            data.DonHangs.DeleteOnSubmit(D_danhmuc);
            data.SubmitChanges();

            return RedirectToAction("Index");
        }

    }
}