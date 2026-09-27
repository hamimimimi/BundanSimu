using UnityEngine;
using System.Linq;

public class UnlockDefinitionRegistry : MonoBehaviour
{
    void BuildAll()
    {
        var builders = FindObjectsOfType<MonoBehaviour>()
            .OfType<IUnlockDefinitionBuilder>();

        foreach (var builder in builders)
        {
            builder.Build();
        }

        Debug.Log("All UnlockDefinitions built");
    }
}