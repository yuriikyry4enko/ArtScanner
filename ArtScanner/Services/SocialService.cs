using System;
using System.Threading.Tasks;
using ArtScanner.Enums;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace ArtScanner.Services
{
    public class SocialService
    {
        public async Task<Xamarin.Auth.Account> LoginAsync(AuthProviders provider, bool is_native = true)
        {
            switch (provider)
            {
                case AuthProviders.Twitter:
                    return await SocialLogin(TwitterSignup(is_native));
                default:
                    return null;
            }
        }

        public async Task<Xamarin.Auth.Account> SocialLogin<T>(T authenticator)
        {
            var tcs1 = new TaskCompletionSource<AuthenticatorEventArgs>();
            EventHandler<AuthenticatorCompletedEventArgs> completed =
                (o, e) =>
                {
                    try
                    {
                        var eventargs = new AuthenticatorEventArgs(e.Account);
                        tcs1.TrySetResult(eventargs);
                    }
                    catch (Exception ex)
                    {
                        var eventargs = new AuthenticatorEventArgs(ex);
                        tcs1.TrySetResult(eventargs);
                    }
                };
            EventHandler<AuthenticatorErrorEventArgs> error =
                (o, e) =>
                {
                    try
                    {
                        if (e.Exception != null)
                        {
                            var eventargs = new AuthenticatorEventArgs(e.Exception);
                            tcs1.TrySetResult(eventargs);
                        }
                        else
                        {
                            var eventargs = new AuthenticatorEventArgs(e.Message);
                            tcs1.TrySetResult(eventargs);
                        }
                    }
                    catch (Exception ex)
                    {
                        var eventargs = new AuthenticatorEventArgs(ex);
                        tcs1.TrySetResult(eventargs);
                    }
                };

            try
            {
                if (typeof(T) == typeof(OAuth2Authenticator))
                {
                    (authenticator as OAuth2Authenticator).Completed += completed;
                    (authenticator as OAuth2Authenticator).Error += error;

                }
                else
                {
                    (authenticator as OAuth1Authenticator).Completed += completed;
                    (authenticator as OAuth1Authenticator).Error += error;
                }

                Builder(authenticator);
                var result = await tcs1.Task;
                return result.Account;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (typeof(T) == typeof(OAuth2Authenticator))
                {
                    (authenticator as OAuth2Authenticator).Completed -= completed;
                    (authenticator as OAuth2Authenticator).Error -= error;
                }
                else
                {
                    (authenticator as OAuth1Authenticator).Completed -= completed;
                    (authenticator as OAuth1Authenticator).Error -= error;
                }
            }
        }

        public void Builder<T>(T authenticator)
        {
            // after initialization (creation and event subscribing) exposing local object 
            if (typeof(T) == typeof(OAuth2Authenticator))
                AuthenticationState.Authenticator = authenticator as OAuth2Authenticator;
            OAuthLoginPresenter presenter = null;
            presenter = new OAuthLoginPresenter();
            presenter.Login(authenticator as Authenticator);
        }

        private static OAuth1Authenticator TwitterSignup(bool native_ui)
        {
            var authenticator = new OAuth1Authenticator(
                Utils.Constants.ApiConstants.consumerKey,
                isUsingNativeUI: native_ui,
                consumerSecret: Utils.Constants.ApiConstants.consumerSecret,
                requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
                authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
                accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
                callbackUrl: new Uri("https://mobile.twitter.com/")
            );
            authenticator.ShowErrors = false;
            authenticator.AllowCancel = false;
            return authenticator;
        }
    }

    public class AuthenticatorEventArgs : EventArgs
    {
        public AuthenticatorEventArgs(string message)
        {
            Message = message;
        }

        public AuthenticatorEventArgs(Exception exception)
        {
            Message = exception.Message;
            Exception = exception;
        }

        public AuthenticatorEventArgs(Xamarin.Auth.Account account)
        {
            Account = account;
        }

        public Exception Exception { get; }

        public string Message { get; }

        public bool IsAuthenticated => Account != null;

        public Xamarin.Auth.Account Account { get; }
    }

    public class AuthenticationState
    {
        public static OAuth2Authenticator Authenticator;
    }
}