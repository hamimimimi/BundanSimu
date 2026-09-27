using UnityEngine;
using System.Collections.Generic;

// 地図生成用

public class FrontierData
    {
        public Vector2Int start;

        public List<Vector2Int> frontier =
            new List<Vector2Int>();
    }


public class LandMapGenerator : MonoBehaviour
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

        HashSet<Vector2Int> used = new HashSet<Vector2Int>();

        List<Vector2Int> candidates =
           CreateStartPoints(data.sizeCategory);
 
        // VerySmollを例外処理。
        if (data.sizeCategory == PopulationSize.VerySmall)
        {
            List<Vector2Int> captalstarts =
                new()
                {
                    Vector2Int.zero
                };

            var captalfrontiers =
                CreateFrontiers(captalstarts, used);

            ExpandDistrict(
                data,
                data.districts[0],
                captalfrontiers[0],
                used);

            return;
        }


        List<Vector2Int> starts =
            SelectStartPoints(data, candidates);



        //首都を設置
        AddTile(
                data,
                Vector2Int.zero,
                AreaType.Capital,
                data.districts[0]);

            used.Add(Vector2Int.zero);


        List<FrontierData> frontiers =
            CreateFrontiers(starts, used);
   


        // 周辺を候補
        for (int i = 0; i < data.districts.Count; i++)
        {
            ExpandDistrict(data, data.districts[i], frontiers[i % frontiers.Count], used);
        }

        // 自国の地図を世界地図へ登録
        foreach (var pair in data.mapTiles)
        {
            data.worldMapTiles[pair.Key] = pair.Value;
        }

    }

    // 開始点の個数決定。（国のサイズにより決定）
    private static int GetStartPointCount(PopulationSize size)
    {
        switch (size)
        {
            case PopulationSize.VerySmall:
                return 1;

            case PopulationSize.Small:
                return 9;

            case PopulationSize.Medium:
                return 16;

            case PopulationSize.Large:
                return 25;

            case PopulationSize.Huge:
                return 36;
        }

        return 1;
    }


    // 開始点設定
    private static List<Vector2Int> CreateStartPoints(
    PopulationSize size)
    {
        List<Vector2Int> result = new();

        int count = 
            GetStartPointCount(size);

        int width = 
            Mathf.RoundToInt(Mathf.Sqrt(count));

        int spacing = 3; // 開始点の間隔（結果を見て調整）

        int start =
            -(width - 1) * spacing / 2;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result.Add(
                    new Vector2Int(
                        start + x * spacing,
                        start + y * spacing
                    ));
            }
        }

        return result;
    }




    // 開始点決定
    public static List<Vector2Int>  SelectStartPoints(
        CountryConstantData data,
        List<Vector2Int> candidates)
    {
        List<Vector2Int> result = 
            new　List<Vector2Int>();

        int districtCount = 
            data.districts.Count;

        int useCount = 
            Mathf.Min(districtCount, candidates.Count);

        for (int i = 0; i < useCount; i++)
        {
            result.Add(candidates[i]);

            data.districts[i].startPosition = candidates[i];
        }



        return result;
    }



    // 探索開始地点（List）を作る。
    public static List<FrontierData> CreateFrontiers(
        List<Vector2Int> starts, HashSet<Vector2Int> used)
    {
        List<FrontierData> result = 
            new List<FrontierData>();


        foreach (var start in starts)
        {
            FrontierData frontier =
                new FrontierData();

            frontier.start = start;

            used.Add(start);
            
           
            foreach (var dir in directions)
            {
                Vector2Int next =
                     start + dir;
                
                if (!used.Contains(next) &&
                     !frontier.frontier.Contains(next))
                {
                    frontier.frontier.Add(next);
                }

            }

            result.Add(frontier);
        }

        return result;
    }


    // 地区を拡張
    static void ExpandDistrict(
            CountryConstantData data,
    　　　　ElectionDistrictData district,
            FrontierData frontierdata,
     　　　 HashSet<Vector2Int> used)
        {

               int total =
                   district.cityCount +
                   district.ruralCount +
                   district.emptyCount;


                int city =
                     district.cityCount;

                int rural = 
                     district.ruralCount;
        
                int empty = 
                     district.emptyCount;

        Vector2Int start = 
            frontierdata.start;

        List<Vector2Int> frontier =
            frontierdata.frontier;

        // 開始地点をまだ塗ってなければ塗る
        if (!data.mapTiles.ContainsKey(start))
        {
            PaintTile(
                data,
                district,
                start,
                ref city,
                ref rural,
                ref empty);

             total--;
        }

        Vector2Int lastPosition = 
            start;

            while (total > 0 && frontier.Count > 0)
            {
               // 次に塗る場所を選択
                Vector2Int pos = SelectNextPosition(frontier, lastPosition);


                if (used.Contains(pos))
                    continue;

                used.Add(pos);

            // タイルを塗る
            PaintTile(
                 data,
                 district,
                 pos,
                 ref city,
                 ref rural,
                 ref empty);

            // 今回使った場所を次回に伸びやすくする。
            lastPosition = pos;


            // 新しい候補を追加する
            foreach (var dir in directions)
                {
                    Vector2Int next = pos + dir;

                    if (!used.Contains(next) &&
                         !frontier.Contains(next))
                    {
                        frontier.Add(next);
                    }
                }
                total--;
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




    // Tileを作る
    private static void AddTile(
    CountryConstantData data,
    Vector2Int pos,
    AreaType type,
    ElectionDistrictData district)
    {
        MapTile tile =
            new MapTile();

        tile.position = pos;

        tile.type = type;

        tile.districtId = district.districtId;

        tile.countryName = data.countryName;

        data.mapTiles[pos] = tile;
    }

    // 1マス塗る
    private static void PaintTile(
    CountryConstantData data,
    ElectionDistrictData district,
    Vector2Int pos,
    ref int city,
    ref int rural,
    ref int empty)
    {
        if (city > 0)
        {
            district.cityTiles.Add(pos);

            AddTile(
                data,
                pos,
                AreaType.City,
                district);

            city--;
        }
        else if (rural > 0)
        {
            district.ruralTiles.Add(pos);

            AddTile(
                data,
                pos,
                AreaType.Rural,
                district);

            rural--;
        }
        else
        {
            district.emptyTiles.Add(pos);

            AddTile(
                data,
                pos,
                AreaType.Empty,
                district);

            empty--;
        }
    }


}



