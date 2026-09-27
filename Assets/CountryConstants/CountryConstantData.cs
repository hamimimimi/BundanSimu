using System;
using UnityEngine;
using System.Collections.Generic;

// CountryConstant関係のDataclass置き場。

[Serializable]
public class CountryConstantData
{
    public string countryName;// 名前

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

    public Dictionary<Vector2Int, MapTile> mapTiles
    = new Dictionary<Vector2Int, MapTile>();

    // 世界地図
    public Dictionary<Vector2Int, MapTile> worldMapTiles
    = new Dictionary<Vector2Int, MapTile>();

}

// 人種割合。生成はしているがほとんど使っていない。仕様を要検討（2026年6月）
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

// 争点からの立ち位置計算用。仕様を検討。（2026年6月）
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
    public string partyId;

    public string districtId;
    
    public bool isAssigned;

    public int coreSupport;
    public int softSupport;
    public string variableKey;
    public int id;

    public int finalVotes;

// 政党と政治家が相互に参照しないためIdから検索用
public PoliticalPartyData GetParty()
{
        if (string.IsNullOrEmpty(partyId))
            return null;
        return CountryConstantManager.Instance.Data.politicalParties
        .Find(p => p.partyId == partyId);
}

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

// 人口規模
public enum PopulationSize
{
    VerySmall,
    Small,
    Medium,
    Large,
    Huge
}
// 年齢層区分。山型・壺型・釣鐘型
public enum PopulationPyramid
{
    Mountain,
    Jar,
    Bell
}

// 産業分布
public enum IndustryType
{
    Primary,    // 第一次産業
    Secondary,  // 第二次産業
    Tertiary    // 第三次産業
}

// 国土区分。首都（一つ）町、村、無人
public enum AreaType
{
    Capital,
    City,
    Rural,
    Empty,
    Sea
}

// 社会制度。2026年６月現在使われていない。制度によって起こるイベント等を検討
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

    public string districtName;　// 2026年6月。未使用。仕様は要検討
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
    public int hideAfterTurns;
    public ElectionView view;


    public Vector2Int startPosition; // 地図の開始地点

    // 座標用

    public List<Vector2Int> cityTiles
    = new();

    public List<Vector2Int> ruralTiles
        = new();

    public List<Vector2Int> emptyTiles
        = new();

}

// 地図用データ。

public class MapTile
{
    // 座標
    public Vector2Int position;
    // 国の名前
    public string countryName;
    // タイルの種類。他国はnull
    public AreaType type;
    // 選挙区ID他国はnull
    public string districtId;
    // 海判定
    public bool isSea; 
}




// 選挙区固有の問題。
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

    public int landCount;// 国の大きさ
    public CountryDistance mapDistance;// 国の距離

    public Vector2Int startPosition;

}

// 距離区分
public enum CountryDistance
{
    Adjacent,       // 隣接
    InRange,        // 枠内
    Partial,        // 一部のみ
    OutOfRange      // 枠外
}



