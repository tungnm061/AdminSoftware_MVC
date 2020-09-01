using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class CategoryKpiBll
    {
        private readonly CategoryKpiDal _categoryKpiDal;
        public CategoryKpiBll()
        {
            _categoryKpiDal = SingletonIpl.GetInstance<CategoryKpiDal>();
        }

        public List<CategoryKpi> GetCategoryKpis()
        {
            return _categoryKpiDal.GetCategoryKpis();
        }

        public CategoryKpi GetCategoryKpi(int kpiId)
        {
            return _categoryKpiDal.GetCategoryKpi(kpiId);
        }

        public bool Insert(CategoryKpi categoryKpi)
        {
            return _categoryKpiDal.Insert(categoryKpi);
        }
        public bool Update(CategoryKpi categoryKpi)
        {
            return _categoryKpiDal.Update(categoryKpi);
        }

        public bool Delete(int categoryKpiId)
        {
            return _categoryKpiDal.Delete(categoryKpiId);
        }
    }
}
