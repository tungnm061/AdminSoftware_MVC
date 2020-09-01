using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class WorkPointConfigDal : BaseDal<ADOProvider>
    {
        public List<WorkPointConfig> GetWorkPointConfigs()
        {
            try
            {
                return UnitOfWork.Procedure<WorkPointConfig>("[kpi].[Get_WorkPointConfigs]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<WorkPointConfig>();
            }
        }

        public WorkPointConfig GetWorkPointConfig(int workPointConfigId)
        {
            try
            {
                return UnitOfWork.Procedure<WorkPointConfig>("[kpi].[Get_WorkPointConfig]", new
                {
                    WorkPointConfigId = workPointConfigId
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new WorkPointConfig();
            }
        }
    }
}