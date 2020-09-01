using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class RecruitResultDal : BaseDal<ADOProvider>
    {
        public List<RecruitResult> GetRecruitResults(long recruitPlanId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<RecruitResult>("[hrm].[Get_RecruitResults]",
                        new {RecruitPlanId = recruitPlanId}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<RecruitResult>();
            }
        }

        public RecruitResult GetRecruitResult(string recruitResultId)
        {
            try
            {
                var multi = UnitOfWork.ProcedureQueryMulti("[hrm].[Get_RecruitResult]",
                    new {RecruitResultId = recruitResultId});
                var recruitResult = multi.Read<RecruitResult>().FirstOrDefault();
                if (recruitResult != null)
                    recruitResult.RecruitResultDetails = multi.Read<RecruitResultDetail>().ToList();
                return recruitResult;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(RecruitResult recruitResult)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RecruitResultId", recruitResult.RecruitResultId);
                param.Add("@Result", recruitResult.Result);
                param.Add("@ApplicantId", recruitResult.ApplicantId);
                param.Add("@CreateBy", recruitResult.CreateBy);
                param.Add("@CreateDate", recruitResult.CreateDate);
                param.Add("@Description", recruitResult.Description);
                param.Add("@EmployeeId", recruitResult.EmployeeId);
                param.Add("@RecruitPlanId", recruitResult.RecruitPlanId);
                param.Add("@XML", XmlHelper.SerializeXml<List<RecruitResultDetail>>(recruitResult.RecruitResultDetails));
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_RecruitResult]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(RecruitResult recruitResult)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RecruitResultId", recruitResult.RecruitResultId);
                param.Add("@Result", recruitResult.Result);
                param.Add("@ApplicantId", recruitResult.ApplicantId);
                param.Add("@CreateBy", recruitResult.CreateBy);
                param.Add("@CreateDate", recruitResult.CreateDate);
                param.Add("@Description", recruitResult.Description);
                param.Add("@EmployeeId", recruitResult.EmployeeId);
                param.Add("@RecruitPlanId", recruitResult.RecruitPlanId);
                param.Add("@XML", XmlHelper.SerializeXml<List<RecruitResultDetail>>(recruitResult.RecruitResultDetails));
                return UnitOfWork.ProcedureExecute("[hrm].[Update_RecruitResult]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string recruitResultId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RecruitResultId", recruitResultId);
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_RecruitResult]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}