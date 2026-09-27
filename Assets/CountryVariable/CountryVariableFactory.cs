using UnityEngine;

public static class CountryVariableFactory
{
    public static void CreatePostVariable(UnlockTagKey unlockKey)
    {
        // post•t‚«ƒL[¶¬
        var postKey = new UnlockTagKey
        {
            tag = unlockKey.tag,
            key = unlockKey.key + "_post"
        };

        var def = UnlockDefinitionManager.Instance.Get(postKey.tag, postKey.key);



        CountryVariable variable =
             Create(
                   postKey.tag,
                   postKey.key,
                   def != null ? def.title : postKey.key);


    }


        // Variable¶¬
       public static CountryVariable Create(UnlockTag tag, string key, string displayName)
        {

        // Šù‚É‘¶İ‚·‚é‚È‚çì‚ç‚È‚¢
             
        UnlockTagKey unlockKey =new UnlockTagKey
               {
                    tag = tag,
                    key = key
               };
        var existing =
                CountryVariableManager.Instance.GetByKey(unlockKey);


        if (existing != null)
            return existing;

        var variable = new CountryVariable();

            variable.tag = tag;
            variable.key = key;
            variable.displayName = displayName;
       
        variable.cap = CapManager.Instance.InitialCap;
        
        variable.Initialize();



        // “o˜^
            CountryVariableManager.Instance.Register(variable);

            return variable;
        }





}