using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;

namespace BannerlordNepotism
{
    public class RuleKingdomBarterable : KingdomBarterable
    {

        public override string StringID
        {
            get
            {
                return "nepotism_rule_kingdom_barterable";
            }
        }

        public RuleKingdomBarterable(Hero owner, PartyBase ownerParty) : base(owner, ownerParty)
        {
        }

        public override void Apply()
        {
            Action<Hero, Hero, List<Barterable>> applyActual = (offererHero, otherHero, barters) =>
            {
                if (barters.Contains(this))
                {
                    CampaignEvents.OnBarterAcceptedEvent.ClearListeners(this);

                    Hero other = offererHero == this.OriginalOwner ? otherHero : offererHero;

                    RuleKingdomAction.Apply(OriginalOwner.Clan.Kingdom, other);
                }
            };
            CampaignEvents.OnBarterAcceptedEvent.AddNonSerializedListener(this, applyActual);
        }
    }
}
