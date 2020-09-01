namespace Entity.Kpi
{
    public class StatisticalFactorWork
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentId { get; set; }
        public decimal UsefulHoursTask { get; set; }
        public decimal UsefulHoursSuggesWork { get; set; }
        public decimal ToltalUsefulHourReal => UsefulHoursTask + UsefulHoursSuggesWork*2;
        public  decimal UsefullHourMin { get; set; }
        public string UsefullHourMinReal { get; set; }
        public int DepartmentCompany { get; set; }
        //public decimal SuggesPoint { get; set; }
        //public string SuggesPointA => (SuggesPoint >= 2) ? "x" : "";
        //public string SuggesPointB => (decimal.Parse("1.5") <= SuggesPoint && SuggesPoint < 2) ? "x" : "";
        //public string SuggesPointC => (1<=SuggesPoint && SuggesPoint < decimal.Parse("1.5")) ? "x" : "";
        //public string SuggesPointD => (decimal.Parse("0.5") <= SuggesPoint && SuggesPoint < 1) ? "x" : "";
        //public string SuggesPointE =>  SuggesPoint < decimal.Parse("0.5") ? "x" : "";
        //public decimal ApprovedPoint { get; set; }
        //public string ApprovedPointA => (ApprovedPoint >= 2) ? "x" : "";
        //public string ApprovedPointB => (1 <= ApprovedPoint && ApprovedPoint < 2) ? "x" : "";
        //public string ApprovedPointC => ApprovedPoint < 1 ? "x" : "";
        //public decimal ComplainPoint { get; set; }
        //public string ComplainPointA => (ComplainPoint >= 1) ? "x" : "";
        //public string ComplainPointB => (decimal.Parse("0.5") <= ComplainPoint && ComplainPoint < 1) ? "x" : "";
        //public string ComplainPointC => (decimal.Parse("0.5")*-1 <= ComplainPoint && ComplainPoint < decimal.Parse("0.5")) ? "x" : "";
        //public string ComplainPointD => (ComplainPoint < decimal.Parse("0.5")*-1) ? "x" : "";
        public string FullName { get; set; }
        public int NumberEmployee { get; set; }
        public decimal AvgPoint
        {
            get
            {
                if (DepartmentCompany == 3)
                {
                    return AvgPointTt;
                }
                if (UsefullHourMin > 0)
                {
                    return (UsefulHoursSuggesWork * 2 + UsefulHoursTask) / (UsefullHourMin);
                }
                return 0;
            }
        }
        public decimal AvgPointTt { get; set; }
        public string AvgPointString => AvgPoint.ToString("n2");
        public decimal FactorPoint { get; set; }
        public string FactorType { get; set; }

        public string DepartmentName { get; set; }
        public string RatingPoint { get; set; }
        public int TotalComplain { get; set; }
        public bool ViceDirectorManagement { get; set; }
        public string FactorTypeReal
        {
            get
            {
                if (TotalComplain >= 1)
                {
                    if (FactorType == "A+")
                        return "A";
                    if (FactorType == "A")
                        return "B";
                    if (FactorType == "B")
                        return "C";
                    if (FactorType == "C")
                        return "D";
                    if (FactorType == "D")
                        return "D";
                }
                return FactorType;
            }
        }
    }
}