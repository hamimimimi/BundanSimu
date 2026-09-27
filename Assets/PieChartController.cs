using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


// 円グラフ表示用スクリプト、色は用途ごとに個別に設定。

public class PieChartController : MonoBehaviour
{
    public Image slicePrefab;
    public Transform pieRoot;
    
    [System.Serializable]
    public class TagColor
    {
        public string tag;
        public Color color;
    }

    Color GetColor(string key)
    {
        // ElectionView 用（候補者）
        if (key.Contains("_"))
        {
            var split = key.Split('_');

            if (int.TryParse(split.Last(), out int index))
            {
                return CandidateColorProvider.GetColor(index);
            }
        }

        // Floating 対応
        if (key == "Floating")
        {
            return new Color(0.7f, 0.7f, 0.7f);
        }

        // 既存 Yes/No/Neutral
        return GetColorForTag(key);
    }

    public Color GetColorForTag(string tag)
    {
        if (string.IsNullOrEmpty(tag))
            return Color.gray;

        // ① Yes / No / Neutral 判定
        if (tag.Contains("Yes"))
            return GenerateColor(tag, Color.red);

        if (tag.Contains("No"))
            return GenerateColor(tag, Color.blue);

        if (tag.Contains("Neutral"))
            return GenerateColor(tag, Color.yellow);

        return Color.gray; // 未定義用
    }

    Color GenerateColor(string key, Color baseColor)
    {
        int hash = key.GetHashCode();

        // 0〜1に正規化
        float offset = (hash % 100) / 100f;

        // 微妙にズラす
        float r = Mathf.Clamp01(baseColor.r + offset * 0.3f - 0.15f);
        float g = Mathf.Clamp01(baseColor.g + offset * 0.3f - 0.15f);
        float b = Mathf.Clamp01(baseColor.b + offset * 0.3f - 0.15f);

        return new Color(r, g, b, 1f);
    }

       public static class CandidateColorProvider
       {
         static Color[] colors =
        {
        new Color(0.9f,0.2f,0.2f),
        new Color(0.2f,0.6f,0.9f),
        new Color(0.2f,0.8f,0.3f),
        new Color(0.9f,0.8f,0.2f),
        new Color(0.8f,0.3f,0.8f),
        new Color(0.3f,0.9f,0.9f),
        new Color(0.9f,0.5f,0.2f),
        new Color(0.5f,0.5f,0.9f),
        new Color(0.4f,0.8f,0.6f),
        new Color(0.9f,0.4f,0.4f),
        new Color(0.4f,0.7f,0.9f),
        new Color(0.7f,0.9f,0.4f),
        new Color(0.9f,0.7f,0.3f),
        new Color(0.6f,0.4f,0.9f),
        new Color(0.3f,0.9f,0.7f)
        };

      public static Color GetColor(int index)
       {
        return colors[index % colors.Length];
        }
       }

    public void DrawPie(Dictionary<string, int> values)
    {
       
        if (!gameObject.activeInHierarchy)
            return;


        // 既存スライス削除
        foreach (Transform child in pieRoot)
        {
            Destroy(child.gameObject);
        }
       
        int total = values.Values.Sum();

        if (total <= 0)
        {
            Debug.LogWarning("PieChart: total is 0, skip drawing");
            return;
        }

        float startAngle = 0f;
        

        var sorted = values.OrderByDescending(kv => kv.Value).ToList();


        foreach (var kv in sorted)
        {
            float ratio = (float)kv.Value / total;
            float angle = ratio * 360f;

            Image slice = Instantiate(slicePrefab, pieRoot);
            slice.fillAmount = ratio;
            slice.color = GetColor(kv.Key);
            slice.rectTransform.localRotation = Quaternion.Euler(0, 0, -startAngle);



            startAngle += angle;
        }
        
    }
}
