using AppBusinessGeneric.Services;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Tests;

public class AuthorizationServiceTest
{
    [Fact]
    public async Task GetAuthorizationToken_SetConnectorNull_ThrowAnException()
    {
        Connector? connector = null;

        var authorizationService = new AuthorizationService();
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => authorizationService.GetAuthorizationToken(connector));
    }

    [Fact]
    public async Task GetAuthorizationToken_SetConnector_WithoutLoginEndPoint_ThrowAnException()
    {
        var connector = new Connector
        {
            LoginEndpoint = ""
        };

        var authorizationService = new AuthorizationService();
        var exception = await Assert.ThrowsAsync<NullReferenceException>(
            () => authorizationService.GetAuthorizationToken(connector));
        Assert.Equal("Login endpoint is missing on the connector.", exception.Message);
    }
    
    [Fact]
    public async Task GetAuthorizationToken_SetConnector_WithoutClientId_ThrowAnException()
    {
        var connector = new Connector
        {
            LoginEndpoint = "https://someEndPoint/",
            ClientId = ""
        };

        var authorizationService = new AuthorizationService();
        var exception = await Assert.ThrowsAsync<NullReferenceException>(
            () => authorizationService.GetAuthorizationToken(connector));
        Assert.Equal("Client Id is missing on the connector.", exception.Message);
        
    }
    
    [Fact]
    public async Task GetAuthorizationToken_SetConnector_WithoutClientSecret_ThrowAnException()
    {
        var connector = new Connector
        {
            LoginEndpoint = "https://someEndPoint/",
            ClientId = "this is a client id",
            ClientSecret = ""
        };

        var authorizationService = new AuthorizationService();
        var exception = await Assert.ThrowsAsync<NullReferenceException>(
            () => authorizationService.GetAuthorizationToken(connector));
        Assert.Equal("Client secret is missing on the connector.", exception.Message);        
    }
   
    [Fact]
    public async Task GetAuthorizationToken_SetConnector_WithoutScope_ThrowAnException()
    {
        var connector = new Connector
        {
            LoginEndpoint = "https://someEndPoint/",
            ClientId = "this is a client id",
            ClientSecret = "this is a client secret",
            Scope = ""
        };

        var authorizationService = new AuthorizationService();
        var exception = await Assert.ThrowsAsync<NullReferenceException>(
            () => authorizationService.GetAuthorizationToken(connector));
        Assert.Equal("Scope is missing on the connector.", exception.Message);  
    }
}