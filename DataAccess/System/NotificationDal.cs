using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.System;

namespace DataAccess.System
{
    public class NotificationDal : BaseDal<ADOProvider>
    {
        public List<Notification> GetNotifications(int userId)
        {
            try
            {
                return UnitOfWork.Procedure<Notification>("[dbo].[Get_Notifications]", new {UserId = userId}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Notification>();
            }
        }

        public bool Insert(Notification notification)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Insert_Notification]", new
                {
                    notification.FromUser,
                    notification.Message,
                    notification.NotificationDate,
                    notification.NotificationId,
                    notification.ToUserId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}