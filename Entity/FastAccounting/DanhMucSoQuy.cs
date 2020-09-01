using System;
// ReSharper disable InconsistentNaming

namespace Entity.FastAccounting
{
    public class DanhMucSoQuy
    {
        public DateTime ngay_ct { get; set; }
        public DateTime ngay_lct { get; set; }
        public string so_ct { get; set; }
        public string ma_ct { get; set; }
        public string ma_ct_in { get; set; }
        public string dien_giai { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public string tk_du { get; set; }
        public string ghi_chu { get; set; }

      
        public decimal ps_no { get; set; }
        public decimal ps_co { get; set; }
        public decimal so_du { get; set; }
        public string ma_dvcs { get; set; }
        public string ten_tk { get; set; }
        public string so_ct_thu
        {
            get
            {
                if (ps_no > 0)
                {
                    return so_ct;
                }
                return "";
            }
        }
        public string so_ct_chi
        {
            get
            {
                if (ps_co > 0)
                {
                    return so_ct;
                }
                return "";
            }
        }
    }
}
