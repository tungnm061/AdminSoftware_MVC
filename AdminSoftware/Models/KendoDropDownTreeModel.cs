using System.Collections.Generic;
// ReSharper disable InconsistentNaming
namespace AdminSoftware.Models
{
    public class KendoDropDownTreeModel
    {
        public string value { get; set; }
        public string text { get; set; }
        public bool expanded { get; set; }
        public List<KendoDropDownTreeModel> ChildModels { get; set; }
    }
}