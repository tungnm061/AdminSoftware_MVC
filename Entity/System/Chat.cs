using System;

namespace Entity.System
{
    public class Chat
    {
        public string ChatId { get; set; }
        public int SenderId { get; set; }
        public int? ReceiptId { get; set; }
        public long? ChatGroupId { get; set; }
        public DateTime SendDate { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string TimeDisplay => TimeDisplayHelper.TimeDisplay(SendDate,DateTime.Now);
    }
}