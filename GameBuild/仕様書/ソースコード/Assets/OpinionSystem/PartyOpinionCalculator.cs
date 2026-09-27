using UnityEngine;

public class PartyOpinionCalculator : MonoBehaviour
{
   public float CalculateSupport(PoliticalPartyData party)
    {
        float score = 0;

        foreach (var policy in party.policies)
        {
            float opinion = IssueOpinionManager.Instance.GetOpinion(policy.issueName);

            if (policy.stance == PolicyStance.Support)
                score += opinion;

            if (policy.stance == PolicyStance.Oppose)
                score += 1 - opinion;
        }

        return score;
    }   
}
