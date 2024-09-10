using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;

namespace BannerlordNepotism
{
    public class JoinKingdomBarterable : JoinKingdomAsClanBarterable
    {

        public override string StringID
        {
            get
            {
                return "nepotism_join_kingdom_barterable";
            }
        }

        public JoinKingdomBarterable(Hero owner, Kingdom targetKingdom) : base(owner, targetKingdom, true)
        {
        }

        public override void Apply()
        {
            if (OriginalOwner.Clan == null)
            {
                JoinKingdomAction.Apply(TargetKingdom, OriginalOwner);
            }
            else
            {
                base.Apply();
            }
        }

        public override int GetUnitValueForFaction(IFaction factionForEvaluation)
        {
            if (OriginalOwner.Clan == null)
            {
                return -10_000;
            }

            int baseVal = base.GetUnitValueForFaction(factionForEvaluation);

            return baseVal / 250;
        }
    }
}
