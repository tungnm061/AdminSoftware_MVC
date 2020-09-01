using System;
using System.Data;
using Core.Base;
using Core.Data;
using Dapper;

namespace DataAccess.System
{
    public class AutoNumberDal : BaseDal<ADOProvider>
    {
        public string GetAutoNumber(string prefix)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Prefix", prefix);
                param.Add("@GenerateCode", "", DbType.String, ParameterDirection.Output);
                var result = UnitOfWork.ProcedureExecute("[dbo].[Get_Generate_AutoNumber]", param);
                if (result)
                {
                    return param.Get<string>("@GenerateCode");
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}