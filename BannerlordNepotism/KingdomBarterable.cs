
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordNepotism
{
    public abstract class KingdomBarterable : Barterable
    {
        public override TextObject Name
        {
            get
            {
                return OriginalOwner.Clan.Kingdom.Name;
            }
        }


        public KingdomBarterable(Hero owner, PartyBase ownerParty) : base(owner, ownerParty)
        {
        }

        public override int GetUnitValueForFaction(IFaction faction)
        {
            //if (faction is Clan clan)
            //{
            //    var newLeader = clan.Leader;
            //    if (newLeader != null)
            //    {
            //        float single1 = (float) newLeader.RandomInt(10000);
            //        float single2 = (float) (newLeader.RandomInt(-25000, 25000) + OriginalOwner.RandomInt(-25000, 25000));
            //        if (newLeader == Hero.MainHero)
            //        {
            //            single2 = 0f;
            //            single1 = 0f;
            //        }
            //        float relation = (float) (newLeader.GetRelation(OriginalOwner) * 1000);
            //        Campaign.Current.Models.DiplomacyModel.GetHeroCommandingStrengthForClan(newLeader);
            //        Campaign.Current.Models.DiplomacyModel.GetHeroCommandingStrengthForClan(OriginalOwner);
            //        float single3 = (newLeader.Clan == null ? 0f : (float) newLeader.Clan.Tier + (newLeader.Clan.Leader == newLeader.MapFaction.Leader ? MathF.Min(3f, (float) newLeader.MapFaction.Fiefs.Count / 10f) + 0.5f : 0f));
            //        float single4 = (OriginalOwner.Clan == null ? 0f : (float) OriginalOwner.Clan.Tier + (OriginalOwner.Clan.Leader == OriginalOwner.MapFaction.Leader ? MathF.Min(3f, (float) OriginalOwner.MapFaction.Fiefs.Count / 10f) + 0.5f : 0f));
            //        float single5 = (faction == newLeader.Clan ? (single4 - single3) * MathF.Abs(single4 - single3) * 1000f : (single3 - single4) * MathF.Abs(single3 - single4) * 1000f);
            //        int relationBetweenClans = FactionManager.GetRelationBetweenClans(OriginalOwner.Clan, newLeader.Clan);
            //        int num = 1000 * relationBetweenClans;
            //        Clan clanAfterMarriage = Campaign.Current.Models.MarriageModel.GetClanAfterMarriage(OriginalOwner, newLeader);
            //        float valueOfHeroForFaction = 0f;
            //        float valueOfHeroForFaction1 = 0f;
            //        if (clanAfterMarriage != OriginalOwner.Clan)
            //        {
            //            if (faction == clanAfterMarriage)
            //            {
            //                valueOfHeroForFaction = Campaign.Current.Models.DiplomacyModel.GetValueOfHeroForFaction(OriginalOwner, clanAfterMarriage, true);
            //            }
            //            else if (faction == OriginalOwner.Clan)
            //            {
            //                valueOfHeroForFaction = Campaign.Current.Models.DiplomacyModel.GetValueOfHeroForFaction(OriginalOwner, OriginalOwner.Clan, true);
            //            }
            //            if (clanAfterMarriage.Kingdom != null && clanAfterMarriage.Kingdom != OriginalOwner.Clan!.Kingdom)
            //            {
            //                valueOfHeroForFaction1 = Campaign.Current.Models.DiplomacyModel.GetValueOfHeroForFaction(OriginalOwner, clanAfterMarriage.Kingdom, true);
            //            }
            //        }
            //        float single6 = 2f * MathF.Min(0f, 20f - MathF.Max(OriginalOwner.Age - 18f, 0f)) * MathF.Min(0f, 20f - MathF.Max(OriginalOwner.Age - 18f, 0f)) * MathF.Min(0f, 20f - MathF.Max(OriginalOwner.Age - 18f, 0f));
            //        return (int) (-50000f + single1 + single2 + relation + valueOfHeroForFaction + (float) num + valueOfHeroForFaction1 + single5 + single6);
            //    }

            //}

            float v = OriginalOwner.Clan.CalculateTotalSettlementValueForFaction(OriginalOwner.Clan.Kingdom);
            return (int) -v / 30;
        }

        public override ImageIdentifier GetVisualIdentifier()
        {
            if (OriginalOwner?.Clan?.Kingdom?.Banner != null)
            {
                return new ImageIdentifier(OriginalOwner.Clan.Kingdom.Banner);
            }

            return new ImageIdentifier(CharacterCode.CreateFrom(this.OriginalOwner!.CharacterObject));
        }

        public override void CheckBarterLink(Barterable linkedBarterable)
        {
            if (linkedBarterable.GetType() == typeof(KingdomBarterable) && linkedBarterable.OriginalOwner == base.OriginalOwner)
            {
                base.AddBarterLink(linkedBarterable);
            }
        }

        public override string GetEncyclopediaLink()
        {
            if (OriginalOwner?.Clan?.Kingdom != null)
            {
                return OriginalOwner.Clan.Kingdom.EncyclopediaLink;
            }
            return this.OriginalOwner!.EncyclopediaLink;
        }
    }
}
