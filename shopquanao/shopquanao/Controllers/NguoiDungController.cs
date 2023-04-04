using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using shopquanao.Models;
using System.Security.Cryptography;
using System.Text;
using shopquanao.Common;
using System.Net.Mail;
using System.Net;

namespace shopquanao.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        MyDataDataContext data = new MyDataDataContext();

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();

        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau1 = collection["matkhau"];
            var matkhau = Encryptor.MD5Hash(matkhau1);

            var MatKhauXacNhan1 = collection["MatKhauXacNhan"];
            var MatKhauXacNhan = Encryptor.MD5Hash(MatKhauXacNhan1);

            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);
            if (matkhau1.Length < 8)
            {
                ViewData["Length>8"] = "Mật khẩu phải dài hơn 7 kí tự";
            }

            else if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phai nhap mat khau xac nhan";
            }
            
            else
            {
                if (!matkhau.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mat khau va mat khau xac nhan phai giong nhau";
                }

                else
                {
                    kh.hoten = hoten;
                    kh.tendangnhap = tendangnhap;
                    kh.matkhau = matkhau;
                    kh.email = email;
                    kh.diachi = diachi;
                    kh.dienthoai = dienthoai;
                    kh.ngaysinh = DateTime.Parse(ngaysinh);

                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();

                    return RedirectToAction("DangNhap");
                }
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();

        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau1 = collection["matkhau"];

            var matkhau = Encryptor.EncryptMD5(matkhau1);

            KhachHang kh =
                data.KhachHangs.FirstOrDefault(n => n.tendangnhap == tendangnhap && n.matkhau == matkhau);

            if (kh != null)
            {

                Session["TaiKhoan"] = kh;
                Session["Username"] = tendangnhap;


                if (kh.RoleID == 1)
                {
                    Session["Admin"] = kh;
                    return RedirectToAction("Index", "Admin");
                }
                else if (kh.RoleID == 3)
                {
                    Session["Staff"] = kh;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("ListSanPham", "SanPham");
                }

            }
            else
            {
                ViewBag.msg = "Tên đăng nhập hoặc mật khẩu không chính xác";
                return View();
            }

        }

        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            Session.Clear();
            return RedirectToAction("ListSanPham", "SanPham");
        }


        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/NguoiDung/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("langtuankiet883@gmail.com", "ShopQuanAo");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "01694512357"; 
            
            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Tai khoan da duoc tao thanh cong";
                body = "chua lam";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset password";
                body = "Bạn vừa gửi link xác thực tài khoản, Hãy click vào link bên dưới để lấy lại mật khẩu<br>" +
                    "<a href=" + link + ">Reset password</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.EnableSsl = true;
                smtp.Send(message);
            }    

            

            
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string message = "";
            MyDataDataContext data = new MyDataDataContext();
            var account = data.KhachHangs.Where(a => a.email == EmailID).FirstOrDefault();
            if (account != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.email, resetCode, "ResetPassword");
                account.resetpasswordcode = resetCode;

                data.SubmitChanges();

                message = "Một đường link đã được gửi đến email của bạn";
            }
            else
            {
                message = "Email không tồn tại";
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            MyDataDataContext data = new MyDataDataContext();
            var user = data.KhachHangs.Where(a => a.resetpasswordcode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                MyDataDataContext data = new MyDataDataContext();
                var user = data.KhachHangs.Where(a => a.resetpasswordcode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.matkhau = Encryptor.MD5Hash(model.NewPassword);
                    user.resetpasswordcode = "";

                    data.SubmitChanges();
                    message = "Cập nhập mật khẩu thành công";
                }
            }
            else
            {
                message = "Cập nhập mật khẩu thất bại";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}