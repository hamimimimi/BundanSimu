using UnityEngine;
using System.Collections.Generic;

// 他国Map配置用開始点決定。各Listを返す。
public static class ForeignCountryPositionGenerator
{
    static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public static List<Vector2Int> GetStartPositions(
        CountryConstantData data,
        ForeignCountryRelationData relation)
    {
        List<Vector2Int> candidates =
            GetBorderCandidates(data);

        if (candidates.Count == 0)
        {
            return new List<Vector2Int>();
        }

        // 隣接
        if (relation.mapDistance == CountryDistance.Adjacent)
        {
            Shuffle(candidates);
            return candidates;
        }

        // 枠内
        if (relation.mapDistance == CountryDistance.InRange)
        {
            return GetFarPositions(
                data,
                candidates,
                10);
        }

        // 一部
        if (relation.mapDistance == CountryDistance.Partial)
        {
            return GetFarPositions(
                data,
                candidates,
                20);
        }

        // 枠外
        return new List<Vector2Int>();
    }


    // 隣接地点リストを作成
    private static List<Vector2Int> GetBorderCandidates(
        CountryConstantData data)
    {
        List<Vector2Int> result = new();

        foreach (var tile in data.mapTiles.Values)
        {
            Vector2Int pos = tile.position;

            foreach (var dir in directions)
            {
                Vector2Int next =
                    pos + dir;

                if (!data.mapTiles.ContainsKey(next) &&
                    !data.worldMapTiles.ContainsKey(next))
                {
                    if (!result.Contains(next))
                    {
                        result.Add(next);
                    }
                }
            }
        }
        Shuffle(result);

        return result;
    }

    // 距離計算。マンハッタン距離。
    private static int Distance(
    Vector2Int a,
    Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x)
             + Mathf.Abs(a.y - b.y);
    }

    private static int GetDistanceFromBorder(
    Vector2Int position,
    List<Vector2Int> borderCandidates)
    {
        int minDistance = int.MaxValue;

        foreach (Vector2Int border in borderCandidates)
        {
            int distance = Distance(position, border);

            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }
    // 離れた場所を候補にする場合
    private static List<Vector2Int> GetFarPositions(
    CountryConstantData data,
    List<Vector2Int> borderCandidates,
    int minDistance)
    {
        List<Vector2Int> candidates = new();

        foreach (Vector2Int pos in borderCandidates)
        {
            if (data.worldMapTiles.ContainsKey(pos))
                continue;

            int distance = GetDistanceFromBorder(pos, borderCandidates);


            if (distance >= minDistance)
            {
                candidates.Add(pos);
            }
        }

        if (candidates.Count == 0)
        {
            candidates = new List<Vector2Int>(borderCandidates);
        }

        Shuffle(candidates);

        return candidates;    
    }



    private static void Shuffle(List<Vector2Int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            Vector2Int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

}
