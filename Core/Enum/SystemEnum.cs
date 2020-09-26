using System.ComponentModel;

namespace Core.Enum
{
    public enum Role
    {
        [Description("Quản trị")] Admin = 1,
        [Description("Người dùng")] User = 2
    }

    public enum ModuleGroup
    {
        [Description("Hệ thống")] System = 1,
        [Description("Hội đồng quản trị")] Bod = 2,
        [Description("Ban giám đốc")] Director = 3,
        [Description("Phòng nhân sự")] Hrm = 4,
        [Description("Phòng kinh doanh")] Sale = 5,
        [Description("Phòng đầu tư")] Investment = 6,
        [Description("Phòng kế toán")] Accounting = 7
    }

    public enum AutoNumberEnum
    {
        [Description("Nhân viên")] NV =1

    }

    public enum StatusOrderEnụm
    {
        //[Description("Has Issues")] HasIssues = 1,
        //[Description("On Hold")] OnHold = 2,
        //[Description("In Production")] InProduction = 3,
        //[Description("Fulfilled")] Fulfilled = 4
        [Description("Đang chờ")] HasIssues = 1,
        [Description("Đã hoàn thành")] OnHold = 2,
        [Description("Đã hủy")] InProduction = 3
    }
    public enum StatusPoEnụm
    {
        [Description("Chờ phê duyệt")] WaitApproval = 1,
        [Description("Bị trả lại")] Cancel = 2,
        [Description("Đã xác nhận")] Approval = 3
    }


    public enum KeySeachOrderEnụm
    {
        [Description("Ngày tạo")] CreateDate = 1,
        [Description("Ngày hoàn thành")] FinishDate = 2
    }

    public enum TypeMoneyEnum
    {
        [Description("USD")] Usd = 1,
        [Description("VND")] Vnd = 2
    }

    public enum StatusCompanyBankEnum
    {
        [Description("Chờ phê duyệt")] WaitApproval = 1,
        [Description("Bị trả lại")] Cancel = 2,
        [Description("Đã xác nhận")] Approval = 3
    }
}