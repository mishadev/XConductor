using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using XConductor.Application.Shared.Service.Abstractions;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Capturing;
using XConductor.Domain.Seedwork.Generating;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Domain.Seedwork.Media.Reader;
using XConductor.Domain.Seedwork.Playback;
using XConductor.Domain.Seedwork.Extensions;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Application.Shared.Service
{
    public class AudioService : IAudioService
    {
        public event EventHandler<MediaEventArgs> OnRecordingStart;
        public event EventHandler<MediaEventArgs> OnRecordingStop;

        public event EventHandler<MediaEventArgs> OnPlayingStart;
        public event EventHandler<MediaEventArgs> OnPlayingStop;

        private ICaptureSession m_capture;
        private IAudioPlayer m_player;

        public AudioService(ICaptureSession capture, IAudioPlayer player)
        {
            this.m_capture = capture;
            this.m_player = player;
        }

        public async Task StartRecording(string sourcePath)
        {
            if (this.HasCapture)
            {
                await this.m_capture.Initialize(sourcePath);

                this.m_capture.SubscribeService(this.RecordingStart, this.RecordingStop);

                await this.m_capture.Start();
            }
        }

        public async Task StopRecording()
        {
            if (this.HasCapture)
            {
                await this.m_capture.Stop();
            }
        }

        public async Task StartPlaying(string sourcePath)
        {
            if (this.HasPlayer)
            {
				await this.m_player.Initialize(sourcePath);

                this.m_player.SubscribeService(this.PlayingStart, this.PlayingStop);

				await this.m_player.Start();
            }
        }

		public async Task PausePlaying()
        {
            if (this.HasPlayer)
            {
				await this.m_player.Pause();
            }
        }

		public async Task StopPlaying()
        {
            if (this.m_player != null)
            {
				await this.m_player.Stop();
            }
        }

        public bool HasCapture
        {
            get
            {
                return this.m_capture != null && this.m_capture.SettingsService != null;
            }
        }

        public bool HasPlayer
        {
            get
            {
                return this.m_player != null && this.m_player.SettingsService != null;
            }
        }

        public bool IsInitialized
        {
            get
            {
                return this.HasCapture && this.HasPlayer;
            }
        }

        public void Dispose()
        {
			if (this.m_capture != null) this.m_capture.Dispose();
			if (this.m_player != null) this.m_player.Dispose();

            this.OnPlayingStart = null;
            this.OnPlayingStop = null;
            this.OnRecordingStop = null;
            this.OnRecordingStart = null;
        }

        private void PlayingStart(object sender, MediaEventArgs e)
        {
            if (this.OnPlayingStart != null)
            {
                this.OnPlayingStart(this, e);
            }
        }

        private void PlayingStop(object sender, MediaEventArgs e)
        {
            if (this.OnPlayingStop != null)
            {
                this.OnPlayingStop(this, e);
            }
        }

        private void RecordingStop(object sender, MediaEventArgs e)
        {
            if (this.OnRecordingStop != null)
            {
                this.OnRecordingStop(this, e);
            }
        }

        private void RecordingStart(object sender, MediaEventArgs e)
        {
            if (this.OnRecordingStart != null)
            {
                this.OnRecordingStart(this, e);
            }
        }
    }
}
