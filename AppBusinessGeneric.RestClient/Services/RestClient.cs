using System.Net;
using System.Net.Http.Headers;
using System.Text;
using AppBusinessGeneric.RestClient.Helpers;
using  AppBusinessGeneric.RestClient.Interfaces;
using AppBusinessGeneric.RestClient.Models;

namespace AppBusinessGeneric.RestClient.Services;

public class RestClient: IRestClient
{
    private string? uri;
    private string? baseUri;
    private int networkTimeout;
    private bool validateResponse;
    private readonly Dictionary<string, string> headers = new();
    private readonly DataContractHelper dataContractHelper = new();
    private HttpClient httpClient = new();

    public string ProcessHeaderUser { get; set; } = string.Empty;

    public RestClient()
    {
    }

    public RestClient(string baseUri, int timeout, bool shouldValidateResponse)
    {
        SettingUpRestClient(baseUri, timeout, shouldValidateResponse);
    }

    public void SettingUpRestClient(string baseUri, int timeout, bool shouldValidateResponse)
    {
        this.baseUri = baseUri.EndsWith("/") ? baseUri : baseUri + "/";
        validateResponse = shouldValidateResponse;
        networkTimeout = timeout;
        ProcessHeaderUser = string.Empty;
        CreateHttpClient();
    }

    public void SetTimeoutInSeconds(int timeout)
    {
        networkTimeout = timeout;
        CreateHttpClient();
    }

    public void SetCredentials(UserCredentials userCredentials)
    {
        if (headers.ContainsKey("Authorization"))
        {
            headers.Remove("Authorization");
        }
        headers.Add("Authorization", userCredentials.GetAuthHeaderString());
        if (httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            httpClient.DefaultRequestHeaders.Remove("User-Agent");
        }
        httpClient.DefaultRequestHeaders.Add("User-Agent", $"Tessitura_SDK_{userCredentials.UserName}");
    }

    public IRestClient WithHeader(string headerkey, string headerValue)
    {
        headers.Add(headerkey, headerValue);
        return this;
    }

    public IRestClient DropHeader(string headerkey)
    {
        headers.Remove(headerkey);
        return this;
    }

    public string? GetBaseUrl()
    {
        return baseUri;
    }

    public IRestClient AtUrl(string uriString)
    {
        uri = baseUri + uriString;
        return this;
    }

    public IRestClient ValidatingResponse()
    {
        validateResponse = true;
        return this;
    }

    public async Task<RestResponse<T>> Get<T>()
    {
        try
        {
            var requestMessage = CreateRequestMessage(HttpMethod.Get, uri);
            var response = await httpClient.SendAsync(requestMessage);

            return await ConstructRestReponse<T>(response);
        }
        catch (Exception ex)
        {
            var description = ex.Message;
            if(ex.InnerException != null)
            {
                description += Environment.NewLine + ex.InnerException.Message;
            }
            var errorMessages = new ErrorMessages {new ErrorMessage {Code = "", Description = description, Details = ex.Source, ErrorPath = ex.StackTrace}};
            return new RestResponse<T>(HttpStatusCode.BadRequest, default(T) ?? (T)new object(), errorMessages, "", HttpStatusCode.BadRequest.ToString());
        }
    }

    public async Task<RestResponse<T>> Head<T>()
    {
        try
        {
            var requestMessage = CreateRequestMessage(HttpMethod.Head, uri);
            var response = await httpClient.SendAsync(requestMessage);

            return await ConstructRestReponse<T>(response);
        }
        catch (Exception ex)
        {
            var description = ex.Message;
            if (ex.InnerException != null)
            {
                description += Environment.NewLine + ex.InnerException.Message;
            }
            var errorMessages = new ErrorMessages {new ErrorMessage {Code = "", Description = description, Details = ex.Source, ErrorPath = ex.StackTrace}};
            return new RestResponse<T>(HttpStatusCode.BadRequest, default(T) ?? (T)new object(), errorMessages, "", HttpStatusCode.BadRequest.ToString());
        }
    }

    public async Task<RestResponse<TResponse>> Post<TRequest, TResponse>(TRequest data)
    {
        try
        {
            var requestMessage = CreateRequestMessage(HttpMethod.Post, uri, data);
            var response = await httpClient.SendAsync(requestMessage);
            return await ConstructRestReponse<TResponse>(response);
        }
        catch (Exception ex)
        {
            var description = ex.Message;
            if (ex.InnerException != null)
            {
                description += Environment.NewLine + ex.InnerException.Message;
            }
            var errorMessages = new ErrorMessages {new ErrorMessage {Code = "", Description = description, Details = ex.Source, ErrorPath = ex.StackTrace}};
            return new RestResponse<TResponse>(
                HttpStatusCode.BadRequest,
                default(TResponse) ?? (TResponse)new object(),
                errorMessages,
                "",
                HttpStatusCode.BadRequest.ToString());
        }
    }
    public async Task<RestResponse<TResponse>> Put<TResponse>()
    {
        try
        {
            var requestMessage = CreateRequestMessage(HttpMethod.Put, uri, "");
            var response = await httpClient.SendAsync(requestMessage);
            return await ConstructRestReponse<TResponse>(response);
        }
        catch (Exception ex)
        {
            var description = ex.Message;
            if (ex.InnerException != null)
            {
                description += Environment.NewLine + ex.InnerException.Message;
            }
            var errorMessages = new ErrorMessages {new ErrorMessage {Code = "", Description = description, Details = ex.Source, ErrorPath = ex.StackTrace}};
            return new RestResponse<TResponse>(
                HttpStatusCode.BadRequest,
                default(TResponse) ?? (TResponse)new object(),
                errorMessages, "",
                HttpStatusCode.BadRequest.ToString());
        }
    }

    public async Task<RestResponse<TResponse>> Put<TRequest, TResponse>(TRequest data)
    {
        try
        {
            var requestMessage = CreateRequestMessage(HttpMethod.Put, uri, data);
            var response = await httpClient.SendAsync(requestMessage);
            return await ConstructRestReponse<TResponse>(response);
        }
        catch (Exception ex)
        {
            var description = ex.Message;
            if (ex.InnerException != null)
            {
                description += Environment.NewLine + ex.InnerException.Message;
            }
            var errorMessages = new ErrorMessages {new ErrorMessage {Code = "", Description = description, Details = ex.Source, ErrorPath = ex.StackTrace}};
            return new RestResponse<TResponse>(
                HttpStatusCode.BadRequest,
                default(TResponse) ?? (TResponse)new object(),
                errorMessages,
                "",
                HttpStatusCode.BadRequest.ToString());
        }
    }

    public async Task<RestResponse<NoResponse>> Delete()
    {
        try
        {
            var requestMessage = CreateRequestMessage(HttpMethod.Delete, uri);
            var response = await httpClient.SendAsync(requestMessage);
            return await ConstructRestReponse<NoResponse>(response);
        }
        catch (Exception ex)
        {
            var description = ex.Message;
            if (ex.InnerException != null)
            {
                description += Environment.NewLine + ex.InnerException.Message;
            }
            var errorMessages = new ErrorMessages {new ErrorMessage {Code = "", Description = description, Details = ex.Source, ErrorPath = ex.StackTrace}};
            return new RestResponse<NoResponse>(HttpStatusCode.BadRequest, new NoResponse(), errorMessages, "", HttpStatusCode.BadRequest.ToString());
        }
    }

    private HttpRequestMessage CreateRequestMessage(HttpMethod method, string? uri)
    {
        var requestMessage = new HttpRequestMessage(method, uri);

        if (!string.IsNullOrEmpty(ProcessHeaderUser))
        {
            requestMessage.Headers.Add("TNProcessingHeader", ProcessHeaderUser);
        }

        foreach (var header in headers)
        {
            requestMessage.Headers.Add(header.Key, header.Value);
        }

        return requestMessage;
    }

    private HttpRequestMessage CreateRequestMessage<TRequest>(HttpMethod method, string? uri, TRequest data)
    {
        var requestMessage = CreateRequestMessage(method, uri);
        requestMessage.Content = new StringContent(dataContractHelper.Serialize(data, ContentType.Json) ?? string.Empty, Encoding.UTF8, "application/json");
        return requestMessage;
    }

    private async Task<RestResponse<T>> ConstructRestReponse<T>(HttpResponseMessage response)
    {
        var responseObj = default(T);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (IsValidStatusCode(response.StatusCode) && !string.IsNullOrEmpty(responseBody))
        {
            if (typeof (T) == typeof (string) && response.Content?.Headers?.ContentType?.MediaType?.ToLower() != "text/xml")
            {
                if (!responseBody.StartsWith("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">") && !responseBody.StartsWith("<string>"))
                {
                    return new RestResponse<T>(response.StatusCode, responseObj ?? (T)new object(), null, responseBody, response.StatusCode.ToString());
                }
            }
            responseObj = dataContractHelper.Deserialize<T>(responseBody, ContentType.Json);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound && string.IsNullOrEmpty(responseBody))
        {
            throw new Exception("HTTP 404 Not Found");
        }
        else if (!string.IsNullOrEmpty(responseBody))
        {
            if (responseBody.StartsWith("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">"))
            {
                throw new Exception(responseBody.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">", "").Replace("</string>", ""));
            }
            var messages = dataContractHelper.Deserialize<ErrorMessages>(responseBody, ContentType.Json);
            return new RestResponse<T>(response.StatusCode, responseObj ?? (T)new object(), messages, responseBody, response.StatusCode.ToString());
        }

        var restResponse = new RestResponse<T>(response.StatusCode, responseObj ?? (T)new object(), null, responseBody, response.StatusCode.ToString());
        if (validateResponse)
        {
            ValidateResponse(restResponse);
        }
        return restResponse;
    }

    public static void ValidateResponse<T>(RestResponse<T> response)
    {
        if (IsValidStatusCode(response.StatusCode))
        {
            return;
        }
        if (response.ErrorMessages == null)
        {
            throw new RestResponseException(response.StatusCode, response.StatusDescription, response.ResponseBody);
        }
        throw new RestResponseException(response.StatusCode, response.StatusDescription, response.ErrorMessages, response.ResponseBody);
    }

    public static bool IsValidStatusCode(HttpStatusCode httpStatusCode)
    {
        return (int) httpStatusCode >= 200 && (int) httpStatusCode < 300 || httpStatusCode == HttpStatusCode.NotModified;
    }

    public void SetHttpClient(HttpClient _httpClient)
    {
        httpClient = _httpClient;
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (networkTimeout > 0)
        {
            httpClient.Timeout = new TimeSpan(0, 0, networkTimeout);
        }
    }
    public void CreateHttpClient()
    {
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (networkTimeout > 0)
        {
            httpClient.Timeout = new TimeSpan(0, 0, networkTimeout);
        }
    }


}
