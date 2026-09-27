using UnityEngine;
using System.Linq;

public class UnlockDefinitionRegistry : MonoBehaviour
{
    void BuildAll()
    {
        var builders = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IUnlockDefinitionBuilder>();

        foreach (var builder in builders)
        {
            builder.Build();
        }

        Debug.Log("All UnlockDefinitions built");
    }
}