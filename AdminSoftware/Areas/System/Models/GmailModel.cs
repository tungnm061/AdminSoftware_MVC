//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Entity.System;

//namespace DuyAmazone.Areas.Printify.Models
//{
//    public class GmailModel
//    {
//        public int Id { get; set; }

//        public string FullName { get; set; }

//        public int UserId { get; set; }

//        public DateTime CreateDate { get; set; }

//        public int CreateBy { get; set; }

//        public DateTime? UpdateDate { get; set; }

//        public int? UpdateBy { get; set; }

//        public bool IsActive { get; set; }

//        public string UserName { get; set; }

//        public string Description { get; set; }


//        public Gmail ToObject()
//        {
//            return new Gmail
//            {
//                Id = Id,
//                FullName = FullName,
//                UserId = UserId,
//                CreateDate = CreateDate,
//                CreateBy = CreateBy,
//                UpdateDate = UpdateDate,
//                UpdateBy = UpdateBy,
//                IsActive = IsActive,
//                Description = Description
//            };
//        }
//    }
//}