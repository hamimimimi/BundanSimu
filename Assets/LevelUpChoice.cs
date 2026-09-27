using UnityEngine;


public class LevelUpChoice 
{
    public StatusTagType plusType;
    public int plusAmount;


    public StatusTagType minusType;
    public int minusAmount;

    public string title;

    public string description;

    public LevelUpChoice(
    string title,
    string description,
    StatusTagType plusType,
    int plusAmount,
    StatusTagType minusType = StatusTagType.InfluencePoint,
    int minusAmount = 0)
    {
        this.title = title;
        this.description = description;
        this.plusType = plusType;
        this.plusAmount = plusAmount;
        this.minusType = minusType;
        this.minusAmount = minusAmount;
    }
}
