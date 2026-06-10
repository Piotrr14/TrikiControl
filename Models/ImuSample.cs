using System.Buffers.Binary;

namespace TrikiControl.Models
{
    public readonly record struct ImuSample(
          long FrameIndex,
          DateTimeOffset TimestampUtc,
          double GyroX,
          double GyroY,
          double GyroZ,
          double AccelX,
          double AccelY,
          double AccelZ,
          short RawGyroX,
          short RawGyroY,
          short RawGyroZ,
          short RawAccelX,
          short RawAccelY,
          short RawAccelZ)
    {
        public static ImuSample FromFrame(byte[] frame, long frameIndex, double gyroScale, double accelScale)
        {
            return FromFrame(frame, frameIndex, gyroScale, accelScale, DateTimeOffset.UtcNow);
        }

        public static ImuSample FromFrame(
            byte[] frame,
            long frameIndex,
            double gyroScale,
            double accelScale,
            DateTimeOffset timestampUtc)
        {
            var rawGyroX = BinaryPrimitives.ReadInt16LittleEndian(frame.AsSpan(2, 2));
            var rawGyroY = BinaryPrimitives.ReadInt16LittleEndian(frame.AsSpan(4, 2));
            var rawGyroZ = BinaryPrimitives.ReadInt16LittleEndian(frame.AsSpan(6, 2));
            var rawAccelX = BinaryPrimitives.ReadInt16LittleEndian(frame.AsSpan(8, 2));
            var rawAccelY = BinaryPrimitives.ReadInt16LittleEndian(frame.AsSpan(10, 2));
            var rawAccelZ = BinaryPrimitives.ReadInt16LittleEndian(frame.AsSpan(12, 2));

            return new ImuSample(
                frameIndex,
                timestampUtc,
                rawGyroX / gyroScale,
                rawGyroY / gyroScale,
                rawGyroZ / gyroScale,
                rawAccelX / accelScale,
                rawAccelY / accelScale,
                rawAccelZ / accelScale,
                rawGyroX,
                rawGyroY,
                rawGyroZ,
                rawAccelX,
                rawAccelY,
                rawAccelZ);
        }
    }
}
