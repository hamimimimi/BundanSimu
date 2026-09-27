using System.Collections.Generic;
using UnityEngine;

public class IssueDefinitionManager : MonoBehaviour
{
    public List<IssueDefinition> definitions = new();

    public void Add(IssueDefinition def)
    {
        definitions.Add(def);
    }

    public List<IssueDefinition> GetAll()
    {
        return definitions;
    }
}