using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class PraiseDisciplineDal : BaseDal<ADOProvider>
    {
        public List<PraiseDiscipline> GetPraiseDisciplines(byte praiseDisciplineType)
        {
            try
            {
                return
                    UnitOfWork.Procedure<PraiseDiscipline>("[hrm].[Get_PraiseDisciplines]",
                        new {PraiseDisciplineType = praiseDisciplineType}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<PraiseDiscipline>();
            }
        }
        public List<PraiseDiscipline> GetPraiseDisciplinesByEmployeeId(byte praiseDisciplineType,long employeeId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<PraiseDiscipline>("[hrm].[Get_PraiseDisciplinesByEmployeeId]",
                        new
                        {
                            PraiseDisciplineType = praiseDisciplineType,
                            EmployeeId = employeeId
                        }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<PraiseDiscipline>();
            }
        }
        public PraiseDiscipline GetPraiseDiscipline(string praiseDisciplineId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PraiseDisciplineId", praiseDisciplineId);
                var multi = UnitOfWork.ProcedureQueryMulti("[hrm].[Get_PraiseDiscipline]", param);
                var praiseDiscipline = multi.Read<PraiseDiscipline>().FirstOrDefault();
                if (praiseDiscipline != null)
                {
                    praiseDiscipline.PraiseDisciplineDetails = multi.Read<PraiseDisciplineDetail>().ToList();
                }
                return praiseDiscipline;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(PraiseDiscipline praiseDiscipline, ref string praiseDisciplineCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PraiseDisciplineId", praiseDiscipline.PraiseDisciplineId);
                param.Add("@PraiseDisciplineType", praiseDiscipline.PraiseDisciplineType);
                param.Add("@Reason", praiseDiscipline.Reason);
                param.Add("@Title", praiseDiscipline.Title);
                param.Add("@CreateBy", praiseDiscipline.CreateBy);
                param.Add("@CreateDate", praiseDiscipline.CreateDate);
                param.Add("@DecisionNumber", praiseDiscipline.DecisionNumber);
                param.Add("@Description", praiseDiscipline.Description);
                param.Add("@Formality", praiseDiscipline.Formality);
                param.Add("@PraiseDisciplineCode", praiseDiscipline.PraiseDisciplineCode, DbType.String,
                    ParameterDirection.InputOutput);
                param.Add("@PraiseDisciplineDate", praiseDiscipline.PraiseDisciplineDate);
                var result = UnitOfWork.ProcedureExecute("[hrm].[Insert_PraiseDiscipline]", param);
                if (result)
                {
                    praiseDisciplineCode = param.Get<string>("@PraiseDisciplineCode");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(PraiseDiscipline praiseDiscipline)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PraiseDisciplineId", praiseDiscipline.PraiseDisciplineId);
                param.Add("@Reason", praiseDiscipline.Reason);
                param.Add("@Title", praiseDiscipline.Title);
                param.Add("@DecisionNumber", praiseDiscipline.DecisionNumber);
                param.Add("@Description", praiseDiscipline.Description);
                param.Add("@Formality", praiseDiscipline.Formality);
                param.Add("@PraiseDisciplineDate", praiseDiscipline.PraiseDisciplineDate);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_PraiseDiscipline]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string praiseDisciplineId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PraiseDisciplineId", praiseDisciplineId);
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_PraiseDiscipline]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool InsertDetail(string praiseDisciplineId, List<PraiseDisciplineDetail> praiseDisciplineDetails)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PraiseDisciplineId", praiseDisciplineId);
                param.Add("@XML", XmlHelper.SerializeXml<List<PraiseDisciplineDetail>>(praiseDisciplineDetails));
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_PraiseDisciplineDetail]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}