using TrikiControl.Actions;
using TrikiControl.Gestures;

namespace TrikiControl.Settings;

public sealed class AppSettings
{
    public bool AutoConnect { get; set; } = true;
    public int RotationThreshold { get; set; } = 25;
    public int VolumeStepMultiplier { get; set; } = 1;

    public Dictionary<GestureType, ActionType> Mappings { get; set; } = new()
    {
        [GestureType.RotateClockwise] = ActionType.VolumeUp,
        [GestureType.RotateCounterClockwise] = ActionType.VolumeDown,
        [GestureType.Shake] = ActionType.Mute,
        [GestureType.FaceDown] = ActionType.PlayPause,
    };
}