using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.ViewModels;

using PagedList;
using PagedList.Mvc; //áp dụng mẫu PageList

namespace ToyStore.Controllers
{
    // mẫu Facade 
    // Lớp Facade sẽ cung cấp các phương thức đơn giản cho việc truy vấn dữ liệu.
    public class PhoneStoreFacade
    {
        private PhoneStoreDataContext data;

        public PhoneStoreFacade()
        {
            data = new PhoneStoreDataContext();
        }

        public IEnumerable<DongSP> GetAllDongSP()
        {
            return data.DongSPs;
        }

        public IEnumerable<SanPham> GetSanPhamsByDong(int id, int pageNum = 1, int pageSize = 12)
        {
            return data.SanPhams.Where(t => t.MaD == id).ToPagedList(pageNum, pageSize);
        }

        public IEnumerable<DongPL> GetPhanLoaiByDong(int idD)
        {
            return data.DongPLs.Where(pl => pl.MaD == idD);
        }

        public IEnumerable<SanPham> GetSanPhamsByLoai(int id, int idD, int pageNum = 1, int pageSize = 12)
        {
            var sanPhams = from sp in data.SanPhams
                           join d in data.DongSPs on sp.MaD equals d.MaD
                           where sp.MaPL == id && sp.MaD == idD
                           select sp;

            return sanPhams.ToPagedList(pageNum, pageSize);
        }

        public IEnumerable<SanPham> GetSanPhamDetails(int id)
        {
            return data.SanPhams.Where(sp => sp.MaSP == id);
        }

        public IEnumerable<SanPham> GetAllSanPhams(int pageNum = 1, int pageSize = 12)
        {
            return data.SanPhams.ToPagedList(pageNum, pageSize);
        }

        public IEnumerable<SanPham> SearchSanPhams(string searchString, int pageNum = 1, int pageSize = 12)
        {
            var sanPhams = from t in data.SanPhams
                           select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.TenSP.Contains(searchString));
            }

            return sanPhams.ToPagedList(pageNum, pageSize);
        }

        public IEnumerable<SanPham> GetSanPhamDeXuat()
        {
            Random rand = new Random();
            int ranNum = rand.Next(1, 229);
            return data.SanPhams.ToList().Skip(ranNum).Take(6);
        }

        // Thêm các phương thức khác tại đây cho các chức năng khác của ứng dụng
    }

    public class PhoneStoreController : Controller
    {
        private PhoneStoreFacade facade;

        //PhoneStoreDataContext data = new PhoneStoreDataContext(); // thay vì hệ thống sẽ truy cập thằng này, controller sẽ tương tác với lớp của Facade
        // GET: PhoneStore

        public PhoneStoreController()
        {
            facade = new PhoneStoreFacade();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DongSP()
        {
            var dongsp = facade.GetAllDongSP();
            return PartialView(dongsp);
        }
        public ActionResult DongSanPham (int id, int ? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);

            var toy = facade.GetSanPhamsByDong(id, pageNum, pageSize);
            ViewBag.Page = pageNum;
            ViewBag.idD = id;
            return View(toy.ToPagedList(pageNum, pageSize));
        }

        public ActionResult PhanLoai(int idD)
        {
            var phanloai = facade.GetPhanLoaiByDong(idD);
            ViewBag.idD = idD;
            return PartialView(phanloai);
        }
        public ActionResult SPtheoLoai (int id, int idD, int ? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);

            //var toy = from sp in data.SanPhams
            //          join d in data.DongSPs
            //          on sp.MaD equals d.MaD
            //          where sp.MaPL == id
            //          select sp;
            //ViewBag.Page = pageNum;
            //ViewBag.idD = idD;
            //return View(toy.ToPagedList(pageNum, pageSize));

            var toy = facade.GetSanPhamsByLoai(id, idD, pageNum, pageSize);
            ViewBag.Page = pageNum;
            ViewBag.idD = idD;
            return View(toy);
        }
        public ActionResult Details (int id)
        {
            var model = new DetailProViewModel()
            {
                Detail = facade.GetSanPhamDetails(id),
            };

            return View(model);
        }
        public ActionResult Products (int ? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);

            var sanpham = facade.GetAllSanPhams(pageNum, pageSize);
            ViewBag.Page = pageNum;
            return View(sanpham.ToPagedList(pageNum, pageSize));
        }

        public ActionResult CollectionPro(int id)
        {
            var collection = facade.GetSanPhamsByDong(id).Take(4).ToList();
            return PartialView(collection);
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Search (string searchString, int ? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            var toy = facade.SearchSanPhams(searchString, pageNum, pageSize);
            
            ViewBag.Search = searchString;
            ViewBag.Page = pageNum;
            return View(toy);
        }
        public IEnumerable<SanPham> SanPhamDeXuat ()
        {
            var sanPhams = facade.GetSanPhamDeXuat();
            return sanPhams;
        }
    }
}