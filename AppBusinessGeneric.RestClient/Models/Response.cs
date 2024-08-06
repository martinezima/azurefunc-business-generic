using System.Net;

namespace AppBusinessGeneric.RestClient.Models;
public partial class Response
{
    public int RequestId { get; set; }

    public object? ResponseObject { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public ErrorMessages? ErrorMessages { get; set; }
}