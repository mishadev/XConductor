// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace XConductor.Presentation.IPhone
{
	[Register ("DelayPageController")]
	partial class DelayPageController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnPlayback { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnStart { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lbResults { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbCurrection_1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbCurrection2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbCurrection3 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbCurrection4 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbFlag { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbFrequency_1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbFrequency_2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbFrequency_3 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbFrequency_4 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbGapChannelTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbGapTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbPeakTime { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnPlayback != null) {
				btnPlayback.Dispose ();
				btnPlayback = null;
			}

			if (btnStart != null) {
				btnStart.Dispose ();
				btnStart = null;
			}

			if (lbResults != null) {
				lbResults.Dispose ();
				lbResults = null;
			}

			if (tbFlag != null) {
				tbFlag.Dispose ();
				tbFlag = null;
			}

			if (tbFrequency_1 != null) {
				tbFrequency_1.Dispose ();
				tbFrequency_1 = null;
			}

			if (tbFrequency_2 != null) {
				tbFrequency_2.Dispose ();
				tbFrequency_2 = null;
			}

			if (tbFrequency_3 != null) {
				tbFrequency_3.Dispose ();
				tbFrequency_3 = null;
			}

			if (tbFrequency_4 != null) {
				tbFrequency_4.Dispose ();
				tbFrequency_4 = null;
			}

			if (tbGapChannelTime != null) {
				tbGapChannelTime.Dispose ();
				tbGapChannelTime = null;
			}

			if (tbGapTime != null) {
				tbGapTime.Dispose ();
				tbGapTime = null;
			}

			if (tbPeakTime != null) {
				tbPeakTime.Dispose ();
				tbPeakTime = null;
			}

			if (tbCurrection_1 != null) {
				tbCurrection_1.Dispose ();
				tbCurrection_1 = null;
			}

			if (tbCurrection2 != null) {
				tbCurrection2.Dispose ();
				tbCurrection2 = null;
			}

			if (tbCurrection3 != null) {
				tbCurrection3.Dispose ();
				tbCurrection3 = null;
			}

			if (tbCurrection4 != null) {
				tbCurrection4.Dispose ();
				tbCurrection4 = null;
			}
		}
	}
}
