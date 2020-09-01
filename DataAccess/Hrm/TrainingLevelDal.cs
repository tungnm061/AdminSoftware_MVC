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
    public class TrainingLevelDal : BaseDal<ADOProvider>
    {
        public List<TrainingLevel> GetTrainingLevels()
        {
            try
            {
                return UnitOfWork.Procedure<TrainingLevel>("[hrm].[Get_TrainingLevels]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TrainingLevel>();
            }
        }

        public TrainingLevel GetTrainingLevel(int trainingLevelId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<TrainingLevel>("[hrm].[Get_TrainingLevel]",
                        new {TrainingLevelId = trainingLevelId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public TrainingLevel GetTrainingLevel(string trainingLevelCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<TrainingLevel>("[hrm].[Get_TrainingLevel_ByCode]",
                        new {TrainingLevelCode = trainingLevelCode}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(TrainingLevel trainingLevel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingLevelId", trainingLevel.TrainingLevelId, DbType.Int32, ParameterDirection.Output);
                param.Add("@LevelName", trainingLevel.LevelName);
                param.Add("@LevelCode", trainingLevel.LevelCode);
                param.Add("@Description", trainingLevel.Description);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_TrainingLevel]", param))
                    return param.Get<int>("@TrainingLevelId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(TrainingLevel trainingLevel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingLevelId", trainingLevel.TrainingLevelId);
                param.Add("@LevelName", trainingLevel.LevelName);
                param.Add("@LevelCode", trainingLevel.LevelCode);
                param.Add("@Description", trainingLevel.Description);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_TrainingLevel]", param);
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
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_TrainingLevel]",
                    new {TrainingLevelId = trainingLevelId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}