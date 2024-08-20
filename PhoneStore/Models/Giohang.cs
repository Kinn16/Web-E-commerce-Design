using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    public class Giohang
    {
        //biến tĩnh để lưu trữ thể hiện duy nhất của class Giohang (Singleton)
        private static Giohang instance;

        PhoneStoreDataContext data = new PhoneStoreDataContext();
        public int iMasp { set; get; }
        public String sTensp { set; get; }
        public String sAnhbia { set; get; }
        public Double dDongia { get; set; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get
            {
                return iSoluong * dDongia;
            }
        }
        // Khai báo một phương thức tạo (constructor) private để ngăn việc tạo đối tượng Giohang bên ngoài lớp này
        public Giohang(int MaSP)
        {
            iMasp = MaSP;
            SanPham sanPham = data.SanPhams.Single(model => model.MaSP == iMasp);
            sTensp = sanPham.TenSP;
            sAnhbia = sanPham.AnhBia;
            dDongia = double.Parse(sanPham.GiaBan.ToString());
            iSoluong = 1;

        }

        // Phương thức tạo thể hiện duy nhất của lớp Giohang (Singleton)
        public static Giohang GetInstance(int MaSP)
        {
            if (instance == null)
            {
                instance = new Giohang(MaSP);
            }
            return instance;
        }
    }
}