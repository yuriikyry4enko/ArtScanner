using System;
using System.Threading.Tasks;
using LinqToTwitter;

namespace ArtScanner.Services
{
    public class TwitterService : ITwitterService
    {
        public IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret)
        {
            throw new NotImplementedException();
        }

        public IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret, string oAuthToken, string oAuthTokenSecret)
        {
            throw new NotImplementedException();
        }

        public UserSecrets GetSecrets()
        {
            throw new NotImplementedException();
        }

        public Task SetSecretsAsync(UserSecrets secrets)
        {
            throw new NotImplementedException();
        }

        public Task SetSecretsAsync(string oauthToken, string oauthSecret)
        {
            throw new NotImplementedException();
        }
    }
}
