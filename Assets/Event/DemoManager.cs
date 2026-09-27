using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public static DemoManager Instance;

    List<ScheduledDemo> scheduled = new List<ScheduledDemo>();
    List<DemoData> active = new List<DemoData>();

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        TurnManager.OnTurnChanged += OnTurnChanged;
    }

    void OnDisable()
    {
        TurnManager.OnTurnChanged -= OnTurnChanged;
    }

    void OnTurnChanged()
    {
        CheckScheduled();
        UpdateActive();
    }

    public void ScheduleDemo(CountryVariable variable)
    {
        int delay = Random.Range(3, 11); // 3`10ƒ^[ƒ“

        scheduled.Add(new ScheduledDemo
        {
            variable = variable,
            triggerTurn = TurnManager.Instance.CurrentTurn + delay
        });
    }

    void CheckScheduled()
    {
        for (int i = scheduled.Count - 1; i >= 0; i--)
        {
            if (TurnManager.Instance.CurrentTurn >= scheduled[i].triggerTurn)
            {
                SpawnDemo(scheduled[i].variable);
                scheduled.RemoveAt(i);
            }
        }
    }

    void SpawnDemo(CountryVariable variable)
    {
        bool isCapital = Random.value < 0.5f;

        ElectionDistrictData district = null;

        if (!isCapital)
        {
            var list = CountryConstantManager.Instance.Data.districts;
            district = list[Random.Range(0, list.Count)];
        }

        int debatePoint = variable.debatePoint;
        int duration = Mathf.Max(1, debatePoint / 2000);

        DemoData demo = new DemoData
        {
            variable = variable,
            isCapital = isCapital,
            district = district,
            remainingTurns = duration
        };

        active.Add(demo);

        AnnounceDemo(demo);
    }

    void AnnounceDemo(DemoData demo)
    {
        string location = demo.isCapital ? "Žñ“s" : demo.district.districtName;

        AnnounceEventViewController.Instance.AddEvent(
            $"ƒfƒ‚”­¶: {location} / {demo.variable.displayName}"
        );
    }


    void UpdateActive()
    {
    for (int i = active.Count - 1; i >= 0; i--)
    {
        var demo = active[i];

        int add = CalculateInstability(demo);
        GovernmentStabilityManager.Instance.AddInstability(add);

        demo.remainingTurns--;

        if (demo.remainingTurns <= 0)
        {
            AnnounceDemoEnd(demo);
            active.RemoveAt(i);
        }
    }
    }

    void AnnounceDemoEnd(DemoData demo)
    {
      string location = demo.isCapital ? "Žñ“s" : demo.district.districtName;

      AnnounceEventViewController.Instance.AddEvent(
        $"{location}‚Å‚Ìu{demo.variable.displayName}v‚Ìƒfƒ‚‚ÍŽû‘©‚µ‚Ü‚µ‚½"
       );
    }

    int CalculateInstability(DemoData demo)
    {
        if (demo.isCapital) return 1;
        return 2;
    }

public DemoSaveData GetSaveData()
{
    DemoSaveData data = new DemoSaveData();

    foreach (var s in scheduled)
    {
        data.scheduled.Add(new ScheduledDemoSaveData
        {
            variableKey = s.variable.TagKey,
            triggerTurn = s.triggerTurn
        });
    }

    foreach (var a in active)
    {
        data.active.Add(new ActiveDemoSaveData
        {
            variableKey = a.variable.TagKey,
            isCapital = a.isCapital,
            districtId = a.district != null ? a.district.districtId : "",
            remainingTurns = a.remainingTurns
        });
    }

    return data;
}

    public void TriggerManualDemo(TargetSelection target, CountryVariable variable)
    {
       
        int debatePoint = variable.debatePoint;

        int duration = Mathf.Max(1, debatePoint / 2000);
        
        DemoData demo = new DemoData
        {
            variable = variable,
            isCapital = target.isCapital,
            district = target.district,
            remainingTurns = duration 
        };

        active.Add(demo);

        AnnounceDemo(demo);
    }





    public void Load(DemoSaveData data)
{
    scheduled.Clear();
    active.Clear();

    // Scheduled•œŒ³
    foreach (var s in data.scheduled)
    {
        var variable = CountryVariableManager.Instance.GetByKey(s.variableKey);

        if (variable == null) continue;

        scheduled.Add(new ScheduledDemo
        {
            variable = variable,
            triggerTurn = s.triggerTurn
        });
    }

    // Active•œŒ³
    foreach (var a in data.active)
    {
        var variable = CountryVariableManager.Instance.GetByKey(a.variableKey);
        if (variable == null) continue;

        ElectionDistrictData district = null;

        if (!a.isCapital)
        {
            var list = CountryConstantManager.Instance.Data.districts;
            district = list.Find(d => d.districtId == a.districtId);
        }

        active.Add(new DemoData
        {
            variable = variable,
            isCapital = a.isCapital,
            district = district,
            remainingTurns = a.remainingTurns
        });
    }
}
}
