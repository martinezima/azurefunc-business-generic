namespace AppBusinessGeneric.RestClient.Models;
public partial class BatchErrorMessage 
{
    public int RequestId {get; set;}
    public int StatusCode {get; set;}
    public string? StatusDescription {get; set;}
    public ErrorMessages? ErrorMessages { get; set; }        
}