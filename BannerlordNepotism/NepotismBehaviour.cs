using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordNepotism
{
    public class NepotismBehaviour : CampaignBehaviorBase
    {
        Color Error = new(178 * 255, 34 * 255, 34 * 255);
        Color Warn = new (189 * 255, 38 * 255, 0);

        #region Overrides
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, this.DailyTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
        #endregion

        #region Event Handlers

        private void DailyTick()
        {
            try
            {
                foreach (var kingdom in Kingdom.All.ToList())
                {
                    try
                    {
                        if (kingdom != null && kingdom.Leader != null)
                        {
                            if (Main.Settings!.PatchMembershipIssues && kingdom.Leader.Clan != kingdom.RulingClan)
                            {
                                var wrongClan = kingdom.Leader.Clan;
                                kingdom.Leader.Clan = kingdom.RulingClan;
                                if (Main.Settings!.VerboseMessages)
                                {
                                    InformationManager.DisplayMessage(new InformationMessage($"FIX: {kingdom.Name.ToString()} leader ({kingdom.Leader.Name.ToString()}) was not in ruling clan {kingdom.RulingClan.Name.ToString()} (was in {wrongClan.Name.ToString()})", Warn));
                                }
                            }

                            if (Main.Settings!.PatchDeadLeaderIssues && !kingdom.IsEliminated && kingdom.Leader.IsDead)
                            {
                                var oldLeader = kingdom.Leader;
                                ChangeClanLeaderAction.ApplyWithoutSelectedNewLeader(kingdom.RulingClan);
                                if (Main.Settings!.VerboseMessages)
                                {
                                    InformationManager.DisplayMessage(new InformationMessage($"FIX: {kingdom.Name.ToString()} leader ({oldLeader.Name.ToString()}) was dead. Elected new leader {kingdom.Leader.Name.ToString()}", Warn)); 
                                }
                            }
                        }

                        if (kingdom != null && kingdom.RulingClan != null)
                        {
                            if (Main.Settings!.PatchMembershipIssues && kingdom.RulingClan.Kingdom != null && kingdom.RulingClan.Kingdom != kingdom)
                            {
                                Campaign.Current.KingdomManager.AbdicateTheThrone(kingdom);
                                if (kingdom.RulingClan.Kingdom != null && kingdom.RulingClan.Kingdom != kingdom)
                                {
                                    kingdom.RulingClan = CreateClan(kingdom);
                                    DestroyClanAction.Apply(kingdom.RulingClan);
                                    if (Main.Settings!.VerboseMessages)
                                    {
                                        InformationManager.DisplayMessage(new InformationMessage($"FIX: {kingdom.Name.ToString()} ruling clan was incorrect. Elected new ruling clan {kingdom.RulingClan.Name.ToString()}", Warn)); 
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        Debug.SetCrashReportCustomString(e.Message);
                        Debug.SetCrashReportCustomStack(e.StackTrace);
                        InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Error));
                    }
                }

                foreach (var clan in Clan.All.ToList())
                {
                    try
                    {
                        if (clan != null && clan.Leader != null)
                        {
                            if (Main.Settings!.PatchMembershipIssues && clan.Leader.Clan != clan)
                            {
                                var wrongClan = clan.Leader.Clan;
                                clan.Leader.Clan = clan;
                                if (Main.Settings!.VerboseMessages)
                                {
                                    InformationManager.DisplayMessage(new InformationMessage($"FIX: {clan.Name.ToString()} leader ({clan.Leader.Name.ToString()}) was not in the clan (was in {wrongClan.Name.ToString()})", Warn)); 
                                }
                            }

                            if (Main.Settings!.PatchDeadLeaderIssues && !clan.IsEliminated && clan.Leader.IsDead)
                            {
                                var oldLeader = clan.Leader;
                                ChangeClanLeaderAction.ApplyWithoutSelectedNewLeader(clan);
                                if (Main.Settings!.VerboseMessages)
                                {
                                    InformationManager.DisplayMessage(new InformationMessage($"FIX: {clan.Name.ToString()} leader ({oldLeader.Name.ToString()}) was dead. Elected new leader {clan.Leader.Name.ToString()}", Warn)); 
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        Debug.SetCrashReportCustomString(e.Message);
                        Debug.SetCrashReportCustomStack(e.StackTrace);
                        InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Error));
                    }
                }
            }
            catch (Exception e)
            {
                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
                InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Error));
            }
        }

        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            if (Main.Settings!.AddJoinClan)
            {
                AddJoinClanDialogs(starter); 
            }
            if (Main.Settings!.AddJoinKingdom)
            {
                AddJoinKingdomDialogs(starter); 
            }
            if (Main.Settings!.AddMergeKingdoms)
            {
                AddMergeKingdomDialogs(starter); 
            }
            if (Main.Settings!.AddRuleKingdom)
            {
                AddRuleKingdomDialogs(starter); 
            }
        }
        #endregion

        #region Private Methods
        private void AddJoinClanDialogs(CampaignGameStarter starter)
        {
            starter.AddPlayerLine("hero_join_clan_ask_nepotism", "hero_main_options", "hero_join_clan_reply_nepotism", "{JOIN_CLAN_ASK_NEPOTISM}", new ConversationSentence.OnConditionDelegate(JoinClanCondition), null, 100, JoinClanClickable, null);
            starter.AddDialogLine("hero_join_clan_reply_nepotism", "hero_join_clan_reply_nepotism", "close_window", "{JOIN_CLAN_RESP_NEPOTISM}", new ConversationSentence.OnConditionDelegate(JoinClanResponse), new ConversationSentence.OnConsequenceDelegate(JoinClanConsequence), 100, null);
        }

        private bool JoinClanClickable(out TextObject explanation)
        {
            Hero mainHero = Hero.MainHero;
            Hero other = Hero.OneToOneConversationHero;

            if (Main.Settings!.RequireRelationshipToJoinClan)
            {
                var relationship = other.GetRelationWithPlayer();
                if (relationship < Main.Settings!.RequiredRelationshipToJoinClan)
                {
                    explanation = new TextObject("{=nepotism_h_01}Nepotism: Relationship ({RELATION}) is not high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToJoinClan);
                    return false;
                }
                else
                {
                    explanation = new TextObject("{=nepotism_h_02}Nepotism: Relationship ({RELATION}) is high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToJoinClan);
                }
            }
            else
            {
                explanation = null;
            }
            return true;
        }

        public bool JoinClanCondition()
        {
            Hero mainHero = Hero.MainHero;
            Hero conversationHero = Hero.OneToOneConversationHero;
            bool isMainHeroFamilyInOtherClan = false;
            bool isClanleader = conversationHero.Clan != null && conversationHero.Clan.Leader == conversationHero;
            if ((Main.Settings!.DirectFamilyOnly ? mainHero.IsFamilyOf(conversationHero) : mainHero.IsRelatedTo(conversationHero)) && conversationHero.Clan != mainHero.Clan)
            {
                isMainHeroFamilyInOtherClan = true;
            }
            TextObject textObject = new TextObject("{=nepotism_01}Join our clan, the family needs you.", null);
            MBTextManager.SetTextVariable("JOIN_CLAN_ASK_NEPOTISM", textObject.ToString(), false);
            return (isMainHeroFamilyInOtherClan && !isClanleader);
        }

        public bool JoinClanResponse()
        {
            MBTextManager.SetTextVariable("JOIN_CLAN_RESP_NEPOTISM", (new TextObject("{=nepotism_02}As you wish.", null)).ToString(), false);
            return true;
        }

        public void JoinClanConsequence()
        {
            Hero mainHero = Hero.MainHero;
            Hero conversationHero = Hero.OneToOneConversationHero;
            if (conversationHero.Occupation != Occupation.Lord)
            {
                conversationHero.SetNewOccupation(Occupation.Lord);
            }
            conversationHero.Clan = mainHero.Clan;

            TextObject message = new TextObject("{=nepotism_n_01}{FAMILY_MEMBER} has joined {CLAN}.", null);
            message.SetTextVariable("FAMILY_MEMBER", conversationHero.Name);
            message.SetTextVariable("CLAN", mainHero.Clan.Name);
            MBInformationManager.AddQuickInformation(message, 0, conversationHero.CharacterObject, "");
        }

        private void AddJoinKingdomDialogs(CampaignGameStarter starter)
        {
            starter.AddPlayerLine("hero_join_kingdom_ask_nepotism", "hero_main_options", "hero_join_kingdom_reply_nepotism", "{JOIN_KINGDOM_ASK_NEPOTISM}", new ConversationSentence.OnConditionDelegate(JoinKingdomCondition), null, 100, JoinKingdomClickable, null);
            starter.AddDialogLine("hero_join_kingdom_reply_nepotism", "hero_join_kingdom_reply_nepotism", "close_window", "{JOIN_KINGDOM_RESP_NEPOTISM}", new ConversationSentence.OnConditionDelegate(JoinKingdomResponse), new ConversationSentence.OnConsequenceDelegate(JoinKingdomConsequence), 100, null);
        }

        private bool JoinKingdomClickable(out TextObject explanation)
        {
            Hero mainHero = Hero.MainHero;
            Hero other = Hero.OneToOneConversationHero;

            if (Main.Settings!.RequireRelationshipToJoinKingdom)
            {
                var relationship = other.GetRelationWithPlayer();
                if (relationship < Main.Settings!.RequiredRelationshipToJoinKingdom)
                {
                    explanation = new TextObject("{=nepotism_h_01}Nepotism: Relationship ({RELATION}) is not high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToJoinKingdom);
                    return false;
                }
                else
                {
                    explanation = new TextObject("{=nepotism_h_02}Nepotism: Relationship ({RELATION}) is high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToJoinKingdom);
                }
            }
            else
            {
                explanation = null;
            }
            return true;
        }

        public bool JoinKingdomCondition()
        {
            Hero mainHero = Hero.MainHero;

            if (mainHero.Clan == null || mainHero.Clan.Kingdom == null)
            {
                return false;
            }

            Hero conversationHero = Hero.OneToOneConversationHero;

            if (conversationHero.Clan == null)
            {
                return false;
            }

            bool isMainHeroFamilyInOtherClan = false;
            bool isClanleader = conversationHero.Clan != null && conversationHero.Clan.Leader == conversationHero;
            bool isOtherKingdom = conversationHero.Clan != null && mainHero.Clan != null && conversationHero.Clan.Kingdom != mainHero.Clan.Kingdom;
            bool isNotRulingClan = conversationHero.Clan != null && conversationHero.Clan.Kingdom != null && conversationHero.Clan.Kingdom.RulingClan != conversationHero.Clan;
            if ((Main.Settings!.DirectFamilyOnly ? mainHero.IsFamilyOf(conversationHero) : mainHero.IsRelatedTo(conversationHero)) && conversationHero.Clan != mainHero.Clan)
            {
                isMainHeroFamilyInOtherClan = true;
            }
            TextObject textObject = new TextObject("{=nepotism_03}Join {KINGDOM}, the family needs you.", null);
            textObject.SetTextVariable("KINGDOM", mainHero.Clan!.Kingdom.Name);
            MBTextManager.SetTextVariable("JOIN_KINGDOM_ASK_NEPOTISM", textObject.ToString(), false);
            return (isMainHeroFamilyInOtherClan && isNotRulingClan && isClanleader && isOtherKingdom);
        }

        public bool JoinKingdomResponse()
        {
            MBTextManager.SetTextVariable("JOIN_KINGDOM_RESP_NEPOTISM", (new TextObject("{=nepotism_02}As you wish.", null)).ToString(), false);
            return true;
        }

        public void JoinKingdomConsequence()
        {
            Hero mainHero = Hero.MainHero;
            Hero conversationHero = Hero.OneToOneConversationHero;
            if (conversationHero.Occupation != Occupation.Lord)
            {
                conversationHero.SetNewOccupation(Occupation.Lord);
            }
            if (conversationHero.Clan == null)
            {
                conversationHero.Clan = mainHero.Clan;
            }
            else
            {
                conversationHero.Clan.ClanLeaveKingdom(false);
                conversationHero.Clan.Kingdom = mainHero.Clan.Kingdom;

                TextObject message = new TextObject("{=nepotism_n_02}{FAMILY_MEMBER} and {CLAN} has joined {KINGDOM}.", null);
                message.SetTextVariable("FAMILY_MEMBER", conversationHero.Name);
                message.SetTextVariable("CLAN", conversationHero.Clan.Name);
                message.SetTextVariable("KINGDOM", mainHero.Clan.Kingdom.Name);
                MBInformationManager.AddQuickInformation(message, 0, conversationHero.CharacterObject, "");
            }
        }

        private void AddMergeKingdomDialogs(CampaignGameStarter starter)
        {
            starter.AddPlayerLine("hero_merge_kingdom_ask_nepotism", "hero_main_options", "hero_merge_kingdom_reply_nepotism", "{MERGE_KINGDOM_ASK_NEPOTISM}", new ConversationSentence.OnConditionDelegate(MergeKingdomCondition), null, 100, MergeKingdomsClickable, null);
            starter.AddDialogLine("hero_merge_kingdom_reply_nepotism", "hero_merge_kingdom_reply_nepotism", "close_window", "{MERGE_KINGDOM_RESP_NEPOTISM}", new ConversationSentence.OnConditionDelegate(MergeKingdomResponse), new ConversationSentence.OnConsequenceDelegate(MergeKingdomConsequence), 100, null);
        }

        private bool MergeKingdomsClickable(out TextObject explanation)
        {
            Hero mainHero = Hero.MainHero;
            Hero other = Hero.OneToOneConversationHero;

            if (Main.Settings!.RequireRelationshipToMergeKingdoms)
            {
                var relationship = other.GetRelationWithPlayer();
                if (relationship < Main.Settings!.RequiredRelationshipToMergeKingdoms)
                {
                    explanation = new TextObject("{=nepotism_h_01}Nepotism: Relationship ({RELATION}) is not high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToMergeKingdoms);
                    return false;
                }
                else
                {
                    explanation = new TextObject("{=nepotism_h_02}Nepotism: Relationship ({RELATION}) is high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToMergeKingdoms);
                }
            }
            else
            {
                explanation = null;
            }
            return true;
        }

        public bool MergeKingdomCondition()
        {
            Hero mainHero = Hero.MainHero;

            if (mainHero.Clan == null || mainHero.Clan.Kingdom == null)
            {
                return false;
            }

            Hero conversationHero = Hero.OneToOneConversationHero;

            if (conversationHero.Clan == null || conversationHero.Clan.Kingdom == null)
            {
                return false;
            }

            bool isMainHeroFamilyInOtherClan = false;
            bool isClanleader = conversationHero.Clan != null && conversationHero.Clan.Leader == conversationHero;
            bool isRulingClan = conversationHero.Clan != null && conversationHero.Clan.Kingdom != null && conversationHero.Clan.Kingdom.RulingClan == conversationHero.Clan;
            bool isOtherKingdom = conversationHero.Clan != null && mainHero.Clan != null && conversationHero.Clan.Kingdom != mainHero.Clan.Kingdom;
            if ((Main.Settings!.DirectFamilyOnly ? mainHero.IsFamilyOf(conversationHero) : mainHero.IsRelatedTo(conversationHero)) && conversationHero.Clan != mainHero.Clan)
            {
                isMainHeroFamilyInOtherClan = true;
            }
            TextObject textObject = new TextObject("{=nepotism_04}We should merge {THEIR_KINGDOM} into {KINGDOM}, for the family.", null);
            textObject.SetTextVariable("KINGDOM", mainHero.Clan!.Kingdom!.Name);
            textObject.SetTextVariable("THEIR_KINGDOM", conversationHero.Clan!.Kingdom!.Name);
            MBTextManager.SetTextVariable("MERGE_KINGDOM_ASK_NEPOTISM", textObject.ToString(), false);
            return (isMainHeroFamilyInOtherClan && isRulingClan && isClanleader && isOtherKingdom);
        }

        public bool MergeKingdomResponse()
        {
            MBTextManager.SetTextVariable("MERGE_KINGDOM_RESP_NEPOTISM", (new TextObject("{=nepotism_02}As you wish.", null)).ToString(), false);
            return true;
        }

        public void MergeKingdomConsequence()
        {
            Hero mainHero = Hero.MainHero;
            Hero conversationHero = Hero.OneToOneConversationHero;

            var oldKingdom = conversationHero.Clan.Kingdom;
            var currentClan = oldKingdom.RulingClan;
            
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
                    ChangeKingdomAction.ApplyByJoinToKingdom(clan, mainHero.Clan.Kingdom, false);

                    TextObject message = new TextObject("{=nepotism_n_03}{CLAN} has joined {KINGDOM}.", null);
                    message.SetTextVariable("CLAN", clan.Name);
                    message.SetTextVariable("KINGDOM", mainHero.Clan.Kingdom.Name);
                    MBInformationManager.AddQuickInformation(message, 0, null, "");
                }

            }
            Campaign.Current.KingdomManager.AbdicateTheThrone(oldKingdom);
            ChangeKingdomAction.ApplyByJoinToKingdom(currentClan, mainHero.Clan.Kingdom, false);
            oldKingdom.RulingClan = CreateClan(oldKingdom);
            DestroyClanAction.Apply(oldKingdom.RulingClan);

            TextObject message2 = new TextObject("{=nepotism_n_04}{THEIR_KINGDOM} has merged into {KINGDOM}.", null);
            message2.SetTextVariable("THEIR_KINGDOM", oldKingdom.Name);
            message2.SetTextVariable("KINGDOM", mainHero.Clan.Kingdom.Name);
            MBInformationManager.AddQuickInformation(message2, 0, null, "");

            if (!oldKingdom.IsEliminated)
            {
                DestroyKingdomAction.Apply(oldKingdom); 
            }
        }

        private void AddRuleKingdomDialogs(CampaignGameStarter starter)
        {
            starter.AddPlayerLine("hero_rule_kingdom_ask_nepotism", "hero_main_options", "hero_rule_kingdom_reply_nepotism", "{RULE_KINGDOM_ASK_NEPOTISM}", new ConversationSentence.OnConditionDelegate(RuleKingdomCondition), null, 100, RuleKingdomsClickable, null);
            starter.AddDialogLine("hero_rule_kingdom_reply_nepotism", "hero_rule_kingdom_reply_nepotism", "close_window", "{RULE_KINGDOM_RESP_NEPOTISM}", new ConversationSentence.OnConditionDelegate(RuleKingdomResponse), new ConversationSentence.OnConsequenceDelegate(RuleKingdomConsequence), 100, null);
        }

        private bool RuleKingdomsClickable(out TextObject explanation)
        {
            Hero mainHero = Hero.MainHero;
            Hero other = Hero.OneToOneConversationHero;

            if (Main.Settings!.RequireRelationshipToRuleKingdom)
            {
                var relationship = other.GetRelationWithPlayer();
                if (relationship < Main.Settings!.RequiredRelationshipToRuleKingdom)
                {
                    explanation = new TextObject("{=nepotism_h_01}Nepotism: Relationship ({RELATION}) is not high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToRuleKingdom);
                    return false;
                }
                else
                {
                    explanation = new TextObject("{=nepotism_h_02}Nepotism: Relationship ({RELATION}) is high enough for success. ({REQUIRED} required)", null);
                    explanation.SetTextVariable("RELATION", relationship);
                    explanation.SetTextVariable("REQUIRED", Main.Settings!.RequiredRelationshipToRuleKingdom);
                }
            }
            else
            {
                explanation = null;
            }

            if (mainHero.Clan.Tier < 4)
            {
                explanation = new TextObject("{=nepotism_h_03}Nepotism: Clan Tier ({TIER}) is not high enough. ({REQUIRED} required)", null);
                explanation.SetTextVariable("TIER", mainHero.Clan.Tier);
                explanation.SetTextVariable("REQUIRED", 4);
                return false;
            }

            return true;
        }

        public bool RuleKingdomCondition()
        {
            Hero mainHero = Hero.MainHero;

            if (mainHero.Clan == null || mainHero.Clan.Kingdom == null)
            {
                return false;
            }

            Hero conversationHero = Hero.OneToOneConversationHero;

            if (conversationHero.Clan == null || conversationHero.Clan.Kingdom == null)
            {
                return false;
            }

            bool isMainHeroFamilyInOtherClan = false;
            bool isClanleader = conversationHero.Clan != null && conversationHero.Clan.Leader == conversationHero;
            bool isRulingClan = conversationHero.Clan != null && conversationHero.Clan.Kingdom != null && conversationHero.Clan.Kingdom.RulingClan == conversationHero.Clan;
            bool isSameKingdom = conversationHero.Clan != null && mainHero.Clan != null && conversationHero.Clan.Kingdom == mainHero.Clan.Kingdom;
            if ((Main.Settings!.DirectFamilyOnly ? mainHero.IsFamilyOf(conversationHero) : mainHero.IsRelatedTo(conversationHero)) && conversationHero.Clan != mainHero.Clan)
            {
                isMainHeroFamilyInOtherClan = true;
            }
            TextObject textObject = new TextObject("{=nepotism_05}Let {CLAN} rule {THEIR_KINGDOM}, for the family.", null);
            textObject.SetTextVariable("CLAN", mainHero.Clan!.Name);
            textObject.SetTextVariable("THEIR_KINGDOM", conversationHero.Clan!.Kingdom!.Name);
            MBTextManager.SetTextVariable("RULE_KINGDOM_ASK_NEPOTISM", textObject.ToString(), false);
            return (isMainHeroFamilyInOtherClan && isRulingClan && isClanleader && isSameKingdom);
        }

        public bool RuleKingdomResponse()
        {
            MBTextManager.SetTextVariable("RULE_KINGDOM_RESP_NEPOTISM", (new TextObject("{=nepotism_02}As you wish.", null)).ToString(), false);
            return true;
        }

        public void RuleKingdomConsequence()
        {
            Hero mainHero = Hero.MainHero;
            Clan clan = mainHero.Clan;

            Hero conversationHero = Hero.OneToOneConversationHero;

            var kingdom = conversationHero.Clan.Kingdom;
            Clan rulingClan = kingdom.RulingClan;



            ChangeRulingClanAction.Apply(kingdom, clan);
            KingSelectionKingdomDecision kingSelectionKingdomDecision = new PlayerAsKingSelectionKingdomDecision(clan, rulingClan)
            {
                IsEnforced = true
            };
            kingdom.AddDecision(kingSelectionKingdomDecision, true);

            // Just to be sure.
            kingdom.RulingClan = mainHero.Clan;

            TextObject message2 = new TextObject("{=nepotism_n_05}{THEIR_KINGDOM} is now ruled by {CLAN}.", null);
            message2.SetTextVariable("THEIR_KINGDOM", kingdom.Name);
            message2.SetTextVariable("CLAN", mainHero.Clan.Name);
            MBInformationManager.AddQuickInformation(message2, 0, null, "");
        }

        private Clan CreateClan(Kingdom? fromKingdom = null)
        {
            Kingdom kingdom = fromKingdom ?? Kingdom.All.GetRandomElement<Kingdom>();
            CultureObject culture = kingdom.Culture;
            Settlement settlement = kingdom.Settlements.FirstOrDefault<Settlement>((Settlement x) => x.IsTown) ?? kingdom.Settlements.GetRandomElement<Settlement>();
            TextObject textObject = NameGenerator.Current.GenerateClanName(culture, settlement);
            Clan clan = Clan.CreateClan($"no_clan_{Clan.All.Count}");
            TextObject textObject1 = new TextObject("{=!}informal", null);
            CultureObject cultureObject = Kingdom.All.GetRandomElement<Kingdom>().Culture;
            Banner banner = Banner.CreateRandomClanBanner(-1);
            Vec2 vec2 = new Vec2();
            clan.InitializeClan(textObject, textObject1, cultureObject, banner, vec2, false);
            CharacterObject characterObject = culture.LordTemplates.FirstOrDefault<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.Lord);
            Settlement randomElement = kingdom.Settlements.GetRandomElement<Settlement>();
            Hero hero = HeroCreator.CreateSpecialHero(characterObject ?? kingdom.Leader.CharacterObject, randomElement, clan, null, MBRandom.RandomInt(18, 36));
            hero.ChangeState(Hero.CharacterStates.Active);
            clan.SetLeader(hero);
            ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom, false);

            return clan;
        }
        #endregion
    }

    public class PlayerAsKingSelectionKingdomDecision : KingSelectionKingdomDecision
    {
        public PlayerAsKingSelectionKingdomDecision(Clan proposerClan, Clan? clanToExclude = null): base(proposerClan,clanToExclude)
        {

        }

        public override IEnumerable<DecisionOutcome> DetermineInitialCandidates()
        {
            yield return new KingSelectionKingdomDecision.KingSelectionDecisionOutcome(Hero.MainHero);
        }

    }

}
