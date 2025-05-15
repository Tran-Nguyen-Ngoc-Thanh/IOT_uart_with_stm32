# COMMUNICATION UART
- [Giới thiệu UART](#giới-thiệu-UART)
  - [Thành phần chuổi](#thành-phần-chuổi)
  - [Ưu và nhược điểm](#ưu-và-nhược-điểm)
- [Hướng dẫn sử dụng](#hướng-dẫn-sử-dụng)
- [Thư viện cần có](#thư-viện-cần-có)
  - [Thêm bằng Nuget](#thêm-bằng-Nuget)
  - [Thêm bằng .NET CLI](#thêm-bằng-.NET-CLI)
- [Tài liệu tham khảo](#tài-liệu-tham-khảo)

# Giới thiệu UART
UART (Universal Asynchronous Receiver-Transmitter) bộ truyền nhận dữ liệu nối tiếp bất đồng bộ, đây là một trong những giao thức truyền thông giữa thiết bị với thiết bị được sử dụng nhiều nhất. Giao tiếp UART được sử dụng nhiều trong các ứng dụng để giao tiếp với các module như: Wifi, Bluetooth, Xbee, module đầu đọc thẻ RFID với Raspberry Pi, Arduino hoặc vi điều khiển khác. Đây cũng là chuẩn giao tiếp thông dụng và phổ biến trong công nghiệp.

Trong giao tiếp UART, hai UART giao tiếp trực tiếp với nhau. UART truyền chuyển đổi dữ liệu song song từ một thiết bị điều khiển như CPU thành dạng nối tiếp, truyền nó nối tiếp đến UART nhận, sau đó chuyển đổi dữ liệu nối tiếp trở lại thành dữ liệu song song cho thiết bị nhận.  
  
UART truyền dữ liệu nối tiếp, theo một trong ba chế độ:
- Full duplex: Giao tiếp đồng thời đến và đi từ mỗi master và slave.  
- Half duplex: Dữ liệu đi theo một hướng tại một thời điểm.  
- Simplex: Chỉ giao tiếp một chiều.
  
![image](https://github.com/user-attachments/assets/3d846035-37f4-41b7-9e17-d052978d6c4d)

## Thành phần chuổi
Dữ liệu truyền qua UART được tổ chức thành các gói. Mỗi gói chứa 1 bit bắt đầu, 5 đến 9 bit dữ liệu (tùy thuộc vào UART), một bit chẵn lẻ tùy chọn và 1 hoặc 2 bit dừng.

- Bit khởi đầu: Đường truyền dữ liệu trong giao tiếp UART được giữ ở mức điện áp cao khi nó không truyền dữ liệu. Để bắt đầu truyền dữ liệu, UART truyền sẽ kéo đường truyền từ mức cao xuống mức thấp trong một chu kỳ đồng hồ. Khi UART 2  phát hiện sự chuyển đổi điện áp cao xuống thấp, nó bắt đầu đọc các bit trong khung dữ liệu ở tần số của tốc độ truyền (Baud rate).

- Khung dữ liệu: Khung dữ liệu chứa dữ liệu thực tế đang được truyền. Nó có thể dài từ 5 bit đến 8 bit nếu sử dụng bit Parity (bit chẵn lẻ). Nếu không sử dụng bit Parity, khung dữ liệu có thể dài 9 bit. Trong hầu hết các trường hợp, dữ liệu được gửi với bit LSB (bit có trọng số thấp nhất) trước tiên.

- Bit chẵn lẻ: Bit chẵn lẻ là một cách để UART nhận cho biết liệu có bất kỳ dữ liệu nào đã thay đổi trong quá trình truyền hay không. Bit có thể bị thay đổi bởi bức xạ điện từ, tốc độ truyền không khớp hoặc truyền dữ liệu khoảng cách xa. Sau khi UART nhận đọc khung dữ liệu, nó sẽ đếm số bit có giá trị là 1 và kiểm tra xem tổng số là số chẵn hay lẻ. Nếu bit chẵn lẻ là 0 (tính chẵn), thì tổng các bit 1 trong khung dữ liệu phải là một số chẵn. Nếu bit chẵn lẻ là 1 (tính lẻ), các bit 1 trong khung dữ liệu sẽ tổng thành một số lẻ. Khi bit chẵn lẻ khớp với dữ liệu, UART sẽ biết rằng quá trình truyền không có lỗi. Nhưng nếu bit chẵn lẻ là 0 và tổng là số lẻ; hoặc bit chẵn lẻ là 1 và tổng số là chẵn, UART sẽ biết rằng các bit trong khung dữ liệu đã thay đổi.

- Bit dừng: Để báo hiệu sự kết thúc của gói dữ liệu, UART gửi sẽ điều khiển đường truyền dữ liệu từ điện áp thấp đến điện áp cao trong ít nhất hai khoảng thời gian bit.

## Ưu và nhược điểm
Ưu điểm:  
- Chỉ sử dụng hai dây.
- Không cần tín hiệu clock.
- Có một bit chẵn lẻ để cho phép kiểm tra lỗi.
- Cấu trúc của gói dữ liệu có thể được thay đổi miễn là cả hai bên đều được thiết lập cho nó.
- Phương pháp có nhiều tài liệu và được sử dụng rộng rãi.

Nhược điểm:
- Kích thước của khung dữ liệu được giới hạn tối đa là 9 bit.
- Không hỗ trợ nhiều hệ thống slave hoặc nhiều hệ thống master.
- Tốc độ truyền của mỗi UART phải nằm trong khoảng 10% của nhau.

# Hướng dẫn sử dụng
Bước 1. Cắm Ports (COM & LPT) và ST-link vào máy tính hoặc sử dụng phần mềm mô phỏng [VSPE](https://eterlogic.com/Products.VSPE_Download.html).

Bước 2. Chọn Ports (COM & LPT), chọn baurate và nhấn nút Connect.

Bước 3. Nạp code vào STM32F205VCT hoặc sử dụng phần mềm Terminal hoặc phần mềm khác để mô phỏng.

Bước 4. Nhấn nút bật/tắt đèn LED trên màn hình để bật/tắt đèn LED trên mô-đun stm32.

Bước 5. Nhấn nút RUN hoặc STOP trên mô-đun stm32 để bật đèn LED RUN hoặc STOP trên màn hình.

# Thư viện cần có
System.IO.Ports 
## Thêm bằng Nuget
- Mở Project của bạn.
- Bấm chuột phải vào project trong Solution Explorer → chọn Manage NuGet Packages.
- Tìm kiếm System.IO.Ports.
- Cài đặt gói System.IO.Ports do Microsoft phát hành.
- Sau đó, bạn có thể import thư viện trong code:
```c
using System.IO.Ports;
```
## Thêm bằng .NET CLI
```c
dotnet add package System.IO.Ports
```
- Sau đó, bạn có thể import thư viện trong code:
```c
using System.IO.Ports;
```


## 📌📌Lưu ý: Chương trình mình viết có tên là BT_2_stm32f205 và BT_2_vs, các bạn có thể tham khảo chương trình thầy mình viết có tên là BT_2_vs_thay_sua 📌📌

# Tài liệu tham khảo
- [Datasheet UART](https://www.analog.com/en/resources/analog-dialogue/articles/uart-a-hardware-communication-protocol.html)
