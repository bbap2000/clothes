create database shopquanao
go
use shopquanao
go

create table DanhMuc
(
idDanhmuc int identity primary key,
tendanhmuc nvarchar(30)
)
go

create table SanPham
(
masp int identity(1,1) primary key,
idDanhmuc int references
DanhMuc(idDanhmuc),
tensp nvarchar(100) not null,
hinh varchar(50),
giaban decimal(18,0),
ngaycapnhat smalldatetime,
soluongton int,
mota nvarchar(MAX),
giamgia int,
giakhuyenmai decimal(18,0)
)
go

create table KhachHangRoles(
RoleID int,
RoleName varchar(30)
primary key(RoleID)
)
go

create table KhachHang(
makh int identity(1,1) primary key,
hoten nvarchar(50),
tendangnhap varchar(20),

matkhau nvarchar(255),
email varchar(50),
diachi nvarchar(100),
dienthoai varchar(15),
ngaysinh date,
RoleID int references KhachHangRoles(RoleID),
resetpasswordcode nvarchar(255)
)
go

create table DonHang(
madon int identity(1,1) primary key,
thanhtoan nvarchar(50),
giaohang nvarchar(255),
ngaydat date,
ngaygiao date,
makh int references KhachHang(makh)
)
go

create table ChiTietDonHang(
madon int references DonHang(madon),
masp int references SanPham(masp),
tensp nvarchar(100) not null,

gia decimal(18,0),
tongsoluong int,
tonggia decimal(18,0),
primary key(madon,masp)
)
go

create table DanhGia(
	Id int identity(1,1) primary key,
	[Content] nvarchar(MAX),
	Rating float,
	Ngaycapnhap datetime,
	id_sp int references SanPham(masp),
	id_kh int references KhachHang(makh)
)
go

INSERT INTO DanhMuc (tendanhmuc)
VALUES (N'Áo khoác nam');
INSERT INTO DanhMuc (tendanhmuc)
VALUES (N'Balo');
INSERT INTO DanhMuc (tendanhmuc)
VALUES (N'Phụ kiện');

go

INSERT INTO KhachHangRoles(RoleID, RoleName)
VALUES (1, 'Admin');
go
INSERT INTO KhachHangRoles(RoleID, RoleName)
VALUES (2, 'User');
go


INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (1,
		   N'Áo khoác nam',
		   N'/Content/uploads/ao.jpg',
		   200000,
		   null,
		   3,
		   N'đây là sản phẩm dành cho nam',
		   null,
		   200000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (1,
		   N'Áo khoác nam 2',
		   N'/Content/uploads/ao2.jpg',
		   200000,
		   null,
		   2,
		   N'đây là sản phẩm dành cho nam 2',
		   null,
		   200000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (1,
		   N'Áo khoác nam 3',
		   N'/Content/uploads/ao3.jpg',
		   150000,
		   null,
		   5,
		   N'đây là sản phẩm dành cho nam 3',
		   null,
		   150000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (1,
		   N'Áo khoác nam 4',
		   N'/Content/uploads/ao4.jpg',
		   150000,
		   null,
		   5,
		   N'đây là sản phẩm dành cho nam 4',
		   null,
		   150000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (2,
		   N'Balo loại 1',
		   N'/Content/uploads/balo.jpg',
		   200000,
		   null,
		   3,
		   N'Balo loại 1',
		   null,
		   210000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (2,
		   N'Balo loại 2',
		   N'/Content/uploads/balo2.jpg',
		   134000,
		   null,
		   30,
		   N'Balo loại 2',
		   null,
		   134000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (2,
		   N'Balo loại 3',
		   N'/Content/uploads/balo3.jpg',
		   134000,
		   null,
		   30,
		   N'Balo loại 3',
		   null,
		   134000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (3,
		   N'Mũ loại 1',
		   N'/Content/uploads/mu.jpg',
		   50000,
		   null,
		   22,
		   N'Mũ loại 1',
		   null,
		   50000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (3,
		   N'Mũ loại 2',
		   N'/Content/uploads/mu2.jpg',
		   42000,
		   null,
		   22,
		   N'Mũ loại 2',
		   null,
		   42000)
GO

INSERT INTO [dbo].[SanPham]
           ([idDanhmuc]
           ,[tensp]
           ,[hinh]
           ,[giaban]
           ,[ngaycapnhat]
           ,[soluongton]
           ,[mota]
	   ,[giamgia]
	   ,[giakhuyenmai])
     VALUES
           (3,
		   N'Mũ loại 3',
		   N'/Content/uploads/mu3.jpg',
		   64000,
		   null,
		   22,
		   N'Mũ loại 3',
		   null,
		   64000)
GO

INSERT INTO [dbo].[KhachHang]
           ([hoten]
           ,[tendangnhap]
           ,[matkhau]
           ,[email]
           ,[diachi]
           ,[dienthoai]
           ,[ngaysinh]
           ,[RoleID])
     VALUES
           ('Admin'
           ,'admin'
           ,'202cb962ac59075b964b07152d234b70'
           ,'admin@gmail.com'
           ,'Le Duc Tho'
           ,0386335691
           ,'01/01/1999'
           ,1)
GO

INSERT INTO [dbo].[KhachHang]
           ([hoten]
           ,[tendangnhap]
           ,[matkhau]
           ,[email]
           ,[diachi]
           ,[dienthoai]
           ,[ngaysinh]
           ,[RoleID])
     VALUES
           ('tin'
           ,'tin'
           ,'202cb962ac59075b964b07152d234b70'
           ,'tin@gmail.com'
           ,''
           ,1234567
           ,'02/09/2000'
           ,2)
GO


INSERT INTO [dbo].[DonHang]
           ([thanhtoan]
           ,[giaohang]
           ,[ngaydat]
           ,[ngaygiao]
           ,[makh])
     VALUES
		(
        'COD',
		N'giao thành công',
		'2022-05-23',
		'2022-05-23',
		2)

GO

INSERT INTO [dbo].[ChiTietDonHang]
           ([madon]
           ,[masp]
           ,[soluong]
           ,[gia]
           ,[tongsoluong]
           ,[tonggia])
     VALUES
           (1,
		   1,
		   2,
		   200000,
		   2,
		   400000)
GO