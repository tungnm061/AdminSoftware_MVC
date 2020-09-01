﻿using System.ComponentModel;

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
}