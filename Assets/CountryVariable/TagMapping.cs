using UnityEngine;

public static class TagMapping
{
    public static UnlockTag VariableToUnlock(DebateTag tag)
    {
        return tag switch
        {
            DebateTag.OptionA => UnlockTag.DebateEconomy,
            DebateTag.OptionB => UnlockTag.DebateMilitary,
            DebateTag.OptionC => UnlockTag.DebateEnvironment,
            _ => UnlockTag.None
        };
    }

    public static UnlockTagKey VariableToUnlockKey(DebateTag tag)
    {
        return new UnlockTagKey(
            VariableToUnlock(tag),
            tag.ToString()
        );
    }
}
