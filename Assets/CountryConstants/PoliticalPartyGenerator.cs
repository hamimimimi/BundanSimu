using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// ê≠ì}ê∂ê¨óp

public static class PoliticalPartyGenerator
{
    // ê≠ì}ê∂ê¨
    public static void Generate(CountryConstantData data)
    {
        int eligiblePopulation =
            (int)(data.population - data.youngPopulation);

        int totalSupporters =
            Mathf.FloorToInt(
                eligiblePopulation *
                Random.Range(0.6f, 0.95f)
            );

        int independent =
            Mathf.FloorToInt(
                eligiblePopulation *
                Random.Range(0f, 0.3f)
            );

        data.independentCount = independent;
        totalSupporters -= independent;

        int partyCount = DeterminePartyCount(data.governmentType);

        var parties = new List<PoliticalPartyData>();

        // éxéùé“ï™îz
        int remaining = totalSupporters;
        for (int i = 0; i < partyCount; i++)
        {
            string Key = $"party_{i}";


            int supporters;
            int partiesLeft = partyCount - i;

            if (i == partyCount - 1)
            {
                supporters = remaining;
            }
            else
            {
                int max = Mathf.Max(0, remaining - (partiesLeft - 1));
                supporters = Random.Range(0, max + 1);
            }
               
              supporters = Mathf.Max(0, supporters);
              supporters = Mathf.Min(supporters, remaining);           

            remaining -= supporters;

            int core = Mathf.FloorToInt(
                supporters * Random.Range(0.1f, 0.5f)
             );

             int soft = supporters - core;

            parties.Add(new PoliticalPartyData
            {
                partyId = Key,
                partyName = $"Party_{i + 1}",
                supporterCount = supporters,

                coreSupporters = core,
                softSupporters = soft
            });


           foreach (var p in parties)
             {
               Debug.Log(
                 $"{p.partyName} supporters={p.supporterCount} core={p.coreSupporters} soft={p.softSupporters}"
                     );
             }
        }


        // ê≠çÙ
        GeneratePolicies(data, parties);

        // ê≠é°â∆
        data.politicalParties = parties;

        PoliticianGenerator.AssignToParties(data);

        foreach (var party in parties)
        {
         CountryVariableFactory.Create(
           UnlockTag.PoliticalParty,
           party.partyId + "_post",
            party.partyName);
        }

      }

       public static void CalculateDistrictSupport(
         ElectionDistrictData district,
         PoliticalPartyData party,
         CountryConstantData data,
         out int core,
         out int soft)
        {
          float rate =
          (float)district.totalPopulation /
          data.population;

          core = Mathf.FloorToInt(party.coreSupporters * rate);
          soft = Mathf.FloorToInt(party.softSupporters * rate);
        }

    static int DeterminePartyCount(GovernmentType type)
    {
        if (type == GovernmentType.Dictatorship)
            return 1;

        int count = 2;
        float chance = 0.8f;

        while (Random.value < chance)
        {
            count++;
            chance *= 0.8f;
        }

        return count;
    }
    // ê≠çÙê∂ê¨
    public static void GeneratePolicies(
    CountryConstantData data,
    List<PoliticalPartyData> parties)
    {
        var issues = new HashSet<string>();

        foreach (var rel in data.foreignRelations)
        {
            if (rel.hasTerritorialIssue)
                issues.Add($"{rel.countryName}_Territory");

            if (rel.hasHistoricalIssue)
                issues.Add($"{rel.countryName}_History");
        }

        foreach (var party in parties)
        {
            foreach (var issue in issues)
            {
                party.policies.Add(new PolicyStanceData
                {
                    issueName = issue,
                    stance = (PolicyStance)Random.Range(-1, 2)
                });
            }
        }
    }

    
}
