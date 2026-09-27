using UnityEngine;
using TMPro;

// 選挙区情報表示。

public class ElectionDistrictView : MonoBehaviour
{
    public TMP_Text leadernameText;
    public TMP_Text totalPopText;
    public TMP_Text partyText;
    
    
    public void Setup(ElectionDistrictData data)
    {
  
     if (data == null) return;

        // リーダー名
        if (data.leader != null)
            leadernameText.text = data.leader.name;
        else
            leadernameText.text = "なし";

        // 政党

        var party = data.leader.GetParty();

        partyText.text =
                party != null ? party.partyName : "無所属";

        // 総人口（後で追加する totalPopulation）
        totalPopText.text = $" {data.totalPopulation} 人";
    }

    
}
