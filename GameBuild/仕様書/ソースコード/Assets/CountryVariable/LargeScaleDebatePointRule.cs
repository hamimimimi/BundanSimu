using UnityEngine;

public static class LargeScaleDebatePointRule
{
    public static int ClampDebatePoint(int currentValue)
    {
        if (CountryConstantManager.Instance == null)
            return currentValue;

        // いまは人口ベース（将来拡張）
        int max = CountryConstantManager.Instance.GetLargeScaleMaxDebatePoint();
        return Mathf.Min(currentValue, max);
    }
}
