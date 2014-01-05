using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

using XConductor.Presentation.IPhone.Screen;
using XConductor.Application.Shared.Service;
using XConductor.Domain.Shared.Transformations;
using XConductor.Domain.Shared.Transformations.Settings;
using XConductor.Domain.iOS.Media.Reader;
using XConductor.Domain.iOS.Media.Reader.Settings;
using XConductor.Domain.iOS.Caturing;
using XConductor.Domain.iOS.Caturing.Settings;
using XConductor.Domain.iOS.Playback;
using XConductor.Domain.iOS.Playback.Settings;
using XConductor.Domain.Shared.Analyzations;
using XConductor.Domain.Shared.Analyzations.Settings;
using XConductor.Domain.Shared.Generating;
using XConductor.Domain.Shared.Generating.Settings;
using XConductor.Domain.iOS.Media.Writer;
using XConductor.Domain.iOS.Media.Writer.Settings;
using XConductor.Presentation.IPhone;

namespace Presentation.IPhone
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;
		DelayPageController m_viewController;

        public static readonly UIColor ColorNavBarTint = UIColor.FromRGB(55, 87, 118);

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            this.window = new UIWindow(UIScreen.MainScreen.Bounds);

            var settings = new MainViewSettings 
                { 
                    CapturedFilePath = "AudioRecording.aif",
                    SampleFilePath = "SampleRecording.aif"
                };

			var service = new DelayAlgorithmService(() => new DelayService(
                audioService: new AudioService(
                    capture: new CaptureSessioniOS(new CaptureSettingsService()),
				    player: new AudioPlayeriOS(new AudioPlayerSettingsService())),
                processingService: new ProcessingService(),
                reader: new AudioReaderiOS(new AudioReaderSettingsService()),
                writer: new AudioWriteriOS(new AudioWriterSettingsService()),
                generator: new ToneGenerator(new ToneGeneratorSettingsService()),
				transformatorThunk: () => new AudioTransformator(new AudioTransformatorSettingsService()),
				analyzerThunk: () => new AudioAnalyzer(new AudioAnalyzerSettingsService())));

			this.m_viewController = new DelayPageController(service, settings);
            this.window.RootViewController = this.m_viewController;

            if (AppDelegate.iOSVersion >= 5)
            {
                UINavigationBar.Appearance.TintColor = AppDelegate.ColorNavBarTint;
            }

			UIApplication.SharedApplication.IdleTimerDisabled = true;

            this.window.MakeKeyAndVisible();

            return true;
        }

        public static int iOSVersion
        {
            get 
            {
                string majorVersionString = UIDevice.CurrentDevice.SystemVersion.Substring(0, 1);
                int majorVersion = Convert.ToInt16(majorVersionString);

                return majorVersion;
            }
        }

        public static bool IsPhone
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
        }

        public static bool IsPad
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
            }
        }

        public static bool HasRetina
        {
            get
            {
                if (UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                    return (UIScreen.MainScreen.Scale == 2.0);
                else
                    return false;
            }
        }
    }
}

