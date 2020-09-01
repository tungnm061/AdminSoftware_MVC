using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class AddressBll
    {
        private readonly AddressDal _addressDal;

        public AddressBll()
        {
            _addressDal = SingletonIpl.GetInstance<AddressDal>();
        }

        public List<City> GetCities()
        {
            return _addressDal.GetCities();
        }

        public List<District> GetDistricts()
        {
            return _addressDal.GetDistricts();
        }

        public List<Ward> GetWards()
        {
            return _addressDal.GetWards();
        }

        public List<District> GetDistricts(int cityId)
        {
            return _addressDal.GetDistricts(cityId);
        }

        public List<Ward> GetWards(int districtId)
        {
            return _addressDal.GetWards(districtId);
        }
    }
}