using UnityEngine;
using System.Collections.Generic;


// 他国の地図生成。
public class ForeignCountryMapGenerator : MonoBehaviour
{
    static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };


    // 起点。
    public static void Generate(CountryConstantData data)
    {
        foreach (var relation in data.foreignRelations)
        {
            // 枠外の場合は描画しない 
            if (relation.mapDistance == CountryDistance.OutOfRange)
                continue;

            // 開始点候補決定
            List<Vector2Int> candidates = ForeignCountryPositionGenerator.GetStartPositions(
                data,
                relation); 


            if (candidates.Count == 0)
            {
              Debug.Log(
                   $"{relation.countryName}: 開始地点が見つかりませんでした。"); 
                continue;
            }

            Vector2Int start = Vector2Int.zero;
            bool found = false;

            foreach (Vector2Int candidate in candidates)
            {
                if (!data.worldMapTiles.ContainsKey(candidate))
                {
                    start = candidate;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.Log(
                    $"{relation.countryName}: " +
                    $"使用可能な開始地点がありません。");
                continue;
            }

            // 開始地点保存
            relation.startPosition = start;

            // この１国だけで使った座標
            HashSet<Vector2Int> used =
                   new HashSet<Vector2Int>();


            // 開始地点から国を生成
            ExpandCountry(data, relation, start, used);
        }
    }


    // 国をを拡張
    private static void ExpandCountry(
            CountryConstantData data,
            ForeignCountryRelationData relation,
            Vector2Int start,
            HashSet<Vector2Int> used)
    {

        int total = relation.landCount;
        
        if (total <= 0)
            return;
        
        List<Vector2Int> frontier =
           new List<Vector2Int>();

        // 開始地点
        if (!data.worldMapTiles.ContainsKey(start))
        {
            AddTile(
                data,
                start,
                relation);

            used.Add(start);
            total--;
        }


        // 開始地点の周囲を候補に追加
         AddFrontier(
          data,
          start,
          frontier,
          used);

        Vector2Int lastPosition = start;

        while (total > 0)
        {
            if (frontier.Count > 0)
            {
                Vector2Int pos = SelectNextPosition(frontier, lastPosition);

                // 既に存在する場所なら飛ばす
                if (data.worldMapTiles.ContainsKey(pos))
                    continue;

                if (used.Contains(pos))
                    continue;

                used.Add(pos);

                // タイルを塗る
                AddTile(
                     data,
                     pos,
                     relation);

                total--;

                // 今回置いたタイルを次の基準にする。
                lastPosition = pos;


                // 新しい候補を追加する
                AddFrontier(
                    data,
                    pos,
                    frontier,
                    used);

                continue;
            }
            // 拡張できない場合
            if (!TryFindRestartPosition(
        data,
        used,
        lastPosition,
        out Vector2Int restartPosition))
            {
                Debug.Log(
                    $"{relation.countryName}: " +
                    "これ以上配置できる場所がありません。");

                break;
            }

            // 新しい開始地点
            AddTile(
                data,
                restartPosition,
                relation);

            used.Add(restartPosition);

            total--;

            lastPosition = restartPosition;

            AddFrontier(
                data,
                restartPosition,
                frontier,
                used);
        

    }

        Debug.Log(
    $"{relation.countryName} : " +
    $"予定={relation.landCount}, " +
    $"残り={total}, " +
    $"Frontier={frontier.Count}"
);

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

// 近い空きタイルを取得。
private static bool TryFindRestartPosition(
    CountryConstantData data,
    HashSet<Vector2Int> used,
    Vector2Int currentPosition,
    out Vector2Int restartPosition)
    {
        int searchRange = 50;

        for (int distance = 1;
             distance <= searchRange;
             distance++)
        {
            for (int x = -distance; x <= distance; x++)
            {
                for (int y = -distance; y <= distance; y++)
                {
                    Vector2Int candidate =
                        currentPosition +
                        new Vector2Int(x, y);

                    if (data.worldMapTiles.ContainsKey(candidate))
                        continue;

                    if (used.Contains(candidate))
                        continue;

                    restartPosition = candidate;
                    return true;
                }
            }
        }

        restartPosition = Vector2Int.zero;
        return false;
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


    // Tileを作る
    private static void AddTile(
    CountryConstantData data,
    Vector2Int pos,
    ForeignCountryRelationData relation)
    {
        MapTile tile =
            new MapTile();

        tile.position = pos;

        tile.countryName =
            relation.countryName;

        // 他国では使わないので仮の値
        tile.districtId = null;
        tile.type = AreaType.Empty;

        data.worldMapTiles[pos] = tile;
    }


}
