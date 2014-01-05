using System;
using System.Threading.Tasks;
using XConductor.Application.Shared.Service.EventArgs;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Application.Shared.Service
{
    internal class DelayServiceContext
    {
        private EventHandler<ProcessingEventArgs> m_sampleGenerationStopHandler = null;
        private EventHandler<MediaEventArgs> m_samplePlayingStartHandler = null;
        private EventHandler<MediaEventArgs> m_samplePlayingStopHandler = null;
        private EventHandler<MediaEventArgs> m_capturingStopHandler = null;
        private EventHandler<ProcessingEventArgs> m_renderStopHandler = null;

        private string m_sampleName;
        private string m_captureName;

        private Task m_start = null;

        private bool m_sampleGenerationStopHandlerCalled;
        private bool m_samplePlayingStartHandlerCalled;
        private bool m_samplePlayingStopHandlerCalled;
        private bool m_capturingStopHandlerCalled;
        private bool m_renderStopHandlerCalled;

        private object m_generatinKey;
        private object m_readingKey;

        public DelayServiceContext(string sampleName, string captureName, object readingKey, object generatinKey)
        {
            this.m_sampleName = sampleName;
            this.m_captureName = captureName;

            this.m_readingKey = readingKey;
            this.m_generatinKey = generatinKey;
        }

        public void InitStartSequence(Func<string, Task> start)
        {
            if (start != null && this.m_start == null)
            {
                this.m_start = start(this.m_sampleName);
            }
        }

        public void InitSampleGenerationStopHandler(Action<string> startPlaying)
        {
            if (startPlaying != null && this.m_sampleGenerationStopHandler == null)
            {
                this.m_sampleGenerationStopHandler = (s, e) =>
                {
                    if (e.ChainKey == this.m_generatinKey && !this.m_sampleGenerationStopHandlerCalled)
                    {
                        this.m_sampleGenerationStopHandlerCalled = true;
                        startPlaying(this.m_sampleName);
                    }
                };
            }
        }

        public void InitSamplePlayingStartHandler(Action<string> startCapturing)
        {
            if (startCapturing != null && this.m_samplePlayingStartHandler == null)
            {
                this.m_samplePlayingStartHandler = (s, e) =>
                {
                    if (!this.m_samplePlayingStartHandlerCalled)
                    {
                        this.m_samplePlayingStartHandlerCalled = true;
                        startCapturing(this.m_captureName);
                    }
                };
            }
        }

        public void InitSamplePlayingStopHandler(Action stopCapturing)
        {
            if (stopCapturing != null && this.m_samplePlayingStopHandler == null)
            {
                this.m_samplePlayingStopHandler = (s, e) =>
                {
                    if (!this.m_samplePlayingStopHandlerCalled)
                    {
                        this.m_samplePlayingStopHandlerCalled = true;
                        stopCapturing();
                    }
                };
            }
        }

        public void InitCapturingStopHandler(Action<string> startAnalizing)
        {
			if (startAnalizing != null && this.m_capturingStopHandler == null)
            {
                this.m_capturingStopHandler = (s, e) =>
                {
                    if (!this.m_capturingStopHandlerCalled)
                    {
                        this.m_capturingStopHandlerCalled = true;
						startAnalizing(this.m_captureName);
                    }
                };
            }
        }

        public void InitRenderingStopHandler(Action resultsHandler)
        {
            if (resultsHandler != null && this.m_renderStopHandler == null)
            {
                this.m_renderStopHandler = (s, e) =>
                {
                    if (e.ChainKey == this.m_readingKey && !this.m_renderStopHandlerCalled)
                    {
                        this.m_renderStopHandlerCalled = true;
                        resultsHandler();
                    }
                };
            }
        }

        public EventHandler<ProcessingEventArgs> SampleGeneratingStopHandler { get { return this.m_sampleGenerationStopHandler; } }

        public EventHandler<MediaEventArgs> SamplePlayingStartHandler { get { return this.m_samplePlayingStartHandler; } }

        public EventHandler<MediaEventArgs> SamplePlayingStopHandler { get { return this.m_samplePlayingStopHandler; } }

        public EventHandler<MediaEventArgs> CapturingStopHandler { get { return this.m_capturingStopHandler; } }

        public EventHandler<ProcessingEventArgs> RenderingStopHandler { get { return this.m_renderStopHandler; } }

        public Task BeginOfSequence { get { return this.m_start; } }

        public override bool Equals(object obj)
        {
            var o = obj as DelayServiceContext;
            return o != null && o.m_sampleName == this.m_sampleName && o.m_captureName == this.m_captureName;
        }

        public override int GetHashCode()
        {
            return (this.m_sampleName ?? string.Empty).GetHashCode() + (this.m_captureName ?? string.Empty).GetHashCode();
        }
    }
}
