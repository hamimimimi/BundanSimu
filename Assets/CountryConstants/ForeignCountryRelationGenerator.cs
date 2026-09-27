using UnityEngine;
using System.Collections.Generic;

// 関係国関係生成。大きさと距離等を決める。

public static class ForeignCountryRelationGenerator
{
    public static List<ForeignCountryRelationData> Generate(CountryConstantData data)
    {
        int count = Random.Range(5, 11);   // 関係国個数。数は多すぎず少なすぎず、要調整。

        var relations = new List<ForeignCountryRelationData>();
        int remainingExport = 100;
        int remainingImport = 100;

        int dataLandCount = data.landAreas.Count;// 自国の土地数


        // 輸出入生成。最低５
        for (int i = 0; i < count; i++)
        {
            int export = (i == count - 1)
                ? remainingExport
                : Random.Range(5, remainingExport - (count - i - 1) * 5);

            int import = (i == count - 1)
                ? remainingImport
                : Random.Range(5, remainingImport - (count - i - 1) * 5);

            remainingExport -= export;
            remainingImport -= import;


            // 関係データを作成
            ForeignCountryRelationData relation =
                new ForeignCountryRelationData();

            relation.countryName = $"Country_{i + 1}";

            // 関係。好悪
            relation.relationType = Random.value < 0.6f
                ? RelationType.Friendly
                : RelationType.Hostile;

            // 力関係。
            relation.powerBalance = (PowerBalance)Random.Range(0, 3);

            // 外交問題。
            relation.hasTerritorialIssue = Random.value < 0.3f;
            relation.hasHistoricalIssue = Random.value < 0.4f;

            // 輸出入
            relation.exportRatio = export;
            relation.importRatio = import;


            // 土地サイズ決定。
            switch (relation.powerBalance)
            {
                case PowerBalance.Superior:
                    relation.landCount =
                        Random.Range(
                            dataLandCount + 1,
                            dataLandCount * 2);
                    break;

                case PowerBalance.Equal:
                    relation.landCount =
                        Random.Range(
                            Mathf.Max(1, dataLandCount / 2),
                            dataLandCount * 3 / 2);
                    break;

                case PowerBalance.Inferior:
                    relation.landCount =
                        Random.Range(
                            Mathf.Max(1, dataLandCount / 4),
                            dataLandCount);
                    break;
            }

            // 距離区分決定。
            float roll = Random.value;

            if (roll < 0.15f)
            {
                relation.mapDistance = CountryDistance.Adjacent;// 隣接
            }
            else if (roll < 0.50f)
            {
                relation.mapDistance = CountryDistance.InRange;// 枠内
            }
            else if (roll < 0.75f)
            {
                relation.mapDistance = CountryDistance.Partial;// 一部
            }
            else
            {
                relation.mapDistance = CountryDistance.OutOfRange;// 枠外
            }

            relations.Add(relation);        

        }

        return relations;

    }

    static readonly Vector2Int[] directions =
{
    Vector2Int.up,
    Vector2Int.down,
    Vector2Int.left,
    Vector2Int.right
};



}
