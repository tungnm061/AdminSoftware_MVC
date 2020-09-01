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
    public class DepartmentDal : BaseDal<ADOProvider>
    {
        public List<Department> GetDepartments(bool? isActive)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IsActive", isActive);
                return UnitOfWork.Procedure<Department>("[hrm].[Get_Departments]", param).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Department>();
            }
        }

        public List<Department> GetDepartments(long parentId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Department>("[hrm].[Get_Department_ByParentId]", new {ParentId = parentId})
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Department>();
            }
        }

        public Department GetDepartment(long departmentId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Department>("[hrm].[Get_Department]", new {DepartmentId = departmentId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Department GetDepartment(string departmentCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Department>("[hrm].[Get_Department_ByCode]",
                        new {DepartmentCode = departmentCode}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Department department)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@DepartmentId", department.DepartmentId, DbType.Int64, ParameterDirection.Output);
                param.Add("@DepartmentName", department.DepartmentName);
                param.Add("@DepartmentCode", department.DepartmentCode);
                param.Add("@Description", department.Description);
                param.Add("@IsActive", department.IsActive);
                param.Add("@ParentId", department.ParentId);
                param.Add("@Path", department.Path);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Department]", param))
                    return param.Get<long>("@DepartmentId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Department department)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@DepartmentId", department.DepartmentId);
                param.Add("@DepartmentName", department.DepartmentName);
                param.Add("@DepartmentCode", department.DepartmentCode);
                param.Add("@Description", department.Description);
                param.Add("@IsActive", department.IsActive);
                param.Add("@ParentId", department.ParentId);
                param.Add("@Path", department.Path);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Department]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long departmentId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Department]", new {DepartmentId = departmentId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}