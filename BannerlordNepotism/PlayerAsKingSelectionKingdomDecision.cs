using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;

namespace BannerlordNepotism
{
    public class PlayerAsKingSelectionKingdomDecision : KingSelectionKingdomDecision
    {
        public PlayerAsKingSelectionKingdomDecision(Clan proposerClan, Clan? clanToExclude = null) : base(proposerClan, clanToExclude)
        {

        }

        public override IEnumerable<DecisionOutcome> DetermineInitialCandidates()
        {
            yield return new KingSelectionKingdomDecision.KingSelectionDecisionOutcome(Hero.MainHero);
        }

    }
}
