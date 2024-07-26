using System.Threading.Tasks;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Interfaces;

public interface IAuthorization
{
    Task<BusinessLayerAuthResponse> GetAuthorizationToken(Connector connector);
}