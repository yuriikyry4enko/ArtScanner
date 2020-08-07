using System;
using System.Threading.Tasks;
using LinqToTwitter;

namespace ArtScanner.Services
{
    interface ITwitterService
    {
        IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret);

        IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret, string oAuthToken, string oAuthTokenSecret);

        UserSecrets GetSecrets();

        Task SetSecretsAsync(UserSecrets secrets);

        Task SetSecretsAsync(string oauthToken, string oauthSecret);
    }

    public class UserSecrets
    {
        public string OAuthSecret { get; set; }

        public string OAuthToken { get; set; }
    }
}
