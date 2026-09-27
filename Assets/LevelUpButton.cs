using UnityEngine;
using TMPro;

// レベルアップ選択肢用
public class LevelUpButton : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;

    private LevelUpChoice choice;


    private LevelUpManager manager;

    public void Setup(LevelUpChoice c, LevelUpManager m)
    {
        choice = c;
        manager = m;


        title.text = c.title;
        description.text = c.description;
    }

    public void OnClick()
    {
      manager.SelectChoice(choice);
    }
}