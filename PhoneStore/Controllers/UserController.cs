using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using System.Web.Security;
using ToyStore.common;

namespace ToyStore.Controllers
{
    public interface IMediator
    {
        void SendMessage(string message, object sender);
        void Register(string message, Action<object> action);
    }
    public class UserMediator : IMediator
    {
        private readonly Dictionary<string, List<Action<object>>> _colleagues;

        public UserMediator()
        {
            _colleagues = new Dictionary<string, List<Action<object>>>();
        }

        public void Register(string message, Action<object> action)
        {
            if (!_colleagues.ContainsKey(message))
            {
                _colleagues[message] = new List<Action<object>>();
            }
            _colleagues[message].Add(action);
        }

        public void SendMessage(string message, object sender)
        {
            if (_colleagues.ContainsKey(message))
            {
                foreach (var action in _colleagues[message])
                {
                    action(sender);
                }
            }
        }
    }

    public class UserController : Controller
    {
        //Mẫu Mediator
        private readonly IMediator _mediator;
        private readonly PasswordDirector _passwordDirector;

        public UserController()
        {
            // Tạo một đối tượng xây dựng mật khẩu (Base64PasswordBuilder)
            IPasswordBuilder passwordBuilder = new Base64PasswordBuilder();

            // Khởi tạo đối tượng PasswordDirector với xây dựng mật khẩu đã tạo
            _passwordDirector = new PasswordDirector(passwordBuilder);

            _mediator = new UserMediator(); // Create an instance of UserMediator
            _mediator.Register("UserSignedUp", (obj) =>
            {
                ViewBag.ThongBao = "Đăng ký thành công!";
            });
        }
        PhoneStoreDataContext data = new PhoneStoreDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var tendn = collection["TenDN"];
            var hoten = collection["HoTenKH"];
            var ngaysinh = String.Format("{0:dd/MM/yyyy}", collection["NgaySinh"]);
            var sdt = collection["SDT"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var matkhau = collection["MatKhau"];
            var matkhaunhaplai = collection["MatKhauNhapLai"];

            _passwordDirector.Construct(matkhau);
            string encodedPassword = _passwordDirector.GetResult();

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi2"] = "Tên không được để trống";
            }
            if (String.IsNullOrEmpty(ngaysinh))
            {
                ngaysinh = "01/01/2000";
            }
            if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi4"] = "Số điện thoại không được để trống";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi5"] = "Địa chỉ không được để trống";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Email không được để trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi7"] = "Mật khẩu không được để trống";
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi8"] = "Mật khẩu không được để trống";
            }
            else if (matkhaunhaplai != matkhau)
            {
                ViewData["Loi9"] = "Mật khẩu không khớp";
            }             
            else
            {
                kh.TaiKhoan = tendn;
                kh.HoTen = hoten;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                kh.DienThoaiKH = sdt;
                kh.DiachiKH = diachi;
                kh.Email = email;
                kh.MatKhau = encodedPassword;
                if (data.KhachHangs.Any(x => x.TaiKhoan == kh.TaiKhoan))
                {
                    ViewBag.LoiTK = "Tài khoản đã tồn tại";
                }
                else if (data.KhachHangs.Any(x => x.Email == kh.Email))
                {
                    ViewBag.LoiEmail = "Email đã được đăng ký";
                }
                else
                {
                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    TempData["SuccessMessage"] = "Đăng ký thành công!";


                    _mediator.SendMessage("UserSignedUp", kh);
                    return RedirectToAction("DangNhap");
                }                
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["Taikhoan"] != null)
            {
                return RedirectToAction("Index", "PhoneStore");                
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection, string returnUrl)
        {
            
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống";
            }

            else
            {
                // Sử dụng cùng một cơ chế mã hóa mật khẩu như trong phương thức DangKy
                _passwordDirector.Construct(matkhau);
                string encodedPassword = _passwordDirector.GetResult();

                // Biến kiểm tra tendn và matkhau có khớp với db k, nếu ko khớp thì null
                KhachHang kh = data.KhachHangs.SingleOrDefault(model => model.TaiKhoan == tendn && model.MatKhau == encodedPassword);
                if (kh != null)
                {
                    FormsAuthentication.SetAuthCookie(kh.HoTen, false);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        //ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                        Session["Taikhoan"] = kh;
                        return RedirectToAction("Index", "PhoneStore");
                    }
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult DonDatHang(string username)
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                return View(data.DonDatHangs.Where(i => i.KhachHang.TaiKhoan == kh.TaiKhoan).ToList().OrderByDescending(n => n.NgayDH));
            }
        }
        public ActionResult Chitietdonhang(string username, int id)
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                return View(data.ChiTietDatHangs.ToList().Where(n => n.SoDH == id && n.DonDatHang.KhachHang.TaiKhoan == kh.TaiKhoan).OrderBy(n => n.MaCT));
            }
        }        
        [HttpGet]
        public ActionResult Information()
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                return View(data.KhachHangs.Single(n => n.TaiKhoan == kh.TaiKhoan));
            }
        }
        [HttpPost]
        public ActionResult Information(FormCollection collection)
        {
            KhachHang sskh = (KhachHang)Session["Taikhoan"];
            KhachHang kh = data.KhachHangs.Single(n => n.TaiKhoan == sskh.TaiKhoan);
            var hoten = collection["HoTen"];
            var diachi = collection["DiachiKH"];
            var sdt = collection["DienThoaiKH"];
            var ngaysinh = collection["NgaySinh"];

            if (ModelState.IsValid)
            {
                kh.HoTen = hoten;
                kh.DiachiKH = diachi;
                kh.DienThoaiKH = sdt;
                kh.NgaySinh = Convert.ToDateTime(ngaysinh);

                data.SubmitChanges();
            }
            return RedirectToAction("Information");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            if (kh == null || kh.ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }

            return View(data.KhachHangs.Single(tk => tk.TaiKhoan == kh.TaiKhoan));
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            KhachHang sskh = (KhachHang)Session["Taikhoan"];
            KhachHang kh = data.KhachHangs.Single(n => n.TaiKhoan == sskh.TaiKhoan);
            
            //Áp dụng mẫu Builder 
            var oldpassword = collection["oldpassword"];
            var newpassword = collection["newpassword"];
            var retypepass = collection["retypepass"];

            // Sử dụng cùng một cơ chế mã hóa mật khẩu như trong phương thức DangKy
            _passwordDirector.Construct(newpassword);
            string encodedNewPassword = _passwordDirector.GetResult();
            _passwordDirector.Construct(oldpassword);
            string encodedPassword = _passwordDirector.GetResult();

            if (String.IsNullOrEmpty(oldpassword) || String.IsNullOrEmpty(newpassword) || String.IsNullOrEmpty(retypepass))
            {
                ViewBag.LoiNhap = "Mật khẩu không được để trống!";
            }

            else if (encodedPassword != kh.MatKhau)
            {
                ViewData["Loi3"] = "Mật khẩu cũ không đúng!";
            }
            else if (retypepass != newpassword)
            {
                ViewData["Loi2"] = "Mật khẩu nhập lại không khớp!";
            }
            else if (encodedNewPassword == kh.MatKhau)
            {
                ViewData["Loi4"] = "Mật khẩu mới không được trùng với mật khẩu cũ!";
            }
            else
            {
                kh.MatKhau = encodedNewPassword;
                data.SubmitChanges();
                return RedirectToAction("Information", "User");
            }
            return this.Information();

        }
        public ActionResult SignOut()
        {
            Session["Taikhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "PhoneStore");
        }
    }
}