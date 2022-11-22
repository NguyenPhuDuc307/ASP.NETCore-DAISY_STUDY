using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaisyStudy.Models.VNPAY
{
    public class RequestPayment
    {
        public string? SoTien { get; set; }
        public string? NoiDungThanhToan { get; set; }
        public string? NgonNgu { get; set; }
        public string? NganHang { get; set; }
        public string? LoaiHangHoa { get; set; }
        public string? ThoiHanThanhToan { get; set; }
    }
}
