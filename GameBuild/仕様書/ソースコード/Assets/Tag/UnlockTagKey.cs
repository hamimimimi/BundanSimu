using System;
using UnityEngine;

[Serializable]
public struct UnlockTagKey : IEquatable<UnlockTagKey>
{
    public UnlockTag tag;
    public string key;

    public bool Equals(UnlockTagKey other)
    {
        return tag == other.tag && key == other.key;
    }

    public override bool Equals(object obj)
    {
        return obj is UnlockTagKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(tag, key);
    }

    public override string ToString()
    {
        return string.IsNullOrEmpty(key)
            ? tag.ToString()
            : $"{tag}:{key}";
    }

    public UnlockTagKey(UnlockTag tag, string key)
    {
        this.tag = tag;
        this.key = key;
    }
}
