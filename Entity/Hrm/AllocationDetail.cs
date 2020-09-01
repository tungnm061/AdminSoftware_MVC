namespace Entity.Hrm
{
    public class AllocationDetail
    {
        public string AllocationDetailId { get; set; }
        public string AllocationId { get; set; }
        public long StationeryId { get; set; }
        public string StationeryName { get; set; }
        public int Quantity { get; set; }
    }
}