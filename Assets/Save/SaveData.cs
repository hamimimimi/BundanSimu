using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int money;
    public int instability;
    public int currentTurn;
     public float playerPoint;   
    public List<MyAccData> myAccs;
    public List<ItemSaveData> items;

    // CountryVariable系（重要）
    public List<VariableSaveData> variables;

    public string saveTime;

    public List<UnlockProgressSaveData> unlockProgress;
    public List<UnlockTagSaveData> unlockedTags;
 
    public DemoSaveData demo;
}

[System.Serializable]
public class ItemSaveData
{
    public string tag;
    public string key;
    public int amount;
}

[System.Serializable]
public class VariableSaveData
{  
    public string tag;
    public string key;
    public int debatePoint;
    public int debateLevel;
}

[System.Serializable]
public class UnlockProgressSaveData
{
    public UnlockTag tag;
    public string key;
    public float progress;
}

[System.Serializable]
public class UnlockTagSaveData
{
    public UnlockTag tag;
    public List<string> keys;
}

[System.Serializable]
public class ScheduledDemoSaveData
{
    public UnlockTagKey variableKey;
    public int triggerTurn;
}
[System.Serializable]
public class ActiveDemoSaveData
{
    public UnlockTagKey variableKey;
    public bool isCapital;
    public string districtId;
    public int remainingTurns;
}

[System.Serializable]
public class DemoSaveData
{
    public List<ScheduledDemoSaveData> scheduled = new();
    public List<ActiveDemoSaveData> active = new();
}