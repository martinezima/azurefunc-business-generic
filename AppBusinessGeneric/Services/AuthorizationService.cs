using System;
using System.Threading.Tasks;
using AppBusinessGeneric.Interfaces;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Services;

public class AuthorizationService: IAuthorization
{
    public AuthorizationService()
    {

    }
    public async Task<BusinessLayerAuthResponse> GetAuthorizationToken(Connector connector)
    {
        var businessLayerAuthResponse = new BusinessLayerAuthResponse();
        if(connector == null) 
        {
            throw new ArgumentException(null, nameof(connector));
        }
        if(string.IsNullOrEmpty(connector.LoginEndpoint))
        {
            throw new NullReferenceException("Login endpoint is missing on the connector.");
        }
        if(string.IsNullOrEmpty(connector.ClientId))
        {
            throw new NullReferenceException("Client Id is missing on the connector.");
        }
        if(string.IsNullOrEmpty(connector.ClientSecret))
        {
            throw new NullReferenceException("Client secret is missing on the connector.");
        }
        if(string.IsNullOrEmpty(connector.Scope))
        {
            throw new NullReferenceException("Scope is missing on the connector.");
        }


        return await Task.FromResult(businessLayerAuthResponse);
    }
}