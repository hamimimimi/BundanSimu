using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


// 地図表示用
public class MapTileView : MonoBehaviour
{


    public static MapTileView Instance;   
   
    private void Awake()
    {
        Instance = this;
    }
    
    [SerializeField]
    Transform mapRoot;

    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    int tileSize = 16;

    [SerializeField]
    Color seaColor = new Color(0.4f, 0.75f, 1.0f);


    // タイルとタイルUIを対応させる。
    private Dictionary<Vector2Int, MapTileUI> tileViews
    = new Dictionary<Vector2Int, MapTileUI>();

    [SerializeField]
    GameObject districtLabelPrefab;

    [SerializeField]
    GameObject countryLabelPrefab;

    [SerializeField]
    Transform labelRoot;

        // 他国の色リスト
    private Dictionary<string, Color> countryColors =
    new Dictionary<string, Color>();

    private readonly List<Color> foreignColors =
    new List<Color>
    {
        Color.green,
        Color.yellow,
        new Color(1f, 0.4f, 0.7f),
        new Color(1f, 0.5f, 0f)
    };


    // 他国用
    private Dictionary<string, GameObject> countryLabels =
    new Dictionary<string, GameObject>();




    private void OnEnable()
    {
        if (CountryConstantManager.Instance != null)
        {
            CountryConstantManager.Instance.OnDataReady += Draw;
        }


    }


    private void OnDisable()
    {
        if (CountryConstantManager.Instance != null)
        {
            CountryConstantManager.Instance.OnDataReady -= Draw;
        }
    }

    // 


    // 地図生成本体。
    public void Draw()
    {
        if (CountryConstantManager.Instance == null)
            return;

        if (CountryConstantManager.Instance.Data == null)
            return;



        CountryConstantData data =
   　　　　 CountryConstantManager.Instance.Data;


        if (data.worldMapTiles == null)
            return;

        ClearMap();

        countryColors.Clear();
        
        // タイルの描画
        DrawTiles(data);

        // 選挙区名の描画
        DrawDistrictLabels(data);        
        
        // 国名の描画
        DrawCountryLabel(data);

        // 境界線を描画
        BorderGenerate(data);
    }


    // タイルの描画
    private void DrawTiles(CountryConstantData data)
    {
        foreach (var tile in data.worldMapTiles.Values)
        {
            GameObject obj =
                Instantiate(tilePrefab, mapRoot);


            MapTileUI tileUI =
                obj.GetComponent<MapTileUI>();

            if (tileUI == null)
            {
                Debug.LogError(
                    "TilePrefabにMapTileUIがありません。"
                );

                Destroy(obj);
                continue;
            }

            RectTransform rt =
                obj.GetComponent<RectTransform>();

            rt.anchoredPosition =
                tile.position * tileSize;

            // タイル境界を非表示。
            tileUI.borderTop.enabled = false;
            tileUI.borderBottom.enabled = false;
            tileUI.borderLeft.enabled = false;
            tileUI.borderRight.enabled = false;

            // 海
            if (tile.isSea)
            {
               tileUI.ground.color = seaColor;
            }
            // 自国
            else if (tile.countryName == data.countryName)
            {
                DrawOwnTile(tileUI, tile);
            }
            // 他国
            else
            {
                DrawForeignTile(tileUI, tile);
            }

            tileViews[tile.position] = tileUI;

        }
    }

    // 自国タイル描画
    private void DrawOwnTile(MapTileUI tileUI, MapTile tile)
    {
        // 首都の色を赤に
            if (tile.type == AreaType.Capital)
            {
                tileUI.ground.color = Color.red;
            }
            else
            {
        // 通常の処理
                tileUI.ground.color =
                    Color.white;
            }
    }

    // 他国タイル描画
    private void DrawForeignTile(MapTileUI tileUI, MapTile tile)
    {
        if (!countryColors.ContainsKey(tile.countryName))
        {
            int index =
                countryColors.Count %
                foreignColors.Count;

            countryColors.Add(
                tile.countryName,
                foreignColors[index]);
        }

        tileUI.ground.color =
            countryColors[tile.countryName];
    }


    // 境界線描画
    private void BorderGenerate(CountryConstantData data)
    {
        var map = data.worldMapTiles;

        foreach (var tile in map.Values)
        {

            if (!tileViews.TryGetValue(
                 tile.position,
                 out MapTileUI tileUI))
            {
                continue;
            }

            Vector2Int upPos =
                 tile.position + Vector2Int.up;
            Vector2Int downPos =
                 tile.position + Vector2Int.down;
            Vector2Int leftPos =
                 tile.position + Vector2Int.left;
            Vector2Int rightPos =
                 tile.position + Vector2Int.right;


            // 上
            if (!map.TryGetValue(upPos,out MapTile upTile))
            {
                // 海やマップ外との境界
                tileUI.borderTop.enabled = true;
            }
            else if (upTile.countryName != tile.countryName)
            {
                // 国境
                tileUI.borderTop.enabled = true;
            }
            else if (tile.countryName == data.countryName &&
                upTile.districtId != tile.districtId)
            {
                // 自国内のdistrict境界
                tileUI.borderTop.enabled = true;
            }



            // 下
            if (!map.TryGetValue(
                downPos,
                out MapTile downTile))
            {
                tileUI.borderBottom.enabled = true;
            }
            else if (downTile.countryName != tile.countryName)
            {
                tileUI.borderBottom.enabled = true;
            }
            else if (tile.countryName == data.countryName &&
   　downTile.districtId != tile.districtId)
            {
                // 自国内のdistrict境界
                tileUI.borderBottom.enabled = true;
            }

            // 左
            if (!map.TryGetValue(
                leftPos,
                out MapTile leftTile))
            {
                tileUI.borderLeft.enabled = true;
            }
            else if (leftTile.countryName != tile.countryName)
            {
                tileUI.borderLeft.enabled = true;
            }
            else if (tile.countryName == data.countryName &&
    leftTile.districtId != tile.districtId)
            {
                // 自国内のdistrict境界
                tileUI.borderLeft.enabled = true;
            }

            // 右
            if (!map.TryGetValue(
                rightPos,
                out MapTile rightTile))
            {
                tileUI.borderRight.enabled = true;
            }
            else if (rightTile.countryName != tile.countryName)
            {
                tileUI.borderRight.enabled = true;
            }
            else if (tile.countryName == data.countryName &&
    rightTile.districtId != tile.districtId)
            {
                // 自国内のdistrict境界
                tileUI.borderRight.enabled = true;
            }
        }

    }

    // 選挙区用名前表示
    private void DrawDistrictLabels(
    CountryConstantData data)
    {
        foreach (var district in data.districts)
        {
            GameObject obj =
                Instantiate(
                    districtLabelPrefab,
                    labelRoot);

            RectTransform rt =
                obj.GetComponent<RectTransform>();

            Vector2 offset =
                new Vector2(0, tileSize * 0.8f);

            rt.anchoredPosition =
                district.startPosition * tileSize + offset;

            TextMeshProUGUI text =
                obj.GetComponent<TextMeshProUGUI>();
            
            if (text != null)
            {
                text.text =
                    district.districtId;
            }
        }
    }

    // 国名表示決定用
    private Vector2Int GetCountryLabelPosition(
    CountryConstantData data,
    string countryName)
    {
        List<Vector2Int> positions = new();

        foreach (var tile in data.worldMapTiles.Values)
        {
            if (tile.countryName == countryName)
            {
                positions.Add(tile.position);
            }
        }

        if (positions.Count == 0)
            return Vector2Int.zero;

        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        foreach (var pos in positions)
        {
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);

            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }

        Vector2 center =
            new Vector2(
                (minX + maxX) / 2f,
                (minY + maxY) / 2f);

        Vector2Int nearest =
            positions[0];

        float minDistance =
            Vector2.Distance(
                nearest,
                center);

        foreach (var pos in positions)
        {
            float distance =
                Vector2.Distance(pos, center);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = pos;
            }
        }

        return nearest;
    }

    // 他国用名前表示
    private void DrawCountryLabel(CountryConstantData data)
    {
        foreach (var relation in data.foreignRelations)
        {
            if (relation.mapDistance ==
                CountryDistance.OutOfRange)
            {
                continue;
            }

            Vector2Int pos =
                GetCountryLabelPosition(data, relation.countryName);

            GameObject label =
                Instantiate(
                    countryLabelPrefab,
                    mapRoot);

            RectTransform rt =
                label.GetComponent<RectTransform>();

            rt.anchoredPosition =
                pos * tileSize;

            // 国の大きさを調べる
            int minX = int.MaxValue;
            int maxX = int.MinValue;

            foreach (var tile in data.worldMapTiles.Values)
            {
                if (tile.countryName != relation.countryName)
                    continue;

                minX = Mathf.Min(minX, tile.position.x);
                maxX = Mathf.Max(maxX, tile.position.x);
            }

            int width =
                maxX - minX + 1;

            // 国の横幅の1/3をラベル幅にする
            float labelWidth =
                width * tileSize / 3f;

            rt.sizeDelta =
                new Vector2(
                    labelWidth,
                    rt.sizeDelta.y);

            // Textコンポーネントに名前を入れる
            TMPro.TMP_Text text =
                label.GetComponent<TMPro.TMP_Text>();

            if (text != null)
            {
                text.text =
                    relation.countryName;
            }
        }
    }

    private void ClearMap()
    {
        foreach (Transform child in mapRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in labelRoot)
        {
            Destroy(child.gameObject);
        }

        tileViews.Clear();
    }

}
