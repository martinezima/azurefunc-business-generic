namespace AppBusinessGeneric.Application.Models;

    public class SupplierConfigurationImportBatches
    {
        public string? ErpSourceId { get; set; }
        public string? Company { get; set; }
        public string? Responsible { get; set; }
        public string? ResponsibleRole { get; set; }
        public string? Supplier { get; set; }
        public List<ContactPerson>? ContactPersons { get; set; }
        public int ExpectedDeliveryTimeInDays { get; set; }
        public bool SendPurchaseOrderAsIntegrationMessage { get; set; }
        public bool SendPurchaseOrderAsPdf { get; set; }
        public string? RegisterGoodsReceiptType { get; set; }
    }