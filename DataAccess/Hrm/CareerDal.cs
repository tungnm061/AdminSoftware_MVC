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
    public class CareerDal : BaseDal<ADOProvider>
    {
        public List<Career> GetCareers()
        {
            try
            {
                return UnitOfWork.Procedure<Career>("[hrm].[Get_Careers]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Career>();
            }
        }

        public Career GetCareer(int nationId)
        {
            try
            {
                return UnitOfWork.Procedure<Career>("[hrm].[Get_Career]", new {CareerId = nationId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(Career nation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CareerId", nation.CareerId, DbType.Int32, ParameterDirection.Output);
                param.Add("@CareerName", nation.CareerName);
                param.Add("@Description", nation.Description);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Career]", param))
                    return param.Get<int>("@CareerId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Career nation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CareerId", nation.CareerId);
                param.Add("@CareerName", nation.CareerName);
                param.Add("@Description", nation.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Career]", param);
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
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Career]", new {CareerId = nationId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}