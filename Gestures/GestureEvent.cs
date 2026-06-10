namespace TrikiControl.Gestures;

public readonly record struct GestureEvent(
    GestureType Type,
    DateTimeOffset TimestampUtc,
    double Value = 0);