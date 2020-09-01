using System;

namespace Entity.Kpi
{
    public class WorkDetail
    {
        public string WorkDetailId { get; set; }
        public string TaskId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ToDateScheduler => DateTime.Parse(ToDate.ToShortDateString() + " " + "12:00:00 PM");
        public int Status { get; set; }
        public decimal? UsefulHours { get; set; }
        public byte WorkType { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public string WorkingNote { get; set; }
        public string Explanation { get; set; }
        public string TaskName { get; set; }
        public string TaskCode { get; set; }
        public string FileConfirm { get; set; }
        public string CreateByName { get; set; }
        public long DepartmentId { get; set; }
        public DateTime? FisnishDate { get; set; }        
        public int NumberWeek
        {
            get
            {
                if (1 <= FromDate.Day && FromDate.Day <= 7)
                {
                    return 1;
                }
                if (8 <= FromDate.Day && FromDate.Day <= 14)
                {
                    return 2;
                }
                if (15 <= FromDate.Day && FromDate.Day <= 21)
                {
                    return 3;
                }
                return 4;
            }
        }
        public string DescriptionTask { get; set; }
        public string EmployeeCode { get; set; }
        public byte CalcType { get; set; }
        public string FisnishDateForMat
        {
            get
            {
                if (FisnishDate != null)
                {
                    return FisnishDate.Value.ToShortTimeString() + "-" + FisnishDate.Value.DayOfWeek + "-" + FisnishDate.Value.ToShortDateString();
                }
                return "";
            }
        }
        public DateTime? ApprovedFisnishDate { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public int? VerifiedBy { get; set; }
        public decimal? UsefulHoursTask { get; set; }
        public string WorkPointType { get; set; }
        public int WorkPointConfigId { get; set; }
        public decimal? WorkPoint { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string ReportTag { get; set; }
        public string StatusStr
        {
            get
            {
                if (Status == 4)
                {
                    return "Đã xác nhận";
                }
                return "";
            }
        }
        public int DepartmentCompany { get; set; }
        // tuan1
        public string FromDate1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    return FromDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string Status1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    if (Status == 4)
                    {
                        return "Đã xác nhận";
                    }
                    return "";
                }
                return "";
            }
        }
        public string ToDate1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    return ToDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string ToDateReal1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    if (FisnishDate != null) return FisnishDate.Value.ToString("dd/MM/yyyy");

                }
                return "";
            }
        }
        public decimal? UsefulHour1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    return UsefulHours;
                }
                return null;
            }
        }
        public decimal? UsefulHourTask1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    return UsefulHoursTask;
                }
                return null;
            }
        }

        public decimal? WorkPoint1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    return WorkPoint;
                }
                return 0;
            }
        }
        public string CancelFinish1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    if (Status != 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
            }
        public string Finish1
        {
            get
            {
                if (NumberWeek == 1)
                {
                    if (Status == 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
        }
        // tuan2
        public string Status2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    if (Status == 4)
                    {
                        return "Đã xác nhận";
                    }
                    return "";
                }
                return "";
            }
        }
        public string FromDate2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    return FromDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public decimal? WorkPoint2
        {
            get
            {
                if (NumberWeek ==2)
                {
                    return WorkPoint;
                }
                return 0;
            }
        }
        public string ToDate2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    return ToDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string ToDateReal2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    if (FisnishDate != null) return FisnishDate.Value.ToString("dd/MM/yyyy");

                }
                return "";
            }
        }
        public decimal? UsefulHour2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    return UsefulHours;
                }
                return null;
            }
        }
        public decimal? UsefulHourTask2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    return UsefulHoursTask;
                }
                return null;
            }
        }

        public string CancelFinish2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    if (Status != 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
        }
        public string Finish2
        {
            get
            {
                if (NumberWeek == 2)
                {
                    if (Status == 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
        }
        //tuan3
        public string FromDate3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    return FromDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string Status3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    if (Status == 4)
                    {
                        return "Đã xác nhận";
                    }
                    return "";
                }
                return "";
            }
        }
        public decimal? WorkPoint3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    return WorkPoint;
                }
                return 0;
            }
        }
        public string ToDate3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    return ToDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string ToDateReal3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    if (FisnishDate != null) return FisnishDate.Value.ToString("dd/MM/yyyy");

                }
                return "";
            }
        }
        public decimal? UsefulHour3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    return UsefulHours;
                }
                return null;
            }
        }
        public decimal? UsefulHourTask3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    return UsefulHoursTask;
                }
                return null;
            }
        }

        public string CancelFinish3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    if (Status != 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
        }
        public string Finish3
        {
            get
            {
                if (NumberWeek == 3)
                {
                    if (Status == 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
        }
        //tuan4
        public string Status4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    if (Status == 4)
                    {
                        return "Đã xác nhận";
                    }
                    return "";
                }
                return "";
            }
        }
        public string FromDate4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    return FromDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public decimal? WorkPoint4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    return WorkPoint;
                }
                return 0;
            }
        }
        public string ToDate4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    return ToDate.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string ToDateReal4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    if (FisnishDate != null) return FisnishDate.Value.ToString("dd/MM/yyyy");

                }
                return "";
            }
        }
        public decimal? UsefulHour4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    return UsefulHours;
                }
                return null;
            }
        }
        public decimal? UsefulHourTask4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    return UsefulHoursTask;
                }
                return null;
            }
        }

        public string CancelFinish4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    if (Status != 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
        }
        public string Finish4
        {
            get
            {
                if (NumberWeek == 4)
                {
                    if (Status == 4)
                    {
                        return "V";
                    }
                    return "";
                }
                return "";
            }
        }

        public int StatusDisplay
        {
            get
            {
                if (Status != 5 && Status != 4 && Status != 3 &&
                    DateTime.Parse(ToDate.ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    return 6;
                }
                return Status;
            }
            
        }
         public string Color
        {
            get
            {
                if (Status == 3)
                {
                    return "background-color: #cce5ff";
                }
                if (Status == 4)
                {
                    return "background-color: #e2e3e5";
                }
                if (Status == 5)
                {
                    return "background-color: #fff3cd";
                }
                if (StatusDisplay==6)
                {
                    return "background-color: #f8d7da";
                }
                return "background-color: #d4edda";
            }
        }
    }

    public class HomeDashBoard
    {
        public string DateId { get; set; }
        public DateTime DateDay { get; set; }
        public DateTime EndDate => DateDay;
        public string Title { get; set; }
        public string CssClass { get; set; }
        public string Check { get; set; }
    }
}