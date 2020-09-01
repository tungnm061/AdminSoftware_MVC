using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
    public class Investor
    {
        public string InvestorId { get; set; }

        public string InvestorCode { get; set; }

        public string FullName { get; set; }

        public string Company { get; set; }

        public string CompanyAddress { get; set; }

        public string Address { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public string Position { get; set; }

        public string MsEnterprise { get; set; }

        public int FoundedYear { get; set; }

        public decimal? CharterCapital { get; set; }

        public int Status { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string Description { get; set; }
    }
}
