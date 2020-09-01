using System;

namespace Entity.System
{
    public class Notification
    {
        public string NotificationId { get; set; }
        public string FromUser { get; set; }
        public int ToUserId { get; set; }
        public DateTime NotificationDate { get; set; }
        public string Message { get; set; }
        public string DisplayDate => NotificationDate.ToString("hh:mm dd/MM/yyyy");
    }
}