using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
namespace ToyStore.Controllers
{
    public class GioHangController : Controller
    {
        // Observer interface
        public interface IShoppingCartObserver
        {
            void ItemAddedToCart(int productId, int quantity);
        }

        // Observable class
        public class ShoppingCart
        {
            private List<IShoppingCartObserver> observers = new List<IShoppingCartObserver>();

            public void Subscribe(IShoppingCartObserver observer)
            {
                observers.Add(observer);
            }

            public void AddItemToCart(int productId, int quantity)
            {
                // Logic to add item to cart
                // Notify observers
                foreach (var observer in observers)
                {
                    observer.ItemAddedToCart(productId, quantity);
                }
            }
        }

        // Observer class
        public class ShoppingCartView : IShoppingCartObserver
        {
            public void ItemAddedToCart(int productId, int quantity)
            {
                // Update user interface with new cart information
                Console.WriteLine($"Sản phẩm {productId} đã được thêm vào giỏ hàng với số lượng {quantity}.");
            }
        }

        PhoneStoreDataContext data = new PhoneStoreDataContext();
        // GET: Cart
        public ActionResult CartDefault()
        {
            return View();
        }
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
                Session.Timeout = 60;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohangChiTiet(int iMaSP, string strURL, FormCollection f)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                // Nếu người dùng chưa đăng nhập, đặt thông báo và chuyển hướng đến trang đăng nhập
                TempData["ErrorMessage"] = "Vui lòng đăng nhập hoặc đăng ký để thực hiện thao tác này.";
                return RedirectToAction("Dangnhap", "User");
            }

            List<Giohang> lstGiohang = Laygiohang(); // Lấy ra Session["Giohang"]
            Giohang sanpham = lstGiohang.Find(n => n.iMasp == iMaSP);// Kiểm tra sách này có tồn tại trong Session["Giohang"] chưa?
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSP);
                lstGiohang.Add(sanpham);
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong += int.Parse(f["txtSoluong"].ToString());
                return Redirect(strURL);
            }
        }
        public ActionResult ThemGiohang(int iMaSP, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang(); // Lấy ra Session["Giohang"]
            Giohang sanpham = lstGiohang.Find(n => n.iMasp == iMaSP);// Kiểm tra sách này có tồn tại trong Session["Giohang"] chưa?
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSP);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private double TongSoLuong()// Method tính tổng số lượng
        {
            double iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        private double TongTien()// Method tính tổng tiền
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        [HttpGet]
        // GET: Giohang
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("CartDefault", "GioHang");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public PartialViewResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = Laygiohang();// Lấy giỏ hàng từ session
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasp == iMaSP); //  kiểm tra sách đã có trong session chưa?
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMasp == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "ToyStore");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult CapnhapGiohang(int iMaSP, int soluong)
        {
            List<Giohang> lstGiohang = Laygiohang();// Lấy giỏ hàng từ session
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasp == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = soluong;
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult SuaGioHang()
        {
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "ToyStore");
            }
            List<Giohang> lstGiohang = Laygiohang();
            return View(lstGiohang);
        }    
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "ToyStore");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "User");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "ToyStore");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.SDT = kh.DienThoaiKH;
            ddh.Diachi = collection["Diachi"];
            ddh.TongTien = Convert.ToInt32(collection["tongtien"]);
            ddh.DaThanhToan = false;
            ddh.TinhTrangGiaoHang = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var i in gh)
            {
                ChiTietDatHang ctdh = new ChiTietDatHang();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaSP = i.iMasp;
                ctdh.SoLuong = i.iSoluong;
                ctdh.DonGia = (int)i.dThanhtien;
                data.ChiTietDatHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("DonDatHang", "User");
        }
        public ActionResult OrderConfirmation()
        {
            return View();
        }
    }
}