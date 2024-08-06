using AppBusinessGeneric.RestClient.Models;

namespace AppBusinessGeneric.RestClient.Interfaces;

public interface IRestClient
{
    string ProcessHeaderUser { get; set; }
    IRestClient AtUrl(string uriString);
    Task<RestResponse<NoResponse>> Delete();
    IRestClient DropHeader(string headerkey);
    Task<RestResponse<T>> Get<T>();
    Task<RestResponse<T>> Head<T>();
    string? GetBaseUrl();
    Task<RestResponse<TResponse>> Post<TRequest, TResponse>(TRequest data);
    Task<RestResponse<TResponse>> Put<TRequest, TResponse>(TRequest data);
    Task<RestResponse<TResponse>> Put<TResponse>();
    void SetTimeoutInSeconds(int timeout);
    void SetCredentials(UserCredentials userCredentials);
    IRestClient ValidatingResponse();
    IRestClient WithHeader(string headerkey, string headerValue);
    // Customization
    void SettingUpRestClient(string baseUri, int timeout, bool shouldValidateResponse);
    void CreateHttpClient();
    void SetHttpClient(HttpClient _httpClient);
}