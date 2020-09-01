using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class ContractBll
    {
        private readonly ContractDal _contractDal;

        public ContractBll()
        {
            _contractDal = SingletonIpl.GetInstance<ContractDal>();
        }

        public List<Contract> GetContracts()
        {
            return _contractDal.GetContracts();
        }

        public List<Contract> GetContractsByEmployeeId(long employeeId)
        {
            return _contractDal.GetContractsByEmployeeId(employeeId);
        }

        public Contract GetContract(string contractId)
        {
            return _contractDal.GetContract(contractId);
        }

        public Contract GetContractByContractCode(string contractCode)
        {
            return _contractDal.GetContractByContractCode(contractCode);
        }

        public bool Insert(Contract contract)
        {
            return _contractDal.Insert(contract);
        }

        public bool Update(Contract contract)
        {
            return _contractDal.Update(contract);
        }

        public bool Delete(string contractId)
        {
            return _contractDal.Delete(contractId);
        }
    }
}