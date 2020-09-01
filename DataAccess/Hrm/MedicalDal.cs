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
    public class MedicalDal : BaseDal<ADOProvider>
    {
        public List<Medical> GetMedicals()
        {
            try
            {
                return UnitOfWork.Procedure<Medical>("[hrm].[Get_Medicals]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Medical>();
            }
        }

        public Medical GetMedical(int medicalId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Medical>("[hrm].[Get_Medical]", new {MedicalId = medicalId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(Medical medical)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Description", medical.Description);
                param.Add("@MedicalId", medical.MedicalId, DbType.Int32, ParameterDirection.Output);
                param.Add("@MedicalName", medical.MedicalName);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Medical]", param))
                    return param.Get<int>("@MedicalId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Medical medical)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Description", medical.Description);
                param.Add("@MedicalId", medical.MedicalId);
                param.Add("@MedicalName", medical.MedicalName);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Medical]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int medicalId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Medical]", new {MedicalId = medicalId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}