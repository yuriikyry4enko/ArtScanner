using System;
using System.Collections.Generic;
using System.Linq;
using ContextMenu.iOS;
using Foundation;
using Lottie.Forms.iOS.Renderers;
using Prism;
using Prism.Ioc;
using UIKit;

namespace ArtScanner.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        //public override UIWindow Window { get; set; }

        //public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        //{
        //    if (Window == null)
        //    {
        //        Window = new UIWindow(frame: UIScreen.MainScreen.Bounds);
        //        var initialViewController = new SplashViewController();
        //        Window.RootViewController = initialViewController;
        //        Window.MakeKeyAndVisible();

        //        return true;
        //    }
        //    else
        //    {
        //        FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
        //        global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
        //        AnimationViewRenderer.Init();
        //        Rg.Plugins.Popup.Popup.Init();
        //        global::Xamarin.Forms.Forms.Init();


        //        ContextMenuViewRenderer.Preserve();

        //        LoadApplication(new App(new iOSInitializer()));

        //        return base.FinishedLaunching(uiApplication, launchOptions);
        //    }
        //}

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            AnimationViewRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();


            ContextMenuViewRenderer.Preserve();

            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
