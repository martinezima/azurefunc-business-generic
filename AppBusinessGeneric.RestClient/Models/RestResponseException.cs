using System.Net;
namespace AppBusinessGeneric.RestClient.Models;

public class RestResponseException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }
    public string StatusDescription { get; private set; }
    public ErrorMessages? ErrorMessages { get; private set; }
    public string ResponseBody { get; private set; }
    public RestResponseException(
        HttpStatusCode statusCode,
        string statusDescription,
        string responseBody)
    {
        StatusCode = statusCode;
        StatusDescription = statusDescription;
        ResponseBody = responseBody;
    }
    public RestResponseException(
        HttpStatusCode statusCode,
        string statusDescription,
        ErrorMessages errorMessages,
        string responseBody)
    {
        StatusCode = statusCode;
        StatusDescription = statusDescription;
        ErrorMessages = errorMessages;
        ResponseBody = responseBody;
    }
}
