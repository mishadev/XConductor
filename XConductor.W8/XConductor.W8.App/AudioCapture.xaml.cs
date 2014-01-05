using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using XConductor.Application.Shared.Service;
using XConductor.Application.Shared.Service.Abstractions;
using XConductor.Domain.Shared.Analyzations;
using XConductor.Domain.Shared.Analyzations.Settings;
using XConductor.Domain.Shared.Generating;
using XConductor.Domain.Shared.Generating.Settings;
using XConductor.Domain.Shared.Transformations;
using XConductor.Domain.Shared.Transformations.Settings;
using XConductor.Domain.W8.Capturing;
using XConductor.Domain.W8.Capturing.Settings;
using XConductor.Domain.W8.Media.Reader;
using XConductor.Domain.W8.Media.Reader.Settings;
using XConductor.Domain.W8.Playback;
using XConductor.Domain.W8.Playback.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.W8.App
{
    public sealed partial class AudioCapture : XConductor.W8.App.Common.LayoutAwarePage
    {
        public static readonly string AUDIO_SAMPLE_NAME = "sample.mp3";
        public static readonly string AUDIO_FILE_NAME = "audio.mp3";

        private Polyline topLineRed = new Polyline();
        private Polyline bottomLineRed = new Polyline();

        private Polyline topLineBlue = new Polyline();
        private Polyline bottomLineBlue = new Polyline();

        private readonly IDelayService m_delayService;

        private double m_idx_r = 0;
        private double m_idx_b = 0;
        private double[] m_delays;

        public AudioCapture()
        {
            this.InitializeComponent();

            this.m_delayService = new DelayService(
                new AudioService(
                    new CaptureSessionW8(new CaptureSettingsService()),
                    new AudioPlayerW8(new AudioPlayerSettingsService(this.playbackElement))
                ),
                new ProcessingService(),
                new MediaFoundationAudioReader(new AudioReaderSettingsService()),
                new ToneGenerator(new ToneGeneratorSettingsService()),
                null,
                new AudioTransformator(new AudioTransformatorSettingsService()),
                new AudioAnalyzer(new AudioAnalyzerSettingsService())
            );

            this.m_delayService.OnDetermineDelayStop += (s, e) => this.OutputResults(e.Data.Delays);
            this.m_delayService.ProcessingService.OnProcessingDataAvailable += (s, e) => this.RenderData(e.ChainKey, (float[])e.State);

            topLineRed.Stroke = new SolidColorBrush(Colors.Red);
            topLineRed.StrokeThickness = 1;
            bottomLineRed.Stroke = new SolidColorBrush(Colors.Red);
            bottomLineRed.StrokeThickness = 1;

            this.playbackCanvas.Children.Add(topLineRed);
            this.playbackCanvas.Children.Add(bottomLineRed);

            topLineBlue.Stroke = new SolidColorBrush(Colors.Blue);
            topLineBlue.StrokeThickness = 1;
            bottomLineBlue.Stroke = new SolidColorBrush(Colors.Blue);
            bottomLineBlue.StrokeThickness = 1;

            this.playbackCanvas.Children.Add(topLineBlue);
            this.playbackCanvas.Children.Add(bottomLineBlue);
        }

        private void OutputResults(double[] p)
        {
            this.textResult.Text += string.Join(" : ", p.Select(i => i.ToString("0.000"))) + Environment.NewLine;
        }

        internal async void btnStartProcessing_Click(Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            topLineRed.Points.Clear();
            bottomLineRed.Points.Clear();
            topLineBlue.Points.Clear();
            bottomLineBlue.Points.Clear();
            this.m_idx_r = 0;
            this.m_idx_b = 0;

            try
            {
                this.m_delayService.StartDetermineDelay(AUDIO_SAMPLE_NAME, AUDIO_FILE_NAME).Await();

                //if (!this.m_handling)
                //{
                //    this.m_audioService.StartRecording(AUDIO_SAMPLE_NAME).Await();
                //    this.m_handling = true;
                //}
                //else
                //{
                //    this.m_audioService.StopRecording().Await();
                //}
            }
            catch
            { }
        }

        private double SampleToYPosition(float value, double yTranslate, double yScale)
        {
            return yTranslate + value * yScale;
        }

        private void RenderPoints(double renderPosition, int topValue, int bottomValue, Polyline lineTop, Polyline lineBottom)
        {
            double yTranslate = 40;
            double yScale = 100;

            double topLinePos = this.SampleToYPosition((float)topValue / byte.MaxValue, yTranslate, yScale);
            double bottomLinePos = this.SampleToYPosition((float)bottomValue / byte.MaxValue, yTranslate, yScale);

            double pos = renderPosition / 8;

            lineTop.Points.Add(new Point(pos, topLinePos));
            lineBottom.Points.Add(new Point(pos, bottomLinePos));
        }

        private void RenderData(object key, float[] date)
        {
            float scale = 0.02f;
            int topOffset = 200;
            int fp = (int)(this.playbackCanvas.Height * date.Average(v => v) * scale);

            if (key == DelayService.READING_PROCESS)
            {
                this.RenderPoints(++this.m_idx_b,
                fp + topOffset,
                -1 * fp + topOffset,
                this.topLineRed,
                this.bottomLineRed);
            }
            else
            {
                this.RenderPoints(++this.m_idx_r,
                fp + topOffset,
                -1 * fp + topOffset,
                this.topLineBlue,
                this.bottomLineBlue);
            }
        }
    }
}
