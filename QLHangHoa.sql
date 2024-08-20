Create Database QLHangHoa
Go
Use QLHangHoa
Go
--Drop table KhachHang
--Go
Create Table	KhachHang
(
	MaKH	Int	Identity(1,1),
	HoTen	Nvarchar(50)	Not Null,
	TaiKhoan	Varchar(50)	Unique,
	MatKhau	Nvarchar(MAX)	Not Null,
	Email	Varchar(100)	Unique,
	DiachiKH	Nvarchar(200),
	DienThoaiKH	Varchar(50),
	NgaySinh	Date,
	Constraint	Pk_KhachHang	Primary Key(MaKH)
)
Go
--Drop table DongSP
--Go
Create Table	DongSP
(
	MaD	Int	Identity(1,1),
	TenDong	Nvarchar(50)	Not Null,
	Constraint Pk_DongSP Primary Key(MaD)
)
Go
--Drop table DongPL
--Go
Create table DongPL
(
	MaPL Int Identity(1,1),
	TenPL Nvarchar(100) not null,
	MaD Int,
	Constraint	Pk_PhanLoai Primary Key(MaPL),
	Constraint	Fk_DSP	Foreign	Key(MaD) References	DongSP(MaD),
)
Go
--Drop table SanPham
--Go
Create Table	SanPham
(
	MaSP	Int	Identity(1,1),
	TenSP	Nvarchar(100)	Not Null,
	GiaBan	Decimal(18,0)	Check(GiaBan>=0),
	MoTa	Nvarchar(Max),
	AnhBia	Varchar(255),
	NgayCapNhat	Date,
	SoLuongTon	Int,
	MaD	Int null,
	MaPL Int null,
	Constraint	Pk_SanPham	Primary Key(MaSP),
	Constraint	Fk_DongSP	Foreign	Key(MaD) References	DongSP(MaD),
	Constraint	Fk_DongPL	Foreign Key(MaPL) References	DongPL(MaPL)
)
Go
--Drop table DonDatHang
--Go
Create Table	DonDatHang
(
	SoDH	Int Identity(1,1),
	MaKH	Int,
	NgayDH	Date default GetDate(),
	NgayGiao	Date,
	SDT varchar(50),
	Diachi nvarchar(200),
	TongTien int,
	DaThanhToan	bit default 0, --0: Chưa thanh toán; 1: Đã thanh toán
	TinhTrangGiaoHang	bit default 0, --0:Đang vận chuyển; 1: Đã giao
	Constraint	Pk_DonDatHang	Primary Key(SoDH),
	Constraint	Fk_KhachHang	Foreign	Key(MaKH)	References	KhachHang(MaKH) On Delete Cascade
)
Go
--Drop table ChiTietDatHang
--Go
Create Table	ChiTietDatHang
(
	MaCT	Int Identity,
	SoDH	Int,
	MaSP	Int,
	SoLuong	Int	Check(SoLuong>0),
	DonGia int Check(DonGia>=0),
	Constraint	Pk_ChiTietDatHang	Primary Key(MaCT),
	Constraint	Fk_DonHang	Foreign	Key(SoDH)	References	DonDatHang(SoDH) On Delete Cascade,
	Constraint	Fk_SP	Foreign Key(MaSP)	References	SanPham(MaSP) On Delete Cascade
)
Go

--Thêm dữ liệu:
---Dòng sản phẩm
	Insert into DongSP Values (N' IPhone')
	Insert into DongSP Values (N' IPad')
	Insert into DongSP values (N' Mac')
	Insert into DongSP values (N' Watch')
	Insert into DongSP values (N' Âm Thanh')
	Insert into DongSP values (N' Phụ Kiện')

--Phân loại dòng
	Insert into DongPL values (N'IPhone 15 series', 1)
	Insert into DongPL values (N'IPhone 14 series', 1)
	Insert into DongPL values (N'IPhone 13 series', 1)
	Insert into DongPL values (N'IPhone 12 series', 1)
	Insert into DongPL values (N'IPhone 11 series', 1)
	Insert into DongPL values (N'IPhone SE', 1)
	Insert into DongPL values (N'IPad Pro M1', 2)
	Insert into DongPL values (N'IPad Pro M2', 2)
	Insert into DongPL values (N'IPad Air', 2)
	Insert into DongPL values (N'IPad 9', 2)
	Insert into DongPL values (N'IPad 10', 2)
	Insert into DongPL values (N'IPad Mini', 2)
	Insert into DongPL values (N'Macbook Pro M2', 3)
	Insert into DongPL values (N'Macbook Pro M3', 3)
	Insert into DongPL values (N'Macbook Air', 3)
	Insert into DongPL values (N'iMac', 3)
	Insert into DongPL values (N'Mac Mini', 3)
	Insert into DongPL values (N'Mac Pro', 3)
	Insert into DongPL values (N'Mac Studio', 3)
	Insert into DongPL values (N'Apple Watch Ultra 2', 4)
	Insert into DongPL values (N'Apple Watch Series 9', 4)
	Insert into DongPL values (N'Apple Watch SE', 4)
	Insert into DongPL values (N'Apple Watch Series 8', 4)
	Insert into DongPL values (N'Apple Watch Series 7', 4)
	Insert into DongPL values (N'Apple Watch Series 6', 4)
	Insert into DongPL values (N'Apple Watch Series 3', 4)
	Insert into DongPL values (N'AirPods', 5)
	Insert into DongPL values (N'AirPods Pro', 5)
	Insert into DongPL values (N'EarPods', 5)
	Insert into DongPL values (N'Marshall', 5)
	Insert into DongPL values (N'Beats', 5)
	Insert into DongPL values (N'Harma Kardon', 5)
	Insert into DongPL values (N'JBL', 5)
	Insert into DongPL values (N'Google', 5)
	Insert into DongPL values (N'Sony', 5)
	Insert into DongPL values (N'Audio Technica', 5)
	Insert into DongPL values (N'Jabra', 5)
	
---Toy
	Insert into SanPham values (N' IPhone 15 128GB', 21690000, N' ','iphone_15_128_1.png','11/15/2023', 7, 1, 1)
	Insert into SanPham values (N' IPhone 15 Plus 128GB', 25490000, N' ','iphone_15_plus_128.png','11/15/2023', 7, 1, 1)
	Insert into SanPham values (N' IPhone 15 Pro 128GB', 20990000, N' ','iphone_15_pro_128.png','11/15/2023', 7, 1, 1)
	Insert into SanPham values (N' IPhone 15 Pro Max 256GB', 33400000, N' ','iphone_15_pro_max_256.png','11/15/2023', 7, 1, 1)
	Insert into SanPham values (N' IPhone 14 Pro Max 128GB', 26550000, N' ','iphone_14_pro_max_128.png','11/15/2023', 7, 1, 2)
	Insert into SanPham values (N' IPhone 15 256GB', 24790000, N' ','iphone_15_256.png','11/15/2023', 7, 1, 1)
	Insert into SanPham values (N' IPhone 14 Plus 128GB', 20990000, N' ','iphone_14_plus_128.png','11/15/2023', 7, 1, 2)
	Insert into SanPham values (N' IPhone 11 64GB', 10490000, N' ','iphone_11_64.png','11/15/2023', 7, 1, 5)

	Insert into SanPham values (N' IPad Pro M1 12.9 Inch Wifi Celluar 512GB', 32990000, N' ','ipad_pro_m1_512gb.png','11/15/2023', 10, 2, 7)
	Insert into SanPham values (N' IPad Pro M2 11 Inch Wifi 128GB', 20150000, N' ','ipad_pro_m2_128.png','11/15/2023', 10, 2, 8)
	Insert into SanPham values (N' IPad Pro M2 11 Inch Wifi Cellular 128GB', 23990000, N' ','ipad_pro_m2_wifi_cellular_128.png','11/15/2023', 10, 2, 8)
	Insert into SanPham values (N' IPad Air 4', 14990000, N' ','ipad_air_4.png','11/15/2023', 10, 2, 9)
	Insert into SanPham values (N' IPad Gen 10th 10.9 inch wifi 64GB', 10750000, N' ','ipad_gen_10_64.png','11/15/2023', 10, 2, 11)

	Insert into SanPham values (N' MacBook Air M1 2020 (8GB Ram | 256GB SSD)', 18790000, N' ','macbook_air_m1_2020_256.png','11/15/2023', 4, 3, 9)
	Insert into SanPham values (N' MacBook Pro 16 M1 Pro (16Core/16GB/1TB)', 49990000, N' ','macbook_pro_16_m1_pro_1TB.png','11/15/2023', 4, 3, 13)
	Insert into SanPham values (N' MacBook Pro 13 inch M3 Pro 2023 (18GB Ram | 1TB SSD)', 59990000, N' ','macbook_pro_2023_1TB.png','11/15/2023', 4, 3, 14)
	Insert into SanPham values (N' MacBook Air M2 2022 (8GB Ram | 256GB SSD)', 59990000, N' ','macbook_air_m2_2022_256.png','11/15/2023', 4, 3, 13)
	   	
	Insert into SanPham values (N' Apple Watch Series 9 Nhôm (GPS + Cellular) 41mm | Sport Band', 12450000, N' ','apple_watch_series_9_nhom.png','11/15/2023', 12, 4, 21)
	Insert into SanPham values (N' Apple Watch Series 9 Thép (GPS + Cellular) 41mm | Sport Band', 18990000, N' ','apple_watch_series_9_nhom.png','11/15/2023', 12, 4, 21)
	Insert into SanPham values (N' Apple Watch Ultra 2 GPS + Cellular 49mm Alpine Loop', 21490000, N' ','apple_watch_ultra_2_alpine_loop.png','11/15/2023', 12, 4, 20)
	Insert into SanPham values (N' Apple Watch SE 2023 GPS Sport Band size S/M', 6150000, N' ','apple_watch_ultra_2.png','11/15/2023', 12, 4, 22)
	Insert into SanPham values (N' Apple Watch Ultra 2 GPS + Cellular 49mm Ocean Band', 21490000, N' ','apple_watch_ultra_2.png','11/15/2023', 12, 4, 20)

	Insert into SanPham values (N' AirPods 2', 2590000, N' ','aidpods_2.png','11/15/2023', 12, 5, 27)
	Insert into SanPham values (N' Loa Marshall Acton II Bluetooth', 6190000, N' ','loa_marshall_acton_II.png','11/15/2023', 12, 5, 30)
	Insert into SanPham values (N' Loa Marshall Stanmore II Bluetooth', 6890000, N' ','loa_marshall_stanmore_II.png','11/15/2023', 12, 5, 30)
	Insert into SanPham values (N' Loa JBL GO 3', 940000, N' ','loa_jbl_go_3.png','11/15/2023', 12, 5, 33)
	Insert into SanPham values (N' Loa Harman Kardon Onyx Studio 8', 5890000, N' ','loa_harman_kardon_onyx_studio_8.png','11/15/2023', 12, 5, 32)
	Insert into SanPham values (N' Tai Nghe Marshall Major IV', 3390000, N' ','aidpods_2.png','11/15/2023', 12, 5, 26)

	Insert into SanPham values (N' Cường lực Apple iPhone 15 series Zeelot Solidleek', 350000, N' ','cuong_luc_apple_iphone_15.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Cường lực Camera Mipow Kingbull Titanshield for iPhone 15 Pro/ Pro Max', 390000, N' ','cuong_luc_camera_iphone_15.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Cường lực chống nhìn trộm MiPow Kingbull cho iPhone 15 series', 350000, N' ','cuong_luc_trong_nhin_trom_iphone_15.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Ốp Lưng Vải Tinh Dệt MagSafe cho iPhone 15', 1690000, N' ','op_lung_vai_tinh_det_magsafe_iphone_15.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Ốp Lưng Silicon MagSafe cho iPhone 15', 1490000, N' ','op_lung_sillicon_magsafe_iphone_15.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Ốp UNIQ Coehl Magnetic Charging Linear For iPhone 15 series', 590000, N' ','op_uniq_coehl_magnetic_charging_linear_iphone_15.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Đồng hồ thông minh Garmin 265 Music', 11690000, N' ','dong_ho_thong_minh_garmin_265_music.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Cáp sạc USB-C to Lightning Cable 1m FAE', 590000, N' ','cap_sac_usb_c_lightning_cable.png','11/15/2023', 20, 6, null)
	Insert into SanPham values (N' Balo TOMTOC (USA) Premium Urban Business', 1400000, N' ','balo_tomtoc_usa_prenimu_urban_business.png','11/15/2023', 20, 6, null)

--Dữ liệu cập nhật: Tài khoản quản trị
Create table Admin
(
	UserAdmin varchar(30) primary key,
	PassAdmin varchar(30) not null,
	Hoten nVarchar(50)
)
Insert into Admin values('admin','123456', N' Thái Anh')