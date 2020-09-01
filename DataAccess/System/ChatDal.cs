using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.System;

namespace DataAccess.System
{
    public class ChatDal : BaseDal<ADOProvider>
    {
        public List<Chat> GetChats(int currentPage, int pageSize, int recordDisplay, ref long totalRecord, int? senderId,
            int? receiptId, long? chatGroupId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CurrentPage", currentPage);
                param.Add("@PageSize", pageSize);
                param.Add("@RecordDisplay", recordDisplay);
                param.Add("@TotalRecord", totalRecord, DbType.Int64, ParameterDirection.Output);
                param.Add("@SenderId", senderId);
                param.Add("@ReceiptId", receiptId);
                param.Add("@ChatGroupId", chatGroupId);
                var chats = UnitOfWork.Procedure<Chat>("[dbo].[Get_Chats]", param).ToList();
                if (chats.Any())
                    totalRecord = param.Get<long>("@TotalRecord");
                return chats;
            }
            catch (Exception ex)
            {
                Logging.PushString(ex.Message);
                return new List<Chat>();
            }
        }

        public bool Insert(Chat chat)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Insert_Chat]", new
                {
                    chat.ChatGroupId,
                    chat.ChatId,
                    chat.Message,
                    chat.ReceiptId,
                    chat.SendDate,
                    chat.SenderId
                });
            }
            catch (Exception ex)
            {
                Logging.PushString(ex.Message);
                return false;
            }
        }
    }
}