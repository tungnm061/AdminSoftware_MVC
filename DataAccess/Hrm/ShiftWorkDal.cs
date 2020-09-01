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
    public class ShiftWorkDal : BaseDal<ADOProvider>
    {
        public List<ShiftWork> GetShiftWorks()
        {
            try
            {
                return UnitOfWork.Procedure<ShiftWork>("[hrm].[Get_ShiftWorks]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<ShiftWork>();
            }
        }

        public ShiftWork GetShiftWork(int shiftWorkId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<ShiftWork>("[hrm].[Get_ShiftWork]", new {ShiftWorkId = shiftWorkId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public ShiftWork GetShiftWork(string shiftWorkCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<ShiftWork>("[hrm].[Get_ShiftWork_ByCode]", new {ShiftWorkCode = shiftWorkCode})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(ShiftWork shiftWork)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Description", shiftWork.Description);
                param.Add("@EndTime", shiftWork.EndTime);
                param.Add("@RelaxEndTime", shiftWork.RelaxEndTime);
                param.Add("@RelaxStartTime", shiftWork.RelaxStartTime);
                param.Add("@ShiftWorkCode", shiftWork.ShiftWorkCode);
                param.Add("@ShiftWorkId", shiftWork.ShiftWorkId, DbType.Int32, ParameterDirection.Output);
                param.Add("@StartTime", shiftWork.StartTime);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_ShiftWork]", param))
                    return param.Get<int>("@ShiftWorkId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(ShiftWork shiftWork)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Description", shiftWork.Description);
                param.Add("@EndTime", shiftWork.EndTime);
                param.Add("@RelaxEndTime", shiftWork.RelaxEndTime);
                param.Add("@RelaxStartTime", shiftWork.RelaxStartTime);
                param.Add("@ShiftWorkCode", shiftWork.ShiftWorkCode);
                param.Add("@ShiftWorkId", shiftWork.ShiftWorkId);
                param.Add("@StartTime", shiftWork.StartTime);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_ShiftWork]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}