using UnityEngine;

// CountryVariable用の表示Tag
public enum DebateTag
{
    None,       // 影響なし
    OptionA,
    OptionB,
    OptionC,
    OptionD,
}

// LargeScaleData用の表示Tag
public enum LargeScaleType
{
    Economy,
    Military,
    Environment
}

// すべてのUnlockButtonで開放されるんののTag
public enum UnlockTag
{
    None,

    // Variable 解放
    DebateEconomy,
    DebateMilitary,
    DebateEnvironment,

    // Constant 解放
    PopulationInfo,
    GovernmentInfo,
    Landinfo,

    // 機能解放
    PolicySystem,
    ElectionSystem,

    ForeignCountryInfo,
PoliticalPartyInfo,
    PoliticalParty,
    Issue,
    ForeignCountry,
    Politician,
    District,
    DistrictIssue,
    ElectionIssue,
    Election
}




