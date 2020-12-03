using System;
using System.Diagnostics;
using ArtScanner.Services;
using Xamarin.Auth;

namespace ArtScanner.Utils.AuthConfigs
{
    public class OAuthProviderSetting
    {
        public string ClientId { get; private set; }
        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string RequestTokenUrl { get; private set; }
        public string AccessTokenUrl { get; private set; }
        public string AuthorizeUrl { get; private set; }
        public string CallbackUrl { get; private set; }

        public enum OauthIdentityProvider
        {
            TWITTER,
        }

        public OAuth1Authenticator LoginWithTwitter()
        {
            OAuth1Authenticator Twitterauth = null;
            try
            {
                Twitterauth = new OAuth1Authenticator(
                           consumerKey: ArtScanner.Utils.Constants.ApiConstants.consumerKey,    // For Twitter login, for configure refer http://www.c-sharpcorner.com/article/register-identity-provider-for-new-oauth-application/
                           consumerSecret: ArtScanner.Utils.Constants.ApiConstants.consumerSecret,  // For Twitter login, for configure refer http://www.c-sharpcorner.com/article/register-identity-provider-for-new-oauth-application/
                           requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
                           authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
                           accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
                           callbackUrl: new Uri("https://mobile.twitter.com/home")    // Set this property to the location the user will be redirected too after successfully authenticating
                       );
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
            return Twitterauth;
        }

    }
}