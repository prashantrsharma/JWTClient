using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace JWTClient
{
    public static class JWTServices
    {
        private static readonly string _tokenUri = ConfigurationManager.AppSettings["TokenURI"];

        public static async Task<IEnumerable<Claim>> GetTokenAsync(string username, string password)
        {
            using (var client = new HttpClient(new HttpClientHandler(), false))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });

                var response = await client.PostAsync(_tokenUri, content, new CancellationToken()).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var responseToken = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var json = JObject.Parse(responseToken);
                    var jwt = json["access_token"].ToString();

                    JwtSecurityTokenHandler handler;
                    JwtSecurityToken jwtTokenContent;

                    try
                    {
                        handler = new JwtSecurityTokenHandler();
                        jwtTokenContent = handler.ReadToken(jwt) as JwtSecurityToken;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    return jwtTokenContent.Claims;

                }
                else
                {
                    var errorResponse = await content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new HttpException((int)HttpStatusCode.InternalServerError,errorResponse);
                }
            }
        }

    }
}