using UnityEngine;
using TMPro;


//　投稿ボタンからCountryVariableへの影響計算。詳しい設定はまだ。

public class PostButtonController : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown ynDropdown;

    [Header("Refs")]
    public MyAccItem accItem;
    public PostTagDropdownController tagDropdown;

    public void OnPostButtonClick()
    {
        if (accItem == null)
            return;

        UnlockTagKey selectedKey = tagDropdown.GetSelectedKey();

        if (selectedKey.tag == UnlockTag.None)
        {
            accItem.OnPosted();
            return;
        }

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
        
        var choice = ConvertChoice();

        //　新しいSimulationを登録
       SimulationInitializer initializer =
         new SimulationInitializer();

       SimulationInstance simulation =
          initializer.CreateSimulation(acc, variable);

       SimulationManager.Instance
          .AddSimulation(simulation);


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
   
        
   
