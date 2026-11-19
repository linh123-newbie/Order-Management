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
- Giao diện cho phép tạo, sửa, xóa và đổi trạng thái đơn. Đơn `Mới` thay đổi được trạng thái và sửa, đơn `Đã giao` và `Huỷ` thì không.
- Báo cáo nhóm theo ngày tạo và chỉ tính các đơn `Đã giao`.

## Giao diện

* Danh sách đơn hàng
<img width="1917" height="871" alt="image" src="https://github.com/user-attachments/assets/eb984706-71df-4bad-85a9-558164c0a2d9" />
* Thêm đơn hàng
<img width="1917" height="867" alt="image" src="https://github.com/user-attachments/assets/834f563b-6128-440e-9e74-24fef3b7403a" />
<img width="1912" height="863" alt="image" src="https://github.com/user-attachments/assets/21395c18-195c-4c8c-b658-d205e73d8ca8" />
* Xoá đơn hàng
<img width="1912" height="871" alt="image" src="https://github.com/user-attachments/assets/d51002fd-1f8a-4355-8a81-777748f221b9" />
<img width="1917" height="870" alt="image" src="https://github.com/user-attachments/assets/2f94dd7d-6f53-49f9-bf5f-d4ca74f6b3b7" />
* Thay đổi trạng thái
<img width="1913" height="867" alt="image" src="https://github.com/user-attachments/assets/cb644065-7332-4618-9dab-64e06e8a7ab1" />
* Báo cáo danh thu
<img width="1913" height="867" alt="image" src="https://github.com/user-attachments/assets/24409291-6b79-44a9-856a-c4a793bdc04b" />

