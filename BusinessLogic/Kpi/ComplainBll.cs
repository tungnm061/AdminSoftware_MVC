using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class ComplainBll
    {
        private readonly ComplainDal _complainDal;

        public ComplainBll()
        {
            _complainDal = SingletonIpl.GetInstance<ComplainDal>();
        }

        public List<Complain> GetComplains(DateTime? fromDate, DateTime? toDate, int? createBy, int action)
        {
            return _complainDal.GetComplains(fromDate, toDate, createBy, action);
        }

        public List<Complain> GetComplains_AccusedBy(DateTime? fromDate, DateTime? toDate, int? accusedBy,int action)
        {
            return _complainDal.GetComplains_AccusedBy(fromDate, toDate, accusedBy, action);
        }

        public Complain GetComplain(string complainId)
        {
            return _complainDal.GetComplain(complainId);
        }

        public bool Update(Complain complain)
        {
            return _complainDal.Update(complain);
        }

        public bool Insert(Complain complain)
        {
            return _complainDal.Insert(complain);
        }

        public bool Delete(string id)
        {
            return _complainDal.Delete(id);
        }
    }
}