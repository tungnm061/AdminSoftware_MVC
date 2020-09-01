using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Sale;

namespace DataAccess.Sale
{
    public class ProjectDal : BaseDal<ADOProvider>
    {
        public List<Project> GetProjects()
        {
            try
            {
                return UnitOfWork.Procedure<Project>("[sale].[Get_Projects]").ToList();

            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Project>();
            }
        }

        public Project GetProject(string id)
        {
            try
            {
                return UnitOfWork.Procedure<Project>("[sale].[Get_Project]", new { ProjectId = id }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new Project();
            }
        }

        public bool Insert(Project project)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ProjectId", project.ProjectId);
                param.Add("ProjectCode", project.ProjectCode);
                param.Add("FullName", project.FullName);
                param.Add("Status", project.Status);
                param.Add("InvestorId", project.InvestorId);
                param.Add("FromDate", project.FromDate);
                param.Add("CreateDate", project.CreateDate);
                param.Add("CreateBy", project.CreateBy);
                param.Add("ToDate", project.ToDate);
                param.Add("Description", project.Description);
                return UnitOfWork.ProcedureExecute("[sale].[Insert_Project]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
        public bool Update(Project project)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ProjectId", project.ProjectId);
                param.Add("ProjectCode", project.ProjectCode);
                param.Add("FullName", project.FullName);
                param.Add("Status", project.Status);
                param.Add("InvestorId", project.InvestorId);
                param.Add("FromDate", project.FromDate);
                param.Add("CreateDate", project.CreateDate);
                param.Add("CreateBy", project.CreateBy);
                param.Add("ToDate", project.ToDate);
                param.Add("Description", project.Description);
                return UnitOfWork.ProcedureExecute("[sale].[Update_Project]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Delete_Project]", new {ProjectId = id});
            }
               catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}
