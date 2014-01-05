using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using XConductor.Application.Shared.Service.Abstractions;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Domain.Seedwork.Common;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Application.Shared.Service
{
	public class DelayAlgorithmService : IDelayService, IWrapper<IDelayService>
    {
		private const string PARAMETER_SAMPLE_NAME = "sampleName";
		private const string PARAMETER_CAPTURE_NAME = "captureName";

		private int m_step;
		private int m_stepCount;
		private IDelayService m_service;
		private List<IDelayServiceResults> m_results;

		private IDictionary<string, string> m_context;

		public event EventHandler<MediaEventArgs> OnDetermineDelayStart;
		public event EventHandler<DataEventArgs<IDelayServiceResults>> OnDetermineDelayStop;
        private Func<IDelayService> m_serviceThunk;

		public IDelayServiceSettings Settings { get; private set; }
		public IAudioService AudioService { get { return this.m_serviceThunk().AudioService; } }
		public IProcessingService ProcessingService { get { return this.m_serviceThunk().ProcessingService; } }

        public DelayAlgorithmService(Func<IDelayService> serviceThunk)
		{
			this.Settings = new DelayServiceSettings();
			this.m_results = new List<IDelayServiceResults>();

            this.m_serviceThunk = serviceThunk;
		}

		public async Task StartDetermineDelay(string sampleName, string captureName)
		{
			if (this.m_step <= 0 || this.m_step >= this.m_stepCount) {
				this.ClearContext ();
			}

			if (this.m_step == 0) {
				this.m_context = new Dictionary<string, string> { { PARAMETER_SAMPLE_NAME, sampleName }, { PARAMETER_CAPTURE_NAME, captureName } };
				await this.MoveNextStep ();
			} else {
				await this.StopDetermineDelay ();
			}
		}

        private IDelayServiceSettings RecreateInnerServices()
        {
			if (this.m_service != null) {
				this.m_service.ProcessingService.Dispose();
				this.m_service.AudioService.Dispose();
				this.m_service = null;
			}
            
			this.m_service = this.m_serviceThunk();

			this.m_service.OnDetermineDelayStart += InnerOnDetermineDelayStart;
			this.m_service.OnDetermineDelayStop += InnerOnDetermineDelayStop;

			this.m_service.Settings.PeakChannelGap = this.Settings.PeakChannelGap;
			this.m_service.Settings.PeakGap = this.Settings.PeakGap;
			this.m_service.Settings.PeakTime = this.Settings.PeakTime;
			this.m_service.Settings.Amplitude = this.Settings.Amplitude;

			return this.m_service.Settings;
        }

		private void ClearContext()
		{
			this.m_context = null;
			this.m_step = 0;
			this.m_results.Clear ();
		}

		public async Task StopDetermineDelay()
		{
			await this.m_service.StopDetermineDelay();
			this.DetermineDelayStoped();
		}

		private void InnerOnDetermineDelayStart(object sender, MediaEventArgs e)
		{
			if (this.m_step == 0) {
				this.DetermineDelayStarted ();
			}
		}

		private async void InnerOnDetermineDelayStop(object sender, DataEventArgs<IDelayServiceResults> e)
		{
			this.m_step++;
			this.m_results.Add (e.Data);

			if (!e.Data.IsValidResults) {
				this.m_step = this.m_stepCount;
			}

			if (this.m_step != this.m_stepCount) {
				await this.MoveNextStep ();
			} else {
				this.DetermineDelayStoped ();
			}
		}

		private async Task MoveNextStep()
		{
			IDelayServiceSettings settings = this.RecreateInnerServices();

			bool isValid = this.IsValidStepConfiguration (settings);

			if (isValid) {
				await this.PerformStep (settings);
			} else {
				await this.PerformStraight (settings);
			}
		}

		private async Task PerformStraight(IDelayServiceSettings settings)
		{
			this.m_stepCount = 1;

			settings.Configurations = this.Settings.Configurations;
			settings.PeakCount = this.Settings.Configurations.Length;

			await this.m_service.StartDetermineDelay (this.m_context [PARAMETER_SAMPLE_NAME], this.m_context [PARAMETER_CAPTURE_NAME]);
		}

		private async Task PerformStep(IDelayServiceSettings settings)
		{
			if (this.m_step == 0) {
				this.m_stepCount = this.Settings.Configurations.Length + 1;

				var values = this.Settings.Configurations.Where (cfg => !cfg.AllChannelsSimultaneously).ToArray ();

				settings.Configurations = values;
				settings.PeakCount = values.Length;

			} else if(this.m_step < this.m_stepCount - 1) {

				var value = this.Settings.Configurations [this.m_step - 1];
				var values = new[] { value };
				settings.Configurations = values;
				settings.PeakCount = values.Length;

			} else { //last step
				IDelayServiceResults rawResults = this.m_results.First();
				int idx = Array.LastIndexOf(rawResults.Delays, rawResults.Delays.Min ());

				int chennel = idx % 2;//chennels
				int frequency = idx / 2;

				var values = new[] { this.Settings.Configurations[frequency], this.Settings.Configurations[this.m_stepCount - 2] };
				settings.Configurations = values;
				settings.PeakCount = values.Length;

				settings.Configurations[0].Chennels = (OrdinalNumbers)(int)Math.Pow(2, chennel);
			}

			string preffix = this.m_step != 0 ? this.m_step + "_" : string.Empty;

			string sampleName = preffix + this.m_context [PARAMETER_SAMPLE_NAME];
			string captureName = preffix + this.m_context [PARAMETER_CAPTURE_NAME];

			await this.m_service.StartDetermineDelay (sampleName, captureName);
		}

		private bool IsValidStepConfiguration(IDelayServiceSettings settings)
		{
			int notSimultaneousCount = this.Settings.Configurations.Count (cfg => !cfg.AllChannelsSimultaneously);
			int simultaneousCount = this.Settings.Configurations.Count (cfg => cfg.AllChannelsSimultaneously);

			return (this.Settings.UseStepsAlgorithm ?? false) && simultaneousCount == 1 && notSimultaneousCount > 0;
		}

		private IDelayServiceResults CreateDelayServiceResults()
		{
			IDelayServiceResults delayResults = this.m_results.First();
			bool allResultsValid = this.m_results.All (r => r.IsValidResults);

			double[] rawResults = delayResults.Delays;
			double[] results = null;

			if (allResultsValid && this.m_results.Count > 1 && this.m_step == this.m_stepCount) {
				byte idx = 0;
				while (idx < this.m_results.Count / 2) {// chennels
					int offset = idx * 2;// chennels
					int baseminidx = rawResults [offset] < rawResults [offset + 1] ? offset : offset + 1;

					double[] refinement = this.m_results [idx+1].Delays;
					double refdiff = Math.Abs (refinement [0] - refinement [1]); //chennels

					int chennel = Math.Abs(baseminidx % 2 - 1) ;//another chennels

					rawResults [offset + chennel] = rawResults [baseminidx] + refdiff;

					idx++;
				}

				int count = rawResults.Length + 1; //+ 1 for bass;
				results = new double[count];

				double[] bassDiff = this.m_results.Last().Delays;
				double bass = bassDiff.Last();
				if (bass > 0) {
					results [count - 1] = bass;
				}

				idx = 0;
				double diff = Math.Max(bassDiff[0] - bassDiff[1], 0);
				while (idx < rawResults.Length) {
					results [idx] = rawResults [idx] + diff;
					idx++;
				}
			}

			return new SimpleDelayServiceResults (results ?? rawResults, allResultsValid);
		}

		IDelayService IWrapper<IDelayService>.Unwrapp()
		{ 
			return this.m_service;
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
				this.OnDetermineDelayStop(this, new DataEventArgs<IDelayServiceResults>(this.CreateDelayServiceResults()));
			}
		}
    }
}
