using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class IncurredSalaryBll
    {
        private readonly IncurredSalaryDal _incurredSalaryDal;

        public IncurredSalaryBll()
        {
            _incurredSalaryDal = SingletonIpl.GetInstance<IncurredSalaryDal>();
        }

        public List<IncurredSalary> GetIncurredSalaries(long? employeeId, DateTime fromDate, DateTime toDate,
            bool calcSalary)
        {
            return _incurredSalaryDal.GetIncurredSalaries(employeeId, fromDate, toDate, calcSalary);
        }

        public IncurredSalary GetIncurredSalary(string incurredSalaryId)
        {
            return _incurredSalaryDal.GetIncurredSalary(incurredSalaryId);
        }

        public bool Insert(IncurredSalary incurredSalary)
        {
            return _incurredSalaryDal.Insert(incurredSalary);
        }

        public bool Update(IncurredSalary incurredSalary)
        {
            return _incurredSalaryDal.Update(incurredSalary);
        }

        public bool Delete(string incurredSalaryId)
        {
            return _incurredSalaryDal.Delete(incurredSalaryId);
        }
    }
}