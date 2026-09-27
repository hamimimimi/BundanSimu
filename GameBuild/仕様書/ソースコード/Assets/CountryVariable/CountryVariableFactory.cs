using UnityEngine;

public static class CountryVariableFactory
{
    public static void CreatePostVariable(UnlockTagKey unlockKey)
    {
        // post付きキー生成
        var postKey = new UnlockTagKey
        {
            tag = unlockKey.tag,
            key = unlockKey.key + "_post"
        };

        // 既に存在するなら作らない
        var existing =
            CountryVariableManager.Instance.GetByKey(postKey);

        if (existing != null)
            return;

        // CountryVariable生成
        CountryVariable variable = new CountryVariable();

        variable.tag = postKey.tag;
        variable.key = postKey.key;

        // 初期値（必要なら調整）
        variable.yes = 0;
        variable.no = 0;
        variable.neutral = 100;

        variable.debatePoint = 0;
        variable.debateLevel = 1;

        // 登録
        CountryVariableManager.Instance.Register(variable);

        Debug.Log($"CountryVariable created : {postKey.tag} / {postKey.key}");
    }
}