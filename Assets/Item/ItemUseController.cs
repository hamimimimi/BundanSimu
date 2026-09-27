using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ItemUseController : MonoBehaviour
{
    public static ItemUseController Instance;

    [SerializeField] GameObject panel;
    UnlockTagKey currentItem;

    public TMP_Dropdown locationDropdown;
    public TMP_Dropdown demoDropdown;

    List<TargetSelection> locations = new();
    List<CountryVariable> variables = new();
    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(UnlockTagKey itemKey)
    {
        currentItem = itemKey;
        panel.SetActive(true);

        BuildLocationDropdown();
        BuildDemoDropdown();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void Execute()
    {
        if (locations.Count == 0 || variables.Count == 0)
        return;

        var target = locations[locationDropdown.value];
        var variable = variables[demoDropdown.value];
    
        // 消費チェック
        if (ItemInventoryManager.Instance.Get(currentItem) <= 0)
            return;

        ItemInventoryManager.Instance.Add(currentItem, -1);

        // デモ発動
        DemoManager.Instance.TriggerManualDemo(target, variable);

        Close();
    }
    
    void BuildLocationDropdown()
{
    locationDropdown.ClearOptions();
    locations.Clear();

    List<string> options = new();

    // 首都
    locations.Add(new TargetSelection { isCapital = true });
    options.Add("首都");

    // 選挙区

var data = CountryConstantManager.Instance?.Data;

if (data == null || data.districts == null)
{
    Debug.LogError("District data is null!");
    return;
}

var list = data.districts;


    foreach (var d in list)
    {
        locations.Add(new TargetSelection
        {
            isCapital = false,
            district = d
        });

        options.Add(d.districtId);
    }

    locationDropdown.AddOptions(options);
}
void BuildDemoDropdown()
{
    demoDropdown.ClearOptions();
    variables.Clear();

    List<string> options = new();

    foreach (var v in CountryVariableManager.Instance.GetAll())
    {
        // 必要ならフィルタ
        if (v.debatePoint <= 0) continue;

        variables.Add(v);
        options.Add(v.displayName);
    }

    demoDropdown.AddOptions(options);
}
}
