using UnityEngine;

public static class DebatePointRule
{
    public static int ClampDebatePoint(CountryVariable variable)
    {
        if (CountryConstantManager.Instance == null)
            return variable.debatePoint;

        int baseCap =
            CountryConstantManager.Instance.GetCapValue(variable.capSource);

        int max = Mathf.FloorToInt(baseCap * variable.capRatio);
        return Mathf.Min(variable.debatePoint, max);
    }

}
