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
    public class EducationLevelDal : BaseDal<ADOProvider>
    {
        public List<EducationLevel> GetEducationLevels()
        {
            try
            {
                return UnitOfWork.Procedure<EducationLevel>("[hrm].[Get_EducationLevels]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<EducationLevel>();
            }
        }

        public EducationLevel GetEducationLevel(int trainingLevelId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<EducationLevel>("[hrm].[Get_EducationLevel]",
                        new {EducationLevelId = trainingLevelId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public EducationLevel GetEducationLevel(string trainingLevelCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<EducationLevel>("[hrm].[Get_EducationLevel_ByCode]",
                        new {EducationLevelCode = trainingLevelCode}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(EducationLevel trainingLevel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EducationLevelId", trainingLevel.EducationLevelId, DbType.Int32, ParameterDirection.Output);
                param.Add("@LevelName", trainingLevel.LevelName);
                param.Add("@LevelCode", trainingLevel.LevelCode);
                param.Add("@Description", trainingLevel.Description);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_EducationLevel]", param))
                    return param.Get<int>("@EducationLevelId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(EducationLevel trainingLevel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EducationLevelId", trainingLevel.EducationLevelId);
                param.Add("@LevelName", trainingLevel.LevelName);
                param.Add("@LevelCode", trainingLevel.LevelCode);
                param.Add("@Description", trainingLevel.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_EducationLevel]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int trainingLevelId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_EducationLevel]",
                    new {EducationLevelId = trainingLevelId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}