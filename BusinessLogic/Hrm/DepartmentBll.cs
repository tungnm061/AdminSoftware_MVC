using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class DepartmentBll
    {
        private readonly DepartmentDal _departmentDal;

        public DepartmentBll()
        {
            _departmentDal = SingletonIpl.GetInstance<DepartmentDal>();
        }

        public List<Department> GetDepartments(bool? isActive)
        {
            return _departmentDal.GetDepartments(isActive);
        }

        public List<Department> GetDepartments(long parentId)
        {
            return _departmentDal.GetDepartments(parentId);
        }

        public Department GetDepartment(long departmentId)
        {
            return _departmentDal.GetDepartment(departmentId);
        }

        public Department GetDepartmentByCode(string departmentCode)
        {
            return _departmentDal.GetDepartment(departmentCode);
        }

        public long Insert(Department department)
        {
            return _departmentDal.Insert(department);
        }

        public bool Update(Department department)
        {
            return _departmentDal.Update(department);
        }

        public bool Delete(long departmentId)
        {
            return _departmentDal.Delete(departmentId);
        }
    }
}