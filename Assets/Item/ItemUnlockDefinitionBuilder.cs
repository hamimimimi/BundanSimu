using UnityEngine;

public class ItemUnlockDefinitionBuilder : MonoBehaviour,
IUnlockDefinitionBuilder
{
    public void Build()
    {
        Create(
            "Demo",
            "ƒfƒ‚æ“±"
        );
    }

    void Create(
        string key,
        string title
    )
    {
        var def =
            ScriptableObject.CreateInstance<UnlockDefinition>();

        def.unlockTag = UnlockTag.Item;
        def.unlockKey = key;
        def.title = title;

        def.requiredPoint = 0;
        def.requiredTags = new UnlockTag[0];

        UnlockDefinitionManager.Instance.Register(def);
    }

}
