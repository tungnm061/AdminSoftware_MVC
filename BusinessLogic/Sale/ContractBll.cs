using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
    public class ContractBll
    {
        private readonly ContractDal _contractDal;
        private readonly ContractDetailDal _contractDetailDal;
        private readonly ProductDal _productDal;

        public ContractBll()
        {
            _contractDal = SingletonIpl.GetInstance<ContractDal>();
            _contractDetailDal = SingletonIpl.GetInstance<ContractDetailDal>();
            _productDal = SingletonIpl.GetInstance<ProductDal>();
        }

        public Contract GetContract(long contractId)
        {
            return _contractDal.GetContract(contractId);
        }

        public List<Contract> GetContracts(DateTime fromDate, DateTime toDate, int? status)
        {
            return _contractDal.GetContracts(fromDate, toDate, status);
        }

        public bool Insert(Contract contract)
        {
            try
            {
                if (contract == null || !contract.ContractDetails.Any())
                    return false;
                using (var scope = new TransactionScope())
                {
                    foreach (var item in contract.ContractDetails)
                    {
                        var product = _productDal.GetProduct(item.ProductId);
                        if (product == null)
                        {
                            scope.Dispose();
                            return false;
                        }

                        var quantityNew = product.Quantity - item.Quantity;
                        product.Quantity = quantityNew;

                        if (!_productDal.Update(product))
                        {
                            scope.Dispose();
                            return false;
                        }
                    }
                    var contractId = _contractDal.Insert(contract);
                    if (contractId > 0)
                    {
                        if (_contractDetailDal.Insert(contractId,
                            contract.ContractDetails))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Contract contract)
        {
            try
            {
                if (contract == null || !contract.ContractDetails.Any())
                    return false;

                using (var scope = new TransactionScope())
                {
                    var contractDetailsInMemory = _contractDal.GetContract(contract.ContractId).ContractDetails;
                    var contractDetailsDeletion =
                        contractDetailsInMemory.Where(
                            x => !contract.ContractDetails.Select(y => y.ProductId).Contains(x.ProductId)).ToList();
                    if (_contractDal.Update(contract))
                    {
                        foreach (var contractDetail in contractDetailsDeletion)
                        {
                            var product = _productDal.GetProduct(contractDetail.ProductId);
                            if (product == null)
                            {
                                scope.Dispose();
                                return false;
                            }
                            var quantityNew = product.Quantity +
                                              contractDetail.Quantity;
                            product.Quantity = quantityNew;
                            //xóa contract detail và cộng ngược số lượng trong kho
                            if (!_contractDetailDal.Delete(contractDetail.ContractDetailId))
                            {
                                scope.Dispose();
                                return false;
                            }
                            if (!_productDal.Update(product))
                            {
                                scope.Dispose();
                                return false;
                            }
                        }
                        foreach (var contractDetail in contract.ContractDetails)
                        {
                            // cập nhật số lượng trong kho
                            var product = _productDal.GetProduct(contractDetail.ProductId);
                            if (product == null)
                            {
                                scope.Dispose();
                                return false;
                            }
                            var contactDetailInMemory =
                                contractDetailsInMemory.Find(x => x.ProductId == contractDetail.ProductId);
                            var quantity = product.Quantity +
                                           (contactDetailInMemory == null ? 0 : contactDetailInMemory.Quantity) -
                                           contractDetail.Quantity;
                            if (quantity < 0)
                            {
                                //vượt quá số lượng tồn rollback lại
                                scope.Dispose();
                                return false;
                            }
                            //update số lượng tồn
                            product.Quantity = quantity;
                            if (!_productDal.Update(product))
                            {
                                scope.Dispose();
                                return false;
                            }
                        }
                        if (_contractDetailDal.Insert(contract.ContractId,
                            contract.ContractDetails))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long contractId)
        {
            return _contractDal.Delete(contractId);
        }
    }
}