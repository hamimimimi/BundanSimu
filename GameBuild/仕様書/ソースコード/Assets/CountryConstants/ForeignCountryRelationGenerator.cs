using UnityEngine;
using System.Collections.Generic;

public static class ForeignCountryRelationGenerator
{
    public static List<ForeignCountryRelationData> Generate()
    {
        int count = Random.Range(5, 11);

        var relations = new List<ForeignCountryRelationData>();
        int remainingExport = 100;
        int remainingImport = 100;

        for (int i = 0; i < count; i++)
        {
            int export = (i == count - 1)
                ? remainingExport
                : Random.Range(5, remainingExport - (count - i - 1) * 5);

            int import = (i == count - 1)
                ? remainingImport
                : Random.Range(5, remainingImport - (count - i - 1) * 5);

            remainingExport -= export;
            remainingImport -= import;

            relations.Add(new ForeignCountryRelationData
            {
                countryName = $"Country_{i + 1}",

                relationType = Random.value < 0.6f
                    ? RelationType.Friendly
                    : RelationType.Hostile,

                powerBalance = (PowerBalance)Random.Range(0, 3),

                hasTerritorialIssue = Random.value < 0.3f,
                hasHistoricalIssue = Random.value < 0.4f,

                exportRatio = export,
                importRatio = import
            });
        }

        return relations;
    }
}
