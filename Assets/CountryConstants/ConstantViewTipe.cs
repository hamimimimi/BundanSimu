
// ConstntView用のTag。増やす際に追加。最終的なレイアウトが決まり次第不要なものは消す。

public enum ConstantViewType
{
    Population,                 // 総人口
    PopulationPyramid,          // 人口年齢分布
    Government,                 // 政府
    Ethnic,                     // 民族
    PoliticalPartySummary,      // 政党名 + 支持者 + 政治家
    PoliticalPartyPolicies,     // 特定政党の政策一覧
    LandSummary,                // 土地合計・種類別数
    ForeignCountryList,         // 関係国一覧
    ForeignCountryDetail        // 関係国1つの詳細
}
