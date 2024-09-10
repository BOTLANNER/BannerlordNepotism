using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerlordNepotism
{
    public static class MergeKingdomAction
    {
        public static void Apply(Kingdom oldKingdom, Hero newRuler)
        {
            var currentClan = oldKingdom.RulingClan;

            TextObject message2 = new TextObject("{=nepotism_n_04}{THEIR_KINGDOM} has merged into {KINGDOM}.", null);
            message2.SetTextVariable("THEIR_KINGDOM", oldKingdom.Name);
            message2.SetTextVariable("KINGDOM", newRuler.Clan.Kingdom.Name);

            var clans = oldKingdom.Clans.ToList();
            foreach (var clan in clans)
            {
                if (clan.IsClanTypeMercenary)
                {
                    ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(clan, false);
                }
                else
                {
                    //clan.ClanLeaveKingdom(false);
                    //clan.Kingdom = mainHero.Clan.Kingdom;
                    ChangeKingdomAction.ApplyByJoinToKingdom(clan, newRuler.Clan.Kingdom, false);

                    TextObject message = new TextObject("{=nepotism_n_03}{CLAN} has joined {KINGDOM}.", null);
                    message.SetTextVariable("CLAN", clan.Name);
                    message.SetTextVariable("KINGDOM", newRuler.Clan.Kingdom.Name);
                    MBInformationManager.AddQuickInformation(message, 0, null, "");
                }

            }
            Campaign.Current.KingdomManager.AbdicateTheThrone(oldKingdom);
            ChangeKingdomAction.ApplyByJoinToKingdom(currentClan, newRuler.Clan.Kingdom, false);
            oldKingdom.RulingClan = CreateClanAction.Apply(oldKingdom);
            DestroyClanAction.Apply(oldKingdom.RulingClan);

            MBInformationManager.AddQuickInformation(message2, 0, null, "");

            if (!oldKingdom.IsEliminated)
            {
                DestroyKingdomAction.Apply(oldKingdom);
            }

            if (newRuler == Hero.MainHero && PlayerEncounter.Current != null)
            {
                PlayerEncounter.LeaveEncounter = true;
            }
        }
    }
}
