using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


public class ElectionView : MonoBehaviour
{
    public PieChartController pieChart;
    public Transform candidateListRoot;
    public GameObject RankItemPrefab;
    public TMP_Text titleText;
    public GameObject pieChartRoot;
    public Transform barGraphRoot;
    public GameObject barItemPrefab;
    
    public void Setup(ElectionDistrictData district)
    {   
        pieChartRoot.SetActive(true);
        barGraphRoot.gameObject.SetActive(false);

        titleText.text = district.districtName;
        var data = new Dictionary<string, int>();

        foreach (Transform child in candidateListRoot)
            Destroy(child.gameObject);


        int index = 0;

        foreach (var c in district.candidates)
        {
            int support = c.coreSupport + c.softSupport;

            var item = Instantiate(RankItemPrefab, candidateListRoot);

            var label = item.GetComponentInChildren<TMP_Text>();
            label.text = $"{c.name} : {support}";

            var img = item.transform.Find("ColorImage").GetComponent<Image>();
            img.color = PieChartController.CandidateColorProvider.GetColor(index);

            data.Add($"{c.name}_{index}", support);

            index++;
        }
        var floatItem = Instantiate(RankItemPrefab, candidateListRoot);

        floatItem.GetComponentInChildren<TMP_Text>().text =
        $"Floating : {district.floatingVoters}";

        floatItem.GetComponentInChildren<Image>().color =
        PieChartController.CandidateColorProvider.GetColor(index);

        data.Add("Floating", district.floatingVoters);

        pieChart.DrawPie(data);
    }

public void SetupResult(ElectionDistrictData district)
{
        gameObject.SetActive(true);

    titleText.text = $"{district.districtName} ëIãìåãâ ";
   
        pieChartRoot.SetActive(false);
        barGraphRoot.gameObject.SetActive(true);

    foreach (Transform child in candidateListRoot)
        Destroy(child.gameObject);

    foreach (Transform child in barGraphRoot)
        Destroy(child.gameObject);

     if (district.candidates == null || district.candidates.Count == 0)
        return;


    // ìäï[êîèáÇ…ï¿Ç—ë÷Ç¶
    var sorted = new List<PoliticianData>(district.candidates);
    sorted.Sort((a, b) => b.finalVotes.CompareTo(a.finalVotes));

    int maxVotes = Mathf.Max(1, sorted[0].finalVotes);
   
    int displayCount = Mathf.Min(5, sorted.Count);
  
     for (int i = 0; i < displayCount; i++)
    {
        var c = sorted[i];

        var rankitem = Instantiate(RankItemPrefab, candidateListRoot);

            var nameText = rankitem.transform.Find("NameText")
                .GetComponent<TMP_Text>();

            var indexText = rankitem.transform.Find("IndexText")
                .GetComponent<TMP_Text>();

            var colorImg = rankitem.transform.Find("ColorImage")
            .GetComponent<Image>();

            nameText.text = c.name;

            indexText.text = (i == 0) ? "ìñëI" : "óéëI";

            colorImg.color =
             PieChartController.CandidateColorProvider.GetColor(i);

            var barItem = Instantiate(barItemPrefab, barGraphRoot);
            
            var fill = barItem.transform.Find("Fill")
                .GetComponent<Image>();

            var voteText = barItem.transform.Find("Fill/VoteText")
                .GetComponent<TMP_Text>();

            float ratio = (float)c.finalVotes / maxVotes;
            fill.fillAmount = ratio;

            voteText.text = $"{c.finalVotes}ï[";

            fill.color =
                PieChartController.CandidateColorProvider.GetColor(i);

     }
}
    public void HideResult()
    {
        pieChartRoot.SetActive(true);
        barGraphRoot.gameObject.SetActive(false);
    }
}