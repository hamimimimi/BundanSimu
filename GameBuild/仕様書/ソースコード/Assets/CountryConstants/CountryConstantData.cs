using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class CountryConstantData
{
    // 人口
    public long population;               // 実数値
    public PopulationSize sizeCategory;    // 極小〜巨大

    // 人口形態
    public PopulationPyramid pyramidType;  // Mountain / Jar / Bell

    // 割合（合計100）
    public int youngPercent;
    public int adultPercent;
    public int oldPercent;

    // 実数人口
    public long youngPopulation;
    public long adultPopulation;
    public long oldPopulation;

    // 政体
    public GovernmentType governmentType;
 
    // 人種
    public bool isDiverse;
    public List<EthnicGroup> ethnicGroups = new List<EthnicGroup>();

    // 国際関係
    public List<ForeignCountryRelationData> foreignRelations;
    
    // 政治家
    public float politicalInterest;
    public List<PoliticalPartyData> politicalParties;
    public int independentCount;

    // すべての政治家
    public List<PoliticianData> allPoliticians = new();
    // 土地
    public int totalLandCount;
    public List<IndustryPopulationData> industryPopulations;
    public List<LandAreaData> landAreas;

    // 選挙区
    public List<ElectionDistrictData> districts;
}

[Serializable]
public class EthnicGroup
{
    public string name;
    public int percentage;
}

// 政党
[Serializable]
public class PoliticalPartyData
{
    public string partyId;
    public string partyName;
    public int supporterCount;
    public int coreSupporters;
    public int softSupporters;

    public List<PolicyStanceData> policies = new();
    public List<PoliticianData> politicians = new();
}
public enum PolicyStance
{
    Oppose = -1,
    Neutral = 0,
    Support = 1
}

// 争点
[Serializable]
public class PolicyStanceData
{
    public string issueName;   // CountryVariable と同名
    public PolicyStance stance;
}

// 政治家
[Serializable]
public class PoliticianData
{
    public string name;
    public PoliticalPartyData party;

    public string districtId;
    
    public bool isAssigned;

    public int coreSupport;
    public int softSupport;
}

// 国土
[Serializable]
public class LandAreaData
{
    public AreaType type;
}

// 産業人口
[Serializable]
public class IndustryPopulationData
{
    public IndustryType industry;
    public int population;
}


public enum PopulationSize
{
    VerySmall,
    Small,
    Medium,
    Large,
    Huge
}

public enum PopulationPyramid
{
    Mountain,
    Jar,
    Bell
}

public enum IndustryType
{
    Primary,    // 第一次産業
    Secondary,  // 第二次産業
    Tertiary    // 第三次産業
}

public enum AreaType
{
    Capital,
    City,
    Rural,
    Empty
}

public enum GovernmentType
{
    ConstitutionalMonarchy,   // 立憲君主制
    Socialism,                // 社会主義体制
    PresidentialRepublic,     // 大統領制
    Monarchy,                 // 君主制
    Dictatorship,             // 独裁体制
    Federalism                // 連邦制
}

// 選挙区

[Serializable]
public class ElectionDistrictData
{
    public string districtId;

    public int capacity;

    public int cityCount;
    public int ruralCount;
    public int emptyCount;

    public long totalPopulation;
    public long voterPopulation;
    
    public PoliticianData leader;
    public List<ElectionIssueData> issues;
    
    public List<PoliticianData> candidates;
　　public int turnsToElection;
　　public bool isAnnounced;
    public int floatingVoters; 
}

[System.Serializable]
public class ElectionIssueData
{
    public string issueId;

    // この選挙区での重要度（重み）
    public float weight;
}




// 関係国関係
public enum RelationType { Friendly, Hostile }
public enum PowerBalance { Superior, Equal, Inferior }

[Serializable]
public class ForeignCountryRelationData
{
    
    public string countryName;

    public RelationType relationType;
    public PowerBalance powerBalance;


    public bool hasTerritorialIssue;
    public bool hasHistoricalIssue;

    public int exportRatio; // 合計100
    public int importRatio; // 合計100
}



