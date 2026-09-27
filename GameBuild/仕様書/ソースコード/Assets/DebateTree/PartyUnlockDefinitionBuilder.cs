using UnityEngine;

public class PartyUnlockDefinitionBuilder : MonoBehaviour, IUnlockDefinitionBuilder
{
    [SerializeField] int baseCost = 10;

   
   public void Build()
    {
        Debug.Log("Build Unlock Definitions");
        var data = CountryConstantManager.Instance.Data;

       
        foreach (var party in data.politicalParties)
        {

            // 投稿用
            Create(party, "_post");

            // 表示用
            Create(party, "_view");
        }

       

        Debug.Log("定義追加した");
    }
    void Create(PoliticalPartyData party, string suffix)
    {
        if (party == null)
        {
            Debug.LogError("party is null!");
        }

        if (UnlockDefinitionManager.Instance == null)
        {
            Debug.LogError("UnlockDefinitionManager is null!");
            return;
        }

        var def = ScriptableObject.CreateInstance<UnlockDefinition>();
            
                def.unlockTag = UnlockTag.PoliticalParty;
                def.unlockKey = party.partyId + suffix;           //  string ID
                def.title = party.partyName + suffix;
                def.requiredPoint = baseCost;
                def.requiredTags = new UnlockTag[0];
            

            UnlockDefinitionManager.Instance.Register(def);
      }
}
