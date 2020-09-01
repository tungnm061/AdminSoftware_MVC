using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class ContractTypeBll
    {
        private readonly ContractTypeDal _contractTypeDal;

        public ContractTypeBll()
        {
            _contractTypeDal = SingletonIpl.GetInstance<ContractTypeDal>();
        }

        public List<ContractType> GetContractTypes(bool? isActive)
        {
            return _contractTypeDal.GetContractTypes(isActive);
        }

        public ContractType GetContractType(int contractTypeId)
        {
            return _contractTypeDal.GetContractType(contractTypeId);
        }

        public int Insert(ContractType contractType)
        {
            return _contractTypeDal.Insert(contractType);
        }

        public bool Update(ContractType contractType)
        {
            return _contractTypeDal.Update(contractType);
        }

        public bool Delete(int contractTypeId)
        {
            return _contractTypeDal.Delete(contractTypeId);
        }
    }
}