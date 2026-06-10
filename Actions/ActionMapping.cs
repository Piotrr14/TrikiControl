using TrikiControl.Gestures;

namespace TrikiControl.Actions;

public sealed record ActionMapping(
    GestureType Gesture,
    ActionType Action,
    string? Value = null);