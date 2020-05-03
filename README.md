<h1>1. Giới thiệu Đồ án NMCNPM - Quản lý Nhà sách</h1>


Việc quản lý sách ở nhà sách hiện nay nếu làm thủ công phải cần tới khá nhiều nhân lức để thực hiện các nghiệp vụ như kiểm kê sách, tạo lập hóa đơn, báo cáo thu chi, …, một số nghiệp vụ như tìm kiếm, thống kê khi thực hiện khá vất vả. Trong khi đó, các nghiệp vụ này đều có thể tin học hóa một cách dễ dàng, việc quản lý trở nên đơn giản và thuận tiện hơn rất nhiều.
<h1>2. Cài đặt</h1>

Yêu cầu:

- Visual Studio 2015 (or higher edition)

- WPF 

- Dapper Plugin (for SQLite)

Để cài đặt, sử dụng git clone https://github.com/LichND/Bookstore-Management.git., hoặc dowload as ZIP tại URL này.

Vì project khi tải về sẽ chưa build nên bạn cần build trước khi chạy thử phần mềm, file database.db sẽ nằm trong debug folder.

<h1>3. Hướng dẫn sử dụng</h1>

Khi build xong bạn sẽ thấy giao diện login, 

![m1](https://user-images.githubusercontent.com/43202025/60238912-302a1b80-98d6-11e9-9b4e-74db32bbbc41.png)

Phần mềm sẽ có 4 quyền cho 4 loại người dùng với tài khoản và mật khẩu tương ứng như sau:

- Quyền tối cao: admin - admin

- Quyền quản lý kho: warehouse - warehouse

- Quyền quản lý nhân sự: personnel - personnel

- Quyền nhân viên: staff - staff

Để tiện kiểm thử thì bạn nên đăng nhập với quyền cao nhất để test được toàn bộ chức năng của phần mềm.

Sau khi đăng nhập thành công thì giao diện trang chủ sẽ hiện lên.

![m2](https://user-images.githubusercontent.com/43202025/60239101-b181ae00-98d6-11e9-8b18-ccc39d0bae93.png)
