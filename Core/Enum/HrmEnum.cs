using System.ComponentModel;

namespace Core.Enum
{
    public enum EmployeeStatusEnum
    {
        [Description("Đang làm việc")] Working = 1,
        [Description("Đã nghỉ việc")] LeaveOff = 2
    }

    public enum MaritalStatus
    {
        [Description("Không xác định")] Unknown = 0,
        [Description("Độc thân")] Single = 1,
        [Description("Góa")] Widowed = 2,
        [Description("Ly thân")] Separated = 3,
        [Description("Đã lập gia đình")] Married = 4,
        [Description("Ly dị")] Divorced = 5
    }

    public enum SexEnum
    {
        [Description("Nam")] Male = 1,
        [Description("Nữ")] Female = 2, //
        [Description("Khác")] Other = 3
    }

    public enum PraiseDisciplineType
    {
        [Description("Khen thưởng")] Praise = 1,
        [Description("Kỷ luật")] Discipline = 2
    }

    public enum DepartmentConpanyEnum
    {
        [Description("Chủ tịch HDQT")] Bod = 1,
        [Description("Tổng giám đốc")] Director = 2,
        [Description("Phó tổng giám đốc")] ViceDirector = 3,
        [Description("Trưởng phòng")] Department = 4,
        [Description("Nhân viên")] Employee = 5
    }

    public enum RecruitResultEnum
    {
        [Description("Chờ")] Waiting = 1,
        [Description("Loại bỏ")] Reject = 2,
        [Description("Không tuyển")] UnPassed = 3,
        [Description("Tuyển dụng")] Passed = 4
    }
}