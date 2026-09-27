using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LandGenerator 
{
    const int MAX_EMPTY_LAND = 1000;

    public static void Generate(CountryConstantData data)
    {
       

        // éYã∆êlå˚
        data.industryPopulations = GenerateIndustryPopulation(data);

        // ínãÊç\ê¨
        data.landAreas = GenerateAreas(data);
    }

    // éYã∆êlå˚
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

    // ìyín
    static List<LandAreaData> GenerateAreas(CountryConstantData data)
    {
        var areas = new List<LandAreaData>();

        // éÒìs
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
            cityCount = nonPrimaryPop / 100_000;
            ruralCount = primaryPop / 50_000;
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
