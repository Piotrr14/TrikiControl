using TrikiControl.Models;

namespace TrikiControl.Ble
{
    public sealed class ImuSampleProcessor
    {
        private readonly AppOptions _options;
        private readonly ImuStats _stats = new();
        private long _nextFrameIndex;

        public ImuSampleProcessor(AppOptions options)
        {
            _options = options;
        }

        public ImuStats Stats => _stats;

        public ImuSample? ProcessFrame(byte[] frame)
        {
            return ProcessFrame(frame, DateTimeOffset.UtcNow);
        }

        public ImuSample? ProcessFrame(byte[] frame, DateTimeOffset timestampUtc)
        {
            _stats.FrameParsed();

            if (_stats.DiscardedStartupSampleCount < _options.StartupDiscardSamples)
            {
                _stats.StartupSampleDiscarded();
                return null;
            }

            var sample = ImuSample.FromFrame(
                frame,
                _nextFrameIndex,
                _options.GyroScale,
                _options.AccelScale,
                timestampUtc);

            _nextFrameIndex++;
            _stats.SampleWritten(sample);
            return sample;
        }
    }
}
