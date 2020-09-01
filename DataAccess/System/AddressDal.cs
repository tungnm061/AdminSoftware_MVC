using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.System;

namespace DataAccess.System
{
    public class AddressDal : BaseDal<ADOProvider>
    {
        public List<City> GetCities()
        {
            try
            {
                return UnitOfWork.Procedure<City>("[dbo].[Get_Cities]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<City>();
            }
        }

        public List<District> GetDistricts()
        {
            try
            {
                return UnitOfWork.Procedure<District>("[dbo].[Get_Districts]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<District>();
            }
        }

        public List<Ward> GetWards()
        {
            try
            {
                return UnitOfWork.Procedure<Ward>("[dbo].[Get_Wards]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Ward>();
            }
        }

        public List<District> GetDistricts(int cityId)
        {
            try
            {
                return UnitOfWork.Procedure<District>("[dbo].[Get_District_ByCityId]", new {CityId = cityId}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<District>();
            }
        }

        public List<Ward> GetWards(int districtId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Ward>("[dbo].[Get_Ward_ByDistrictId]", new {DistrictId = districtId}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Ward>();
            }
        }
    }
}