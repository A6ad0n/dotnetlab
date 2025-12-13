using AutoMapper;
using Duende.IdentityServer.Models;
using IdentityModel.Client;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Auth.Entities;

namespace PizzaApp.BL.Features.Auth.Providers;

public class AuthProvider(
    IMapper mapper,
    IHttpClientFactory httpClientFactory,
    string identityServerUri,
    string clientId,
    string clientSecret)
    : IAuthProvider
{
    public async Task<TokensResponse> AuthorizeUserAsync(string username, string password)
    {
        var client = httpClientFactory.CreateClient();
        var discoveryDocument = await client.GetDiscoveryDocumentAsync(identityServerUri);
        if (discoveryDocument.IsError)
        {
            throw new BusinessLogicException<CommonResultCode>(CommonResultCode.IdentityServerError);
        }

        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = discoveryDocument.TokenEndpoint,
            GrantType = GrantType.ResourceOwnerPassword,
            ClientId = clientId,
            ClientSecret = clientSecret,
            UserName = username,
            Password = password,
            Scope = "api offline_access"
        });

        if (tokenResponse.IsError)
        {
            throw new BusinessLogicException<CommonResultCode>(CommonResultCode.IdentityServerError);
        }

        return mapper.Map<TokensResponse>(tokenResponse);
    }

    public async Task<TokensResponse> RefreshTokenAsync(string refreshToken)
    {
        var client = httpClientFactory.CreateClient();
        var discoveryDocument = await client.GetDiscoveryDocumentAsync(identityServerUri);
        if (discoveryDocument.IsError)
        {
            throw new BusinessLogicException<CommonResultCode>(CommonResultCode.IdentityServerError);
        }

        var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = clientId,
            ClientSecret = clientSecret,
            RefreshToken = refreshToken
        });
        
        if (tokenResponse.IsError)
        {
            throw new BusinessLogicException<CommonResultCode>(CommonResultCode.IdentityServerError);
        }

        return mapper.Map<TokensResponse>(tokenResponse);
    }
}