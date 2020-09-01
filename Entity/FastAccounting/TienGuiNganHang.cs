using System;

// ReSharper disable InconsistentNaming

namespace Entity.FastAccounting
{
    public class TienGuiNganHang
    {
        public DateTime ngay_ct { get; set; }
        public DateTime ngay_lct { get; set; }
        public string so_ct { get; set; }
        public string ma_ct { get; set; }
        public string ma_ct_in { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public string dien_giai { get; set; }
        public decimal ps_no { get; set; }
        public decimal ps_co { get; set; }
        public string tk_du { get; set; }
        public string ten_tk_du { get; set; }
        public decimal con_lai => ps_co - ps_no;
        public string ghi_chu { get; set; }
    }
}
