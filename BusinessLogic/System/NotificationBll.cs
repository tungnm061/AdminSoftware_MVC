using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class NotificationBll
    {
        private readonly NotificationDal _notificationDal;

        public NotificationBll()
        {
            _notificationDal = SingletonIpl.GetInstance<NotificationDal>();
        }

        public List<Notification> GetNotifications(int userId)
        {
            return _notificationDal.GetNotifications(userId);
        }

        public bool Insert(Notification notification)
        {
            return _notificationDal.Insert(notification);
        }
    }
}