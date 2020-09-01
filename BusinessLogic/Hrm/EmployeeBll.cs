using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;
using Entity.System;

namespace BusinessLogic.Hrm
{
    public class EmployeeBll
    {
        private readonly EmployeeDal _employeeDal;

        public EmployeeBll()
        {
            _employeeDal = SingletonIpl.GetInstance<EmployeeDal>();
        }

        public List<Employee> GetEmployeesByPath(string path)
        {
            return _employeeDal.GetEmployeesByPath(path);
        }

        public List<Employee> GetEmployees(bool? isActive)
        {
            return _employeeDal.GetEmployees(isActive);
        }

        public List<Employee> GetEmployeesTimeSheet(bool? isActive)
        {
            return _employeeDal.GetEmployeesTimeSheet(isActive);
        }

        public List<Employee> GetEmployeesSalary(bool? isActive)
        {
            return _employeeDal.GetEmployeesSalary(isActive);
        }

        public List<Employee> GetEmployeesByKeyword(string keyword)
        {
            return _employeeDal.GetEmployeesByKeyword(keyword);
        }

        public List<Employee> GetEmployeesForInsurance(bool? join)
        {
            return _employeeDal.GetEmployeesForInsurance(join);
        }

        public List<User> GetEmployeesByDepartmentAndPosition(byte? position, long? departmentId)
        {
            return _employeeDal.GetEmployeesByDepartmentAndPosition(position, departmentId);
        }

        public Employee GetEmployee(long employeeId)
        {
            return _employeeDal.GetEmployee(employeeId);
        }

        public Employee GetEmployeeByTimeSheetCode(string timesheetCode)
        {
            return _employeeDal.GetEmployeeByTimeSheetCode(timesheetCode);
        }

        public Employee GetEmployee(string employeeCode)
        {
            return _employeeDal.GetEmployee(employeeCode);
        }

        public long Insert(Employee employee, ref string employeeCode)
        {
            return _employeeDal.Insert(employee, ref employeeCode);
        }

        public bool Update(Employee employee)
        {
            return _employeeDal.Update(employee);
        }

        public bool Delete(long employeeId)
        {
            return _employeeDal.Delete(employeeId);
        }
    }
}