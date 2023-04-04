using shopquanao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopquanao.Controllers
{
    public class XemDonHangController : Controller
    {
        // GET: XemDonHang
        MyDataDataContext data = new MyDataDataContext();
        // GET: DanhMuc
        //---------Index-DanhMuc------------
        public ActionResult Index()
        {
            var all_danhmuc = from dh in data.DonHangs
                              join kh in data.KhachHangs on dh.makh equals kh.makh
                              where kh.tendangnhap == Session["Username"]
            select dh;

            return View(all_danhmuc);

        }

        //---------Detail-------------
        public ActionResult DetailsCTDH2(int id)
        {
            var results = (from t1 in data.ChiTietDonHangs
                           join t2 in data.DonHangs
                           on new { t1.madon } equals
                               new { t2.madon }
                           where t2.madon == id
                           select t1).ToList();
            List<SanPham> sanpham = data.SanPhams.ToList();
            List<ChiTietDonHang> ctdh = data.ChiTietDonHangs.ToList();
            List<KhachHang> khachhang = data.KhachHangs.ToList();
            List<DonHang> donhang = data.DonHangs.ToList();
            var ViewKH2 = from kh in khachhang join dh in donhang
                         on kh.makh equals dh.makh
                         where dh.madon == id && kh.makh == dh.makh
                         select new ViewModel
                         {
                             khachhang = kh,
                             donhang = dh
                         };
            var ViewSP = from sp in sanpham
                         join ct in ctdh
      on sp.masp equals ct.masp
                         where ct.madon == id && sp.masp == ct.masp
                         select new ViewModel
                         {
                             sanpham = sp,
                             ctdh = ct
                         };

            ViewBag.ViewSP = ViewSP;

            ViewBag.ViewChiTietDH2 = ViewKH2;
            return View(results);
        }

    }
}