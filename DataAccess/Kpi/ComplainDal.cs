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
    public class ComplainDal : BaseDal<ADOProvider>
    {
        public List<Complain> GetComplains(DateTime? fromDate, DateTime? toDate, int? createBy, int action)
        {
            return UnitOfWork.Procedure<Complain>("[kpi].[Get_Complains]", new
            {
                FromDate = fromDate,
                ToDate = toDate,
                CreateBy = createBy,
                Action = action
            }).ToList();
        }

        public List<Complain> GetComplains_AccusedBy(DateTime? fromDate, DateTime? toDate, int? accusedBy,int action)
        {
            return UnitOfWork.Procedure<Complain>("[kpi].[Get_Complains_AccusedBy]", new
            {
                FromDate = fromDate,
                ToDate = toDate,
                AccusedBy = accusedBy,
                Action = action
            }).ToList();
        }

        public Complain GetComplain(string complainId)
        {
            return
                UnitOfWork.Procedure<Complain>("[kpi].[Get_Complain]", new {ComplainId = complainId}).FirstOrDefault();
        }

        public bool Insert(Complain complain)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ComplainId", complain.ComplainId);
                param.Add("@AccusedBy", complain.AccusedBy);
                param.Add("@CreateBy", complain.CreateBy);
                param.Add("@ConfirmedBy", complain.ConfirmedBy);
                param.Add("@ConfirmedDate", complain.ConfirmedDate);
                param.Add("@CreateDate", complain.CreateDate);
                param.Add("@Description", complain.Description);
                param.Add("@Status", complain.Status);
                return UnitOfWork.ProcedureExecute("[kpi].[Insert_Complain]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Complain complain)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ComplainId", complain.ComplainId);
                param.Add("@AccusedBy", complain.AccusedBy);
                param.Add("@CreateBy", complain.CreateBy);
                param.Add("@ConfirmedBy", complain.ConfirmedBy);
                param.Add("@ConfirmedDate", complain.ConfirmedDate);
                param.Add("@CreateDate", complain.CreateDate);
                param.Add("@Description", complain.Description);
                param.Add("@Status", complain.Status);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_Complain]", param);
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
                var param = new DynamicParameters();
                param.Add("@ComplainId", id);
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_Complain]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}