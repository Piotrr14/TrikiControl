namespace TrikiControl.Models
{

    public sealed class ImuStats
    {
        public long NotificationCount { get; private set; }
        public long ParsedFrameCount { get; private set; }
        public long DiscardedStartupSampleCount { get; private set; }
        public long WrittenSampleCount { get; private set; }
        public long DroppedByteCount { get; private set; }
        public double LastNotificationGapMilliseconds { get; private set; }
        public double MaxNotificationGapMilliseconds { get; private set; }
        private DateTimeOffset? _lastNotificationTimestampUtc;

        public void NotificationReceived() => NotificationReceived(DateTimeOffset.UtcNow);
        public void NotificationReceived(DateTimeOffset timestampUtc)
        {
            if (_lastNotificationTimestampUtc is not null)
            {
                LastNotificationGapMilliseconds = Math.Max(
                    0.0,
                    (timestampUtc - _lastNotificationTimestampUtc.Value).TotalMilliseconds);
                MaxNotificationGapMilliseconds = Math.Max(
                    MaxNotificationGapMilliseconds,
                    LastNotificationGapMilliseconds);
            }

            _lastNotificationTimestampUtc = timestampUtc;
            NotificationCount++;
        }
        public void FrameParsed() => ParsedFrameCount++;
        public void StartupSampleDiscarded() => DiscardedStartupSampleCount++;
        public void SetDroppedBytes(long droppedByteCount) => DroppedByteCount = droppedByteCount;
        public void SampleWritten(ImuSample sample)
        {
            WrittenSampleCount++;
        }

        public void CopyFrom(ImuStats stats)
        {
            ParsedFrameCount = stats.ParsedFrameCount;
            DiscardedStartupSampleCount = stats.DiscardedStartupSampleCount;
            WrittenSampleCount = stats.WrittenSampleCount;
        }
    }
}
