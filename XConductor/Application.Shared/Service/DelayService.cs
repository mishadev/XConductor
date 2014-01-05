using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XConductor.Application.Shared.Service.Abstractions;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Analyzations;
using XConductor.Domain.Seedwork.Common;
using XConductor.Domain.Seedwork.Generating;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Domain.Seedwork.Media.Reader;
using XConductor.Domain.Seedwork.Media.Writer;
using XConductor.Domain.Seedwork.Transformations;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;
using XConductor.Application.Shared.Service.EventArgs;

namespace XConductor.Application.Shared.Service
{
    public class DelayService : IDelayService
    {
        public static readonly object GENERATING_PROCESS = new object();
        public static readonly object READING_PROCESS = new object();

        public event EventHandler<MediaEventArgs> OnDetermineDelayStart;
        public event EventHandler<DataEventArgs<IDelayServiceResults>> OnDetermineDelayStop;

        private IAudioService m_audioService;
        private IProcessingService m_processingService;

		private Func<IAudioTransformator> m_transformatorThunk;
		private Func<IAudioAnalyzer> m_analyzerThunk;

        private IList<double> m_resultsSample = new List<double>();
        private IList<double> m_resultsCaptured = new List<double>();
        private IAudioReader m_reader;
        private IToneGenerator m_generator;
        private IAudioWriter m_writer;

        private IObservableDomainService m_majorService;

        public IAudioService AudioService
        {
            get { return this.m_audioService; }
        }

        public IProcessingService ProcessingService
        {
            get { return this.m_processingService; }
        }

        public DelayService(
            IAudioService audioService,
            IProcessingService processingService,
            IAudioReader reader,
            IToneGenerator generator,
            IAudioWriter writer,
            Func<IAudioTransformator> transformatorThunk,
			Func<IAudioAnalyzer> analyzerThunk)
        {
			this.Settings = new DelayServiceSettings ();

            this.m_audioService = audioService;
            this.m_processingService = processingService;

            this.m_reader = reader;
            this.m_generator = generator;

            this.m_writer = writer;

			this.m_transformatorThunk = transformatorThunk;
			this.m_analyzerThunk = analyzerThunk;

			this.m_majorService = this.m_generator;
        }

        private void RecreateServices()
        {
            this.m_resultsSample.Clear();
            this.m_resultsCaptured.Clear();

			this.m_processingStart = null;
			this.m_processingStop = null;
			this.m_playingStart = null;
			this.m_playingStop = null;
			this.m_recordingStop = null;

            this.InitProcessing(this.m_majorService);
        }

        public void InitProcessing(IObservableDomainService majorDomainService)
        {
            this.m_processingService.OnProcessingResultsAvailable += (s, arg) =>
            {
                if (arg.ChainKey == READING_PROCESS)
                    this.m_resultsCaptured.Add((double)arg.State);

                if (arg.ChainKey == GENERATING_PROCESS)
                    this.m_resultsSample.Add((double)arg.State);
            };

            this.m_processingService.Create(GENERATING_PROCESS, majorDomainService);
            this.m_processingService.Create(READING_PROCESS, this.m_reader);

			IChainedDomainService[] GENERATING_chain = new IChainedDomainService[] { this.m_writer, this.m_transformatorThunk(), this.m_analyzerThunk() };
			IChainedDomainService[] READING_chain = new IChainedDomainService[] { this.m_transformatorThunk(), this.m_analyzerThunk() };

			this.m_processingService.AddRange(GENERATING_PROCESS, GENERATING_chain);
			this.m_processingService.AddRange(READING_PROCESS, READING_chain);
        }

        public async Task StartDetermineDelay(string sampleName, string captureName)
        {
            if (this.HasAudioService)
            {
                this.m_majorService.SettingsService.SetContext(this.Settings.AsPropertiesDictionary().ToArray());
                
				await this.CreateSequence(sampleName, captureName);

				this.DetermineDelayStarted();
            }
        }

        private async Task CreateSequence(string sampleName, string captureName)
        {
            this.RecreateServices();

			this.m_processingService.OnProcessingStop += this.GetProcessingStop(sampleName);
			this.m_audioService.OnPlayingStart += this.GetPlayingStart(captureName);
			this.m_audioService.OnPlayingStop += this.GetPlayingStop();
			this.m_audioService.OnRecordingStop += this.GetRecordingStop(captureName);

			this.m_processingService.OnProcessingStart += this.GetProcessingStart();

			await this.m_processingService.Start(GENERATING_PROCESS, sampleName);
        }

		EventHandler<ProcessingEventArgs> m_processingStart;
		private EventHandler<ProcessingEventArgs> GetProcessingStart()
		{
			return this.m_processingStart;
		}

		EventHandler<ProcessingEventArgs> m_processingStop;
		private EventHandler<ProcessingEventArgs> GetProcessingStop(string sample)
		{
			return this.m_processingStop ?? 
				(this.m_processingStop = async (s, e) => {
					if (e.ChainKey == GENERATING_PROCESS) {
						await this.m_audioService.StartPlaying (sample); 
					} else {
						await this.StopDetermineDelay();
					}
				});
		}

		EventHandler<MediaEventArgs> m_playingStart;
		private EventHandler<MediaEventArgs> GetPlayingStart(string capture)
		{
			return this.m_playingStart ?? 
				(this.m_playingStart = async (s, e) => { 
					await this.m_audioService.StartRecording(capture); 
				});
		}

		EventHandler<MediaEventArgs> m_playingStop;
		private EventHandler<MediaEventArgs> GetPlayingStop()
		{
			return this.m_playingStop ?? 
				(this.m_playingStop = async (s, e) => { 
					await this.m_audioService.StopRecording(); 
				});
		}

		EventHandler<MediaEventArgs> m_recordingStop;
		private EventHandler<MediaEventArgs> GetRecordingStop(string capture)
		{
			return this.m_recordingStop ?? 
				(this.m_recordingStop = async (s, e) => {
					await this.m_processingService.Start(READING_PROCESS, capture);
				});
		}

        public async Task StopDetermineDelay()
        {
            if (this.HasAudioService)
            {
                await this.m_audioService.StopRecording();

				this.DetermineDelayStoped();
            }
        }

        public bool HasAudioService
        {
            get
            {
                return this.m_audioService != null && this.m_audioService.IsInitialized;
            }
        }

		private void DetermineDelayStarted()
        {
            if (this.OnDetermineDelayStart != null)
            {
                this.OnDetermineDelayStart(this, MediaEventArgs.Empty);
            }
        }

        private void DetermineDelayStoped()
        {
            if (this.OnDetermineDelayStop != null)
            {
                this.OnDetermineDelayStop(this,
					new DataEventArgs<IDelayServiceResults>(
						new ComputeableDelayServiceResults(
							this.m_resultsSample.ToArray(), 
							this.m_resultsCaptured.ToArray()
						)
					)
                );
            }
        }

		public IDelayServiceSettings Settings { get; private set; }
    }
}
