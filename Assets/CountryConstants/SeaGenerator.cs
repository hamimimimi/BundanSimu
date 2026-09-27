using UnityEngine;
using System.Collections.Generic;

// 海生成
public static class SeaGenerator 
{
    static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public static void Generate(CountryConstantData data)
    {
        int seaCount =
            Random.Range(5, 11);

        for (int i = 0; i < seaCount; i++)
        {
            int seaSize =
                Random.Range(10, 201);

            GenerateSea(data, seaSize);
        }
    }

    private static Vector2Int GetRandomStartPosition(
    CountryConstantData data)
    {
        int x = Random.Range(-60, 61);
        int y = Random.Range(-34, 35);

        return new Vector2Int(x, y);
    }


    private static void AddSeaTile(
    CountryConstantData data,
    Vector2Int pos)
    {
        MapTile tile = new MapTile();

        tile.position = pos;
        tile.countryName = null;
        tile.districtId = null;
        tile.type = AreaType.Sea;

        data.worldMapTiles[pos] = tile;
    }


    private static void GenerateSea(
        CountryConstantData data,
        int seaSize)
    {
        Vector2Int start;

        do
        {
            start = GetRandomStartPosition(data);
        }
        while (data.worldMapTiles.ContainsKey(start));

        HashSet<Vector2Int> used =
            new HashSet<Vector2Int>();

        List<Vector2Int> frontier =
            new List<Vector2Int>();

        AddSeaTile(
            data,
            start);

        used.Add(start);

        seaSize--;

        AddFrontier(
            data,
            start,
            frontier,
            used);

        Vector2Int lastPosition = start;

        while (seaSize > 0)
        {
            if (frontier.Count > 0)
            {
                break;
            }

            Vector2Int pos =
                SelectNextPosition(
                    frontier,
                    lastPosition);

            if (data.worldMapTiles.ContainsKey(pos))
                continue;

            if (used.Contains(pos))
                continue;

            used.Add(pos);

            AddSeaTile(
                data,
                pos);

            seaSize--;

            lastPosition = pos;

            AddFrontier(
                data,
                pos,
                frontier,
                used);

        }
    }

    private static void AddFrontier(
    CountryConstantData data,
    Vector2Int position,
    List<Vector2Int> frontier,
    HashSet<Vector2Int> used)
    {
        foreach (var dir in directions)
        {
            Vector2Int next =
                position + dir;

            if (used.Contains(next))
                continue;

            if (data.worldMapTiles.ContainsKey(next))
                continue;

            if (frontier.Contains(next))
                continue;

            frontier.Add(next);
        }
    }

    // 次に塗るタイルを選択
    private static Vector2Int SelectNextPosition(
    List<Vector2Int> frontier,
    Vector2Int lastPosition)
    {
        // 最後に塗ったタイルの隣にある候補を探す
        List<Vector2Int> nearCandidates =
            new List<Vector2Int>();

        foreach (Vector2Int pos in frontier)
        {
            foreach (Vector2Int dir in directions)
            {
                if (pos == lastPosition + dir)
                {
                    nearCandidates.Add(pos);
                    break;
                }
            }
        }

        // 70%の確率で、最後に塗ったタイルの隣から選ぶ
        if (nearCandidates.Count > 0 &&
            Random.value < 0.7f)
        {
            int index =
                Random.Range(0, nearCandidates.Count);

            Vector2Int result =
                nearCandidates[index];

            frontier.Remove(result);

            return result;
        }

        // 30%はFrontier全体からランダム
        int randomIndex =
            Random.Range(0, frontier.Count);

        Vector2Int randomResult =
            frontier[randomIndex];

        frontier.RemoveAt(randomIndex);

        return randomResult;
    }
}
