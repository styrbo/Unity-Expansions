using System;

[AttributeUsage(AttributeTargets.Field)]
public class NullError : Attribute
{
#if UNITY_EDITOR
    public WarningLevel Level;
#endif

    public NullError(WarningLevel level)
    {
#if UNITY_EDITOR
        Level = level;
#endif
    }
}
