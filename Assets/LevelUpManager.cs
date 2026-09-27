using UnityEngine;
using System.Collections.Generic;


// レベルアップ関係
public class LevelUpManager : MonoBehaviour
{
  public static LevelUpManager Instance;

    [SerializeField] GameObject levelUpPanel;
    [SerializeField] Transform content;
    [SerializeField] GameObject buttonPrefab;

    private MyAccData currentAcc;
    private MyAccItem currentItem;


    public void Open(MyAccItem item)
    {
        currentItem = item;
        currentAcc = item.Data;
        
        levelUpPanel.SetActive(true);

        CreateButtons();
    }

    void Awake()
    {
        Instance = this; 

        if (levelUpPanel != null ) 
        levelUpPanel.SetActive(false);
    }

    public void CheckLevelUp(MyAccItem item)
    {
        while (item.Data.influencePoint >= item.Data.nextLevelUpInfluence)
        {
            item.Data.nextLevelUpInfluence += 1000;

            Open(item);
        }
    }

    public void SelectChoice(LevelUpChoice choice)
    {
        currentAcc.AddStatusLevel(choice.plusType, choice.plusAmount);

        if (choice.minusAmount != 0)
        {
            currentAcc.AddStatusLevel(choice.minusType, choice.minusAmount);
        }

        currentItem.RefreshLevelTexts();

        ClosePanel();
    }

    // 選択肢生成。通常3つ。
    List<LevelUpChoice> GenerateChoices()
    {
        List<LevelUpChoice> all = LevelUpDatabase.Create();

        List<LevelUpChoice> result = new();

        while (result.Count < 3 && all.Count > 0)
        {
            int index = Random.Range(0, all.Count);

            result.Add(all[index]);

            all.RemoveAt(index);
        }

        return result;
    }


    // ボタン生成。
    void CreateButtons()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        List<LevelUpChoice> choices = GenerateChoices();

        foreach (LevelUpChoice choice in choices)
        {
            GameObject obj =
                Instantiate(buttonPrefab, content);

            obj.GetComponent<LevelUpButton>()
                .Setup(choice, this);
        }
    }

    public void ClosePanel()
    {
        levelUpPanel.SetActive(false);
    }
    

}
