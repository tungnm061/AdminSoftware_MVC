using System;

namespace Entity.Kpi
{
    public class AssignWork
    {
        public string AssignWorkId { get; set; }
        public string TaskId { get; set; }
        public int CreateBy { get; set; }
        public int AssignBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FromDate { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public byte Status { get; set; }
        public DateTime ToDate { get; set; }
        public string Explanation { get; set; }
        public string WorkingNote { get; set; }
        public decimal? UsefulHours { get; set; }
        public string Description { get; set; }
        public string TaskName { get; set; }
        public string TaskCode { get; set; }
        public string AssignName { get; set; }
        public string CreateByName { get; set; }
        public string AssignFullName { get; set; }
        public string AssignCode { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public string WorkPointType { get; set; }
        public int WorkPointConfigId { get; set; }
        public decimal? WorkPoint { get; set; }
        public int Quantity { get; set; }
        public int? DepartmentFollowBy { get; set; }
        public int? DirectorFollowBy { get; set; }
        public string FileConfirm { get; set; }
        public int ActionCompany { get; set; }
        public int? TypeAssignWork { get; set; }
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
                if (DateTime.Parse(ToDate.ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    return "background-color: #f8d7da";
                }
                return "background-color: #d4edda";
            }
        }
    }
}