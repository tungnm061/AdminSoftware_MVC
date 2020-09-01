using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class SalaryBll
    {
        private readonly SalaryDal _salaryDal;

        public SalaryBll()
        {
            _salaryDal = SingletonIpl.GetInstance<SalaryDal>();
        }

        public Salary GetSalaryByEmployeeId(long employeeId)
        {
            return _salaryDal.GetSalaryByEmployeeId(employeeId);
        }
        public List<Salary> GetSalaries()
        {
            return _salaryDal.GetSalaries();
        }

        public List<Salary> GetSalaries(long employeeId)
        {
            return _salaryDal.GetSalaries(employeeId);
        }

        public Salary GetSalary(string salaryId)
        {
            return _salaryDal.GetSalary(salaryId);
        }

        public bool Insert(Salary salary)
        {
            return _salaryDal.Insert(salary);
        }

        public bool Update(Salary salary)
        {
            return _salaryDal.Update(salary);
        }

        public bool Delete(string salaryId)
        {
            return _salaryDal.Delete(salaryId);
        }
    }
}