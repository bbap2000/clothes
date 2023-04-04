using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopquanao.Models;

namespace shopquanao.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();

        public List<Giohang> Laygiohang()
        {
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang == null)
            {
                listGiohang = new List<Giohang>();
                Session["Giohang"] = listGiohang;
            }
            return listGiohang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> listGiohang = Laygiohang();
            Giohang sanpham = listGiohang.Find(n => n.masp == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        public ActionResult MuaNgay(int id, FormCollection collection)
        {
            List<Giohang> listGiohang = Laygiohang();
            Giohang sanpham = listGiohang.Find(n => n.masp == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);

                listGiohang.Add(sanpham);
                return RedirectToAction("GioHang", "GioHang");
            }
            else
            {
                sanpham.iSoluong++;
                return RedirectToAction("GioHang", "GioHang");
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Sum(n => n.iSoluong);
            }
            return tsl;
        }

        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                tt = listGiohang.Sum(n => n.iSoluong * n.giakhuyenmai);
            }
            return tt;
        }


        public ActionResult GioHang()
        {
            List<Giohang> listGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGiohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<Giohang> listGiohang = Laygiohang();
            Giohang sanpham = listGiohang.SingleOrDefault(n => n.masp == id);
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.masp == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhapGiohang(int id, FormCollection collection)
        {
            List<Giohang> listGiohang = Laygiohang();
            Giohang sanpham = listGiohang.SingleOrDefault(n => n.masp == id);
            SanPham sanpham1 = data.SanPhams.Single(n => n.masp == id);
            if (sanpham != null)
            {
                int inputSL = int.Parse(collection["txtSoLg"].ToString());
                if (inputSL > sanpham1.soluongton)
                {
                    TempData["msg"] = "<script>alert('Số lượng sản phẩm vượt quá số lượng tồn kho');</script>";
                }
                else
                {
                    sanpham.iSoluong = inputSL;
                }

            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("SanPham", "ListSanPham");
            }
            List<Giohang> listGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(listGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            SanPham s = new SanPham();
            List<Giohang> gh = Laygiohang();


            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;

            string str = "đang xử lý";
            dh.giaohang = str;
            dh.thanhtoan = "COD";

            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masp = item.masp;
                ctdh.soluong = item.iSoluong;

                ctdh.gia = (decimal?)item.giakhuyenmai;

                ctdh.tongsoluong = TongSoLuong();
                ctdh.tonggia = (decimal?)TongTien();
                s = data.SanPhams.Single(n => n.masp == item.masp);
                s.soluongton = ctdh.soluong;
                data.SubmitChanges();

                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;

            return RedirectToAction("XacnhanDonhang", "GioHang");
        }

        public ActionResult XacnhanDonhang()
        {
            return View();
        }
        // GET: GioHang

    }
}