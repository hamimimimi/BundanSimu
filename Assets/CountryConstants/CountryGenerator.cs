using UnityEngine;
using System.Collections.Generic;


// CountryConstant計算。

public static class CountryGenerator
{
    public static CountryConstantData Generate()
    {

        // データ作りのみ。
        // データ見て何かするものはCountryConstantManagerのイベントを見て実行。
        
        CountryConstantData data = new CountryConstantData();


        // 人口関連
        GeneratePopulation(data);
        GeneratePyramid(data);
        CalculatePopulationRatio(data);


        CapManager.Instance.InitializeCaps(data);

        // 自国の土地関連
        LandGenerator.Generate(data);


        // 選挙区関連
        data.districts = ElectionDistrictGenerator.Generate(data);

        ElectionDistrictIssueGenerator.Generate(data);
 
        // 自国の地図関連
        LandMapGenerator.Generate(data);        
        
        // 海生成
        SeaGenerator.Generate(data);
        // 他国関係

        data.foreignRelations =
        ForeignCountryRelationGenerator.Generate(data);

       foreach (var rel in data.foreignRelations)
       {
        ForeignCountryManager.Instance.Register(rel);
        }
        ForeignCountryBuilder.Build(data);

        ForeignCountryMapGenerator.Generate(data);

        SeaMapGenerator.Generate(data);


        // 政治と社会関連

        data.governmentType = RandomGovernment();
        GenerateEthnicGroups(data);

        PoliticalPartyGenerator.Generate(data);
        DistrictLeaderAssigner.Assign(data);
        IssueGenerator.Generate(data);




        return data;
    }



    // 人口計算。（現実の国の人口を一部数値を丸めてから算出）
    // 各種数値はゲームバランスを考えて要検討。
    // 多い（少ない）ことで難易度が大きく変わらないよう注意
    private static void GeneratePopulation(CountryConstantData data)
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (roll < 4)
            SetPopulation(data, PopulationSize.VerySmall, 10_000, 100_000);
        else if (roll < 49)
            SetPopulation(data, PopulationSize.Small, 100_000, 10_000_000);
        else if (roll < 93)
            SetPopulation(data, PopulationSize.Medium, 10_000_000, 100_000_000);
        else if (roll < 99)
            SetPopulation(data, PopulationSize.Large, 100_000_000, 300_000_000);
        else
            SetPopulation(data, PopulationSize.Huge, 1_000_000_000, 1_400_000_000);
    }

    private static void SetPopulation(
        CountryConstantData data,
        PopulationSize size,
        long min,
        long max)
    {
        data.sizeCategory = size;
        data.population = UnityEngine.Random.Range((int)min, (int)max);
    }

    // 年齢分布計算。それぞれの割合は現実の国のデータから数字の切りをよくして算出。
    private static void GeneratePyramid(CountryConstantData data)
    {
        int roll = UnityEngine.Random.Range(0, 3);

        if (roll == 0)
            SetMountain(data);
        else if (roll == 1)
            SetJar(data);
        else
            SetBell(data);
    }

    private static void SetMountain(CountryConstantData data)
    {
        data.pyramidType = PopulationPyramid.Mountain;
        data.youngPercent = UnityEngine.Random.Range(50, 61);

        int maxAdult = 100 - data.youngPercent - 1; 
        data.adultPercent = Random.Range(39, maxAdult + 1);

        data.oldPercent = 100 - data.youngPercent - data.adultPercent;
    }

    private static void SetJar(CountryConstantData data)
    {
        data.pyramidType = PopulationPyramid.Jar;
        data.youngPercent = UnityEngine.Random.Range(23, 28);
        data.adultPercent = UnityEngine.Random.Range(58, 63);
        data.oldPercent = 100 - data.youngPercent - data.adultPercent;
    }

    private static void SetBell(CountryConstantData data)
    {
        data.pyramidType = PopulationPyramid.Bell;
        data.youngPercent = UnityEngine.Random.Range(16, 21);
        data.adultPercent = UnityEngine.Random.Range(56, 61);
        data.oldPercent = 100 - data.youngPercent - data.adultPercent;
    }

    private static void CalculatePopulationRatio(CountryConstantData data)
    {
        data.youngPopulation =
            data.population * data.youngPercent / 100;

        data.adultPopulation =
            data.population * data.adultPercent / 100;

        data.oldPopulation =
            data.population * data.oldPercent / 100;
    }

    //Gevernment

    public static GovernmentType RandomGovernment()
    {
        var values = System.Enum.GetValues(typeof(GovernmentType));
        return (GovernmentType)values.GetValue(Random.Range(0, values.Length));
    }

    //Ethnic

    private static void GenerateEthnicGroups(CountryConstantData data)
    {
        data.ethnicGroups.Clear();
        data.isDiverse = Random.value < 0.5f;

        if (data.isDiverse)
            GenerateDiversity(data.ethnicGroups);
        else
            GenerateUniformity(data.ethnicGroups);
    }

    // 人種の多様性により分岐。数値は適当。算出方法も要検討
    private static void GenerateDiversity(List<EthnicGroup> groups)
    {
        int a = Random.Range(35, 61);
        int b = Random.Range(30, 36);
        int remaining = 100 - a - b;

        var others = Distribute(remaining, 3, 1);

        groups.Add(new EthnicGroup { name = "A", percentage = a });
        groups.Add(new EthnicGroup { name = "B", percentage = b });

        string[] names = { "C", "D", "E" };
        for (int i = 0; i < 3; i++)
            groups.Add(new EthnicGroup { name = names[i], percentage = others[i] });
    }

    private static void GenerateUniformity(List<EthnicGroup> groups)
    {
        int a = Random.Range(70, 96);
        int remaining = 100 - a;

        var others = Distribute(remaining, 4, 1);

        groups.Add(new EthnicGroup { name = "A", percentage = a });

        string[] names = { "B", "C", "D", "E" };
        for (int i = 0; i < 4; i++)
            groups.Add(new EthnicGroup { name = names[i], percentage = others[i] });
    }

    private static List<int> Distribute(int total, int count, int minEach)
    {
        List<int> result = new List<int>();
        int remaining = total - count * minEach;

        for (int i = 0; i < count; i++)
        {
            int value = (i == count - 1) ? remaining : Random.Range(0, remaining + 1);
            result.Add(value + minEach);
            remaining -= value;
        }

        return result;
    }
}


