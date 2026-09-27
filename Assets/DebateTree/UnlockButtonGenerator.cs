using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public class UnlockButtonGenerator : MonoBehaviour
{
    [Header("Setup")]
    public UnlockButton buttonPrefab;
    public Transform parent;

    [System.Serializable]
    public class UnlockButtonContainer
    {
        public UnlockTag tag;

        public Transform parent;
        
    }

    public UnlockButtonContainer[] containers;

    public HashSet<string> createdKeys = new();

    Transform GetParent(UnlockTag tag)
    {
        foreach (var c in containers)
        {
            if (c.tag == tag)
                return c.parent;
        }
        return parent;
    }

    public void Generate()
    {
        var manager = UnlockDefinitionManager.Instance;

        if (manager == null)
        {
            Debug.LogError("Manager null");
            return;
        }

        var defs = manager.GetAll();

        Debug.Log("Total defs: " + defs.Count);


        foreach (var def in defs)
        {
            if (def.keyVariants == null || def.keyVariants.Length == 0)
            {
                CreateButton(def, def.unlockKey);
                continue;
            }

            foreach (var variant in def.keyVariants)
            {
                CreateButton(def, def.unlockKey + variant);
            }
        }
    }
        void CreateButton(UnlockDefinition def, string fullKey)
        {    
        
           string unique = def.unlockTag + ":" + fullKey;

        //  既に作成済みならスキップ
        if (createdKeys.Contains(unique))
            return;

        createdKeys.Add(unique);

            var targetParent = GetParent(def.unlockTag);

            var button = Instantiate(buttonPrefab, targetParent);

            button.unlockKey = new UnlockTagKey
            {
                tag = def.unlockTag,
                key = fullKey
            };

            button.requiredUnlockedKeys = 
            (def.requiredTags ?? System.Array.Empty<UnlockTag>())
                .Select(t => new UnlockTagKey { tag = t, key = "" })
                .ToArray();

            button.titleText.text = def.title;
            button.requiredPoint = def.requiredPoint;
            button.conditionText.text = def.description;
        }
    }

