namespace TrikiControl.Models;

public sealed record AppOptions(
    string DeviceName,
    double GyroScale,
    double AccelScale,
    byte[] StartCommand,
    int StartupDiscardSamples,
    int ScanTimeoutSeconds,
    int SettleDelaySeconds)
{
    public static AppOptions Default => new(
        "Triki",
        131.0,
        2048.0,
        Convert.FromHexString("201000D007680003"),
        20,
        30,
        3);
}