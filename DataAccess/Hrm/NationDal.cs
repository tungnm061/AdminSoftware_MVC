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
    public class NationDal : BaseDal<ADOProvider>
    {
        public List<Nation> GetNations()
        {
            try
            {
                return UnitOfWork.Procedure<Nation>("[hrm].[Get_Nations]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Nation>();
            }
        }

        public Nation GetNation(int nationId)
        {
            try
            {
                return UnitOfWork.Procedure<Nation>("[hrm].[Get_Nation]", new {NationId = nationId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(Nation nation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@NationId", nation.NationId, DbType.Int32, ParameterDirection.Output);
                param.Add("@NationName", nation.NationName);
                param.Add("@Description", nation.Description);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Nation]", param))
                    return param.Get<int>("@NationId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Nation nation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@NationId", nation.NationId);
                param.Add("@NationName", nation.NationName);
                param.Add("@Description", nation.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Nation]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int nationId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Nation]", new {NationId = nationId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}