namespace TrikiControl.Ble
{
    public sealed class FrameParser
    {
        private const int FrameLength = 14;
        private readonly List<byte> _buffer = new();

        public long DroppedByteCount { get; private set; }

        public IEnumerable<byte[]> Push(byte[] bytes)
        {
            _buffer.AddRange(bytes);

            while (true)
            {
                var headerIndex = FindHeader();
                if (headerIndex < 0)
                {
                    if (_buffer.Count > 0)
                    {
                        var keepTrailingHeaderByte = _buffer[^1] == 0x22;
                        var dropCount = keepTrailingHeaderByte ? _buffer.Count - 1 : _buffer.Count;
                        DroppedByteCount += dropCount;

                        if (keepTrailingHeaderByte)
                        {
                            var trailingByte = _buffer[^1];
                            _buffer.Clear();
                            _buffer.Add(trailingByte);
                        }
                        else
                        {
                            _buffer.Clear();
                        }
                    }
                    yield break;
                }

                if (headerIndex > 0)
                {
                    DroppedByteCount += headerIndex;
                    _buffer.RemoveRange(0, headerIndex);
                }

                if (_buffer.Count < FrameLength)
                {
                    yield break;
                }

                var frame = _buffer.GetRange(0, FrameLength).ToArray();
                _buffer.RemoveRange(0, FrameLength);
                yield return frame;
            }
        }

        private int FindHeader()
        {
            for (var i = 0; i < _buffer.Count - 1; i++)
            {
                if (_buffer[i] == 0x22 && _buffer[i + 1] == 0x00)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
