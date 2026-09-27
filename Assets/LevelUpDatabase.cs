using UnityEngine;
using System.Collections.Generic;


// ƒŒƒxƒ‹ƒAƒbƒv‚Æ–¼‘O
public class LevelUpDatabase : MonoBehaviour
{
    public static List<LevelUpChoice> Create()
        {
        return new List<LevelUpChoice>()
        {
            new LevelUpChoice(
            "˜bIÒ",
            "ŠgU—Í‹­‰» +1",
            StatusTagType.InfluencePoint,
            1),
            new LevelUpChoice(
            "l–]",
            "M—p—¦‹­‰» +1",
            StatusTagType.CreditRatio,
            1),
            new LevelUpChoice(
            "Td",
            "ŠëŒ¯“xŒ¸­ +1",
            StatusTagType.RiskLevel,
            1),
            new LevelUpChoice(
            "’n–¨",
            "î•ñ—Í‹­‰» +1",
            StatusTagType.InformationPoint,
            1),
            new LevelUpChoice(
            "¤Ë",
            "‘‹àŠl“¾ +1",
            StatusTagType.Money,
            1),
            new LevelUpChoice(
            "‚Â‚Ü‚İH‚¢",
            "ŠgU—Í‹­‰» +2\nM—p—¦‹­‰» -1",
            StatusTagType.InfluencePoint,
            2,
            StatusTagType.CreditRatio,
            -1),
            new LevelUpChoice(
            "“Åã‰Æ",
            "ŠgU—Í‹­‰» +2\nŠëŒ¯“xŒ¸­ -1",
            StatusTagType.InfluencePoint,
            2,
            StatusTagType.RiskLevel,
            -1),
            new LevelUpChoice(
            "“`“¹t",
            "ŠgU—Í‹­‰» +2\nî•ñ—Í‹­‰» -1",
            StatusTagType.InfluencePoint,
            2,
            StatusTagType.InformationPoint,
            -1),
            new LevelUpChoice(
            "é“`‰®",
            "ŠgU—Í‹­‰» +2\n‘‹àŠl“¾ -1",
            StatusTagType.InfluencePoint,
            2,
            StatusTagType.Money,
            -1),
            new LevelUpChoice(
            "˜_‹q",
            "M—p—¦‹­‰» +2\nŠgU—Í‹­‰» -1",
            StatusTagType.CreditRatio,
            2,
            StatusTagType.InfluencePoint,
            -1),
            new LevelUpChoice(
            "ƒGƒŠ[ƒg",
            "M—p—¦‹­‰» +2\nŠëŒ¯“xŒ¸­ -1",
            StatusTagType.CreditRatio,
            2,
            StatusTagType.RiskLevel,
            -1),
            new LevelUpChoice(
            "ŒÇ‚",
            "M—p—¦‹­‰» +2\nî•ñ—Í‹­‰» -1",
            StatusTagType.CreditRatio,
            2,
            StatusTagType.InformationPoint,
            -1),
            new LevelUpChoice(
            "‘¾ŒÛ‚¿",
            "M—p—¦‹­‰» +2\n‘‹àŠl“¾ -1",
            StatusTagType.CreditRatio,
            2,
            StatusTagType.Money,
            -1),
            new LevelUpChoice(
            "Td”h",
            "ŠëŒ¯“xŒ¸­ +2\nŠgU—Í‹­‰» -1",
            StatusTagType.RiskLevel,
            2,
            StatusTagType.InfluencePoint,
            -1),
            new LevelUpChoice(
            "•—Œ©Œ{",
            "ŠëŒ¯“xŒ¸­ +2\nM—p—¦‹­‰» -1",
            StatusTagType.RiskLevel,
            2,
            StatusTagType.CreditRatio,
            -1),
            new LevelUpChoice(
            "•·‚«ãè",
            "ŠëŒ¯“xŒ¸­ +2\nî•ñ—Í‹­‰» -1",
            StatusTagType.RiskLevel,
            2,
            StatusTagType.InformationPoint,
            -1),
            new LevelUpChoice(
            "—pS–_",
            "ŠëŒ¯“xŒ¸­ +2\n‘‹àŠl“¾ -1",
            StatusTagType.RiskLevel,
            2,
            StatusTagType.Money,
            -1),
            new LevelUpChoice(
            "•·‚«ê",
            "î•ñ—Í‹­‰» +2\nŠgU—Í‹­‰» -1",
            StatusTagType.InformationPoint,
            2,
            StatusTagType.InfluencePoint,
            -1),
            new LevelUpChoice(
            "–³ß‘€",
            "î•ñ—Í‹­‰» +2\nM—p—¦‹­‰» -1",
            StatusTagType.InformationPoint,
            2,
            StatusTagType.CreditRatio,
            -1),
            new LevelUpChoice(
            "’²¸ˆõ",
            "î•ñ—Í‹­‰» +2\nŠëŒ¯“xŒ¸­ -1",
            StatusTagType.InformationPoint,
            2,
            StatusTagType.RiskLevel,
            -1),
            new LevelUpChoice(
            "–§’ã",
            "î•ñ—Í‹­‰» +2\n‘‹àŠl“¾ -1",
            StatusTagType.InformationPoint,
            2,
            StatusTagType.Money,
            -1),
            new LevelUpChoice(
            "ˆÍ‚¢‚İ",
            "‘‹àŠl“¾ +2\nŠgU—Í‹­‰» -1",
            StatusTagType.Money,
            2,
            StatusTagType.InfluencePoint,
            -1),
            new LevelUpChoice(
            "‘Šêt",
            "‘‹àŠl“¾ +2\nM—p—¦‹­‰» -1",
            StatusTagType.Money,
            2,
            StatusTagType.CreditRatio,
            -1),
            new LevelUpChoice(
            "‹­—~",
            "‘‹àŠl“¾ +2\nŠëŒ¯“xŒ¸­ -1",
            StatusTagType.Money,
            2,
            StatusTagType.RiskLevel,
            -1),
            new LevelUpChoice(
            "ç‘K“z",
            "‘‹àŠl“¾ +2\nî•ñ—Í‹­‰» -1",
            StatusTagType.Money,
            2,
            StatusTagType.InformationPoint,
            -1),


        };

}

}
