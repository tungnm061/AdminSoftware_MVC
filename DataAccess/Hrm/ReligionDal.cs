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
    public class ReligionDal : BaseDal<ADOProvider>
    {
        public List<Religion> GetReligions()
        {
            try
            {
                return UnitOfWork.Procedure<Religion>("[hrm].[Get_Religions]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Religion>();
            }
        }

        public Religion GetReligion(int religionId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Religion>("[hrm].[Get_Religion]", new {ReligionId = religionId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(Religion religion)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ReligionId", religion.ReligionId, DbType.Int32, ParameterDirection.Output);
                param.Add("@ReligionName", religion.ReligionName);
                param.Add("@Description", religion.Description);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Religion]", param))
                    return param.Get<int>("@ReligionId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Religion religion)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ReligionId", religion.ReligionId);
                param.Add("@ReligionName", religion.ReligionName);
                param.Add("@Description", religion.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Religion]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int religionId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Religion]", new {ReligionId = religionId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}