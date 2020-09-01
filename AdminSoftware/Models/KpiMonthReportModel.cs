namespace AdminSoftware.Models
{
    public class KpiMonthReportModel
    {
        public string Title { get; set; }
        public string FullName { get; set; }
        public string TaskCode { get; set; }

        public string TaskName
        {
            get
            {
                if (string.IsNullOrEmpty(Description) || Description == "")
                {
                    return TaskNameX;
                }
                return Description;
            }

        }

        public string Description { get; set; }
        public string TaskNameX { get; set; }
        public string StartTaskWeek1 { get; set; }
        public string EndTaskWeek1 { get; set; }
        public decimal TimeTaskWeek1 { get; set; }
        public decimal PointTaskWeek1 { get; set; }
        public string StartTaskWeek2 { get; set; }
        public string EndTaskWeek2 { get; set; }
        public decimal TimeTaskWeek2 { get; set; }
        public decimal PointTaskWeek2 { get; set; }
        public string StartTaskWeek3 { get; set; }
        public string EndTaskWeek3 { get; set; }
        public decimal TimeTaskWeek3 { get; set; }
        public decimal PointTaskWeek3 { get; set; }
        public string StartTaskWeek4 { get; set; }
        public string EndTaskWeek4 { get; set; }
        public decimal TimeTaskWeek4 { get; set; }
        public decimal PointTaskWeek4 { get; set; }
        public string Total { get; set; }
        public string Tag { get; set; }
    }
}