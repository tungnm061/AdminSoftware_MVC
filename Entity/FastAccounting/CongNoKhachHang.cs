using System;

// ReSharper disable InconsistentNaming

namespace Entity.FastAccounting
{
    public class CongNoKhachHang
    {
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public decimal dk { get; set; }
        public string du_dau { get; set; }
        public decimal ck { get; set; }
        public string du_cuoi { get; set; }
        public decimal ps_no { get; set; }
        public decimal ps_co { get; set; }
        public string dien_giai { get; set; }
        public DateTime ngay_ct { get; set; }
        public DateTime ngay_lct { get; set; }
        public string so_ct { get; set; }
        public string ma_ct0 { get; set; }
        public string ma_ct { get; set; }
        public string tk_du { get; set; }
    }
}
