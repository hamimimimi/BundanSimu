using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 選挙関係

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


    // ターンカウント関係10ターンで告示、

    void OnTurnChanged()
    {  
        Debug.Log("ElectionManager Turn Changed"); 
        var data = CountryConstantManager.Instance.Data;


        foreach (var district in data.districts)
        {
            Debug.Log($"{district.districtId} turns={district.turnsToElection}");
            district.turnsToElection--;

        if (district.hideAfterTurns > 0)
         {
           district.hideAfterTurns--;

         if (district.hideAfterTurns == 0)
            {
              if (district.view != null)
               district.view.gameObject.SetActive(false);
             }
          }

        if (district.isAnnounced)
        {
            UpdateSupport(district);

            ElectionViewController.Instance.Refresh(district);
        }

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

    // 告示、

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

          pol.partyId = party.partyId;

           AddCandidate(district, pol, usedParties);

          data.allPoliticians.Add(pol);
          }

        foreach (var p in available)
        {
            if (district.candidates.Count >= MAX_CANDIDATES) break;

            string partyId =
             string.IsNullOrEmpty(p.partyId)
               ? "independent"
               : p.partyId;

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
        if (string.IsNullOrEmpty(c.partyId)) continue;

        var party = data.politicalParties.Find(p => p.partyId == c.partyId);
       
            if (party == null) continue;
        
            
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

    private bool generating;

    void CreateCandidateUnlockButtons(
        ElectionDistrictData district)
    { 
        if (generating) return;
        generating = true;
       
        var generator =
            FindFirstObjectByType<UnlockButtonGenerator>();

        if (generator == null)
        {
            Debug.LogWarning("UnlockButtonGenerator not found");
            return;
        }

        foreach (var c in district.candidates)
        {
            string baseKey = $"candidate_{c.id}";
            c.variableKey = baseKey;
            string keyStr = $"candidate_{c.id}_post";

            UnlockTagKey key = new UnlockTagKey
            {
                tag = UnlockTag.Politician,
                key = keyStr
            };

            // 既に存在するか確認
            var existing =
                CountryVariableManager.Instance
                    .GetByKey(key);

            if (existing != null) continue;
            if (UnlockDefinitionManager.Instance.Exists(
                UnlockTag.Politician,
                keyStr))
                continue;

            UnlockDefinitionManager.Instance
                .CreateDynamicDefinition(
                    UnlockTag.Politician,
                    keyStr,
                    c.name,
                    "候補者が登場しました"
                );

           CountryVariableFactory.CreatePostVariable(
            new UnlockTagKey
            {
                tag = UnlockTag.Politician,
                key = baseKey
            });
        }
            generator.Generate();
        
        generating = false;
    }

    // 候補者生成

    void AddCandidate(
    ElectionDistrictData district,
    PoliticianData p,
    HashSet<string> usedParties)
    {
        string partyId =
    string.IsNullOrEmpty(p.partyId)
    ? "independent"
    : p.partyId;

        if (usedParties.Contains(partyId)) return;

        district.candidates.Add(p);
        usedParties.Add(partyId);

        p.isAssigned = true; // ★ロック
    }



    public void UpdateSupport(ElectionDistrictData district)
    {
        Debug.Log("UpdateSupport called");
        foreach (var c in district.candidates)
        {
            var key = new UnlockTagKey
            {
            tag = UnlockTag.Politician,
            key = c.variableKey + "_post"
            };

            var varData = CountryVariableManager.Instance.GetByKey(key);

            if (varData == null) continue;

            float yes = varData.yes;
            float no = varData.no;
            float neutral = varData.neutral;

            float total = yes + no + neutral;
            if (total <= 0) continue;

            float delta = (yes - no) / total;

            int change = Mathf.RoundToInt(delta * 100f);

            c.softSupport += change;
            c.softSupport = Mathf.Max(0, c.softSupport);
        }
       
    }

    // 選挙結果計算。

    public void CalculateResult(ElectionDistrictData district)
    {
        int floating = district.floatingVoters;

        int equalShare = floating / 2 / district.candidates.Count;
        int debatePool = floating / 2;

        foreach (var c in district.candidates)
        {
            int votes = 0;

            // 岩盤
            votes += c.coreSupport;

            // 弱支持
            votes += Mathf.RoundToInt(c.softSupport * 0.8f);

            // 均等浮動票
            votes += equalShare;

            // Debate補正
            var varData = CountryVariableManager.Instance.GetByKey(new UnlockTagKey
            {
                tag = UnlockTag.Politician,
                key = c.variableKey + "_post"
            });

            if (varData != null)
            {
                int level = Mathf.Min(varData.debateLevel, 3);
            votes += Mathf.RoundToInt(debatePool * (level / 100f));
            }

            c.finalVotes = votes;
        }
    }


    // 選挙実行。

    void ExecuteElection(ElectionDistrictData district)
    {
        if (district.view == null)
        {
            Debug.LogError($"ElectionView missing for {district.districtId}");
            return;
        }

        Debug.Log($"{district.districtId} 選挙実行");

    CalculateResult(district);

    // 最大得票者
    PoliticianData winner = null;
    int max = -1;

    foreach (var c in district.candidates)
    {
        if (c.finalVotes > max)
        {
            max = c.finalVotes;
            winner = c;
        }
    }

    district.leader = winner;

    ElectionDistrictViewController.Instance
    .RefreshDistrict(district);
    // UI更新
    district.view.gameObject.SetActive(true);
    district.view.SetupResult(district);

    // 非表示カウント開始
    district.hideAfterTurns = 3;
        // 次回選挙
        district.turnsToElection = 50;
    }

}
