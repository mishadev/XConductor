using System;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using XConductor.Application.Shared.Service.Abstractions;
using XConductor.Presentation.IPhone.Screen;
using XConductor.Domain.Seedwork.Common;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;
using System.Collections.Generic;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Domain.Seedwork.Generating.Settings;

namespace XConductor.Presentation.IPhone
{
	[Flags]
	public enum AppSettings
	{
		None = 0,
		UsePhaseShifting = 1,
		UseStepsAlgorithm = 2
	}

    public partial class DelayPageController : UIViewController
    {
        private IDelayService m_service;
        private MainViewSettings m_settings;

        public DelayPageController(IDelayService service, MainViewSettings settings)
            : base("DelayPageController", null)
        {
            this.m_service = service;
            this.m_settings = settings;
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.m_service.OnDetermineDelayStop += m_service_OnDetermineDelayStop;
            this.m_service.OnDetermineDelayStart += m_service_OnDetermineDelayStart;

			this.tbCurrection_1.ShouldReturn = DoReturn;
			this.tbCurrection2.ShouldReturn = DoReturn;
			this.tbCurrection3.ShouldReturn = DoReturn;
			this.tbCurrection4.ShouldReturn = DoReturn;

            this.tbFrequency_1.ShouldReturn = DoReturn;
            this.tbFrequency_2.ShouldReturn = DoReturn;
            this.tbFrequency_3.ShouldReturn = DoReturn;
            this.tbFrequency_4.ShouldReturn = DoReturn;

            this.tbGapTime.ShouldReturn = DoReturn;
            this.tbPeakTime.ShouldReturn = DoReturn;
            this.tbGapChannelTime.ShouldReturn = DoReturn;
			this.tbFlag.ShouldReturn = DoReturn;

			this.tbCurrection_1.ShouldChangeCharacters = DoChange;
			this.tbCurrection2.ShouldChangeCharacters = DoChange;
			this.tbCurrection3.ShouldChangeCharacters = DoChange;
			this.tbCurrection4.ShouldChangeCharacters = DoChange;

            this.tbFrequency_1.ShouldChangeCharacters = DoChange;
            this.tbFrequency_2.ShouldChangeCharacters = DoChange;
            this.tbFrequency_3.ShouldChangeCharacters = DoChange;
            this.tbFrequency_4.ShouldChangeCharacters = DoChange;

            this.tbGapTime.ShouldChangeCharacters = DoChange;
            this.tbPeakTime.ShouldChangeCharacters = DoChange;
            this.tbGapChannelTime.ShouldChangeCharacters = DoChange;
			this.tbFlag.ShouldChangeCharacters = DoChange;

			this.btnStart.TouchUpInside += async (sender, e) =>
			{
				this.btnStart.Enabled = false;
				this.btnPlayback.Enabled = false;

				this.SetDelaySettings();

				await this.m_service.StartDetermineDelay(this.m_settings.SampleFilePath, this.m_settings.CapturedFilePath);
			};

			this.btnPlayback.TouchUpInside += async (sender, e) =>
			{
				await this.m_service.AudioService.StopPlaying();
				await this.m_service.AudioService.StartPlaying(this.m_settings.CapturedFilePath);
			};
        }

        private void SetDelaySettings()
        {
            this.m_service.Settings.Amplitude = 100;

			int?[] values = new int?[] {
				IntParceOrDefault(this.tbFrequency_1.Text),
				IntParceOrDefault(this.tbFrequency_2.Text),
				IntParceOrDefault(this.tbFrequency_3.Text),
				IntParceOrDefault(this.tbFrequency_4.Text)
			};

			if (values.Any(v => v.HasValue))
            {
				List<WaveConfiguration> list = new List<WaveConfiguration>();

				int idx = 0;
				while (values.Length > idx) {
					int? value = values [idx];
					if (value.HasValue) {
						list.Add(new WaveConfiguration 
						{ 
							Frequency = value.Value,
							AllChannelsSimultaneously = idx == (values.Length - 1)
						});
					}
					idx++;
				}

				var array = list.ToArray ();

				this.m_service.Settings.Configurations = array;
				this.m_service.Settings.PeakCount = array.Length;
            }

			this.m_service.Settings.UsePhaseShifting = (AppSettings.UsePhaseShifting & (AppSettings)(IntParceOrDefault (this.tbFlag.Text) ?? 0)) != AppSettings.None;
			this.m_service.Settings.UseStepsAlgorithm = (AppSettings.UseStepsAlgorithm & (AppSettings)(IntParceOrDefault (this.tbFlag.Text) ?? 0)) != AppSettings.None;

            this.m_service.Settings.PeakChannelGap = IntParceOrDefault(this.tbGapChannelTime.Text);
            this.m_service.Settings.PeakGap = IntParceOrDefault(this.tbGapTime.Text);
            this.m_service.Settings.PeakTime = IntParceOrDefault(this.tbPeakTime.Text);
        }

        private static int? IntParceOrDefault(string text)
        {
            int value;

            if (int.TryParse(text, out value))
                return value;

            return null;
        }

		private static float? FloatParceOrDefault(string text)
		{
			float value;

			if (float.TryParse(text, out value))
				return value;

			return null;
		}

        private void m_service_OnDetermineDelayStart(object sender, MediaEventArgs e)
        {
			this.lbResults.TextColor = UIColor.Black;
            this.lbResults.Text = "Processing ...";
        }

        void m_service_OnDetermineDelayStop(object sender, DataEventArgs<IDelayServiceResults> e)
        {
            this.btnStart.Enabled = true;
			this.btnPlayback.Enabled = true;

            this.lbResults.Text = string.Empty;
			this.lbResults.TextColor = UIColor.Black;

			float[] values = new float[4];
			if (!e.Data.IsValidResults) {
				this.lbResults.Text = "Weak sound or too mach noice!" + Environment.NewLine;
				this.lbResults.TextColor = UIColor.Red;
			} else {
				values = new float[4]; 
				var k = 0;

				var temp = FloatParceOrDefault (this.tbCurrection_1.Text);
				if (temp.HasValue) {
					values[k++] = temp.Value;
				}

				temp = FloatParceOrDefault (this.tbCurrection2.Text);
				if (temp.HasValue) {
					values[k++] = temp.Value;
				}

				temp = FloatParceOrDefault (this.tbCurrection3.Text);
				if (temp.HasValue) {
					values[k++] = temp.Value;
				}

				temp = FloatParceOrDefault (this.tbCurrection4.Text);
				if (temp.HasValue) {
					values[k++] = temp.Value;
				}
			}

            var idx = 1;
			double currection = values [0];
            foreach (var delay in e.Data.Delays)
            {
				this.lbResults.Text += (delay * 1000 + currection).ToString("0.00  ");

				if (idx++ % 2 == 0) {
					this.lbResults.Text += Environment.NewLine;
					currection = values[idx / 2];
				}
            }

			this.btnPlayback.Enabled = true;
        }

        bool DoReturn(UITextField tf)
        {
            tf.ResignFirstResponder();
            return true;
        }

        bool DoChange(UITextField tf, NSRange range, string rep)
        {
            return DoChange(tf, range, rep, 5);
        }

        bool DoChange(UITextField tf, NSRange range, string rep, int charsCount)
        {
			var res = rep == string.Empty || (range.Location < charsCount && Regex.IsMatch(rep, "^\\d{1," + charsCount + "}|\\.$"));
            return res;
        }
    }
}

