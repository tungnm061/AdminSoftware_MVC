using System.ComponentModel;

namespace Core.Enum
{
    public enum CalTypeNum
    {
        [Description("Lần/Sản Phẩm")] Type1 = 1,
        [Description("Lần/Ngày")] Type2 = 2,
        [Description("Lần/Tuần")] Type3 = 3,
        [Description("Lần/Tháng")]Type4 = 4,//
        [Description("Lần/Công Việc")] Type5 = 5,
        [Description("Lần/Khách Hàng")] Type6 = 6,
        [Description("Lần/Hợp Đồng")] Type7 = 7,
        [Description("Lần")] Type8 = 8,
        [Description("Lần/Kế hoạch")]Type9= 9
        
    }
    public enum StatusWorkDetail
    {
        [Description("CV kế hoạch")]
        WorkPlanDetail = 1,
        [Description("CV nhóm")]
        WorkStreamDetail = 2,
        [Description("CV phát sinh")]
        SuggesWork = 3,
        [Description("CV được giao")]
        AssignWork = 4
    }
    public enum WorkPointType
    {
        [Description("A")]
        A,
        [Description("B")]
        B,
        [Description("C")]
        C,
        [Description("D")]
        D,
        [Description("E")]
        E
    }
    public enum StatusComplain
    {
        [Description("Chưa xác nhận")]
        NoApproved = 1,
        [Description("Xác nhận đúng")]
        TrueApproved = 2,
        [Description("Xác nhận sai")]
        FalseApproved = 3
    }
    public enum WorkDetailStatusEnum
    {
        [Description("Chưa thực hiện")]
        InPlan = 1,
        [Description("Đang thực hiện")]
        Working = 2,
        [Description("Đã hoàn thành")]
        Finish = 3,
        [Description("Đã xác nhận")]
        ApprovedFinish = 4,
        [Description("Chưa hoàn thành")]
        UnFinish = 5,
        [Description("Qúa hạn")]
        OutDate = 6
    }

    public enum DayNum
    {
        [Description("Thứ hai")]
        Monday = 1,
        [Description("Thứ ba")]
        Tuesday = 2,
        [Description("Thứ tư")]
        Wednesday = 3,
        [Description("Thứ năm")]
        Thursday = 4,
        [Description("Thứ sáu")]
        Friday = 5,
        [Description("Thứ bảy")]
        Saturday = 6,
        [Description("Chủ nhật")]
        Sunday = 0
    }
}