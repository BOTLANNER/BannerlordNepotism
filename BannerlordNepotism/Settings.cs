using System.Collections.Generic;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
#if MCM_v5
using MCM.Abstractions.Base.Global;
#else
using MCM.Abstractions.Settings.Base.Global;
#endif

namespace BannerlordNepotism
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => $"{Main.Name}_v1";
        public override string DisplayName => Main.DisplayName;
        public override string FolderName => Main.Name;
        public override string FormatType => "json";

        private const string AddJoinClan_Hint = "Adds dialogue for family members to join player clan. [ Default: ON ]";

        [SettingPropertyBool("Add Join Clan Dialogue", HintText = AddJoinClan_Hint, RequireRestart = true, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Dialogue Settings", GroupOrder = 0)]
        public bool AddJoinClan { get; set; } = true;

        private const string AddJoinKingdom_Hint = "Adds dialogue for family members who are clan leaders to join player kingdom. [ Default: ON ]";

        [SettingPropertyBool("Add Join Kingdom Dialogue", HintText = AddJoinKingdom_Hint, RequireRestart = true, Order = 1, IsToggle = false)]
        [SettingPropertyGroup("Dialogue Settings")]
        public bool AddJoinKingdom { get; set; } = true;

        private const string AddMergeKingdoms_Hint = "Adds dialogue for family members who are rulers of kingdoms to merge into player kingdom. [ Default: ON ]";

        [SettingPropertyBool("Add Merge Kingdoms Dialogue", HintText = AddMergeKingdoms_Hint, RequireRestart = true, Order = 2, IsToggle = false)]
        [SettingPropertyGroup("Dialogue Settings")]
        public bool AddMergeKingdoms { get; set; } = true;

        private const string AddRuleKingdom_Hint = "Adds dialogue for family members who are rulers of kingdoms to let the player clan become the ruler if the player clan are vassals of the kingdom. (Clan tier 4 is required) [ Default: ON ]";

        [SettingPropertyBool("Add Become Kingdom Ruler Dialogue", HintText = AddRuleKingdom_Hint, RequireRestart = true, Order = 3, IsToggle = false)]
        [SettingPropertyGroup("Dialogue Settings")]
        public bool AddRuleKingdom { get; set; } = true;

        private const string DirectFamilyOnly_Hint = "Only applies dialogue to direct family members. When disabled will apply to extended relatives as well (recommended). [ Default: OFF ]";

        [SettingPropertyBool("Direct Family Only", HintText = DirectFamilyOnly_Hint, RequireRestart = false, Order = 4, IsToggle = false)]
        [SettingPropertyGroup("Family Detection Settings", GroupOrder = 1)]
        public bool DirectFamilyOnly { get; set; } = false;

        private const string IncludeByMarriage_Hint = "Also applies dialogue to family members by marriage. When 'Direct Family Only' is enabled will only apply to player spouse and in-laws. Alternatively will also apply to in-laws of any other relative (and their in-laws, etc) [ Default: ON ]";

        [SettingPropertyBool("Include Relatives By Marriage", HintText = IncludeByMarriage_Hint, RequireRestart = false, Order = 5, IsToggle = false)]
        [SettingPropertyGroup("Family Detection Settings")]
        public bool IncludeByMarriage { get; set; } = true;


        private const string RequireRelationshipToJoinClan_Hint = "Requires specified relationship level for player to be able to ask family member to join clan. [ Default: ON ]";

        [SettingPropertyBool("Require relationship level for Join Clan", HintText = RequireRelationshipToJoinClan_Hint, RequireRestart = false, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings", GroupOrder = 2)]
        public bool RequireRelationshipToJoinClan { get; set; } = true;


        private const string RequiredRelationshipToJoinClan_Hint = "Requires specified relationship level for player to be able to ask family member to join clan. [ Default: 30 ]";

        [SettingPropertyFloatingInteger("Required relationship level for Join Clan", minValue: 0f, maxValue: 100f, HintText = RequiredRelationshipToJoinClan_Hint, RequireRestart = false, Order = 1)]
        [SettingPropertyGroup("Requirements Settings")]
        public float RequiredRelationshipToJoinClan { get; set; } = 30f;


        private const string RequireRelationshipToJoinKingdom_Hint = "Requires specified relationship level for player to be able to ask family member clan to join kingdom. [ Default: ON ]";

        [SettingPropertyBool("Require relationship level for Join Kingdom", HintText = RequireRelationshipToJoinKingdom_Hint, RequireRestart = false, Order = 2, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireRelationshipToJoinKingdom { get; set; } = true;


        private const string RequiredRelationshipToJoinKingdom_Hint = "Required relationship level for player to be able to ask family member clan to join kingdom. [ Default: 60 ]";

        [SettingPropertyFloatingInteger("Required relationship level for Join Kingdom", minValue: 0f, maxValue: 100f, HintText = RequiredRelationshipToJoinKingdom_Hint, RequireRestart = false, Order = 3)]
        [SettingPropertyGroup("Requirements Settings")]
        public float RequiredRelationshipToJoinKingdom { get; set; } = 60f;


        private const string RequireBarterToJoinKingdom_Hint = "Requires a barter to be successful for family member to join the kingdom. [ Default: ON ]";

        [SettingPropertyBool("Require barter for Join Kingdom", HintText = RequireBarterToJoinKingdom_Hint, RequireRestart = false, Order = 4, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireBarterToJoinKingdom { get; set; } = true;


        private const string RequireRelationshipToMergeKingdoms_Hint = "Requires specified relationship level for player to be able to ask family member to merge kingdoms. [ Default: ON ]";

        [SettingPropertyBool("Require relationship level for Merge Kingdoms", HintText = RequireRelationshipToMergeKingdoms_Hint, RequireRestart = false, Order = 5, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireRelationshipToMergeKingdoms { get; set; } = true;


        private const string RequiredRelationshipToMergeKingdoms_Hint = "Required relationship level for player to be able to ask family member to merge kingdoms. [ Default: 60 ]";

        [SettingPropertyFloatingInteger("Required relationship level for Merge Kingdoms", minValue: 0f, maxValue: 100f, HintText = RequiredRelationshipToMergeKingdoms_Hint, RequireRestart = false, Order = 6)]
        [SettingPropertyGroup("Requirements Settings")]
        public float RequiredRelationshipToMergeKingdoms { get; set; } = 60f;


        private const string RequireBarterToMergeKingdoms_Hint = "Requires a barter to be successful for player to merge kingdoms. [ Default: ON ]";

        [SettingPropertyBool("Require barter for Merge Kingdoms", HintText = RequireBarterToMergeKingdoms_Hint, RequireRestart = false, Order = 7, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireBarterToMergeKingdoms { get; set; } = true;


        private const string RequireRelationshipToRuleKingdom_Hint = "Requires specified relationship level for player to be able to ask family member to allow player to rule the kingdom. [ Default: ON ]";

        [SettingPropertyBool("Require relationship level for Rule Kingdom", HintText = RequireRelationshipToRuleKingdom_Hint, RequireRestart = false, Order = 8, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireRelationshipToRuleKingdom { get; set; } = true;


        private const string RequiredRelationshipToRuleKingdom_Hint = "Required relationship level for player to be able to ask family member to allow player to rule the kingdom. [ Default: 60 ]";

        [SettingPropertyFloatingInteger("Required relationship level for Rule Kingdom", minValue: 0f, maxValue: 100f, HintText = RequiredRelationshipToRuleKingdom_Hint, RequireRestart = false, Order = 9)]
        [SettingPropertyGroup("Requirements Settings")]
        public float RequiredRelationshipToRuleKingdom { get; set; } = 60f;


        private const string RequireBarterToRuleKingdom_Hint = "Requires a barter to be successful for player to rule the kingdom. [ Default: ON ]";

        [SettingPropertyBool("Require barter for Rule Kingdom", HintText = RequireBarterToRuleKingdom_Hint, RequireRestart = false, Order = 10, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireBarterToRuleKingdom { get; set; } = true;

        private const string BarterDifficulty_Hint = "Difficulty for barters. Higher difficulty will require larger amounts, lower difficulty is less expensive. [ Default: 0.99 ]";

        [SettingPropertyFloatingInteger("Barter Difficulty", 0.01f, 2.00f, "0.00", HintText = BarterDifficulty_Hint, RequireRestart = false, Order = 11)]
        [SettingPropertyGroup("Requirements Settings")]
        public float BarterDifficulty { get; set; } = 0.99f;


        private const string PatchFamilyControl_Hint = "Patches Family Control mod if found to prevent duplicate dialogs for joining player clan. [ Default: ON ]";

        [SettingPropertyBool("Patch Family Control", HintText = PatchFamilyControl_Hint, RequireRestart = true, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Patches", GroupOrder = 3)]
        public bool PatchFamilyControl { get; set; } = true;

        private const string PatchMembershipIssues_Hint = "Patches issues with clan/kingdom leaders not being members of that which they lead or ruling clans not being part of kingdoms that they rule. Can cause compatibility issues with other mods if this is intended. [ Default: ON ]";

        [SettingPropertyBool("Patch Leader Memberships", HintText = PatchMembershipIssues_Hint, RequireRestart = false, Order = 1, IsToggle = false)]
        [SettingPropertyGroup("Patches")]
        public bool PatchMembershipIssues { get; set; } = true;

        private const string PatchDeadLeaderIssues_Hint = "Patches issues with clan/kingdom leaders being dead but still in charge. [ Default: ON ]";

        [SettingPropertyBool("Patch Dead Leaders", HintText = PatchDeadLeaderIssues_Hint, RequireRestart = false, Order = 2, IsToggle = false)]
        [SettingPropertyGroup("Patches")]
        public bool PatchDeadLeaderIssues { get; set; } = true;

        private const string VerboseMessages_Hint = "Display detailed messages when auto-correcting detected problems. [ Default: ON ]";

        [SettingPropertyBool("Detailed Messages", HintText = VerboseMessages_Hint, RequireRestart = false, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Messages", GroupOrder = 4)]
        public bool VerboseMessages { get; set; } = true;
    }
}
