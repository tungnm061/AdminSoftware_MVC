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
    public class SchoolDal : BaseDal<ADOProvider>
    {
        public List<School> GetSchools()
        {
            try
            {
                return UnitOfWork.Procedure<School>("[hrm].[Get_Schools]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<School>();
            }
        }

        public School GetSchool(int nationId)
        {
            try
            {
                return UnitOfWork.Procedure<School>("[hrm].[Get_School]", new {SchoolId = nationId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(School nation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SchoolId", nation.SchoolId, DbType.Int32, ParameterDirection.Output);
                param.Add("@SchoolName", nation.SchoolName);
                param.Add("@Description", nation.Description);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_School]", param))
                    return param.Get<int>("@SchoolId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(School nation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SchoolId", nation.SchoolId);
                param.Add("@SchoolName", nation.SchoolName);
                param.Add("@Description", nation.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_School]", param);
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
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_School]", new {SchoolId = nationId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}