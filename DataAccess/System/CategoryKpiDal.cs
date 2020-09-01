using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.System;

namespace DataAccess.System
{
    public class CategoryKpiDal : BaseDal<ADOProvider>
    {
        public List<CategoryKpi> GetCategoryKpis()
        {
            try
            {
                return UnitOfWork.Procedure<CategoryKpi>("[dbo].[Get_CategoryKpis]").ToList();
            }
            catch (Exception ex)
            {
                
                Logging.PutError(ex.Message, ex);
                return new List<CategoryKpi>();
            }
        }
        public CategoryKpi GetCategoryKpi(int kpiId)
        {
            try
            {
                return UnitOfWork.Procedure<CategoryKpi>("[dbo].[Get_CategoryKpi]",new
                {
                    CategoryKpiId = kpiId
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                Logging.PutError(ex.Message, ex);
                return new CategoryKpi();
            }
        }
        public bool Insert(CategoryKpi categoryKpi)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CategoryKpiId", categoryKpi.CategoryKpiId);
                param.Add("@KpiCode", categoryKpi.KpiCode);
                param.Add("@KpiName", categoryKpi.KpiName);
                param.Add("@Description", categoryKpi.Description);
                param.Add("@CreateDate", categoryKpi.CreateDate);
                param.Add("@CreateBy", categoryKpi.CreateBy);
                return UnitOfWork.ProcedureExecute("[dbo].[Insert_CategoryKpi]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
        public bool Update(CategoryKpi categoryKpi)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CategoryKpiId", categoryKpi.CategoryKpiId);
                param.Add("@KpiCode", categoryKpi.KpiCode);
                param.Add("@KpiName", categoryKpi.KpiName);
                param.Add("@Description", categoryKpi.Description);
                param.Add("@CreateDate", categoryKpi.CreateDate);
                param.Add("@CreateBy", categoryKpi.CreateBy);
                return UnitOfWork.ProcedureExecute("[dbo].[Update_CategoryKpi]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
        public bool Delete(int categoryKpiId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Delete_CategoryKpi]", new { CategoryKpiId = categoryKpiId });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}
