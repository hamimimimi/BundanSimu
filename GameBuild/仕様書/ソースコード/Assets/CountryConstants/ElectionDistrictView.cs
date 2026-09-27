using UnityEngine;
using TMPro;

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
        if (data.leader != null && data.leader.party != null)
            partyText.text = data.leader.party.partyName;
        else
            partyText.text = "無所属";

        // 総人口（後で追加する totalPopulation）
        totalPopText.text = $" {data.totalPopulation} 人";
    }

    
}
