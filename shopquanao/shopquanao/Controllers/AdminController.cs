using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopquanao.Models;
using PagedList;
namespace shopquanao.Controllers
{
    public class AdminController : BaseController
    {
        MyDataDataContext data = new MyDataDataContext();

        #region

        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();
            ViewBag.Online = HttpContext.Application["Online"].ToString();

            ViewBag.TongDoanhThu = ThongKeDoanhThu();
            ViewBag.TongDDH = ThongKeDonHang();
            ViewBag.TongKhachHang = ThongKeKhachHang();

            return View();
        }

        public decimal ThongKeDoanhThu()
        {
            decimal TongDoanhThu = data.ChiTietDonHangs
                .Sum(
                n => n.soluong * n.gia
            ).Value;
            return TongDoanhThu;
        }

        public double ThongKeDonHang()
        {
            double slddh = data.DonHangs.Count();
            return slddh;
        }
        public double ThongKeKhachHang()
        {
            double slkh = data.KhachHangs.Count();
            return slkh;
        }
        public ActionResult DoanhThu()
        {
            ViewBag.TongDoanhThu = ThongKeDoanhThu();

            var getlist = data.SanPhams.ToList();
            SelectList list = new SelectList(getlist, "masp", "tensp");

            return View();
        }
        public ActionResult DoanhThuNgay(FormCollection collection)
        {
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            var getDay = DateTime.Parse(ngaygiao.ToString()).Day;
            var getMonth = DateTime.Parse(ngaygiao.ToString()).Month;
            var getYear = DateTime.Parse(ngaygiao.ToString()).Year;

            ViewBag.Ngay = getDay;
            ViewBag.Thang = getMonth;
            ViewBag.Nam = getYear;
            ViewBag.DoanhThuNgay = ThongKeDoanhThuNgay(getDay, getMonth, getYear);
            ViewBag.DoanhThuNgayCount = ThongKeDoanhThuNgayCount(getDay, getMonth, getYear);
            List<DonHang> donhang = data.DonHangs.ToList();
            List<ChiTietDonHang> ctdh = data.ChiTietDonHangs.ToList();
            var ViewSP = (from sp in donhang
                          join ct in ctdh
                          on sp.madon equals ct.madon
                          where sp.ngaydat.Value.Day == getDay && sp.ngaydat.Value.Month == getMonth && sp.ngaydat.Value.Year == getYear && ct.status == 1
                          select new ViewModel
                          {
                              donhang = sp,
                              ctdh = ct
                          }).GroupBy(test => test.ctdh.madon)
                   .Select(grp => grp.First());


            ViewBag.ListCountDTN = ViewSP;

            return View();
        }
        public ActionResult DoanhThuThang(FormCollection collection)
        {
            var ngaygiao = String.Format("{0:mm/dd/yyyy}", collection["NgayGiao"]);
            var getMonth = DateTime.Parse(ngaygiao.ToString()).Month;
            var getYear = DateTime.Parse(ngaygiao.ToString()).Year;

            ViewBag.Thang = getMonth;
            ViewBag.Nam = getYear;
            ViewBag.DoanhThuThang = ThongKeDoanhThuThang(getMonth, getYear);
            ViewBag.DoanhThuThangCount = ThongKeDoanhThuThangCount(getMonth, getYear);

            List<DonHang> donhang = data.DonHangs.ToList();
            List<ChiTietDonHang> ctdh = data.ChiTietDonHangs.ToList();
            var ViewSP = (from sp in donhang
                          join ct in ctdh
                          on sp.madon equals ct.madon
                          where sp.ngaydat.Value.Month == getMonth && sp.ngaydat.Value.Year == getYear && ct.status == 1
                          select new ViewModel
                          {
                              donhang = sp,
                              ctdh = ct
                          }).GroupBy(test => test.ctdh.madon)
                   .Select(grp => grp.First());


            ViewBag.ListCountDTN = ViewSP;

            return View();
        }
        public decimal ThongKeDoanhTungSanPhamChoGia(int id)
        {
            decimal TongDoanhThu = data.ChiTietDonHangs
                .Where(m => m.masp == id && m.status == 1)
                .Sum(
                n => (n.soluong * n.gia)
            ).Value;
            return TongDoanhThu;
        }
        public decimal ThongKeDoanhTungSanPhamChoSL(int id)
        {
            decimal TongDoanhThu = data.ChiTietDonHangs
                .Where(m => m.masp == id && m.status == 1)
                .Sum(
                n => (n.soluong)
            ).Value;
            return TongDoanhThu;
        }
        public decimal ThongKeDoanhThuNgay(int Ngay, int Thang, int Nam)
        {
            var listDDH = data.DonHangs.Where(n => n.ngaydat.Value.Day == Ngay && n.ngaydat.Value.Month == Thang &&
            n.ngaydat.Value.Year == Nam);
            decimal tongtien = 0;
            foreach (var item in listDDH)
            {
                tongtien += decimal.Parse(item.ChiTietDonHangs
                    .Where(m => m.status == 1)
                    .Sum(
                    n => n.soluong * n.gia).Value.ToString()
                );
            }
            return tongtien;
        }
        public decimal ThongKeDoanhThuNgayCount(int Ngay, int Thang, int Nam)
        {
            var listDDH = data.DonHangs.Where(n => n.ngaydat.Value.Day == Ngay && n.ngaydat.Value.Month == Thang &&
            n.ngaydat.Value.Year == Nam);
            decimal tongtien = 0;
            foreach (var item in listDDH)
            {
                tongtien += decimal.Parse(item.ChiTietDonHangs
                    .Where(m => m.status == 1)
                    .Sum(
                    n => n.soluong).Value.ToString()
                );
            }
            return tongtien;
        }
        public decimal ThongKeDoanhThuThang(int Thang, int Nam)
        {
            var listDDH = data.DonHangs.Where(n => n.ngaydat.Value.Month == Thang &&
            n.ngaydat.Value.Year == Nam);
            decimal tongtien = 0;
            foreach (var item in listDDH)
            {
                tongtien += decimal.Parse(item.ChiTietDonHangs
                    .Where(m => m.status == 1)
                    .Sum(
                    n => n.soluong * n.gia).Value.ToString()
                );
            }
            return tongtien;
        }
        public decimal ThongKeDoanhThuThangCount(int Thang, int Nam)
        {
            var listDDH = data.DonHangs.Where(n => n.ngaydat.Value.Month == Thang &&
            n.ngaydat.Value.Year == Nam);
            decimal tongtien = 0;
            foreach (var item in listDDH)
            {
                tongtien += decimal.Parse(item.ChiTietDonHangs
                    .Where(m => m.status == 1)
                    .Sum(
                    n => n.soluong).Value.ToString()
                );
            }
            return tongtien;
        }

        public ActionResult QLSanPham(int? page)
        {
            if (page == null) page = 1;
            var all_sach = (from s in data.SanPhams select s).OrderBy(m => m.masp);
            int pageSize = 5;
            int pageNum = page ?? 1;
            return View(all_sach.ToPagedList(pageNum, pageSize));
        }


        public ActionResult Details(int id)
        {
            var D_SanPham = data.SanPhams.Where(m => m.masp == id).First();
            return View(D_SanPham);
        }

        public ActionResult Create()
        {
            var getlist = data.DanhMucs.ToList();
            SelectList list = new SelectList(getlist, "idDanhmuc", "tendanhmuc");
            ViewBag.fulllist = list;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, SanPham s)
        {
            var E_tensp = collection["tensp"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_giamgia = Convert.ToInt32(collection["giamgia"]);
            var E_ngaycapnhat = DateTime.Now;
            var E_iddanhmuc = collection["idDanhmuc"];
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_mota = collection["mota"];
            if (string.IsNullOrEmpty(E_tensp))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.tensp = E_tensp.ToString();
                s.hinh = E_hinh;
                s.giaban = E_giaban;
                s.giamgia = E_giamgia;

                s.idDanhmuc = Convert.ToInt32(E_iddanhmuc);
                s.ngaycapnhat = E_ngaycapnhat;
                s.soluongton = E_soluongton;
                s.mota = E_mota;

                var x = s.giaban;
                var y = s.giamgia;

                var z = (x * y) / 100;

                var price = x - z;

                s.giakhuyenmai = price;

                data.SanPhams.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var getlist = data.DanhMucs.ToList();
            SelectList list = new SelectList(getlist, "idDanhmuc", "tendanhmuc");
            ViewBag.fulllist = list;

            var E_sach = data.SanPhams.First(m => m.masp == id);
            return View(E_sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_sach = data.SanPhams.First(m => m.masp == id);
            var E_tensach = collection["tensp"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_giamgia = collection["giamgia"];
            var E_ngaycapnhat = DateTime.Now;
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_mota = collection["mota"];
            E_sach.masp = id;
            if (string.IsNullOrEmpty(E_tensach))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_sach.tensp = E_tensach;
                E_sach.hinh = E_hinh;

                E_sach.giaban = E_giaban;
                E_sach.giamgia = Convert.ToInt32(E_giamgia);
                E_sach.ngaycapnhat = E_ngaycapnhat;
                E_sach.soluongton = E_soluongton;
                E_sach.mota = E_mota;

                var x = E_sach.giaban;
                var y = E_sach.giamgia;

                var z = (x * y) / 100;

                var price = x - z;

                E_sach.giakhuyenmai = price;

                UpdateModel(E_sach);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        //--Lay duong dan hinh anh khi sua
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        public ActionResult Delete(int id)
        {
            var D_sach = data.SanPhams.First(m => m.masp == id);
            return View(D_sach);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_sach = data.SanPhams.Where(m => m.masp == id).First();
            data.SanPhams.DeleteOnSubmit(D_sach);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        #endregion

        
        public ActionResult ChiaQuyen()
        {
            List<KhachHang> khachhang = data.KhachHangs.ToList();
            List<KhachHangRole> khachhangrole = data.KhachHangRoles.ToList();

            var main = from kh in khachhang
                       join r in khachhangrole
                       on kh.RoleID equals r.RoleID
                       where (kh.RoleID == r.RoleID)
                       select new ViewModel
                       {
                           khachhang = kh,
                           khachhangrole = r
                       };
            ViewBag.Main = main;
            return View();
        }

        public ActionResult SuaQuyen(int id)
        {
            var E_category = data.KhachHangs.First(m => m.makh == id);
            return View(E_category);
        }
        [HttpPost]
        public ActionResult SuaQuyen(int id, FormCollection collection)
        {
            var danhmuc = data.KhachHangs.First(m => m.makh == id);
            var E_tendanhmuc = collection["RoleID"];
            if (string.IsNullOrEmpty(E_tendanhmuc))
            {
                ViewData["Error"] = "Du lieu khong duoc de trong!";
            }
            else
            {
                danhmuc.RoleID = Convert.ToInt32(E_tendanhmuc);
                UpdateModel(danhmuc);
                data.SubmitChanges();
                return RedirectToAction("Admin/ChiaQuyen");
            }
            return this.Edit(id);
        }
    }
}