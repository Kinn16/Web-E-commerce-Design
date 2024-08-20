using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using System.IO;
using System.Web.Security;
using static ToyStore.Controllers.AdminController;

namespace ToyStore.Controllers
{
    public class AdLoginController : Controller
    {
        // GET: AdLogin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            Admin ad = (Admin)Session["Taikhoanadmin"];
            if (ad != null)
            {
                return RedirectToAction("Index", "Admin", new { area = "", controller = "AdminController", action = "Index" });

            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection, string returnUrl)
        {
            PhoneStoreDataContext data = new PhoneStoreDataContext();
            //Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu chưa được nhập";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    FormsAuthentication.SetAuthCookie(ad.Hoten, false);
                    //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    //    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    //{
                    //    return Redirect(returnUrl);
                    //}
                    //else
                    //{
                        //ViewBag.ThongBaoDangNhap = "Đăng nhập thành công";
                        Session["Taikhoanadmin"] = ad;
                        return RedirectToAction("Index", "Admin");
                    //}
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult SignOut()
        {
            Session["Taikhoanadmin"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "AdLogin");
        }
        // Mẫu Factory
        public class ThemmoiSPStrategyFactory
        {
            public static IThemmoiSPStrategy CreateStrategy(PhoneStoreDataContext data, int strategyType)
            {
                switch (strategyType)
                {
                    case 1:
                        return new ThemmoiSPStrategy1(data);
                    case 2:
                        return new ThemmoiSPStrategy2(data);
                    default:
                        throw new ArgumentException("Invalid strategy type.");
                }
            }
        }
    }
}