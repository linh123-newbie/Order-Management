# Quản lý đơn hàng

Ứng dụng ASP.NET Core MVC và Web API sử dụng SQLite để quản lý đơn hàng và xem báo cáo doanh thu theo ngày.

## Cài đặt và chạy

Yêu cầu: .NET SDK 8 và công cụ `dotnet-ef` phiên bản 8.

```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.11
dotnet ef database update
dotnet run --launch-profile http
```

Nếu đã cài `dotnet-ef`, bỏ qua lệnh cài đặt. Mở <http://localhost:5051/Home/Home> để sử dụng. Các component hiện gọi API tại `http://localhost:5051`, do đó cần chạy ứng dụng trên cổng này. Cấu hình SQLite mặc định trong `appsettings.json` sử dụng file `sqlite.db` tương đối với thư mục chạy; chạy các lệnh từ thư mục dự án để dùng cùng database.

## Chức năng và giả định

- Mỗi đơn hàng có một mặt hàng; thành tiền được tính bằng số lượng nhân đơn giá.
- Đơn mới có trạng thái `Mới` và ngày tạo lấy theo giờ máy chủ. Các đơn cũ có thể chưa có ngày tạo.
- Giao diện cho phép tạo, sửa, xóa và đổi trạng thái đơn. Báo cáo nhóm theo ngày tạo và chỉ tính các đơn `Đã giao`.
- Không có hệ thống đăng nhập hoặc phân quyền.

## Giới hạn hiện tại

- Các thao tác xóa chưa kiểm tra trạng thái đơn; chưa có test tự động.
- Chưa kèm ảnh chụp hoặc video minh họa tạo đơn, đổi trạng thái và xem báo cáo.
