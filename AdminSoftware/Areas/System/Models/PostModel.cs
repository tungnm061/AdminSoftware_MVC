using System;
using System.ComponentModel.DataAnnotations;
using Entity.System;

namespace AdminSoftware.Areas.System.Models
{
    public class PostModel
    {
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        [Required]
        public string PostContent { get; set; }

        [Required]
        public bool IsFeature { get; set; }

        public Post ToObject()
        {
            return new Post
            {
                PostId = PostId,
                Title = Title,
                PublishDate = PublishDate,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                PostContent = PostContent,
                IsFeature = IsFeature
            };
        }
    }
}