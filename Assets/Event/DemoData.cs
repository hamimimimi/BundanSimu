using System;

[Serializable]
public class DemoData
{
    public CountryVariable variable;

    public bool isCapital;
    public ElectionDistrictData district;

    public int remainingTurns;
}
class ScheduledDemo
{
    public CountryVariable variable;
    public int triggerTurn;
}