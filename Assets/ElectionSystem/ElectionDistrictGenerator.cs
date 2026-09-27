using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public static class ElectionDistrictGenerator 
{
    public static List<ElectionDistrictData> Generate(CountryConstantData data)
    {
        List<ElectionDistrictData> districts = new();
        
        // 有権者数

        long voters = data.population - data.youngPopulation;

        // 土地

        int cityRemain =
            data.landAreas.Count(a => a.type == AreaType.City);

        int ruralRemain =
            data.landAreas.Count(a => a.type == AreaType.Rural);

        int emptyRemain =
            data.landAreas.Count(a => a.type == AreaType.Empty);
        
        int districtIndex = 0;

        // 1区のみ

        if (data.sizeCategory == PopulationSize.VerySmall)
        {
            var single = CreateSingleDistrict(
                voters,
                cityRemain,
                ruralRemain,
                emptyRemain
            );

            single.districtId = $"District_{districtIndex}";
            districts.Add(single);

            return districts;
        }

        // 生成

        int safety = 0;

        while ((cityRemain  > 0 || ruralRemain > 0) && safety < 10000)           
        {
            safety++;

          // 余りを足す
        if (cityRemain + ruralRemain <= 50)
         {
                if(cityRemain + ruralRemain == 0)
                    break;

        ElectionDistrictData last = new();

        last.districtId = $"District_{districtIndex++}";

        last.cityCount = cityRemain;
        last.ruralCount = ruralRemain;
        last.emptyCount = emptyRemain;

        last.capacity = last.cityCount + last.ruralCount;

        last.turnsToElection = UnityEngine.Random.Range(20, 51);

        districts.Add(last);

        cityRemain = 0;
        ruralRemain = 0;
        emptyRemain = 0;

        break;
         }
            int capacity = UnityEngine.Random.Range(20, 51);

            ElectionDistrictData d = new();
            d.districtId = $"District_{districtIndex++}";

            d.capacity = capacity;         
           
                int maxCity = Mathf.Min(capacity, cityRemain);
                d.cityCount = UnityEngine.Random.Range(0, maxCity + 1);
                cityRemain -= d.cityCount;
                                      
            int remainCapacity = capacity - d.cityCount;
          
                int maxRural = Mathf.Min(remainCapacity, ruralRemain);
                d.ruralCount = UnityEngine.Random.Range(0, maxRural + 1);
                ruralRemain -= d.ruralCount;                          
           
           

            if (d.cityCount + d.ruralCount == 0)
               {
              if (cityRemain > 0)
             {
               d.cityCount = 1;
               cityRemain--;
              }
                else if (ruralRemain > 0)
               {
                 d.ruralCount = 1;
                 ruralRemain--;
               }
              }

            if (emptyRemain > 0)
            {               
                d.emptyCount = UnityEngine.Random.Range(0, emptyRemain + 1);
                emptyRemain -= d.emptyCount;               
            }

            
            d.voterPopulation =
                voters / (districts.Count + 1);
            
            d.turnsToElection = UnityEngine.Random.Range(20, 51);

           if (d.cityCount + d.ruralCount == 0)
             {
               continue;
             }

            districts.Add(d);
        }
        if (safety >= 10000)
        {
            Debug.LogError("無限ループ防止発動");
        }


　　　　CalculateDistrictVoters(data, districts);

        return districts;

    }

    // VerySmall 用
    static ElectionDistrictData CreateSingleDistrict(
        long voters,
        int city,
        int rural,
        int empty)
    {
        return new ElectionDistrictData
        {
            districtId = "District_0",
            capacity = city + rural + empty,
            cityCount = city,
            ruralCount = rural,
            emptyCount = empty,
            voterPopulation = voters,
            turnsToElection = UnityEngine.Random.Range(20, 51)
        };
    }
static void CalculateDistrictVoters(CountryConstantData data, List<ElectionDistrictData> districts)
{
    
    // 国全体の土地数
    
    int totalCity =
        data.landAreas.Count(a => a.type == AreaType.City);

    int totalRural =
        data.landAreas.Count(a => a.type == AreaType.Rural);

    
    // 産業人口取得
    
    int primaryPop = data.industryPopulations
        .Find(i => i.industry == IndustryType.Primary).population;

    int secondaryPop = data.industryPopulations
        .Find(i => i.industry == IndustryType.Secondary).population;

    int tertiaryPop = data.industryPopulations
        .Find(i => i.industry == IndustryType.Tertiary).population;

    int nonPrimaryPop = secondaryPop + tertiaryPop;

    
    // 1エリアあたり人口
    
    float ruralPopPerArea = 0f;
    float cityPopPerArea = 0f;

    if (totalRural > 0)
        ruralPopPerArea = (float)primaryPop / totalRural;

    if (totalCity > 0)
        cityPopPerArea = (float)nonPrimaryPop / totalCity;

    
    // 有権者率
    
    float voterRate =
        (data.adultPercent + data.oldPercent) / 100f;

    
    // 各選挙区計算
    
    foreach (var d in districts)
    {
        float totalPop =
            d.cityCount * cityPopPerArea +
            d.ruralCount * ruralPopPerArea;

        d.totalPopulation = Mathf.FloorToInt(totalPop);
        d.voterPopulation = Mathf.FloorToInt(totalPop * voterRate);
    }

    Debug.Log("選挙区ごとの有権者人口計算完了");
}
}
      
