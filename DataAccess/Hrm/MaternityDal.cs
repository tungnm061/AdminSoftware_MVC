using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class MaternityDal : BaseDal<ADOProvider>
    {
        public List<Maternity> GetMaternitys()
        {
            try
            {
                return UnitOfWork.Procedure<Maternity>("[hrm].[Get_Maternitys]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Maternity>();
            }
        }

        public Maternity GetMaternity(string maternityId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Maternity>("[hrm].[Get_Maternity]", new {MaternityId = maternityId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new Maternity();
            }
        }

        public bool Insert(Maternity maternity)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@MaternityId", maternity.MaternityId);
                param.Add("@EmployeeId", maternity.EmployeeId);
                param.Add("@FromDate", maternity.FromDate);
                param.Add("@ToDate", maternity.ToDate);
                param.Add("@StartTime", maternity.StartTime);
                param.Add("@EndTime", maternity.EndTime);
                param.Add("@RelaxStartTime", maternity.RelaxStartTime);
                param.Add("@RelaxEndTime", maternity.RelaxEndTime);
                param.Add("@CreateDate", maternity.CreateDate);
                param.Add("@CreateBy", maternity.CreateBy);
                param.Add("@Description", maternity.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_Maternity]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Maternity maternity)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@MaternityId", maternity.MaternityId);
                param.Add("@EmployeeId", maternity.EmployeeId);
                param.Add("@FromDate", maternity.FromDate);
                param.Add("@ToDate", maternity.ToDate);
                param.Add("@StartTime", maternity.StartTime);
                param.Add("@EndTime", maternity.EndTime);
                param.Add("@RelaxStartTime", maternity.RelaxStartTime);
                param.Add("@RelaxEndTime", maternity.RelaxEndTime);
                param.Add("@CreateDate", maternity.CreateDate);
                param.Add("@CreateBy", maternity.CreateBy);
                param.Add("@Description", maternity.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Maternity]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string maternityId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Maternity]", new {MaternityId = maternityId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}