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
    public class PositionDal : BaseDal<ADOProvider>
    {
        public List<Position> GetPositions()
        {
            try
            {
                return UnitOfWork.Procedure<Position>("[hrm].[Get_Positions]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Position>();
            }
        }

        public Position GetPosition(int positionId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Position>("[hrm].[Get_Position]", new {PositionId = positionId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Position GetPosition(string positionCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Position>("[hrm].[Get_Position_ByCode]", new {PositionCode = positionCode})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(Position position)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PositionId", position.PositionId, DbType.Int32, ParameterDirection.Output);
                param.Add("@PositionName", position.PositionName);
                param.Add("@PositionCode", position.PositionCode);
                param.Add("@Description", position.Description);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Position]", param))
                    return param.Get<int>("@PositionId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Position position)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PositionId", position.PositionId);
                param.Add("@PositionName", position.PositionName);
                param.Add("@PositionCode", position.PositionCode);
                param.Add("@Description", position.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Position]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int positionId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Position]", new {PositionId = positionId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}