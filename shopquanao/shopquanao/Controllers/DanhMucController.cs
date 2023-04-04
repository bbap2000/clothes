using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopquanao.Models;

namespace shopquanao.Controllers
{
    public class DanhMucController : BaseController
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: DanhMuc
        //---------Index-DanhMuc------------
        public ActionResult Index()
        {
            var all_danhmuc = from tt in data.DanhMucs select tt;
            return View(all_danhmuc);
        }

        //---------Detail-DanhMuc------------
        public ActionResult Details(int id)
        {
            var Data_danhmuc = data.DanhMucs.Where(m => m.idDanhmuc == id).First();
            return View(Data_danhmuc);
        }

        //---------Create(tao moi the loai sach)-DanhMuc------------
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, DanhMuc tl)
        {
            var ten = collection["tendanhmuc"];
            if (string.IsNullOrEmpty(ten))
            {
                ViewData["Error"] = "Du lieu khong duoc de trong!";
            }
            else
            {
                tl.tendanhmuc = ten;
                data.DanhMucs.InsertOnSubmit(tl);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }

        //---------Edit-DanhMuc------------
        public ActionResult Edit(int id)
        {
            var E_category = data.DanhMucs.First(m => m.idDanhmuc == id);
            return View(E_category);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var danhmuc = data.DanhMucs.First(m => m.idDanhmuc == id);
            var E_tendanhmuc = collection["tendanhmuc"];
            danhmuc.idDanhmuc = id;
            if (string.IsNullOrEmpty(E_tendanhmuc))
            {
                ViewData["Error"] = "Du lieu khong duoc de trong!";
            }
            else
            {
                danhmuc.tendanhmuc = E_tendanhmuc;
                UpdateModel(danhmuc);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        //---------Delete------------
        public ActionResult Delete(int id)
        {
            var D_danhmuc = data.DanhMucs.First(m => m.idDanhmuc == id);
            return View(D_danhmuc);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_danhmuc = data.DanhMucs.Where(m => m.idDanhmuc == id).First();
            data.DanhMucs.DeleteOnSubmit(D_danhmuc);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}