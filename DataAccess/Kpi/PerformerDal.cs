using System;
using System.Collections.Generic;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class PerformerDal : BaseDal<ADOProvider>
    {
        public bool Inserts(string workStreamId, List<Performer> performers)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamId", workStreamId);
                param.Add("@XML", XmlHelper.SerializeXml<List<Performer>>(performers));
                return UnitOfWork.ProcedureExecute("[kpi].[Insert_Performers]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}