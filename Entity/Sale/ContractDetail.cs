namespace Entity.Sale
{
    public class ContractDetail
    {
        public string ContractDetailId { get; set; }
        public long ContractId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }

    }
}