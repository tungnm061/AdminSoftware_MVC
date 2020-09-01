using System;
using System.Collections.Generic;
using System.Transactions;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class JobChangeBll
    {
        private readonly EmployeeDal _employeeDal;
        private readonly JobChangeDal _jobChangeDal;

        public JobChangeBll()
        {
            _jobChangeDal = SingletonIpl.GetInstance<JobChangeDal>();
            _employeeDal = SingletonIpl.GetInstance<EmployeeDal>();
        }

        public List<JobChange> GetJobChanges()
        {
            return _jobChangeDal.GetJobChanges();
        }

        public List<JobChange> GetJobChangesByEmployeeId(long employeeId)
        {
            return _jobChangeDal.GetJobChangesByEmployeeId(employeeId);
        }

        public JobChange GetJobChange(string jobChangeId)
        {
            return _jobChangeDal.GetJobChange(jobChangeId);
        }

        public bool Insert(JobChange jobChange, ref string jobChangeCode)
        {
            try
            {
                if (jobChange == null)
                    return false;
                using (var scope = new TransactionScope())
                {
                    if (_jobChangeDal.Insert(jobChange, ref jobChangeCode))
                    {
                        var employee = _employeeDal.GetEmployee(jobChange.EmployeeId);
                        employee.DepartmentId = jobChange.ToDepartmentId;
                        employee.PositionId = jobChange.ToPositionId;
                        if (_employeeDal.Update(employee))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(JobChange jobChange)
        {
            try
            {
                if (jobChange == null)
                    return false;
                using (var scope = new TransactionScope())
                {
                    if (_jobChangeDal.Update(jobChange))
                    {
                        var jobChangeInMemory = _jobChangeDal.GetJobChange(jobChange.JobChangeId);
                        var employeeInMemory = _employeeDal.GetEmployee(jobChangeInMemory.EmployeeId);
                        employeeInMemory.DepartmentId = jobChange.FromDepartmentId;
                        employeeInMemory.PositionId = jobChange.FromPositionId;
                        if (!_employeeDal.Update(employeeInMemory))
                        {
                            scope.Dispose();
                            return false;
                        }
                        var employee = _employeeDal.GetEmployee(jobChange.EmployeeId);
                        employee.DepartmentId = jobChange.ToDepartmentId;
                        employee.PositionId = jobChange.ToPositionId;
                        if (!_employeeDal.Update(employee))
                        {
                            scope.Dispose();
                            return false;
                        }
                        scope.Complete();
                        return true;
                    }
                    scope.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(JobChange jobChange)
        {
            try
            {
                if (jobChange == null)
                    return false;
                using (var scope = new TransactionScope())
                {
                    var employeeInMemory = _employeeDal.GetEmployee(jobChange.EmployeeId);
                    employeeInMemory.DepartmentId = jobChange.FromDepartmentId;
                    employeeInMemory.PositionId = jobChange.FromPositionId;
                    if (!_employeeDal.Update(employeeInMemory))
                    {
                        scope.Dispose();
                        return false;
                    }
                    if (_jobChangeDal.Delete(jobChange.JobChangeId))
                    {
                        scope.Complete();
                        return true;
                    }
                    scope.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}