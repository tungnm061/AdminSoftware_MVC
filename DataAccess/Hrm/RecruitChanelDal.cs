using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class RecruitChanelDal : BaseDal<ADOProvider>
    {
        public List<RecruitChanel> GetRecruitChanels(bool? isActive)
        {
            try
            {
                return
                    UnitOfWork.Procedure<RecruitChanel>("[hrm].[Get_RecruitChanels]", new {IsActive = isActive})
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<RecruitChanel>();
            }
        }

        public RecruitChanel GetRecruitChanel(int recruitChanelId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<RecruitChanel>("[hrm].[Get_RecruitChanel]",
                        new {RecruitChanelId = recruitChanelId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(RecruitChanel recruitChanel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ChanelName", recruitChanel.ChanelName);
                param.Add("@Description", recruitChanel.Description);
                param.Add("@IsActive", recruitChanel.IsActive);
                param.Add("@CreateBy", recruitChanel.CreateBy);
                param.Add("@CreateDate", recruitChanel.CreateDate);
                param.Add("@RecruitChanelId", recruitChanel.RecruitChanelId, DbType.Int32, ParameterDirection.Output);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_RecruitChanel]", param))
                    return param.Get<int>("@RecruitChanelId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(RecruitChanel recruitChanel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ChanelName", recruitChanel.ChanelName);
                param.Add("@Description", recruitChanel.Description);
                param.Add("@IsActive", recruitChanel.IsActive);
                param.Add("@RecruitChanelId", recruitChanel.RecruitChanelId);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_RecruitChanel]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int recruitChanelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RecruitChanelId", recruitChanelId);
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_RecruitChanel]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}