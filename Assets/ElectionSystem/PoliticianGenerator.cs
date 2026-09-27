using UnityEngine;
using System.Collections.Generic;

public static class PoliticianGenerator
{
    static string[] firstNames =
    {
        "Alex", "Chris", "Jordan", "Taylor", "Morgan",
        "Riley", "Casey", "Jamie", "Avery", "Quinn"
    };

    static string[] lastNames =
    {
        "Smith", "Johnson", "Brown", "Williams",
        "Miller", "Davis", "Wilson", "Moore"
    };

    static int nextId = 0;
    static HashSet<string> usedNames = new();
    

    // 単体生成
   
    public static PoliticianData GenerateSingle(
        PoliticalPartyData party = null)
    {
        string name;

        do
        {
            name = GenerateName();
        }
        while (usedNames.Contains(name));

        usedNames.Add(name);

        var pol = new PoliticianData
        {
            id = nextId++,
            name = name,
            partyId = party != null ? party.partyId : null
        };

        return pol;
    }

    

    // 複数生成（政党なし）
    
    public static List<PoliticianData> GenerateBatch(int count)
    {
        List<PoliticianData> list = new();

        for (int i = 0; i < count; i++)      
            list.Add(GenerateSingle(null));
        

        return list;
    }

    // 名前生成
    
    static string GenerateName()
    {
        string first =
            firstNames[Random.Range(0, firstNames.Length)];

        string last =
            lastNames[Random.Range(0, lastNames.Length)];

        return $"{first} {last}";
    }   







    // 政党に応じて生成
   
    public static void AssignToParties(
        CountryConstantData data)
    {
        if (data.politicalParties == null) return;

        int totalSupporters =
            Mathf.Max(1,
                (int)(data.population - data.youngPopulation));

        foreach (var party in data.politicalParties)
        {
            float ratio =
                (float)party.supporterCount / totalSupporters;

            int count;

            if (data.governmentType == GovernmentType.Dictatorship)
            {
                count = 1;
            }
            else
            {
                count = (ratio >= 0.2f && Random.value < 0.5f)
                    ? 2
                    : 1;
            }

            for (int i = 0; i < count; i++)
            {
                var pol = GenerateSingle(party);
                party.politicians.Add(pol);
            }
        }

        // 独裁の追加政治家
        if (data.governmentType == GovernmentType.Dictatorship &&
            data.politicalParties.Count > 0)
        {
            int extra = Random.Range(1, 4);

            for (int i = 0; i < extra; i++)
            {
                var pol = GenerateSingle(data.politicalParties[0]);
                data.politicalParties[0].politicians.Add(pol);
            }
        }
    }

    
    // 無所属生成
    
    public static List<PoliticianData> GenerateIndependents(
        int count)
    {
        List<PoliticianData> list = new();

        for (int i = 0; i < count; i++)
        {
            list.Add(GenerateSingle(null));
        }

        return list;
    }


}