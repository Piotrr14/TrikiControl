using TrikiControl.Models;

namespace TrikiControl.Ble
{
    public sealed class BleNotificationProcessor
    {
        private readonly FrameParser _parser = new();
        private readonly ImuSampleProcessor _processor;
        private readonly ImuStats _stats;

        public BleNotificationProcessor(AppOptions options, ImuStats stats)
        {
            _processor = new ImuSampleProcessor(options);
            _stats = stats;
        }

        public IReadOnlyList<ImuSample> Process(byte[] bytes, DateTimeOffset notificationTimestampUtc)
        {
            _stats.NotificationReceived(notificationTimestampUtc);
            var samples = new List<ImuSample>();

            foreach (var frame in _parser.Push(bytes))
            {
                var sample = _processor.ProcessFrame(frame, notificationTimestampUtc);

                if (sample is null)
                {
                    continue;
                }

                _stats.CopyFrom(_processor.Stats);
                _stats.SetDroppedBytes(_parser.DroppedByteCount);
                samples.Add(sample.Value);
            }

            _stats.CopyFrom(_processor.Stats);
            _stats.SetDroppedBytes(_parser.DroppedByteCount);
            return samples;
        }
    }
}
