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
   public class InvestorDal : BaseDal<ADOProvider>
   {
       public List<Investor> GetInvestors()
       {
           try
           {
               return UnitOfWork.Procedure<Investor>("[sale].[Get_Investors]", new {}).ToList();
           }
           catch (Exception ex)
           {
               Logging.PutError(ex.Message,ex);
                return new List<Investor>();
           }
       }
        public Investor GetInvestor(string investorId)
        {
            try
            {
                return UnitOfWork.Procedure<Investor>("[sale].[Get_Investor]", new { InvestorId = investorId }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new Investor();
            }
        }

       public bool Insert(Investor investor)
       {
           try
           {
               var param = new DynamicParameters();
                param.Add("@InvestorId",investor.InvestorId);
                param.Add("@InvestorCode", investor.InvestorCode);
                param.Add("@FullName", investor.FullName);
                param.Add("@Company", investor.Company);
                param.Add("@CompanyAddress", investor.CompanyAddress);
                param.Add("@Address", investor.Address);
                param.Add("@CityId", investor.CityId);
                param.Add("@DistrictId", investor.DistrictId);
                param.Add("@Position", investor.Position);
                param.Add("@MsEnterprise", investor.MsEnterprise);
                param.Add("@FoundedYear", investor.FoundedYear);
                param.Add("@CharterCapital", investor.CharterCapital);
                param.Add("@Status", investor.Status);
                param.Add("@CreateBy", investor.CreateBy);
                param.Add("@CreateDate", investor.CreateDate);
                param.Add("@Description", investor.Description);
               return UnitOfWork.ProcedureExecute("[sale].[Insert_Investor]", param);

           }
            catch (Exception ex)
           {
                Logging.PutError(ex.Message, ex);
                return false;
            }
       }

        public bool Update(Investor investor)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@InvestorId", investor.InvestorId);
                param.Add("@InvestorCode", investor.InvestorCode);
                param.Add("@FullName", investor.FullName);
                param.Add("@Company", investor.Company);
                param.Add("@CompanyAddress", investor.CompanyAddress);
                param.Add("@Address", investor.Address);
                param.Add("@CityId", investor.CityId);
                param.Add("@DistrictId", investor.DistrictId);
                param.Add("@Position", investor.Position);
                param.Add("@MsEnterprise", investor.MsEnterprise);
                param.Add("@FoundedYear", investor.FoundedYear);
                param.Add("@CharterCapital", investor.CharterCapital);
                param.Add("@Status", investor.Status);
                param.Add("@CreateBy", investor.CreateBy);
                param.Add("@CreateDate", investor.CreateDate);
                param.Add("@Description", investor.Description);
                return UnitOfWork.ProcedureExecute("[sale].[Update_Investor]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

       public bool Delete(string investorId)
       {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Delete_Investor]", new
                {
                    InvestorId = investorId
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
