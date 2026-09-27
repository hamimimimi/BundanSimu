using UnityEngine;
using System.Collections.Generic;

public static class ElectionDistrictIssueGenerator
{
    public static void Generate(CountryConstantData data)
    {
        if (data.districts == null) return;

        foreach (var district in data.districts)
        {
            district.issues = new List<ElectionIssueData>();

           
            //  リーダー評価Issue
          
            district.issues.Add(new ElectionIssueData
            {
                issueId = $"leader_popularity_{district.districtId}",
                weight = Random.Range(0.3f, 0.8f)
            });

            
            // ② ランダムIssue追加
            
            int extraCount = Random.Range(1, 3);

            for (int i = 0; i < extraCount; i++)
            {
                string issueId = GetRandomIssue();

                district.issues.Add(new ElectionIssueData
                {
                    issueId = issueId,
                    weight = Random.Range(0.1f, 0.5f)
                });
            }
           
        }

        Debug.Log("選挙区Issue生成完了");
    }

    static string GetRandomIssue()
    {
        string[] pool =
        {
            "economy",
            "welfare",
            "security",
            "education"
        };

        return pool[Random.Range(0, pool.Length)];
    }
}