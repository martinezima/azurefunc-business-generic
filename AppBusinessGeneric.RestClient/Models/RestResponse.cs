using System.Net;

namespace AppBusinessGeneric.RestClient.Models;

public class RestResponse<T>
{
    public RestResponse(
        HttpStatusCode statusCode,
        T responseObj,
        ErrorMessages? errorMessages,
        string responseBody,
        string statusDescription)
    {
        StatusCode = statusCode;
        ResponseObject = responseObj;
        ErrorMessages = errorMessages;
        ResponseBody = responseBody;
        StatusDescription = statusDescription;
    }
    public HttpStatusCode StatusCode { get; private set; }
    public string StatusDescription { get; private set; }
    public T ResponseObject { get; private set; }
    public ErrorMessages? ErrorMessages { get; private set; }
    public string ResponseBody { get; private set; }
}