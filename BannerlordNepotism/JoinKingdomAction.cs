
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerlordNepotism
{
    public static class JoinKingdomAction
    {
        public static void Apply(Kingdom kingdom, Hero newMember)
        {
            if (newMember.Occupation != Occupation.Lord)
            {
                newMember.SetNewOccupation(Occupation.Lord);
            }
            if (newMember.Clan == null)
            {
                newMember.Clan = kingdom.Leader.Clan;
            }
            else
            {
                newMember.Clan.ClanLeaveKingdom(false);
                newMember.Clan.Kingdom = kingdom;

                TextObject message = new TextObject("{=nepotism_n_02}{FAMILY_MEMBER} and {CLAN} has joined {KINGDOM}.", null);
                message.SetTextVariable("FAMILY_MEMBER", newMember.Name);
                message.SetTextVariable("CLAN", newMember.Clan.Name);
                message.SetTextVariable("KINGDOM", kingdom.Name);
                MBInformationManager.AddQuickInformation(message, 0, newMember.CharacterObject, "");
            }

            if (kingdom.Leader == Hero.MainHero && PlayerEncounter.Current != null)
            {
                PlayerEncounter.LeaveEncounter = true;
            }
        }
    }
}
