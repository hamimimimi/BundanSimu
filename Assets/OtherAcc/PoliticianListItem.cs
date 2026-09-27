using UnityEngine;
using TMPro;

public class PoliticianListItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text jobText;
    public TMP_Text partyText;

    public void Setup(PoliticianData data)
    {
        nameText.text = data.name;
        jobText.text = "ê≠é°â∆";

        var party = data.GetParty();

        partyText.text =
            party != null ? party.partyName : "ñ≥èäëÆ";
    }
}