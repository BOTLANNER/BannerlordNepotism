
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerlordNepotism
{
    public static class RuleKingdomAction
    {
        public static void Apply(Kingdom kingdom, Hero newRuler)
        {
            Clan clan = newRuler.Clan;
            Clan rulingClan = kingdom.RulingClan;

            ChangeRulingClanAction.Apply(kingdom, clan);
            KingSelectionKingdomDecision kingSelectionKingdomDecision = new PlayerAsKingSelectionKingdomDecision(clan, rulingClan)
            {
                IsEnforced = true
            };
            kingdom.AddDecision(kingSelectionKingdomDecision, true);

            // Just to be sure.
            kingdom.RulingClan = newRuler.Clan;

            TextObject message2 = new TextObject("{=nepotism_n_05}{THEIR_KINGDOM} is now ruled by {CLAN}.", null);
            message2.SetTextVariable("THEIR_KINGDOM", kingdom.Name);
            message2.SetTextVariable("CLAN", newRuler.Clan.Name);
            MBInformationManager.AddQuickInformation(message2, 0, null);



            if (newRuler == Hero.MainHero && PlayerEncounter.Current != null)
            {
                PlayerEncounter.LeaveEncounter = true;
            }
        }
    }
}
