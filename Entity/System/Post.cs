using System;
using System.Collections;

namespace Entity.System
{
    public class Post 
    {
        public int PostId { get; set; }

        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public string PostContent { get; set; }

        public bool IsFeature { get; set; }
  
    }
}