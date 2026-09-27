using UnityEngine;
using System.Collections.Generic;

public static class DistrictLeaderAssigner
{
    public static void Assign(CountryConstantData data)
    {
        if (data.districts == null || data.districts.Count == 0)
        {
            Debug.LogError("districts がない");
            return;
        }

        
        // 全政治家プール作成
        
        List<PoliticianData> pool = new();

        // 政党所属
        foreach (var party in data.politicalParties)
        {
            pool.AddRange(party.politicians);
        }

        // 無所属（適当に生成）
        int independentCount = Mathf.Max(1, data.districts.Count / 2);

        var independents =
            PoliticianGenerator.GenerateIndependents(independentCount);

        pool.AddRange(independents);

        // -------------------------
        // 各選挙区に割り当て
        // -------------------------
        foreach (var district in data.districts)
        {
            if (pool.Count == 0)
            {
                Debug.LogWarning("政治家不足 → 追加生成");

                pool.Add(
                    PoliticianGenerator.GenerateSingle(null)
                );
            }

            int index = Random.Range(0, pool.Count);
            var politician = pool[index];

            pool.RemoveAt(index);

            // ★ 双方向設定
            district.leader = politician;
            politician.districtId = district.districtId;
        }

        Debug.Log("選挙区リーダー割り当て完了");
    }
}