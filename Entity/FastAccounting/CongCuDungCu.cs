// ReSharper disable InconsistentNaming
namespace Entity.FastAccounting
{
    public class CongCuDungCu
    {
        public string ten_nv { get; set; }
        public string ma_nv { get; set; }
        public decimal nguyen_gia { get; set; }
        public decimal gt_da_kh { get; set; }
        public decimal gt_cl { get; set; }
    }

    public class FirstResult
    {
        public decimal nguyen_gia { get; set; }
        public decimal gt_da_kh { get; set; }
        public decimal gt_cl { get; set; }
    }
}