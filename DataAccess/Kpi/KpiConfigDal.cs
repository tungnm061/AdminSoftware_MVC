using System;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class KpiConfigDal : BaseDal<ADOProvider>
    {
        public KpiConfig GetKpiConfig()
        {
            try
            {
                return UnitOfWork.Procedure<KpiConfig>("[kpi].[Get_KpiConfig]").FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Update(KpiConfig kpiConfig)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@KpiConfigId", kpiConfig.KpiConfigId);
                param.Add("@MinHours", kpiConfig.MinHours);
                param.Add("@MaxHours", kpiConfig.MaxHours);
                param.Add("@PlanningDay", kpiConfig.PlanningDay);
                param.Add("@PlanningHourMin", kpiConfig.PlanningHourMin);
                param.Add("@PlanningHourMax", kpiConfig.PlanningHourMax);
                param.Add("@HourConfirmMin", kpiConfig.HourConfirmMin);
                param.Add("@HourConfirmMax", kpiConfig.HourConfirmMax);
                param.Add("@Notification", kpiConfig.Notification);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_KpiConfig]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}