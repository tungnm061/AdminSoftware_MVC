using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSoftware.Models
{
    public class DataExcel
    {
        public List<Dictionary<string, string>> Data { get; set; }
        public string SheetName { get; set; }
    }
}