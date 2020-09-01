using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum CustomerStatusEnum
    {
        [Description("Cá Nhân")]
        Personal = 1,
        [Description("Công Ty")]
        Company = 2
    }
    public enum InvestorStatusEnum
    {
        [Description("Hoạt động")]
        IsActive = 1,
        [Description("Ngừng hoạt động")]
        Lock = 2
    }
    public enum ContractStatusEnum
    {
        [Description("Đã thanh toán")]
        Status1 = 1,
        [Description("Chưa thanh toán")]
        Status2 = 2,
        [Description("Thanh toán 1 nửa")]
        Status3 = 3
    }

    public enum AutoNumberSale
    {
        [Description("Khách hàng")] KH = 6,
        [Description("Sản phẩm")] SP = 8,
        [Description("Hợp đồng")] HD = 9
        

    }
}
