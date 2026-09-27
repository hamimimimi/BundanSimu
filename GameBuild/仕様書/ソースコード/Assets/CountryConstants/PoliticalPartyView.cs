using UnityEngine;
using TMPro;

public class PoliticalPartyView : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text supporterText;
    public TMP_Text policyText;

    public void Setup(PoliticalPartyData data)
    {
        nameText.text = data.partyName;
        supporterText.text = $"éxéùé“: {data.supporterCount}";

       
    }
}
