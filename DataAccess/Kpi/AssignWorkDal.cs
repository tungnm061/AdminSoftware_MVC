using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class AssignWorkDal : BaseDal<ADOProvider>
    {
        public List<AssignWork> GetAssignWorks(int? status, long? createBy, long? assignBy, DateTime? fromDate,
            DateTime? toDate)
        {
            try
            {
                return UnitOfWork.Procedure<AssignWork>("[kpi].[Get_AssignWorks]", new
                {
                    Status = status,
                    CreateBy = createBy,
                    AssignBy = assignBy,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<AssignWork>();
            }
        }
        public List<AssignWork> GetAssignWorkByFollows(int employeeId)
        {
            try
            {
                return UnitOfWork.Procedure<AssignWork>("[kpi].[Get_AssignWorkByFollows]", new
                {
                    EmployeeId = employeeId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<AssignWork>();
            }
        }
        public List<AssignWork> GetAssignWorkByUserIds( long? createBy, long? assignBy)
        {
            try
            {
                return UnitOfWork.Procedure<AssignWork>("[kpi].[Get_AssignWorks_ByUserId]", new
                {
                    CreateBy = createBy,
                    AssignBy = assignBy
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<AssignWork>();
            }
        }
        public AssignWork GetAssignWork(string assignWorkId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<AssignWork>("[kpi].[Get_AssignWork]", new {AssignWorkId = assignWorkId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(AssignWork assignWork)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@AssignWorkId", assignWork.AssignWorkId);
                param.Add("@TaskId", assignWork.TaskId);
                param.Add("@CreateBy", assignWork.CreateBy);
                param.Add("@AssignBy", assignWork.AssignBy);
                param.Add("@CreateDate", assignWork.CreateDate);
                param.Add("@FromDate", assignWork.FromDate);
                param.Add("@Status", assignWork.Status);
                param.Add("@ToDate", assignWork.ToDate);
                param.Add("@Description", assignWork.Description);
                param.Add("@UsefulHours", assignWork.UsefulHours);
                param.Add("@Explanation", assignWork.Explanation);
                param.Add("@WorkingNote", assignWork.WorkingNote);
                param.Add("@ApprovedFisnishDate", assignWork.ApprovedFisnishDate);
                param.Add("@ApprovedFisnishBy", assignWork.ApprovedFisnishBy);
                param.Add("@FisnishDate", assignWork.FisnishDate);
                param.Add("@WorkPointType", assignWork.WorkPointType);
                param.Add("@DepartmentFisnishBy", assignWork.DepartmentFisnishBy);
                param.Add("@DepartmentFisnishDate", assignWork.DepartmentFisnishDate);
                param.Add("@Quantity", assignWork.Quantity);
                param.Add("@DepartmentFollowBy", assignWork.DepartmentFollowBy);
                param.Add("@DirectorFollowBy", assignWork.DirectorFollowBy);
                param.Add("@TypeAssignWork", assignWork.TypeAssignWork);
                return (UnitOfWork.ProcedureExecute("[kpi].[Insert_AssignWork]", param));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(AssignWork assignWork)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@AssignWorkId", assignWork.AssignWorkId);
                param.Add("@TaskId", assignWork.TaskId);
                param.Add("@CreateBy", assignWork.CreateBy);
                param.Add("@AssignBy", assignWork.AssignBy);
                param.Add("@CreateDate", assignWork.CreateDate);
                param.Add("@FromDate", assignWork.FromDate);
                param.Add("@Status", assignWork.Status);
                param.Add("@ToDate", assignWork.ToDate);
                param.Add("@Description", assignWork.Description);
                param.Add("@UsefulHours", assignWork.UsefulHours);
                param.Add("@Explanation", assignWork.Explanation);
                param.Add("@WorkingNote", assignWork.WorkingNote);
                param.Add("@ApprovedFisnishDate", assignWork.ApprovedFisnishDate);
                param.Add("@ApprovedFisnishBy", assignWork.ApprovedFisnishBy);
                param.Add("@FisnishDate", assignWork.FisnishDate);
                param.Add("@WorkPointType", assignWork.WorkPointType);
                param.Add("@WorkPoint", assignWork.WorkPoint);
                param.Add("@DepartmentFisnishBy", assignWork.DepartmentFisnishBy);
                param.Add("@DepartmentFisnishDate", assignWork.DepartmentFisnishDate);
                param.Add("@Quantity", assignWork.Quantity);
                param.Add("@FileConfirm", assignWork.FileConfirm);
                param.Add("@DepartmentFollowBy", assignWork.DepartmentFollowBy);
                param.Add("@DirectorFollowBy", assignWork.DirectorFollowBy);
                param.Add("@TypeAssignWork", assignWork.TypeAssignWork);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_AssignWork]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string assignWorkId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_AssignWork]", new
                {
                    AssignWorkId = assignWorkId
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