using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 土地生成。産業人口から土地を生成。

public class LandGenerator 
{
    const int MAX_EMPTY_LAND = 1000;   // 無人の土地をランダムで追加
    const int CITY_POP_COUNT = 100000; // 一単位当たりの町の人口。
    const int RURAL_POP_COUNT = 50000; // 一単位当たりの村の人口

    public static void Generate(CountryConstantData data)
    {
       

        // 産業人口
        data.industryPopulations = GenerateIndustryPopulation(data);

        // 地区構成
        data.landAreas = GenerateAreas(data);
    }

    // 産業人口
    static List<IndustryPopulationData> GenerateIndustryPopulation(
    CountryConstantData data)
    {
        bool developed = UnityEngine.Random.value > 0.5f;

        int p1, p2, p3;

        if (!developed)
        {
            p1 = UnityEngine.Random.Range(40, 91);
            p3 = UnityEngine.Random.Range(1, 51);
            p2 = 100 - p1 - p3;
        }
        else
        {
            p1 = UnityEngine.Random.Range(1, 11);
            p3 = UnityEngine.Random.Range(50, 81);
            p2 = 100 - p1 - p3;
        }

        return new List<IndustryPopulationData>
    {
        new()
        {
            industry = IndustryType.Primary,
            population = Mathf.FloorToInt(data.population * p1 / 100f)
        },
        new()
        {
            industry = IndustryType.Secondary,
            population = Mathf.FloorToInt(data.population * p2 / 100f)
        },
        new()
        {
            industry = IndustryType.Tertiary,
            population = Mathf.FloorToInt(data.population * p3 / 100f)
        }
    };
    }

    // 土地
    static List<LandAreaData> GenerateAreas(CountryConstantData data)
    {
        var areas = new List<LandAreaData>();

        // 首都
        areas.Add(new LandAreaData { type = AreaType.Capital });

        int primaryPop = data.industryPopulations
            .Find(i => i.industry == IndustryType.Primary).population;

        int nonPrimaryPop = data.industryPopulations
            .FindAll(i => i.industry != IndustryType.Primary)
            .Sum(i => i.population);

        int cityCount;
        int ruralCount;

        if (data.sizeCategory == PopulationSize.VerySmall)
        {
            cityCount = UnityEngine.Random.Range(0, 4);
            ruralCount = UnityEngine.Random.Range(0, 4);
        }
        else
        {
            cityCount = nonPrimaryPop / CITY_POP_COUNT; 
            ruralCount = primaryPop / RURAL_POP_COUNT; 
        }

        for (int i = 0; i < cityCount; i++)
            areas.Add(new LandAreaData { type = AreaType.City });

        for (int i = 0; i < ruralCount; i++)
            areas.Add(new LandAreaData { type = AreaType.Rural });

        int emptyCount = UnityEngine.Random.Range(
            0, MAX_EMPTY_LAND + 1);

        for (int i = 0; i < emptyCount; i++)
            areas.Add(new LandAreaData { type = AreaType.Empty });

        return areas;
    }

}
