using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class ChatBll
    {
        private readonly ChatDal _chatDal;

        public ChatBll()
        {
            _chatDal = SingletonIpl.GetInstance<ChatDal>();
        }

        public List<Chat> GetChats(int currentPage, int pageSize, int recordDisplay, ref long totalRecord,
            int? senderId = null,
            int? receiptId = null, long? chatGroupId = null)
        {
            return _chatDal.GetChats(currentPage, pageSize, recordDisplay, ref totalRecord, senderId, receiptId,
                chatGroupId);
        }

        public bool Insert(Chat chat)
        {
            return _chatDal.Insert(chat);
        }
    }
}