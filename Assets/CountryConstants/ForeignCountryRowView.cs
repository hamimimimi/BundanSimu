using UnityEngine;
using TMPro;

// ä÷åWçëï\é¶ópÅB

public class ForeignCountryRowView : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text relationText;
    public TMP_Text powerText;
    public TMP_Text territoryText;
    public TMP_Text historyText;
    public TMP_Text exportText;
    public TMP_Text importText;

    public void Set(ForeignCountryRelationData d)
    {
        nameText.text = d.countryName;
        relationText.text = d.relationType.ToString();
        powerText.text = d.powerBalance.ToString();
        territoryText.text = d.hasTerritorialIssue ? "Å¸" : "-";
        historyText.text = d.hasHistoricalIssue ? "Å¸" : "-";
        exportText.text = d.exportRatio.ToString();
        importText.text = d.importRatio.ToString();
    }
}
