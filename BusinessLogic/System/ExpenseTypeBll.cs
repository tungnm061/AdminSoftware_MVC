using DataAccess.System;
using System.Collections.Generic;
using Core.Singleton;
using Entity.System;

namespace BusinessLogic.System
{
    public class ExpenseTypeBll
    {
        private readonly ExpenseTypeDal _expenseTypeDal;

        public ExpenseTypeBll()
        {
            _expenseTypeDal = SingletonIpl.GetInstance<ExpenseTypeDal>();
        }

        public List<ExpenseType> GetExpenseTypes(bool? isActive)
        {
            return _expenseTypeDal.GetExpenseTypes(isActive);
        }

        public ExpenseType GetExpenseType(int id)
        {
            return _expenseTypeDal.GetExpenseType(id);
        }

        public int Insert(ExpenseType obj)
        {
            return _expenseTypeDal.Insert(obj);
        }

        public bool Update(ExpenseType obj)
        {
            return _expenseTypeDal.Update(obj);
        }

        public bool Delete(int id)
        {
            return _expenseTypeDal.Delete(id);
        }
    }
}
