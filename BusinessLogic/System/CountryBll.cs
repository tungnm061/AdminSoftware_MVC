using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class CountryBll
    {
        private readonly CountryDal _countryDal;

        public CountryBll()
        {
            _countryDal = SingletonIpl.GetInstance<CountryDal>();
        }

        public List<Country> GetCountries()
        {
            return _countryDal.GetCountries();
        }

        public Country GetCountry(int countryId)
        {
            return _countryDal.GetCountry(countryId);
        }

        public Country GetCountry(string countryCode)
        {
            return _countryDal.GetCountry(countryCode);
        }

        public int Insert(Country country)
        {
            return _countryDal.Insert(country);
        }

        public bool Update(Country country)
        {
            return _countryDal.Update(country);
        }

        public bool Delete(int countryId)
        {
            return _countryDal.Delete(countryId);
        }
    }
}