using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

//　入力、計算、反映への司令塔

public class PostButtonController : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown postTagDropdown;
    public TMP_Dropdown ynDropdown;

    [Header("Refs")]
    public MyAccItem accItem;

    // Dropdown index → UnlockTagKey 対応表
    private List<UnlockTagKey> dropdownKeys = new();

    private void Start()
    {
        RefreshTagDropdown();

        // Tag 解放時に自動更新
        if (TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged += RefreshTagDropdown;
        }
    }

    private void OnDestroy()
    {
        if (TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged -= RefreshTagDropdown;
        }
    }

    // ===== Dropdown 再構築 =====
    private void RefreshTagDropdown()
    {
        postTagDropdown.ClearOptions();
        dropdownKeys.Clear();

        var options = TagManager.Instance
            .GetUnlockedTagKeys()
            .Where(tk =>
            {
                if (tk.tag == UnlockTag.None) return false;
                return true;

            })
            .Select(tk =>
            {
                dropdownKeys.Add(tk);
                return tk.ToString(); // 表示用
            })
            .ToList();

        options.Insert(0, "None");
        dropdownKeys.Insert(0, default);

        postTagDropdown.AddOptions(options);
    }

    public void OnPostButtonClick()
    {
        if (accItem == null)
            return;

        int index = postTagDropdown.value;

        
        if (index == 0)
        {
            accItem.OnPosted();
            return;
        }

        UnlockTagKey selectedKey = dropdownKeys[index];

        var manager = CountryVariableManager.Instance;

        var variable = manager.GetByKey(selectedKey);
        if (variable == null)
        {
            Debug.LogWarning($"CountryVariable not found for {selectedKey}");
            return;
        }

         var acc = accItem.Data;

        if (acc == null)
            return;
         //　選択取得
        var choice = ConvertChoice();

        //　変化量計算
       int mainAdd =
        Mathf.FloorToInt(acc.influencePoint * acc.creditRatio / 500f);

       int subAdd =
        Mathf.FloorToInt(acc.influencePoint * acc.RiskLevel / 1000f);

       int neutralAdd = 150;

        // CountryVariable 側
        manager.ApplyPost(selectedKey, choice, mainAdd, subAdd, neutralAdd);

        // MyAcc 側（UnlockProgress・Opinion）
        accItem.ApplyPost(selectedKey, choice);

        accItem.OnPosted();
    }
  
     private CountryVariable.PostChoice ConvertChoice()
    {
        string text = ynDropdown.options[ynDropdown.value].text;

        return text switch
        {
            "Yes" => CountryVariable.PostChoice.Yes,
            "No" => CountryVariable.PostChoice.No,
            _ => CountryVariable.PostChoice.Neutral
        };
    }
}
   
        
   
