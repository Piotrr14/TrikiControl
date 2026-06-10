namespace TrikiControl.Ble
{
    public readonly record struct BleNotification(byte[] Bytes, DateTimeOffset TimestampUtc);
}
