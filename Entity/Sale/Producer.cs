using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Helper.ExtendedAttributes;

namespace Entity.Sale
{
    public class Producer
    {
    public int ProducerId { get; set; }
    [LocalizeRequired]
    [DisplayName("Mã nhà sản xuất")]
    public string ProducerCode { get; set; }

    [LocalizeRequired]
    [LocalizeStringLength(250)]
    [DisplayName("Tên nhà sản xuất")]
    public string ProducerName { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public string Description { get; set; }
    }

}
