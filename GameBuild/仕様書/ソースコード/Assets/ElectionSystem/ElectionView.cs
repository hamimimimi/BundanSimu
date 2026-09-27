using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


public class ElectionView : MonoBehaviour
{
    public PieChartController pieChart;
    public TMP_Text candidateText;

    public void Setup(ElectionDistrictData district)
    {
        var data = new Dictionary<string, int>();
        
       
        int index = 0;

        foreach (var c in district.candidates)
        {
            int support = c.coreSupport + c.softSupport;
            data.Add($"{c.name}_{index}", support);
            index++;
        }

        data.Add("Floating", district.floatingVoters);

        pieChart.DrawPie(data);

        var slices = pieChart.pieRoot.GetComponentsInChildren<Image>();

        for (int i = 0; i < slices.Length; i++)
        {
             slices[i].color =
             PieChartController.CandidateColorProvider.GetColor(i);
        }
        string text = "";

        foreach (var c in district.candidates)
        {
            int support = c.coreSupport + c.softSupport;
            text += $"{c.name} : {support}\n";
        }

        text += $"Floating : {district.floatingVoters}";

        candidateText.text = text;
       
    }
}