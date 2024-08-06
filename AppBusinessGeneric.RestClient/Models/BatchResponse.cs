namespace AppBusinessGeneric.RestClient.Models;
    public partial class BatchResponse
    {
        public Responses? Responses { get; set; }

        public bool BatchFailed { get; set; }
    }