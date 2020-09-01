using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class ApplicantDal : BaseDal<ADOProvider>
    {
        public List<Applicant> GetApplicants(long recruitPlanId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Applicant>("[hrm].[Get_Applicants]", new {RecruitPlanId = recruitPlanId})
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Applicant>();
            }
        }

        public Applicant GetApplicant(string applicantId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Applicant>("[hrm].[Get_Applicant]", new {ApplicantId = applicantId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(Applicant applicant)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_Applicant]", new
                {
                    applicant.ApplicantId,
                    applicant.ChanelId,
                    applicant.CityBirthPlace,
                    applicant.CountryId,
                    applicant.CreateBy,
                    applicant.CreateDate,
                    applicant.CvDate,
                    applicant.DateOfBirth,
                    applicant.Description,
                    applicant.Email,
                    applicant.FullName,
                    applicant.IdentityCardNumber,
                    applicant.NationId,
                    applicant.PermanentAddress,
                    applicant.PhoneNumber,
                    applicant.RecruitPlanId,
                    applicant.ReligionId,
                    applicant.Sex,
                    applicant.TrainingLevelId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Applicant applicant)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Applicant]", new
                {
                    applicant.ApplicantId,
                    applicant.ChanelId,
                    applicant.CityBirthPlace,
                    applicant.CountryId,
                    applicant.CreateBy,
                    applicant.CreateDate,
                    applicant.CvDate,
                    applicant.DateOfBirth,
                    applicant.Description,
                    applicant.Email,
                    applicant.FullName,
                    applicant.IdentityCardNumber,
                    applicant.NationId,
                    applicant.PermanentAddress,
                    applicant.PhoneNumber,
                    applicant.RecruitPlanId,
                    applicant.ReligionId,
                    applicant.Sex,
                    applicant.TrainingLevelId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string applicantId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Applicant]", new
                {
                    ApplicantId = applicantId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}