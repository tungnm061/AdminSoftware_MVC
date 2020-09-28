using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.System
{
    public class TextNote
    {
        public string TextNoteId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Text { get; set; }
    }
}
