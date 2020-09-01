using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.System;

namespace DataAccess.System
{
    public class CountryDal : BaseDal<ADOProvider>
    {
        public List<Country> GetCountries()
        {
            try
            {
                return UnitOfWork.Procedure<Country>("[dbo].[Get_Countries]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Country>();
            }
        }

        public Country GetCountry(int countryId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Country>("[dbo].[Get_Country]", new {CountryId = countryId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Country GetCountry(string countryCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Country>("[dbo].[Get_Country_ByCode]", new {CountryCode = countryCode})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(Country country)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CountryId", country.CountryId, DbType.Int32, ParameterDirection.Output);
                param.Add("@CountryName", country.CountryName);
                param.Add("@CountryCode", country.CountryCode);
                param.Add("@Description", country.Description);
                if (UnitOfWork.ProcedureExecute("[dbo].[Insert_Country]", param))
                    return param.Get<int>("@CountryId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Country country)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CountryId", country.CountryId);
                param.Add("@CountryName", country.CountryName);
                param.Add("@CountryCode", country.CountryCode);
                param.Add("@Description", country.Description);
                return UnitOfWork.ProcedureExecute("[dbo].[Update_Country]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int countryId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Delete_Country]", new {CountryId = countryId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}