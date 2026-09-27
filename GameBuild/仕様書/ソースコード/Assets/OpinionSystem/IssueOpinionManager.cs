using System.Collections.Generic;
using UnityEngine;

public class IssueOpinionManager : MonoBehaviour
{
    public static IssueOpinionManager Instance;

    Dictionary<string, float> opinions = new();

    void Awake()
    {
        Instance = this;
    }

    public float GetOpinion(string issue)
    {
        if (opinions.ContainsKey(issue))
            return opinions[issue];

        return 0.5f;
    }

    public void ChangeOpinion(string issue, float delta)
    {
        if (!opinions.ContainsKey(issue))
            opinions[issue] = 0.5f;

        opinions[issue] += delta;

        opinions[issue] = Mathf.Clamp01(opinions[issue]);
    }
}