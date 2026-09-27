using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElectionManager : MonoBehaviour
{
    private void OnEnable()
    {
        TurnManager.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= OnTurnChanged;
    }

    void OnTurnChanged()
    {  
        Debug.Log("ElectionManager Turn Changed"); 
        var data = CountryConstantManager.Instance.Data;

        foreach (var district in data.districts)
        {
            Debug.Log($"{district.districtId} turns={district.turnsToElection}");
            district.turnsToElection--;

            if (district.turnsToElection == 10 && !district.isAnnounced)
            {
                AnnounceElection(district, data);
            }

            if (district.turnsToElection == 0)
            {
                ExecuteElection(district);
            }
        }
    }


    void AnnounceElection(ElectionDistrictData district, CountryConstantData data)
    {
              
        district.isAnnounced = true;
        
        if (data.allPoliticians == null)       
          data.allPoliticians = new List<PoliticianData>();           

         AnnounceEventViewController.Instance.AddEvent(
        $"{district.districtId} で選挙が告示されました（{district.turnsToElection} ターン後に選挙が行われます）"
         );

        district.candidates = new List<PoliticianData>();

        var usedParties = new HashSet<string>();


        // 現職

        if (district.leader != null)
        
            AddCandidate(district, district.leader, usedParties);

        // 既存政治家

        var available = data.allPoliticians
            .Where(p => !p.isAssigned)
            .OrderBy(_ => Random.value)
            .ToList();
        
        const int MAX_CANDIDATES = 15;



        foreach (var party in data.politicalParties)
         {
          var pol = PoliticianGenerator.GenerateSingle();

          pol.party = party;

           AddCandidate(district, pol, usedParties);

          data.allPoliticians.Add(pol);
          }

        foreach (var p in available)
        {
            if (district.candidates.Count >= MAX_CANDIDATES) break;

            string partyId = p.party?.partyId ?? "independent";

            if (usedParties.Contains(partyId)) continue;

            AddCandidate(district, p, usedParties);
        }
       
        // 足りなければ生成

        int safety = 0;

        while (district.candidates.Count < 2 && safety < 15)
        {
           safety++;

            var newPol = PoliticianGenerator.GenerateSingle();

            AddCandidate(district, newPol, usedParties);

            data.allPoliticians.Add(newPol);
        }

        Debug.Log($"{district.districtId} 告示完了 候補者数: {district.candidates.Count}");
        
        CreateCandidateUnlockButtons(district);

        CalculateInitialSupport(district, data);
        
        foreach (var c in district.candidates)
         {
           PoliticianListController.Instance.Add(c);
         }

         ElectionViewController.Instance.Create(district);

         foreach (var c in district.candidates)
          {
            ElectionVariableGenerator.CreateCandidateVariable(
             district.districtId,
             c.name
            );
          }
    }

    // 支持層計算
    void CalculateInitialSupport(
        ElectionDistrictData district,
        CountryConstantData data)
   {
        long totalAssigned = 0;

        // 初期値
        foreach (var c in district.candidates)
        {
            c.coreSupport = 1;
            c.softSupport = 1;
            totalAssigned += 2;
        }
       
    // 政党支持配分
    foreach (var c in district.candidates)
    {
        if (c.party == null) continue;

        var party = c.party;

        float rate =
            (float)district.totalPopulation /
            data.population;

        int core =
            Mathf.FloorToInt(party.coreSupporters * rate);

        int soft =
            Mathf.FloorToInt(party.softSupporters * rate * 0.8f);

        c.coreSupport += core;
        c.softSupport += soft;

        totalAssigned += core + soft;
    }

       foreach (var c in district.candidates)
        { 
        ApplyIssueSupport(district,data,c,ref totalAssigned);
        }

        // 浮動票

    long floating = district.totalPopulation - totalAssigned;

        if (floating < 0) floating = 0;

        district.floatingVoters = (int)floating;

    Debug.Log(
        $"{district.districtId} 支持層確定 " +
        $"浮動票={district.floatingVoters}"
    );

}

    // Issue補正

void ApplyIssueSupport(
    ElectionDistrictData district,
    CountryConstantData data,
    PoliticianData c,
    ref long totalAssigned)
{
    if (district.issues == null) return;
    foreach (var issue in district.issues)
    {
        UnlockTagKey key = new UnlockTagKey
        {
            tag = UnlockTag.ElectionIssue,
            key = issue.issueId
        };

        var variable =
            CountryVariableManager.Instance
                .GetByKey(key);

        if (variable == null) continue;

        float total =
            variable.yes +
            variable.no +
            variable.neutral;

        if (total <= 0) continue;

        float yesRate =
            variable.yes / total;

        long bonus =
            (long)(
                district.totalPopulation *
                0.05f *
                yesRate *
                issue.weight
            );

        c.softSupport += (int)bonus;
        totalAssigned += bonus;
    }
}
 
    
    // UnlockButton生成
 
    void CreateCandidateUnlockButtons(
        ElectionDistrictData district)
    {
        var generator =
            FindObjectOfType<UnlockButtonGenerator>();

        if (generator == null)
        {
            Debug.LogWarning("UnlockButtonGenerator not found");
            return;
        }

        foreach (var c in district.candidates)
        {
            UnlockTagKey key = new UnlockTagKey
            {
                tag = UnlockTag.Politician,
                key = c.name
            };

            // 既に存在するか確認
            var existing =
                CountryVariableManager.Instance
                    .GetByKey(key);

            if (existing != null) continue;
            if (UnlockDefinitionManager.Instance.Exists(
                UnlockTag.Politician,
                c.name))
              continue;

            UnlockDefinitionManager.Instance
                .CreateDynamicDefinition(
                    UnlockTag.Politician,
                    c.name,
                    c.name,
                    "候補者が登場しました"
                );

            generator.Generate();
        }
    }

    void AddCandidate(
    ElectionDistrictData district,
    PoliticianData p,
    HashSet<string> usedParties)
    {
        string partyId = p.party?.partyId ?? "independent";

        if (usedParties.Contains(partyId)) return;

        district.candidates.Add(p);
        usedParties.Add(partyId);

        p.isAssigned = true; // ★ロック
    }


    void ExecuteElection(ElectionDistrictData district)
    {
        Debug.Log($"{district.districtId} 選挙実行");

        // とりあえずランダム（後で差し替え）
        var winner = district.candidates[Random.Range(0, district.candidates.Count)];

        district.leader = winner;

        // リセット
        foreach (var c in district.candidates)
        {
            c.isAssigned = false;
        }

        district.candidates.Clear();
        district.isAnnounced = false;

        // 次回選挙
        district.turnsToElection = 50;
    }

}
